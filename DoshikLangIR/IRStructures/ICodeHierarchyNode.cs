namespace DoshikLangIR
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

        public static TParent FindNearestParentOfType<TParent>(this ICodeHierarchyNode node)
            where TParent : ICodeHierarchyNode
        {
            if (node.Parent == null)
                return default;

            if (node.Parent is TParent result)
                return result;

            return node.Parent.FindNearestParentOfType<TParent>();
        }
    }
}
