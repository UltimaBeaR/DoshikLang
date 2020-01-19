using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation
{
    /// <summary>
    /// Объявление всего файла с исходным кодом (набор объявлений топового уровня - ивенты, пользовательские функции, глобальные переменные)
    /// </summary>
    public class CompilationUnit
    {
        public CompilationUnit()
        {
            Variables = new Dictionary<string, Variable>();
            Events = new Dictionary<string, MethodDeclaration>();
        }

        public Dictionary<string, Variable> Variables { get; private set; }
        public Dictionary<string, MethodDeclaration> Events { get; private set; }
    }

    public class MethodDeclaration
    {
        /// <summary>
        /// Имя типа возвращаемого значения из ивента. Если это null, значит указан void тип
        /// </summary>
        public string ReturnTypeOrVoid { get; set; }

        public string Name { get; set; }

        // Является ли событие кастомным или определенным пользователем
        public bool IsCustom { get; set; }

        public List<MethodDeclarationParameter> Parameters { get; set; }

        public DoshikParser.BlockContext AntlrBody { get; set; }
    }

    public class MethodDeclarationParameter
    {
        public bool IsOutput { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class Variable
    {
        public bool IsPublic { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public DoshikParser.VariableInitializerContext AntlrInitializer { get; set; }
    }
}
