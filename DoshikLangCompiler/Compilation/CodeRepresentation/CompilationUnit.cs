using System.Collections.Generic;

// В итоге компиляции должно получиться дерево из разных элементов где в корне будет CompilationUnit
// Нужно везде внедрить как минимум возможность иметь parent-а, чтобы можно было обходить дерево сверху вниз
// + должна быть возможность всегда обходить вниз но это уже можно делать тягая конкретные поля (например список параметров методов или тело метода)
// делать просто список child-ов смысла нет. Причем parent тоже будет не универсальный а всегда известного типа
// (для параметра это будет список параметров, для списка параметров - объявление метода и тд)

namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    // IVariableDeclarator, т.к. в этом месте объявляются переменные глобального уровня.
    // При этом не указывается конкретное место (типа конкретного statement-а), где была декларация.
    // считается что переменные все объявляются на уровне compilation unit-а и порядок их объявления не имеет значения
    // (в отличие от локальных переменных в теле методов)
    public class CompilationUnit : IScopeOwner, IVariableDeclarator
    {
        // Variable type = CompilationUnitVariable

        public Dictionary<string, MethodDeclaration> Events { get; } = new Dictionary<string, MethodDeclaration>();

        public Scope Scope { get; } = new Scope();
    }

    public class CompilationUnitVariable : Variable
    {
        public bool IsPublic { get; set; }
    }
}
