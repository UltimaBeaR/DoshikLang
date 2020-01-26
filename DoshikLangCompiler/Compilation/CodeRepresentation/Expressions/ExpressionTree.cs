using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation.CodeRepresentation.Expressions
{
    public class ExpressionTree : ICodeHierarchyNode
    {
        public ExpressionTree(ICodeHierarchyNode parent)
        {
            Parent = parent;
        }

        public ICodeHierarchyNode Parent { get; }

        public IExpression RootExpression { get; set; }
    }

    public interface IExpression
    {
        List<ExpressionSlot> InputSlots { get; }
        List<ExpressionSlot> OutputSlots { get; }
    }

    public abstract class ExpressionBase : IExpression
    {
        public List<ExpressionSlot> InputSlots { get; } = new List<ExpressionSlot>();
        public List<ExpressionSlot> OutputSlots { get; } = new List<ExpressionSlot>();
    }

    /// <summary>
    /// Связь между двумя выражениями. У одного из выражений эта связь идет во входное значение, а у другого в выходное
    /// Связь не может существовать сама по себе. Всегда должны быть 2 стороны - входная и выходная
    /// </summary>
    public class ExpressionSlot
    {
        /// <summary>
        /// Тип данных (из udon api)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Выражение, к которому этот слот направлен на входное значение (то есть выражение справа от слота)
        /// </summary>
        public IExpression InputSideExpression { get; set; }

        /// <summary>
        /// Выражение, к которому этот слот направлен на выходное значение (то есть выражение слева от слота)
        /// </summary>
        public IExpression OutputSideExpression { get; set; }
    }

    /// <summary>
    /// В левое выражение (если оно валидно для присваивания, например если это референс на переменную или индексатор массива или референс на проперти и т.д.)
    /// присваивает правое. Результат текущего выражения всегда void, то есть оно не имеет выходных слотов.
    /// </summary>
    public class AssignmentExpression : ExpressionBase
    {
        public ExpressionSlot Left { get; set; }
        public ExpressionSlot Right { get; set; }
    }

    /// <summary>
    /// Получает уже объявленную переменную и возвращает ее значение в выходном слоте
    /// </summary>
    public class VariableReferenceExpression : ExpressionBase
    {
        public Variable Variable { get; set; }
    }

    /// <summary>
    /// Объявлет константу и возвращает ее значение в выходном слоте
    /// По сути это упоминание литерала определенного типа в коде (int, float, string и т.д.)
    /// </summary>
    public class ConstantValueExpression : ExpressionBase
    {
        /// <summary>
        /// Тип из udon api
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// Значение в виде dotnet типа (скорее всего тут будут только примитивные типы, например int, string, float, int64 и тд)
        /// </summary>
        public object DotnetValue { get; set; }
    }

    /// <summary>
    /// Вызов метода. 
    /// </summary>
    public class MethodCallExpression : ExpressionBase
    {
        /// <summary>
        /// Выражение, результат которого передается как входной слот "instance" (он же this)
        /// и в итоговом assembly коде будет передаваться как первый входной параметр для вызова метода.
        /// </summary>
        public ExpressionSlot Instance { get; set; }
    }

    public class StaticMethodCallExpression : ExpressionBase
    {
        /// <summary>
        /// Тип (из udon api), к которому применяется вызов метода
        /// </summary>
        public string Type { get; set; }
    }

    public class IfElseExpression : ExpressionBase
    {
        public ExpressionSlot Condition { get; set; }
        public ExpressionSlot IfBody { get; set; }
        public ExpressionSlot ElseBody { get; set; }
    }
}
