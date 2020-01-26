namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    public interface ICodeHierarchyNode
    {
        ICodeHierarchyNode Parent { get; }
    }

    public static class CodeHierarchyNodeExtensions
    {
        public static IScopeOwner FindNearestScopeOwner(this ICodeHierarchyNode node)
        {
            if (node is IScopeOwner nodeScopeOwner)
                return nodeScopeOwner;

            if (node.Parent == null)
                return null;

            return node.Parent.FindNearestScopeOwner();
        }
    }
}
