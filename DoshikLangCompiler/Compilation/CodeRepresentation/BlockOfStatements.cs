using DoshikLangCompiler.Compilation.CodeRepresentation.Expressions.Tree;
using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    public abstract class Statement : ICodeHierarchyNode
    {
        public Statement(ICodeHierarchyNode parent)
        {
            Parent = parent;
        }

        public ICodeHierarchyNode Parent { get; private set; }
    }

    public class BlockOfStatements : Statement, IScopeOwner
    {
        public BlockOfStatements(ICodeHierarchyNode parent, Scope parentScope)
            : base(parent)
        {
            Scope = new Scope(this, parentScope);
        }

        public Scope Scope { get; private set; }

        public List<Statement> Statements { get; } = new List<Statement>();
    }

    // Объявление локальной переменной. Может встречаться внутри блока либо for init части
    public class LocalVariableDeclarationStatement : Statement, IVariableDeclarator
    {
        public LocalVariableDeclarationStatement(ICodeHierarchyNode parent)
            : base(parent)
        {
        }

        public Variable Variable { get; set; }

        /// <summary>
        /// Инициализирующее выражение. Может быть null, если оно не задано
        /// </summary>
        public ExpressionTree Initializer { get; set; }
    }

    public class ExpressionStatement : Statement
    {
        public ExpressionStatement(ICodeHierarchyNode parent)
            : base(parent)
        {
        }

        public ExpressionTree Expression { get; set; }
    }
}
