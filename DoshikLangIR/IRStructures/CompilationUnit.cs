using Doshik;
using System;
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

        public void AddConstant(Constant constant)
        {
            if (Constants.Find(x => Constant.Equals(x, constant)) == null)
                Constants.Add(constant);
        }
    }

    public class Constant
    {
        // Тип константы
        public DataType Type { get; set; }

        // Если установлено в true, значит значением является ключевое слово "this"
        public bool IsThis { get; set; }

        // В другом случае и если DotnetTypeString != null, значит значением ялвяется dotnet тип (при этом Type будет ссылаться на System.Type)
        public string DotnetTypeString { get; set; }

        // В другом случае значением является конкретное значение (object)
        public object DotnetValue { get; set; }

        public static Constant CreateAsDotnetTypeString(string dotnetTypeString, TypeLibrary typeLibrary)
        {
            return new Constant
            {
                Type = typeLibrary.FindByKnownType(KnownType.Type),
                IsThis = false,
                DotnetTypeString = dotnetTypeString,
                DotnetValue = null
            };
        }

        public static Constant CreateAsThis(DataType type)
        {
            return new Constant
            {
                Type = type,
                IsThis = true,
                DotnetTypeString = null,
                DotnetValue = null
            };
        }

        public static Constant CreateAsDotnetValue(DataType type, object dotnetValue)
        {
            return new Constant
            {
                Type = type,
                IsThis = false,
                DotnetTypeString = null,
                DotnetValue = dotnetValue
            };
        }

        public static bool Equals(Constant left, Constant right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException();

            if (left.Type != right.Type)
                return false;

            if (left.IsThis || right.IsThis)
                return left.IsThis == right.IsThis;

            if (left.DotnetTypeString != null || right.DotnetTypeString != null)
                return left.DotnetTypeString == right.DotnetTypeString;

            if (left.DotnetValue == null && right.DotnetValue == null)
                return true;

            if (left.DotnetValue == null || right.DotnetValue == null)
                return false;

            return left.DotnetValue.Equals(right.DotnetValue);
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
