using System.Collections.Generic;

namespace DoshikLangIR
{
    public class TypeDotExpressionNode : ExpressionNode<DoshikParser.TypeDotExpressionContext>
    {
        public TypeDotExpressionNode(DoshikParser.TypeDotExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public DataType LeftType { get; set; }

        // Right - либо идентификатор, либо вызов метода (одно из двух есть, другое null)

        public string RightIdentifier { get; set; }
        public MethodCallExpressionNodeData RightMethodCallData { get; set; }
    }

    public class DotExpressionNode : ExpressionNode<DoshikParser.DotExpressionContext>
    {
        public DotExpressionNode(DoshikParser.DotExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        // Несмотря на то что есть TypeDotExpressionNode, тут Left тоже может быть именем типа, в случае если это просто Identifier
        // То есть сначала надо попробовать зарезолвить его по имени типа, а если такого типа не нашлось, то уже по имени переменной
        public IExpressionNode Left { get; set; }

        // Right - либо идентификатор, либо вызов метода (одно из двух есть, другое null)

        public string RightIdentifier { get; set; }
        public MethodCallExpressionNodeData RightMethodCallData { get; set; }
    }

    public class BracketsExpressionNode : ExpressionNode<DoshikParser.BracketsExpressionContext>
    {
        public BracketsExpressionNode(DoshikParser.BracketsExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Left { get; set; }
        public IExpressionNode Right { get; set; }
    }

    public class DefaultOfTypeExpressionNode : ExpressionNode<DoshikParser.DefaultOfTypeExpressionContext>
    {
        public DefaultOfTypeExpressionNode(DoshikParser.DefaultOfTypeExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public DataType Type { get; set; }
    }

    public class TypeOfExpressionNode : ExpressionNode<DoshikParser.TypeOfExpressionContext>
    {
        public TypeOfExpressionNode(DoshikParser.TypeOfExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public DataType Type { get; set; }
    }

    public class MethodCallExpressionNode : ExpressionNode<DoshikParser.MethodCallExpressionContext>
    {
        public MethodCallExpressionNode(DoshikParser.MethodCallExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public MethodCallExpressionNodeData MethodCallData { get; } = new MethodCallExpressionNodeData();
    }

    public class NewCallExpressionNode : ExpressionNode<DoshikParser.NewCallExpressionContext>
    {
        public NewCallExpressionNode(DoshikParser.NewCallExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public DataType Type { get; set; }

        public List<MethodCallParameterExpressionNodeData> Parameters { get; } = new List<MethodCallParameterExpressionNodeData>();
    }

    public class TypecastExpressionNode : ExpressionNode<DoshikParser.TypecastExpressionContext>
    {
        public TypecastExpressionNode(DoshikParser.TypecastExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public DataType Type { get; set; }

        public IExpressionNode Expression { get; set; }
    }

    public class UnaryPostfixExpressionNode : ExpressionNode<DoshikParser.UnaryPostfixExpressionContext>
    {
        public UnaryPostfixExpressionNode(DoshikParser.UnaryPostfixExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Expression { get; set; }

        public PostfixOption Postfix { get; set; }

        public enum PostfixOption
        {
            /// <summary>
            /// expression++
            /// </summary>
            Increment,

            /// <summary>
            /// expression--
            /// </summary>
            Decrement
        }
    }

    public class UnaryPrefixExpressionNode : ExpressionNode<DoshikParser.UnaryPrefixExpressionContext>
    {
        public UnaryPrefixExpressionNode(DoshikParser.UnaryPrefixExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Expression { get; set; }

        public PrefixOption Prefix { get; set; }

        public enum PrefixOption
        {
            /// <summary>
            /// +expression
            /// </summary>
            Plus,

            /// <summary>
            /// -expression
            /// </summary>
            Minus,

            /// <summary>
            /// ++expression
            /// </summary>
            Increment,

            /// <summary>
            /// --expression
            /// </summary>
            Decrement
        }
    }

    public class NotExpressionNode : ExpressionNode<DoshikParser.NotExpressionContext>
    {
        public NotExpressionNode(DoshikParser.NotExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Expression { get; set; }
    }

    public class MultiplicationExpressionNode : ExpressionNode<DoshikParser.MultiplicationExpressionContext>
    {
        public MultiplicationExpressionNode(DoshikParser.MultiplicationExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Left { get; set; }
        public IExpressionNode Right { get; set; }

        public OperatorOption Operator { get; set; }

        public enum OperatorOption
        {
            /// <summary>
            /// left * right
            /// </summary>
            Multiply,

            /// <summary>
            /// left / right
            /// </summary>
            Divide,

            /// <summary>
            /// left % right
            /// </summary>
            Mod
        }
    }

    public class AdditionExpressionNode : ExpressionNode<DoshikParser.AdditionExpressionContext>
    {
        public AdditionExpressionNode(DoshikParser.AdditionExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Left { get; set; }
        public IExpressionNode Right { get; set; }

        public OperatorOption Operator { get; set; }

        public enum OperatorOption
        {
            /// <summary>
            /// left + right
            /// </summary>
            Plus,

            /// <summary>
            /// left - right
            /// </summary>
            Minus
        }
    }

