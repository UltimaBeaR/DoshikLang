using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    public class MethodDeclaration
    {
        public CompilationUnit Parent { get; set; }

        /// <summary>
        /// Имя типа возвращаемого значения из ивента. Если это null, значит указан void тип
        /// </summary>
        public string ReturnTypeOrVoid { get; set; }

        public string Name { get; set; }

        // Является ли событие кастомным или определенным пользователем
        public bool IsCustom { get; set; }

        public MethodDeclarationParameters Parameters { get; } = new MethodDeclarationParameters();

        public DoshikParser.BlockContext AntlrBody { get; set; }
    }

    public class MethodDeclarationParameters : IScopeOwner
    {
        public MethodDeclaration Parent { get; set; }

        public List<MethodDeclarationParameter> Parameters { get; } = new List<MethodDeclarationParameter>();

        public Scope Scope { get; } = new Scope();
    }

    public class MethodDeclarationParameter : IVariableDeclarator
    {
        public MethodDeclarationParameters Parent { get; set; }

        public bool IsOutput { get; set; }

        public Variable Variable { get; set; }
    }
}
