using System.Collections.Generic;

namespace DoshikLangCompiler.UAssemblyGeneration
{
    public class UAssemblyEventBodyEmitter
    {
        public UAssemblyEventBodyEmitter(string eventName)
        {
            EventName = eventName;
            Instructions = new List<UAssemblyInstruction>();
        }

        public string EventName { get; private set; }

        public List<UAssemblyInstruction> Instructions { get; private set; }

        public UAssemblyInstruction NOP()
        {
            var instruction = new UAssemblyInstruction("NOP", _instructionsToByteCount["NOP"], null);

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction PUSH_varableName(string varableName)
        {
            var instruction = new UAssemblyInstruction("PUSH", _instructionsToByteCount["PUSH"], new List<IUAssemblyInstructionParameter>
            {
                new UAssemblyInstructionParameter_VariableName() { VariableName = varableName }
            });

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction POP()
        {
            var instruction = new UAssemblyInstruction("POP", _instructionsToByteCount["POP"], null);

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction COPY()
        {
            var instruction = new UAssemblyInstruction("COPY", _instructionsToByteCount["COPY"], null);

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction JUMP_globalLabel(string globalLabel)
        {
            var instruction = new UAssemblyInstruction("JUMP", _instructionsToByteCount["JUMP"], new List<IUAssemblyInstructionParameter>
            {
                new UAssemblyInstructionParameter_CodeAddress() { GlobalLabel = globalLabel }
            });

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction JUMP_instructionOffset(int instructionOffset)
        {
            var instruction = new UAssemblyInstruction("JUMP", _instructionsToByteCount["JUMP"], new List<IUAssemblyInstructionParameter>
            {
                new UAssemblyInstructionParameter_CodeAddress() { InstructionOffset = instructionOffset }
            });

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction JUMP_absoluteAddress(uint absoluteAddress)
        {
            var instruction = new UAssemblyInstruction("JUMP", _instructionsToByteCount["JUMP"], new List<IUAssemblyInstructionParameter>
            {
                new UAssemblyInstructionParameter_CodeAddress() { Address = absoluteAddress }
            });

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction JUMP_IF_FALSE_globalLabel(string globalLabel)
        {
            var instruction = new UAssemblyInstruction("JUMP_IF_FALSE", _instructionsToByteCount["JUMP_IF_FALSE"], new List<IUAssemblyInstructionParameter>
            {
                new UAssemblyInstructionParameter_CodeAddress() { GlobalLabel = globalLabel }
            });

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction JUMP_IF_FALSE_instructionOffset(int instructionOffset)
        {
            var instruction = new UAssemblyInstruction("JUMP_IF_FALSE", _instructionsToByteCount["JUMP_IF_FALSE"], new List<IUAssemblyInstructionParameter>
            {
                new UAssemblyInstructionParameter_CodeAddress() { InstructionOffset = instructionOffset }
            });

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction JUMP_IF_FALSE_absoluteAddress(uint absoluteAddress)
        {
            var instruction = new UAssemblyInstruction("JUMP_IF_FALSE", _instructionsToByteCount["JUMP_IF_FALSE"], new List<IUAssemblyInstructionParameter>
            {
                new UAssemblyInstructionParameter_CodeAddress() { Address = absoluteAddress }
            });

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction JUMP_INDIRECT()
        {
            var instruction = new UAssemblyInstruction("JUMP_INDIRECT", _instructionsToByteCount["JUMP_INDIRECT"], new List<IUAssemblyInstructionParameter>
            {
                new UAssemblyInstructionParameter_CodeAddress() { Address = UAssemblyBuilder.maxCodeAddress }
            });

            Instructions.Add(instruction);

            return instruction;
        }

        public UAssemblyInstruction EXTERN_externMethodSignature(string externMethodSignature)
        {
            var instruction = new UAssemblyInstruction("EXTERN", _instructionsToByteCount["EXTERN"], new List<IUAssemblyInstructionParameter>
            {
                new UAssemblyInstructionParameter_StringLiteral() { StringValue = externMethodSignature }
            });

            Instructions.Add(instruction);

            return instruction;
        }

        // Взято из VRC.Udon.Compiler.dll -> VRC.Udon.Compiler.Compilers.UdonGraphCompiler.InstructionsToByteCount
        public static Dictionary<string, ushort> _instructionsToByteCount = new Dictionary<string, ushort>()
        {
            { "ANNOTATION", 5},
            { "COPY", 1},
            { "EXTERN", 5},
            { "JUMP", 5},
            { "JUMP_IF_FALSE", 5},
            { "JUMP_INDIRECT", 5},
            { "NOP", 1 },
            { "POP", 1 },
            { "PUSH", 5 },
        };
    }
}
