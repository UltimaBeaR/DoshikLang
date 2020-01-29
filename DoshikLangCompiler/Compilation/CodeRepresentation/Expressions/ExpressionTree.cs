using DoshikLangCompiler.Compilation.CodeRepresentation.Expressions.Nodes;
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

    // ToDo: это выражение должно быть временным (смотреть Note на почте) - оно находится в expression tree только до тех пор пока не произойдет дополнительная обработка
    // - после этого оно заменяется на getvariable expression (если это получение значения переменной) либо setvariable expression (если это присвоение нового значения переменной).
    // эта замена может быть произведена только путем анализа готового expression tree - именно по этому нужны такие временные элементы
    // один из кейсов - это getvariable в случае если оно находится в input для параметра вызова метода, но это setvariable если оно находится в
    // input-е assignment expression-а как левый операнд (то, ВО ЧТО идет присвоение)
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
