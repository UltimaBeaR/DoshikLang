using System.Collections.Generic;

namespace DoshikLangCompiler.UAssemblyGeneration
{
    public class UAssemblyBuilderCode
    {
        public string UdonAssemblyCode { get; set; }
        public Dictionary<string, DefaultHeapValue> DefaultHeapValues { get; set; }
    }
}
