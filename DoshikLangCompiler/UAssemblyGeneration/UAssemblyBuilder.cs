using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoshikLangCompiler.UAssemblyGeneration
{
    public class UAssemblyBuilder
    {
        public UAssemblyBuilder()
        {
            _variables = new Dictionary<string, Variable>();
            _events = new Dictionary<string, UAssemblyEventBodyEmitter>();
        }

        public const uint maxCodeAddress = 0xFFFFFF;

        public IReadOnlyDictionary<string, UAssemblyEventBodyEmitter> Events { get { return _events; } }

        public UAssemblyBuilderCode MakeCode(bool humanReadable)
        {
            var code = new UAssemblyBuilderCode();

            code.UdonAssemblyCode = GenerateCode(PrepareToCodeGeneration(), humanReadable);
            code.DefaultHeapValues = MakeDefaultHeapValues();

            return code;
        }

        public UAssemblyEventBodyEmitter AddOrGetEvent(string eventName)
        {
            UAssemblyEventBodyEmitter eventBody;
            if (!_events.TryGetValue(eventName, out eventBody))
            {
                eventBody = new UAssemblyEventBodyEmitter(eventName);
                _events[eventName] = eventBody;
            }

            return eventBody;
        }

        public void AddVariable(bool isPublic, string name, string type, object defaultValue = null, bool isThisDefaultValue = false)
        {
            if (_variables.ContainsKey(name))
                throw new Exception("variable with such name already exists");

            _variables[name] = new Variable() { IsPublic = isPublic, Name = name, Type = type, DefaultValue = defaultValue, IsThisDefaultValue = isThisDefaultValue };
        }

        private PreparedToCodeGenerationData PrepareToCodeGeneration()
        {
            var result = new PreparedToCodeGenerationData()
            {
                CodeAddressSpace = new List<InstructionWithAddress>(),
                EventsUsedInAddressSpace = new List<UAssemblyEventBodyEmitter>(),
                GlobalLabelsMap = new Dictionary<string, InstructionWithAddress>()
            };

            uint currentAddress = 0;

            foreach (var eventBody in _events.Values)
            {
                if (eventBody.Instructions.Count > 0)
                    result.EventsUsedInAddressSpace.Add(eventBody);

                for (int eventInstructionIdx = 0; eventInstructionIdx < eventBody.Instructions.Count; eventInstructionIdx++)
                {
                    var instruction = eventBody.Instructions[eventInstructionIdx];

                    result.CodeAddressSpace.Add(new InstructionWithAddress()
                    {
                        Instruction = instruction,
                        EventBody = eventBody,
                        Address = currentAddress,
                        IsFirstInstructionInEvent = eventInstructionIdx == 0,
                        IsLastInstructionInEvent = eventInstructionIdx == eventBody.Instructions.Count - 1
                    });

                    currentAddress += instruction.ByteCount;
                }
            }

            // Строим карту глобальных маркеров

            foreach (var instruction in result.CodeAddressSpace.Where(x => x.Instruction.GlobalLabel != null))
            {
                if (result.GlobalLabelsMap.ContainsKey(instruction.Instruction.GlobalLabel))
                    throw new Exception("multiple instructions have same global label");

                result.GlobalLabelsMap[instruction.Instruction.GlobalLabel] = instruction;
            }

            // Разрешаем адреса (делаем все адреса в инструкциях абсолютными)

            // ToDo: пока делаю грязный хак - мутирую текущие значения, добавляя абсолютный адрес в них, если его там еще нет
            // по хорошему нельзя тут ничего мутировать, метод построения кода должен строить код и не мутировать состояние билдера

            for (int addressSpaceInstructionIdx = 0; addressSpaceInstructionIdx < result.CodeAddressSpace.Count; addressSpaceInstructionIdx++)
            {
                var instruction = result.CodeAddressSpace[addressSpaceInstructionIdx];

                foreach (var codeAddressParameter in instruction.Instruction.Parameters.OfType<UAssemblyInstructionParameter_CodeAddress>())
                {
                    if (codeAddressParameter.Address != null)
                        continue;

                    if (codeAddressParameter.InstructionOffset != null)
                    {
                        var targetInstructionAddressSpaceIdx = Math.Min(result.CodeAddressSpace.Count - 1, Math.Max(0, addressSpaceInstructionIdx + codeAddressParameter.InstructionOffset.Value));

                        codeAddressParameter.Address = result.CodeAddressSpace[targetInstructionAddressSpaceIdx].Address;
                    }
                    else if (codeAddressParameter.GlobalLabel != null)
                    {
                        InstructionWithAddress targetInstruction;
                        if (!result.GlobalLabelsMap.TryGetValue(codeAddressParameter.GlobalLabel, out targetInstruction))
                            throw new Exception("target global label not found");

                        codeAddressParameter.Address = targetInstruction.Address;
                    }
                    else
                        throw new NotImplementedException();
                }
            }

            return result;
        }

        private string GenerateCode(PreparedToCodeGenerationData codeAddressSpace, bool humanReadable)
        {
            var sb = new StringBuilder();

            void AppendTabIfHumanReadable()
            {
                if (humanReadable)
                    sb.Append("    ");
            }

            void AppendLineIfHumanReadable()
            {
                if (humanReadable)
                    sb.AppendLine();
            }

            if (humanReadable)
                sb.AppendLine("# compiled with UAssemblyBuilder");

            AppendLineIfHumanReadable();

            sb.AppendLine(".data_start");

            AppendLineIfHumanReadable();

            // variable exports
            foreach (var variable in _variables.Values.Where(x => x.IsPublic))
            {
                AppendTabIfHumanReadable();
                sb.AppendLine($".export { variable.Name }");
            }

            AppendLineIfHumanReadable();

            // variable definitions
            foreach (var variable in _variables.Values)
            {
                AppendTabIfHumanReadable();
                sb.AppendLine($"{ variable.Name }: %{ variable.Type }, { (variable.IsThisDefaultValue ? "this" : "null") }");
            }

            AppendLineIfHumanReadable();

            sb.AppendLine(".data_end");

            AppendLineIfHumanReadable();

            sb.AppendLine(".code_start");

            AppendLineIfHumanReadable();

            // events export
            foreach (var eventBody in codeAddressSpace.EventsUsedInAddressSpace)
            {
                AppendTabIfHumanReadable();
                sb.AppendLine($".export { eventBody.EventName }");
            }

            AppendLineIfHumanReadable();

            // events and instructions (inside of events)
            foreach (var instruction in codeAddressSpace.CodeAddressSpace)
            {
                void RenderInstruction()
                {
                    AppendTabIfHumanReadable();
                    AppendTabIfHumanReadable();

                    sb.Append($"{ instruction.Instruction.Name }");

                    foreach (var parameter in instruction.Instruction.Parameters)
                    {
                        sb.Append(", ");

                        if (parameter is UAssemblyInstructionParameter_StringLiteral parameterStringLiteral)
                        {
                            sb.Append("\"");
                            sb.Append(parameterStringLiteral.StringValue);
                            sb.Append("\"");
                        }
                        else if (parameter is UAssemblyInstructionParameter_VariableName parameterVariableName)
                        {
                            sb.Append(parameterVariableName.VariableName);
                        }
                        else if (parameter is UAssemblyInstructionParameter_CodeAddress parameterCodeAddress)
                        {
                            var address = Math.Min(parameterCodeAddress.Address.Value, maxCodeAddress);

                            sb.Append("0x" + address.ToString("X6"));
                        }
                        else
                            throw new NotImplementedException();
                    }

                    if (humanReadable)
                        sb.Append("  # 0x" + instruction.Address.ToString("X6"));

                    sb.AppendLine();
                }

                if (instruction.IsFirstInstructionInEvent)
                {
                    AppendTabIfHumanReadable();
                    sb.AppendLine($"{ instruction.EventBody.EventName }:");
                }

                RenderInstruction();

                if (instruction.IsLastInstructionInEvent)
                    AppendLineIfHumanReadable();
            }

            AppendLineIfHumanReadable();

            sb.AppendLine(".code_end");

            return sb.ToString();
        }

        private Dictionary<string, (object value, Type type)> MakeDefaultHeapValues()
        {
            var result = new Dictionary<string, (object value, Type type)>();
            foreach (var variable in _variables.Values)
            {
                if (variable.IsThisDefaultValue || variable.DefaultValue == null)
                    continue;

                result.Add(variable.Name, (variable.DefaultValue, variable.DefaultValue.GetType()));
            }

            return result;
        }

        private Dictionary<string, Variable> _variables;
        private Dictionary<string, UAssemblyEventBodyEmitter> _events;

        private class Variable
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public object DefaultValue { get; set; }

            public bool IsThisDefaultValue { get; set; }

            public bool IsPublic { get; set; }
        }

        private class PreparedToCodeGenerationData
        {
            public List<InstructionWithAddress> CodeAddressSpace { get; set; }
            public List<UAssemblyEventBodyEmitter> EventsUsedInAddressSpace { get; set; }
            public Dictionary<string, InstructionWithAddress> GlobalLabelsMap { get; set; }
        }

        private class InstructionWithAddress
        {
            public UAssemblyInstruction Instruction { get; set; }

            public UAssemblyEventBodyEmitter EventBody { get; set; }

            public uint Address { get; set; }

            public bool IsFirstInstructionInEvent { get; set; }
            public bool IsLastInstructionInEvent { get; set; }
        }
    }
}
