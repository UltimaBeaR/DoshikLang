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
            Variables = new List<Variable>();
            Events = new List<EventDeclaration>();
        }

        public List<Variable> Variables { get; private set; }
        public List<EventDeclaration> Events { get; private set; }
    }

    public class EventDeclaration
    {

    }

    public class Variable
    {
        public bool IsPublic { get; set; }

        public string Type { get; set; }
    }
}
