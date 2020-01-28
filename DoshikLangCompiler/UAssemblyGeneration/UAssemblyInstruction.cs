using System.Collections.Generic;

namespace DoshikLangCompiler.UAssemblyGeneration
{
    public class UAssemblyInstruction
    {
        public UAssemblyInstruction(string name, ushort byteCount, List<IUAssemblyInstructionParameter> parameters)
        {
            Name = name;
            ByteCount = byteCount;
            Parameters = parameters ?? new List<IUAssemblyInstructionParameter>();
        }

        public string Name { get; private set; }

        public ushort ByteCount { get; private set; }

        public List<IUAssemblyInstructionParameter> Parameters { get; private set; }

        public string GlobalLabel { get; set; }
    }

    public interface IUAssemblyInstructionParameter
    {
    }

    public class UAssemblyInstructionParameter_StringLiteral : IUAssemblyInstructionParameter
    {
        public string StringValue { get; set; }
    }

    public class UAssemblyInstructionParameter_VariableName : IUAssemblyInstructionParameter
    {
        public string VariableName { get; set; }
    }

    public class UAssemblyInstructionParameter_CodeAddress : IUAssemblyInstructionParameter
    {
        public string GlobalLabel { get; set; }

        public int? InstructionOffset { get; set; }

        public uint? Address { get; set; }
    }
}
