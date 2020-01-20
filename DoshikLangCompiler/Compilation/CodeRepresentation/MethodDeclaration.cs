using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation.CodeRepresentation
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
        /// Имя типа возвращаемого значения из ивента. Если это null, значит указан void тип
        /// </summary>
        public string ReturnTypeOrVoid { get; set; }

        public string Name { get; set; }

        // Является ли событие кастомным или определенным пользователем
        public bool IsCustom { get; set; }

        public MethodDeclarationParameters Parameters { get; set; }

        public DoshikParser.BlockContext AntlrBody { get; set; }

        public BlockOfStatements BodyBlock { get; set; }
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
