using Doshik;
using System.Collections.Generic;

// В итоге компиляции должно получиться дерево из разных элементов где в корне будет CompilationUnit
// Нужно везде внедрить как минимум возможность иметь parent-а, чтобы можно было обходить дерево сверху вниз
// + должна быть возможность всегда обходить вниз но это уже можно делать тягая конкретные поля (например список параметров методов или тело метода)
// делать просто список child-ов смысла нет. Причем parent тоже будет не универсальный а всегда известного типа
// (для параметра это будет список параметров, для списка параметров - объявление метода и тд)

namespace DoshikLangIR
{
    // IVariableDeclarator, т.к. в этом месте объявляются переменные глобального уровня.
    // При этом не указывается конкретное место (типа конкретного statement-а), где была декларация.
    // считается что переменные все объявляются на уровне compilation unit-а и порядок их объявления не имеет значения
    // (в отличие от локальных переменных в теле методов)
    public class CompilationUnit : ICodeHierarchyNode, IScopeOwner, IVariableDeclarator
    {
        // Variable type = CompilationUnitVariable

        public CompilationUnit(DoshikExternalApi externalApi)
        {
            // parent scope = null, так как это рут в иерархии scope-ов
            Scope = new Scope(this, null);

            ExternalApi = externalApi;
        }

        ICodeHierarchyNode ICodeHierarchyNode.Parent => null;
        public Scope Scope { get; private set; }

        public DoshikExternalApi ExternalApi { get; private set; }

        public Dictionary<string, EventDeclaration> Events { get; } = new Dictionary<string, EventDeclaration>();

        public List<Constant> Constants { get; } = new List<Constant>();

        public void AddConstant(DataType type, object dotnetValue, bool isThis = false)
        {
            if (Constants.Find(x => x.Equals(type, dotnetValue, isThis)) == null)
            {
                Constants.Add(
                    new Constant
                    {
                        Type = type,
                        DotnetValue = isThis ? null : dotnetValue,
                        IsThis = isThis
                    }
                );
            }
        }
    }

    public class Constant
    {
        public DataType Type { get; set; }
        public object DotnetValue { get; set; }
        public bool IsThis { get; set; }

        public bool Equals(DataType type, object dotnetValue, bool isThis)
        {
            if (Type != type)
                return false;

            if (IsThis && isThis)
                return true;

            if (DotnetValue == null && dotnetValue == null)
                return true;

            if (DotnetValue == null || dotnetValue == null)
                return false;

            return DotnetValue.Equals(dotnetValue);
        }
    }

    public class CompilationUnitVariable : Variable
    {
        public CompilationUnitVariable(IVariableDeclarator declarator)
            : base(declarator)
        {
        }

        public bool IsPublic { get; set; }
    }
}
