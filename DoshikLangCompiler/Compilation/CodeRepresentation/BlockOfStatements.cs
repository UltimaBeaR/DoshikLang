using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    public class BlockOfStatements : ICodeHierarchyNode, IScopeOwner
    {
        public BlockOfStatements(ICodeHierarchyNode parent, Scope parentScope)
        {
            Parent = parent;
            Scope = new Scope(this, parentScope);
        }

        public ICodeHierarchyNode Parent { get; private set; }
        public Scope Scope { get; private set; }

        public List<Statement> Statements { get; } = new List<Statement>();
    }

    // ToDo: abstract?
    public class Statement : ICodeHierarchyNode
    {
        public Statement(ICodeHierarchyNode parent)
        {
            Parent = parent;
        }

        public ICodeHierarchyNode Parent { get; private set; }
    }
}
