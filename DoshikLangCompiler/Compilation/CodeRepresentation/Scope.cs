using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    /// <summary>
    /// Область видимости. Пока включает только переменные, но поидее тут могут быть также и методы (если появятся например вложенные методы)
    /// </summary>
    public class Scope
    {
        public Scope ParentScope { get; set; }

        public IScopeOwner Owner { get; set; }

        public Dictionary<string, Variable> Variables { get; } = new Dictionary<string, Variable>();

        // ToDo: тут (или где-то отдельно) можно определить методы для поиска переменной по имени в текущем scope-е
        // - сначала искать среди своих Variables, затем подниматься на уровень выше и делать такой же поиск там
    }

    /// <summary>
    /// Место в иерархии, в котором создается новый scope
    /// </summary>
    public interface IScopeOwner
    {
        Scope Scope { get; }
    }
}
