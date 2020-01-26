using Antlr4.Runtime.Misc;
using DoshikLangCompiler.Compilation.CodeRepresentation;
using DoshikLangCompiler.Compilation.CodeRepresentation.Expressions;
using DoshikLangCompiler.Compilation.CodeRepresentation.Expressions.Nodes;
using System.Collections.Generic;

namespace DoshikLangCompiler.Compilation.Visitors
{
    public class ExpressionCreationVisitor : CompilationContextVisitorBase<object>
    {
        public ExpressionCreationVisitor(CompilationContext compilationContext)
            : base(compilationContext)
        {
            Sequence = new ExpressionNodeSequence();
        }

        public ExpressionNodeSequence Sequence { get; }

        public static ExpressionTree Apply(CompilationContext compilationContext, ICodeHierarchyNode expressionParent, DoshikParser.ExpressionContext antlrContext)
        {
            var visitor = new ExpressionCreationVisitor(compilationContext);
            visitor.Visit(antlrContext);

            return new ExpressionBuilder().Build(compilationContext, expressionParent, visitor.Sequence);
        }

        public override object VisitTypeDotExpression([NotNull] DoshikParser.TypeDotExpressionContext context)
        {
            VisitChildren(context);

            var node = new TypeDotExpressionNode(context);

            var foundType = GetTypeNameVisitor.Apply(_compilationContext, context.left);
            foundType.ThrowIfNotFound(_compilationContext);

            node.LeftType = foundType.DataType;

            if (context.rightIdentifier != null)
            {
                node.RightIdentifier = context.rightIdentifier.Text;
            }
            else
            {
                node.RightMethodCallData = new MethodCallExpressionNodeData();
                GetMethodCallData(context.rightMethodCall, node.RightMethodCallData);
            }

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitDotExpression([NotNull] DoshikParser.DotExpressionContext context)
        {
            VisitChildren(context);

            var node = new DotExpressionNode(context);

            node.Left = Sequence.FindExpressionByAntlrContext(context.left);

            if (context.rightIdentifier != null)
            {
                node.RightIdentifier = context.rightIdentifier.Text;
            }
            else
            {
                node.RightMethodCallData = new MethodCallExpressionNodeData();
                GetMethodCallData(context.rightMethodCall, node.RightMethodCallData);
            }

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitBracketsExpression([NotNull] DoshikParser.BracketsExpressionContext context)
        {
            VisitChildren(context);

            var node = new BracketsExpressionNode(context);

            node.Left = Sequence.FindExpressionByAntlrContext(context.left);
            node.Right = Sequence.FindExpressionByAntlrContext(context.right);

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitMethodCallExpression([NotNull] DoshikParser.MethodCallExpressionContext context)
        {
            VisitChildren(context);

            var node = new MethodCallExpressionNode(context);

            GetMethodCallData(context.methodCall(), node.MethodCallData);

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitNewCallExpression([NotNull] DoshikParser.NewCallExpressionContext context)
        {
            VisitChildren(context);

            var node = new NewCallExpressionNode(context);

            var newCallCtx = context.newCall();

            var foundType = GetTypeNameVisitor.Apply(_compilationContext, newCallCtx.typeType());
            foundType.ThrowIfNotFound(_compilationContext);
            node.Type = foundType.DataType;

            node.Parameters.AddRange(GetMethodCallParameters(newCallCtx.methodCallParams()));

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitTypecastExpression([NotNull] DoshikParser.TypecastExpressionContext context)
        {
            VisitChildren(context);

            var node = new TypecastExpressionNode(context);

            var foundType = GetTypeNameVisitor.Apply(_compilationContext, context.typeType());
            foundType.ThrowIfNotFound(_compilationContext);
            node.Type = foundType.DataType;

            node.Expression = Sequence.FindExpressionByAntlrContext(context.expression());

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitUnaryPostfixExpression([NotNull] DoshikParser.UnaryPostfixExpressionContext context)
        {
            VisitChildren(context);

            var node = new UnaryPostfixExpressionNode(context);

            node.Expression = Sequence.FindExpressionByAntlrContext(context.expression());

            if (context.postfix.Type == DoshikParser.INC)
                node.Postfix = UnaryPostfixExpressionNode.PostfixOption.Increment;
            else if (context.postfix.Type == DoshikParser.DEC)
                node.Postfix = UnaryPostfixExpressionNode.PostfixOption.Decrement;
            else
                throw new System.NotImplementedException();

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitUnaryPrefixExpression([NotNull] DoshikParser.UnaryPrefixExpressionContext context)
        {
            VisitChildren(context);

            var node = new UnaryPrefixExpressionNode(context);

            node.Expression = Sequence.FindExpressionByAntlrContext(context.expression());

            if (context.prefix.Type == DoshikParser.ADD)
                node.Prefix = UnaryPrefixExpressionNode.PrefixOption.Plus;
            else if (context.prefix.Type == DoshikParser.SUB)
                node.Prefix = UnaryPrefixExpressionNode.PrefixOption.Minus;
            else if (context.prefix.Type == DoshikParser.INC)
                node.Prefix = UnaryPrefixExpressionNode.PrefixOption.Increment;
            else if (context.prefix.Type == DoshikParser.DEC)
                node.Prefix = UnaryPrefixExpressionNode.PrefixOption.Decrement;
            else
                throw new System.NotImplementedException();

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitNotExpression([NotNull] DoshikParser.NotExpressionContext context)
        {
            VisitChildren(context);

            var node = new NotExpressionNode(context);

            node.Expression = Sequence.FindExpressionByAntlrContext(context.expression());

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitMultiplicationExpression([NotNull] DoshikParser.MultiplicationExpressionContext context)
        {
            VisitChildren(context);

            var node = new MultiplicationExpressionNode(context);

            node.Left = Sequence.FindExpressionByAntlrContext(context.left);
            node.Right = Sequence.FindExpressionByAntlrContext(context.right);

            if (context.@operator.Type == DoshikParser.MUL)
                node.Operator = MultiplicationExpressionNode.OperatorOption.Multiply;
            else if (context.@operator.Type == DoshikParser.DIV)
                node.Operator = MultiplicationExpressionNode.OperatorOption.Divide;
            else if (context.@operator.Type == DoshikParser.MOD)
                node.Operator = MultiplicationExpressionNode.OperatorOption.Mod;
            else
                throw new System.NotImplementedException();

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitAdditionExpression([NotNull] DoshikParser.AdditionExpressionContext context)
        {
            VisitChildren(context);

            var node = new AdditionExpressionNode(context);

            node.Left = Sequence.FindExpressionByAntlrContext(context.left);
            node.Right = Sequence.FindExpressionByAntlrContext(context.right);

            if (context.@operator.Type == DoshikParser.ADD)
                node.Operator = AdditionExpressionNode.OperatorOption.Plus;
            else if (context.@operator.Type == DoshikParser.SUB)
                node.Operator = AdditionExpressionNode.OperatorOption.Minus;
            else
                throw new System.NotImplementedException();

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitRelativeExpression([NotNull] DoshikParser.RelativeExpressionContext context)
        {
            VisitChildren(context);

            var node = new RelativeExpressionNode(context);

            node.Left = Sequence.FindExpressionByAntlrContext(context.left);
            node.Right = Sequence.FindExpressionByAntlrContext(context.right);

            if (context.@operator.Type == DoshikParser.LE)
                node.Operator = RelativeExpressionNode.OperatorOption.LesserOrEquals;
            else if (context.@operator.Type == DoshikParser.GE)
                node.Operator = RelativeExpressionNode.OperatorOption.GreaterOrEquals;
            if (context.@operator.Type == DoshikParser.LT)
                node.Operator = RelativeExpressionNode.OperatorOption.Lesser;
            else if (context.@operator.Type == DoshikParser.GT)
                node.Operator = RelativeExpressionNode.OperatorOption.Greater;
            else
                throw new System.NotImplementedException();

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitEqualsExpression([NotNull] DoshikParser.EqualsExpressionContext context)
        {
            VisitChildren(context);

            var node = new EqualsExpressionNode(context);

            node.Left = Sequence.FindExpressionByAntlrContext(context.left);
            node.Right = Sequence.FindExpressionByAntlrContext(context.right);

            if (context.@operator.Type == DoshikParser.EQUAL)
                node.Operator = EqualsExpressionNode.OperatorOption.Equals;
            else if (context.@operator.Type == DoshikParser.NOTEQUAL)
                node.Operator = EqualsExpressionNode.OperatorOption.NotEquals;
            else
                throw new System.NotImplementedException();

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitAndExpression([NotNull] DoshikParser.AndExpressionContext context)
        {
            VisitChildren(context);

            var node = new AndExpressionNode(context);

            node.Left = Sequence.FindExpressionByAntlrContext(context.left);
            node.Right = Sequence.FindExpressionByAntlrContext(context.right);

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitOrExpression([NotNull] DoshikParser.OrExpressionContext context)
        {
            VisitChildren(context);

            var node = new OrExpressionNode(context);

            node.Left = Sequence.FindExpressionByAntlrContext(context.left);
            node.Right = Sequence.FindExpressionByAntlrContext(context.right);

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitIfElseExpression([NotNull] DoshikParser.IfElseExpressionContext context)
        {
            VisitChildren(context);

            var node = new IfElseExpressionNode(context);

            node.Condition = Sequence.FindExpressionByAntlrContext(context.condition);
            node.TrueExpression = Sequence.FindExpressionByAntlrContext(context.trueExpression);
            node.ElseExpression = Sequence.FindExpressionByAntlrContext(context.elseExpression);

            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitAssignmentExpression([NotNull] DoshikParser.AssignmentExpressionContext context)
        {
            VisitChildren(context);

            var node = new AssignmentExpressionNode(context);
            Sequence.Sequence.Add(node);

            node.Left = Sequence.FindExpressionByAntlrContext(context.left);
            node.Right = Sequence.FindExpressionByAntlrContext(context.right);

            if (context.@operator.Type == DoshikParser.ASSIGN)
                node.Operator = AssignmentExpressionNode.OperatorOption.Assign;
            else if (context.@operator.Type == DoshikParser.ADD_ASSIGN)
                node.Operator = AssignmentExpressionNode.OperatorOption.PlusAssign;
            else if (context.@operator.Type == DoshikParser.SUB_ASSIGN)
                node.Operator = AssignmentExpressionNode.OperatorOption.MinusAssign;
            else if (context.@operator.Type == DoshikParser.MUL_ASSIGN)
                node.Operator = AssignmentExpressionNode.OperatorOption.MultiplyAssign;
            else if (context.@operator.Type == DoshikParser.DIV_ASSIGN)
                node.Operator = AssignmentExpressionNode.OperatorOption.DivideAssign;
            else if (context.@operator.Type == DoshikParser.MOD_ASSIGN)
                node.Operator = AssignmentExpressionNode.OperatorOption.ModAssign;
            else
                throw new System.NotImplementedException();

            return null;
        }

        public override object VisitParenthesisExpression([NotNull] DoshikParser.ParenthesisExpressionContext context)
        {
            VisitChildren(context);

            var node = new ParenthesisExpressionNode(context);
            Sequence.Sequence.Add(node);

            node.Expression = Sequence.FindExpressionByAntlrContext(context.expression());

            return null;
        }

        public override object VisitLiteralExpression([NotNull] DoshikParser.LiteralExpressionContext context)
        {
            var node = new LiteralExpressionNode(context);
            Sequence.Sequence.Add(node);

            var literalCtx = context.literal();
            var integerLiteralCtx = literalCtx.integerLiteral();

            if (integerLiteralCtx != null && integerLiteralCtx.INT_LITERAL() != null)
            {
                node.LiteralValue = integerLiteralCtx.INT_LITERAL().GetText();
                node.LiteralType = LiteralExpressionNode.LiteralTypeOption.Int;
            }
            else if (integerLiteralCtx != null && integerLiteralCtx.INT_HEX_LITERAL() != null)
            {
                node.LiteralValue = integerLiteralCtx.INT_HEX_LITERAL().GetText();
                node.LiteralType = LiteralExpressionNode.LiteralTypeOption.IntHex;
            }
            else if (literalCtx.FLOAT_LITERAL() != null)
            {
                node.LiteralValue = literalCtx.FLOAT_LITERAL().GetText();
                node.LiteralType = LiteralExpressionNode.LiteralTypeOption.Float;
            }
            else if (literalCtx.STRING_LITERAL() != null)
            {
                node.LiteralValue = literalCtx.STRING_LITERAL().GetText();
                node.LiteralType = LiteralExpressionNode.LiteralTypeOption.String;
            }
            else if (literalCtx.BOOL_LITERAL() != null)
            {
                node.LiteralValue = literalCtx.BOOL_LITERAL().GetText();
                node.LiteralType = LiteralExpressionNode.LiteralTypeOption.Bool;
            }
            else if (literalCtx.NULL_LITERAL() != null)
            {
                node.LiteralValue = literalCtx.NULL_LITERAL().GetText();
                node.LiteralType = LiteralExpressionNode.LiteralTypeOption.Null;
            }
            else
                throw new System.NotImplementedException();

            return null;
        }

        public override object VisitIdentifierExpression([NotNull] DoshikParser.IdentifierExpressionContext context)
        {
            var node = new IdentifierExpressionNode(context);

            node.Identifier = context.IDENTIFIER().GetText();

            Sequence.Sequence.Add(node);

            return null;
        }

        private void GetMethodCallData(DoshikParser.MethodCallContext methodCallCtx, MethodCallExpressionNodeData dataToPopulate)
        {
            dataToPopulate.Name = methodCallCtx.methodName.Text;

            dataToPopulate.TypeArguments.AddRange(GetTypeArguments(methodCallCtx.typeArguments()));

            dataToPopulate.Parameters.AddRange(GetMethodCallParameters(methodCallCtx.methodCallParams()));
        }

        private IEnumerable<DataType> GetTypeArguments(DoshikParser.TypeArgumentsContext typeArgumentsCtx)
        {
            if (typeArgumentsCtx == null)
                yield break;

            foreach (var typeArgumentCtx in typeArgumentsCtx.typeArgument())
            {
                var foundType = GetTypeNameVisitor.Apply(_compilationContext, typeArgumentCtx.typeType());
                foundType.ThrowIfNotFound(_compilationContext);
                yield return foundType.DataType;
            }
        }

        private IEnumerable<MethodCallParameterExpressionNodeData> GetMethodCallParameters(DoshikParser.MethodCallParamsContext paramsCtx)
        {
            if (paramsCtx == null)
                yield break;

            foreach (var paramCtx in paramsCtx.methodCallParam())
            {
                var parameter = new MethodCallParameterExpressionNodeData();

                parameter.IsOut = paramCtx.OUT() != null;
                parameter.Expression = Sequence.FindExpressionByAntlrContext(paramCtx.expression());

                yield return parameter;
            }
        }
    }
}
