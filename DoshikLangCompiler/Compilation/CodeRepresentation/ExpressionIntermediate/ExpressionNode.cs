using Antlr4.Runtime.Tree;
using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation.CodeRepresentation.ExpressionIntermediate
{
    public class ExpressionNodeSequence
    {
        // Выражения из данного дерева, но в виде линейного списка (там есть каждый из нодов), в том порядке, в котором они должны выполняться в итоге
        // В каждом выражении, в зависимости от типа, есть какие-то параметры, которые сами могут быть выражениями. Если идти слева направо по списку (то есть по порядку)
        // то получится что в кажом очередном выражении все операнды находятся слева от текущего в списке (то есть уже были пройдены = обработаны)
        public List<IExpressionNode> Sequence { get; } = new List<IExpressionNode>();

        public IExpressionNode FindExpressionByAntlrContext(IParseTree antlrContext)
        {
            if (antlrContext is DoshikParser.PrimaryExpressionContext primaryContext)
            {
                antlrContext = primaryContext.primary();
            }

            foreach (var node in Sequence)
            {
                if (node.AntlrContext == antlrContext)
                    return node;
            }

            return null;
        }
    }

    public interface IExpressionNode
    {
        IExpressionNode Parent { get; }

        IParseTree AntlrContext { get; }
    }

    public abstract class ExpressionNode<TAntlrContext> : IExpressionNode
        where TAntlrContext : IParseTree
    {
        public ExpressionNode(TAntlrContext antlrContext)
        {
            AntlrContext = antlrContext;
        }

        public TAntlrContext AntlrContext { get; set; }

        IParseTree IExpressionNode.AntlrContext => AntlrContext;

        public IExpressionNode Parent { get; set; }
    }

    public class DotExpressionNode : ExpressionNode<DoshikParser.DotExpressionContext>
    {
        public DotExpressionNode(DoshikParser.DotExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class BracketsExpressionNode : ExpressionNode<DoshikParser.BracketsExpressionContext>
    {
        public BracketsExpressionNode(DoshikParser.BracketsExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class MethodCallExpressionNode : ExpressionNode<DoshikParser.MethodCallExpressionContext>
    {
        public MethodCallExpressionNode(DoshikParser.MethodCallExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class NewCallExpressionNode : ExpressionNode<DoshikParser.NewCallExpressionContext>
    {
        public NewCallExpressionNode(DoshikParser.NewCallExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class TypecastExpressionNode : ExpressionNode<DoshikParser.TypecastExpressionContext>
    {
        public TypecastExpressionNode(DoshikParser.TypecastExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class UnaryPostfixExpressionNode : ExpressionNode<DoshikParser.UnaryPostfixExpressionContext>
    {
        public UnaryPostfixExpressionNode(DoshikParser.UnaryPostfixExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class UnaryPrefixExpressionNode : ExpressionNode<DoshikParser.UnaryPrefixExpressionContext>
    {
        public UnaryPrefixExpressionNode(DoshikParser.UnaryPrefixExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class NotExpressionNode : ExpressionNode<DoshikParser.NotExpressionContext>
    {
        public NotExpressionNode(DoshikParser.NotExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
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
    }

    public class EqualsExpressionNode : ExpressionNode<DoshikParser.EqualsExpressionContext>
    {
        public EqualsExpressionNode(DoshikParser.EqualsExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class AndExpressionNode : ExpressionNode<DoshikParser.AndExpressionContext>
    {
        public AndExpressionNode(DoshikParser.AndExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class OrExpressionNode : ExpressionNode<DoshikParser.OrExpressionContext>
    {
        public OrExpressionNode(DoshikParser.OrExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class IfElseExpressionNode : ExpressionNode<DoshikParser.IfElseExpressionContext>
    {
        public IfElseExpressionNode(DoshikParser.IfElseExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class AssignmentExpressionNode : ExpressionNode<DoshikParser.AssignmentExpressionContext>
    {
        public AssignmentExpressionNode(DoshikParser.AssignmentExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class ParenthesisExpressionNode : ExpressionNode<DoshikParser.ParenthesisExpressionContext>
    {
        public ParenthesisExpressionNode(DoshikParser.ParenthesisExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class LiteralExpressionNode : ExpressionNode<DoshikParser.LiteralExpressionContext>
    {
        public LiteralExpressionNode(DoshikParser.LiteralExpressionContext antlrContext)
            : base(antlrContext)
        {
        }
    }

    public class IdentifierExpressionNode : ExpressionNode<DoshikParser.IdentifierExpressionContext>
    {
        public IdentifierExpressionNode(DoshikParser.IdentifierExpressionContext antlrContext)
            : base(antlrContext)
        {
        }

        public string Identifier { get; set; }
    }
}
