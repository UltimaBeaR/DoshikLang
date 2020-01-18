using System;
using System.Collections.Generic;

namespace DoshikLangCompiler.UAssemblyGeneration
{
    public class UAssemblyBuilderCode
    {
        public string UdonAssemblyCode { get; set; }
        public Dictionary<string, (object value, Type type)> DefaultHeapValues { get; set; }
    }
}
