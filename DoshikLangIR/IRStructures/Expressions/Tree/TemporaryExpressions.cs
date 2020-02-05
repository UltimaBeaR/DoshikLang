namespace DoshikLangIR
{
    /// <summary>
    /// В левое выражение (если оно валидно для присваивания, например если это референс на переменную или индексатор массива или референс на проперти и т.д.)
    /// присваивает правое. Результат текущего выражения всегда void, то есть оно не имеет выходных слотов.
    /// </summary>
    public class AssignmentExpression : ExpressionBase
    {
        public ExpressionSlot Left { get; set; }
        public ExpressionSlot Right { get; set; }

        public AssignmentExpressionNode.OperatorOption Operator { get; set; }
    }

    /// <summary>
    /// Выражение, полученное из идентификатора, хранящее в себе тип данных. Не референсится в финальном expression tree
    /// </summary>
    public class TypeHolderDummyExpression : ExpressionBase
    {
        public DataType Type { get; set; }
    }

    /// <summary>
    /// Получает уже объявленную переменную и возвращает ее значение в выходном слоте
    /// </summary>
    public class VariableReferenceExpression : ExpressionBase
    {
        /// <summary>
        /// Переменная. Ассоциирована с выходным слотом
        /// </summary>
        public Variable Variable { get; set; }
    }
}
