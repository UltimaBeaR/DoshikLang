using Doshik;

namespace DoshikLangIR
{
    public class GetVariableExpression : ExpressionBase
    {
        /// <summary>
        /// Переменная. Ассоциирована с выходным слотом
        /// </summary>
        public Variable Variable { get; set; }
    }

    public class SetVariableExpression : ExpressionBase
    {
        /// <summary>
        /// Переменная. Ассоциирована с выходным слотом
        /// </summary>
        public Variable Variable { get; set; }

        /// <summary>
        /// Выражение, результат которого присваивается в значение переменной
        /// </summary>
        public ExpressionSlot Expression { get; set; }
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
        public DataType ValueType { get; set; }

        /// <summary>
        /// Значение в виде dotnet типа (скорее всего тут будут только примитивные типы, например int, string, float, int64 и тд)
        /// </summary>
        public object DotnetValue { get; set; }

        /// <summary>
        /// Если true, значит DotnetValue игнорируется и вместо него используется значение "this"
        /// </summary>
        public bool IsThis { get; set; }
    }

    /// <summary>
    /// Вызов метода. 
    /// </summary>
    public class InstanceMethodCallExpression : ExpressionBase, IMethodCallExpression
    {
        /// <summary>
        /// Выражение, результат которого передается как входной слот "instance" (он же this)
        /// и в итоговом assembly коде будет передаваться как первый входной параметр для вызова метода.
        /// </summary>
        public ExpressionSlot Instance { get; set; }

        public DoshikExternalApiTypeMethodOverload MethodOverload { get; set; }
    }

    public class StaticMethodCallExpression : ExpressionBase, IMethodCallExpression
    {
        /// <summary>
        /// Тип (из udon api), к которому применяется вызов метода
        /// </summary>
        public DataType Type { get; set; }

        /// <summary>
        /// Найденная перегрузка метода в АПИ (если потом появятся кастомные методы, то тут должно быть что-то НЕ ТОЛЬКО из апи (нужно будет завернуть в какую-нибудь абстракцию)
        /// </summary>
        public DoshikExternalApiTypeMethodOverload MethodOverload { get; set; }
    }

    public class IfElseExpression : ExpressionBase
    {
        public ExpressionSlot Condition { get; set; }
        public ExpressionSlot IfBody { get; set; }
        public ExpressionSlot ElseBody { get; set; }
    }

    public class TypecastExpression : ExpressionBase
    {
        /// <summary>
        /// Тип, в который осуществляется преобразование
        /// </summary>
        public DataType Type { get; set; }

        /// <summary>
        /// Выражение, которое преобразуется в заданный тип
        /// </summary>
        public ExpressionSlot Expression { get; set; }
    }
}
