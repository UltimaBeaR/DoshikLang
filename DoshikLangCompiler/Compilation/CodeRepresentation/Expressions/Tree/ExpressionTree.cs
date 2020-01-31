using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation.CodeRepresentation.Expressions.Tree
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
        ExpressionSlot ReturnOutputSlot { get; set; }
        List<ExpressionSlot> AdditionalOutputSlots { get; }
    }

    public abstract class ExpressionBase : IExpression
    {
        /// <summary>
        /// Входные слоты - соответствуют входным параметрам метода либо операндам оператора.
        /// </summary>
        public List<ExpressionSlot> InputSlots { get; } = new List<ExpressionSlot>();

        /// <summary>
        /// Единственный required слот с возвращаемым значением. Может иметь тип void (в этом случае в нем гарантированно не будет input-ов)
        /// </summary>
        public ExpressionSlot ReturnOutputSlot { get; set; }

        /// <summary>
        /// Слоты, которые могут понадобится если у вызова метода, соответствующего этому expression-у будут out параметры
        /// </summary>
        public List<ExpressionSlot> AdditionalOutputSlots { get; } = new List<ExpressionSlot>();
    }

    /// <summary>
    /// Связь между двумя выражениями. У одного из выражений эта связь идет во входное значение, а у другого в выходное
    /// Связь не может существовать сама по себе. Всегда должны быть 2 стороны - входная и выходная
    /// </summary>
    public class ExpressionSlot
    {
        // ToDo: сделать метод SetInput / SetOutput, которые будут прописывать в слот заданный expression плюс у expression-а прописывать себя как слот и удалять старое соединение из expression-а

        public ExpressionSlot(DataType type, IExpression outputSideExpression)
        {
            Type = type;
            OutputSideExpression = outputSideExpression;
        }

        /// <summary>
        /// Тип данных (из udon api)
        /// </summary>
        public DataType Type { get; private set; }

        /// <summary>
        /// Выражение, к которому этот слот направлен на входное значение (то есть выражение справа от слота)
        /// </summary>
        public IExpression InputSideExpression { get; set; }

        /// <summary>
        /// Выражение, к которому этот слот направлен на выходное значение (то есть выражение слева от слота)
        /// </summary>
        public IExpression OutputSideExpression { get; set; }
    }
}
