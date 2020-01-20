using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    /// <summary>
    /// Область видимости. Пока включает только переменные, но поидее тут могут быть также и методы (если появятся например вложенные методы)
    /// </summary>
    public class Scope
    {
        public Scope(IScopeOwner owner, Scope parentScope)
        {
            Owner = owner;
            ParentScope = parentScope;
        }

        public Scope ParentScope { get; private set; }

        public IScopeOwner Owner { get; private set; }

        public Dictionary<string, Variable> Variables { get; } = new Dictionary<string, Variable>();

        // ToDo: тут (или где-то отдельно) можно определить методы для поиска переменной по имени в текущем scope-е
        // - сначала искать среди своих Variables, затем подниматься на уровень выше и делать такой же поиск там

        public Variable FindVariableByName(string variableName)
        {
            if (Variables.TryGetValue(variableName, out var foundVariable))
                return foundVariable;

            if (ParentScope == null)
                return null;

            return ParentScope.FindVariableByName(variableName);
        }
    }

    /// <summary>
    /// Место в иерархии, в котором создается новый scope
    /// </summary>
    public interface IScopeOwner : ICodeHierarchyNode
    {
        /// <summary>
        /// Scope, который создает этот элемент иерархии при входе в него (указатель текущего scope перемещается на него).
        /// При выходе из этого элемента иерархии, scope перемещается на родительский. То есть Scope.ParentScope
        /// </summary>
        Scope Scope { get; }
    }
}
