using Antlr4.Runtime.Misc;
using System;

namespace DoshikLangCompiler
{
    public class TestVisitor: DoshikParserBaseVisitor<object>
    {
        public override object VisitAdditionExpression([NotNull] DoshikParser.AdditionExpressionContext context)
        {
            var leftResult = (float)Visit(context.expression()[0]);
            var rightResult = (float)Visit(context.expression()[1]);

            return leftResult + (context.bop.Type == DoshikParser.ADD ? 1 : -1) * rightResult;
        }

        public override object VisitMultiplicationExpression([NotNull] DoshikParser.MultiplicationExpressionContext context)
        {
            var leftResult = (float)Visit(context.expression()[0]);
            var rightResult = (float)Visit(context.expression()[1]);

            if (context.bop.Type == DoshikParser.DIV)
                return leftResult / rightResult;
            else if (context.bop.Type == DoshikParser.MUL)
                return leftResult * rightResult;
            else
                throw new NotImplementedException();
        }

        public override object VisitParenthesisExpression([NotNull] DoshikParser.ParenthesisExpressionContext context)
        {
            return (float)Visit(context.expression());
        }


        public override object VisitLiteralExpression([NotNull] DoshikParser.LiteralExpressionContext context)
        {
            return float.Parse(context.GetText(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
