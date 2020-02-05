using System.Collections.Generic;

namespace DoshikLangIR
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

        /// <summary>
        /// thisScopeOnly - значит искать только в текущем scope, без учета родительских scope
        /// </summary>
        /// <returns></returns>
        public Variable FindVariableByName(string variableName, bool thisScopeOnly = false)
        {
            if (Variables.TryGetValue(variableName, out var foundVariable))
                return foundVariable;

            if (thisScopeOnly)
                return null;

            if (ParentScope == null)
                return null;

            return ParentScope.FindVariableByName(variableName, false);
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
