using Antlr4.Runtime.Misc;
using DoshikLangCompiler.Compilation.CodeRepresentation;
using DoshikLangCompiler.Compilation.CodeRepresentation.ExpressionIntermediate;

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

        public static Expression Apply(CompilationContext compilationContext, ICodeHierarchyNode expressionParent, DoshikParser.ExpressionContext antlrContext)
        {
            var visitor = new ExpressionCreationVisitor(compilationContext);

            visitor.Visit(antlrContext);

            var sequence = visitor.Sequence;

            // ToDo: тут надо преобразовать sequence в Expression объект. Обработать sequence максимально возможным образом и сделать на его
            // основе. Определить где референсы к переменным и были ли они объявлены (валидация). сейчас там идентификаторы и в рамках одной ноды нельзя сказать
            // что это - переменная или часть qualified name через точку. Для этого как раз и нужно иметь полностью дерево чтобы его анализировать.
            // надо определить вызовы методов, длинные вызовы через точку. это могут быть как неймспейсные вызовы так и переходы по полям структуры.
            // на данный момент ни то ни другое не поддерживается, но элементы такие выделить надо (просто далее при обработке если такие будут найдены то будет синтаксическая ошибка)
            // кстати щас уже скоро скорее всего будут имена вида UnityEngine.Debug.Log() то есть поддержка неймспейсов, так что это будет актуально.
            // Еще на этом же этапе нужно опредеять где какие методы вызвались (нужны ноды вызова методов с возможным полем instance, либо static type, и т.д.)
            // операторы конструкторов, операторы сложения, сравнения и т.д. - все это заменяется на такие вот ноды с вызовом методов соотвествующих перегрузок операторов.
            // и тут же нужен механизм определения правильной перегрузки на основе типа входных переменных (эти данные поидее все должны быть) и данных в АПИ.
            //
            // В итоге имея такой граф (или последовательность?) из по сути почти полностью набора нод - вызовов методов, будет просто построить итоговый assembly код, там
            // будут чередоваться копирования, и вызовы разных методов (еще там временные переменные и т.д.,)

            // ToDo: еще в самом sequence надо определить все ноды и все их параметры (сделать код создания всех нод: щас есть только identifier, addition, multiply)
            // и пройтись по иерархии рекурсивно начиная с последнего элемента последовательности (он же корень дерева) и проставить всем внутренним нодам родителя

            return new Expression(expressionParent);
        }

        public override object VisitDotExpression([NotNull] DoshikParser.DotExpressionContext context)
        {
            VisitChildren(context);

            var node = new DotExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitBracketsExpression([NotNull] DoshikParser.BracketsExpressionContext context)
        {
            VisitChildren(context);

            var node = new BracketsExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitMethodCallExpression([NotNull] DoshikParser.MethodCallExpressionContext context)
        {
            VisitChildren(context);

            var node = new MethodCallExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitNewCallExpression([NotNull] DoshikParser.NewCallExpressionContext context)
        {
            VisitChildren(context);

            var node = new NewCallExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitTypecastExpression([NotNull] DoshikParser.TypecastExpressionContext context)
        {
            VisitChildren(context);

            var node = new TypecastExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitUnaryPostfixExpression([NotNull] DoshikParser.UnaryPostfixExpressionContext context)
        {
            VisitChildren(context);

            var node = new UnaryPostfixExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitUnaryPrefixExpression([NotNull] DoshikParser.UnaryPrefixExpressionContext context)
        {
            VisitChildren(context);

            var node = new UnaryPrefixExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitNotExpression([NotNull] DoshikParser.NotExpressionContext context)
        {
            VisitChildren(context);

            var node = new NotExpressionNode(context);
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
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitEqualsExpression([NotNull] DoshikParser.EqualsExpressionContext context)
        {
            VisitChildren(context);

            var node = new EqualsExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitAndExpression([NotNull] DoshikParser.AndExpressionContext context)
        {
            VisitChildren(context);

            var node = new AndExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitOrExpression([NotNull] DoshikParser.OrExpressionContext context)
        {
            VisitChildren(context);

            var node = new OrExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitIfElseExpression([NotNull] DoshikParser.IfElseExpressionContext context)
        {
            VisitChildren(context);

            var node = new IfElseExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitAssignmentExpression([NotNull] DoshikParser.AssignmentExpressionContext context)
        {
            VisitChildren(context);

            var node = new AssignmentExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitParenthesisExpression([NotNull] DoshikParser.ParenthesisExpressionContext context)
        {
            VisitChildren(context);

            var node = new ParenthesisExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitLiteralExpression([NotNull] DoshikParser.LiteralExpressionContext context)
        {
            var node = new LiteralExpressionNode(context);
            Sequence.Sequence.Add(node);

            return null;
        }

        public override object VisitIdentifierExpression([NotNull] DoshikParser.IdentifierExpressionContext context)
        {
            var node = new IdentifierExpressionNode(context);

            node.Identifier = context.IDENTIFIER().GetText();

            Sequence.Sequence.Add(node);

            return null;
        }
    }
}