    public class RelativeExpressionNode : ExpressionNode<DoshikParser.RelativeExpressionContext>
    {
        public RelativeExpressionNode(DoshikParser.RelativeExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Left { get; set; }
        public IExpressionNode Right { get; set; }

        public OperatorOption Operator { get; set; }

        public enum OperatorOption
        {
            /// <summary>
            /// left LesserOrEquals right
            /// </summary>
            LesserOrEquals,

            /// <summary>
            /// left GreaterOrEquals right
            /// </summary>
            GreaterOrEquals,

            /// <summary>
            /// left Lesser right
            /// </summary>
            Lesser,

            /// <summary>
            /// left Greater right
            /// </summary>
            Greater
        }
    }

    public class EqualsExpressionNode : ExpressionNode<DoshikParser.EqualsExpressionContext>
    {
        public EqualsExpressionNode(DoshikParser.EqualsExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Left { get; set; }
        public IExpressionNode Right { get; set; }

        public OperatorOption Operator { get; set; }

        public enum OperatorOption
        {
            /// <summary>
            /// left == right
            /// </summary>
            Equals,

            /// <summary>
            /// left != right
            /// </summary>
            NotEquals
        }
    }

    public class AndExpressionNode : ExpressionNode<DoshikParser.AndExpressionContext>
    {
        public AndExpressionNode(DoshikParser.AndExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Left { get; set; }
        public IExpressionNode Right { get; set; }
    }

    public class OrExpressionNode : ExpressionNode<DoshikParser.OrExpressionContext>
    {
        public OrExpressionNode(DoshikParser.OrExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Left { get; set; }
        public IExpressionNode Right { get; set; }
    }

    public class IfElseExpressionNode : ExpressionNode<DoshikParser.IfElseExpressionContext>
    {
        public IfElseExpressionNode(DoshikParser.IfElseExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        // ToDo: оно парсится как будто это вызов функции IfElse(condition, trueExpression, falseExpression)
        // то есть сначала обрабатывает все 3 операнда а потом сам ifelse. По сути надо тут flow менять, то
        // есть не порядок в выражении должен решать когда что выполнять а тут должен порядок определяться так же как в statement-е, то есть операции jump-а по условию
        // condition-а. То есть единственное что должно тут обрабатыватья как expression - это condition и сама операция ifelse.
        // экспрешены true / false должны быть вычеслены независимо (код для них должен быть сгенерирован независимо) внутри веток if/else (то есть секций разделенных джампами в коде).

        public IExpressionNode Condition { get; set; }


        // ToDo: видимо надо заменить эти вещи на Antlr context для под-деревьев выражений и их вычислять уже отдельно.

        public IExpressionNode TrueExpression { get; set; }
        public IExpressionNode ElseExpression { get; set; }
    }

    public class AssignmentExpressionNode : ExpressionNode<DoshikParser.AssignmentExpressionContext>
    {
        public AssignmentExpressionNode(DoshikParser.AssignmentExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Left { get; set; }
        public IExpressionNode Right { get; set; }

        public OperatorOption Operator { get; set; }

        public enum OperatorOption
        {
            /// <summary>
            /// left = right
            /// </summary>
            Assign,

            /// <summary>
            /// left += right
            /// </summary>
            PlusAssign,

            /// <summary>
            /// left -= right
            /// </summary>
            MinusAssign,

            /// <summary>
            /// left *= right
            /// </summary>
            MultiplyAssign,

            /// <summary>
            /// left /= right
            /// </summary>
            DivideAssign,

            /// <summary>
            /// left %= right
            /// </summary>
            ModAssign
        }
    }

    public class ParenthesisExpressionNode : ExpressionNode<DoshikParser.ParenthesisExpressionContext>
    {
        public ParenthesisExpressionNode(DoshikParser.ParenthesisExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public IExpressionNode Expression { get; set; }
    }

    public class LiteralExpressionNode : ExpressionNode<DoshikParser.LiteralExpressionContext>
    {
        public LiteralExpressionNode(DoshikParser.LiteralExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public string LiteralValue { get; set; }

        public LiteralTypeOption LiteralType { get; set; }

        public enum LiteralTypeOption
        {
            Int,
            IntHex,
            Float,
            String,
            Bool,
            Null
        }
    }

    public class IdentifierExpressionNode : ExpressionNode<DoshikParser.IdentifierExpressionContext>
    {
        public IdentifierExpressionNode(DoshikParser.IdentifierExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        /// <summary>
        /// Может обозначать как имя переменной так и часть имени через точку (и возможно еще что-то)
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// true, если этот идентификатор упоминается как левая часть выражения с точкой
        /// (устанавливается при обработке выражения с точкой, то есть отложенно)
        /// </summary>
        public bool IsLeftOfDotExpression { get; set; }
    }
}
