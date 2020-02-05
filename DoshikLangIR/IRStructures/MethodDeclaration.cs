using Doshik;
using System.Collections.Generic;

namespace DoshikLangIR
{
    public class MethodDeclaration : ICodeHierarchyNode
    {
        public MethodDeclaration(CompilationUnit parent)
        {
            Parent = parent;
        }

        public CompilationUnit Parent { get; private set; }
        ICodeHierarchyNode ICodeHierarchyNode.Parent => Parent;

        /// <summary>
        /// Имя типа возвращаемого значения из ивента. Может быть void
        /// </summary>
        public DataType ReturnTypeOrVoid { get; set; }

        public string Name { get; set; }

        public MethodDeclarationParameters Parameters { get; set; }

        public DoshikParser.BlockContext AntlrBody { get; set; }

        public BlockOfStatements BodyBlock { get; set; }
    }

    public class EventDeclaration : MethodDeclaration
    {
        public EventDeclaration(CompilationUnit parent)
            : base(parent)
        {
        }

        public DoshikExternalApiEvent ExternalEvent { get; set; }

        // Является ли событие кастомным или определенным пользователем
        public bool IsCustom
        {
            get
            {
                return ExternalEvent == null;
            }
        }
    }

    public class MethodDeclarationParameters : ICodeHierarchyNode, IScopeOwner
    {
        public MethodDeclarationParameters(MethodDeclaration parent, Scope parentScope)
        {
            Parent = parent;
            Scope = new Scope(this, parentScope);
        }

        public MethodDeclaration Parent { get; private set; }
        ICodeHierarchyNode ICodeHierarchyNode.Parent => Parent;
        public Scope Scope { get; private set; }

        public List<MethodDeclarationParameter> Parameters { get; } = new List<MethodDeclarationParameter>();
    }

    public class MethodDeclarationParameter : ICodeHierarchyNode, IVariableDeclarator
    {
        public MethodDeclarationParameter(MethodDeclarationParameters parent)
        {
            Parent = parent;
        }

        public MethodDeclarationParameters Parent { get; private set; }
        ICodeHierarchyNode ICodeHierarchyNode.Parent => Parent;

        public bool IsOutput { get; set; }

        public Variable Variable { get; set; }
    }
}
