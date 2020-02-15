using System;

namespace DoshikLangCompiler
{
    public abstract class DefaultHeapValue
    {
    }

    public class ConcreteValueDefaultHeapValue : DefaultHeapValue
    {
        public object Value { get; set; }
        public Type Type { get; set; }
    }

    public class TypeAsStringDefaultHeapValue : DefaultHeapValue
    {
        public string TypeAsString { get; set; }
    }
}
