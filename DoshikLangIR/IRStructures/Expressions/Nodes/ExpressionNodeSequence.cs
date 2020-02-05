using Antlr4.Runtime.Tree;
using System.Collections.Generic;

namespace DoshikLangIR
{
    // Ноды это просто результат обработки expression-ов antlr парсером без особой обработки, но в более удобном для работы виде и представленный в виде последовательности операций для
    // выполенения expression-а, а не в виде дерева

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

    public class MethodCallExpressionNodeData
    {
        public string Name { get; set; }

        /// <summary>
        /// generic type arguments. Указываются названия типов
        /// </summary>
        public List<DataType> TypeArguments { get; } = new List<DataType>();

        public List<MethodCallParameterExpressionNodeData> Parameters { get; } = new List<MethodCallParameterExpressionNodeData>();
    }

    public class MethodCallParameterExpressionNodeData
    {
        public bool IsOut { get; set; }

        /// <summary>
        /// Может содержать любое выражение, но если стоит IsOut, то нужно будет проверить что в этом случае Expression может быть только идентификатором с названием уже объявленной переменной
        /// </summary>
        public IExpressionNode Expression { get; set; }
    }
}
