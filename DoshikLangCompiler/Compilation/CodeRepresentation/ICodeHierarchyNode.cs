namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    public interface ICodeHierarchyNode
    {
        ICodeHierarchyNode Parent { get; }
    }
}
