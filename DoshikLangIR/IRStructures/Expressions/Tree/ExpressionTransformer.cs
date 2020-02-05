using System.Linq;

namespace DoshikLangIR
{
    /// <summary>
    /// Трансформирует ноды в ExpressionTree, упрощая их. Некоторые типы нод убираются совсем и заменяются на другие типы нод
    /// </summary>
    public class ExpressionTransformer
    {
        public void Transform(CompilationContext compilationContext, ExpressionTree expressionTree)
        {
            _compilationContext = compilationContext;
            _expressionTree = expressionTree;

            // Трансформируем присвоения
            TransformAssignments();

            // После того как все присвоения были трансформированы, больше не остается ссылок на переменные в контексте установки значения,
            // соответственно оставшиеся ссылки на переменные можно преобразовать в получение значения
            TransformVariableReferencesToGetVariable();
        }

        private void TransformAssignments()
        {
            // Преобразование операторов присвоения

            // Операции присвоения заменяются на:
            // 1) в случае присвоения в переменную - замена на SetVariableExpression
            // 2) в случае присвоения в индексатор массива(или другой) - вызов метода установки значения в индексатор = .Set()
            // 3) в случае присвоения в property - вызов метода = .set_PropertyName()

            // в случае если операция присвоения также содержит дополнительную операцию (например +=, *= и тд), то дополнительно добавляется соответствующий вызов метода перегрузки оператора

            while (true)
            {
                var assignmentExpression = _expressionTree.RootExpression.TraverseSubtree()
                    .OfType<AssignmentExpression>()
                    .FirstOrDefault();

                if (assignmentExpression == null)
                    break;

                TransformAssignment(assignmentExpression);
            }
        }

        private void TransformAssignment(AssignmentExpression assignmentExpression)
        {
            if (assignmentExpression.Operator != AssignmentExpressionNode.OperatorOption.Assign)
                throw _compilationContext.ThrowCompilationError("complex assignment operators are not supported yet");

            var leftSideExpression = assignmentExpression.Left.OutputSideExpression;
            var rightSideExpression = assignmentExpression.Right.OutputSideExpression;

            if (leftSideExpression is VariableReferenceExpression variableReferenceExpression)
            {
                var setVariableExpression = new SetVariableExpression();
                setVariableExpression.Variable = variableReferenceExpression.Variable;

                setVariableExpression.Expression = rightSideExpression.ReturnOutputSlot;
                setVariableExpression.InputSlots.Add(setVariableExpression.Expression);
                rightSideExpression.ReturnOutputSlot.InputSideExpression = setVariableExpression;

                setVariableExpression.ReturnOutputSlot = assignmentExpression.ReturnOutputSlot;
                setVariableExpression.ReturnOutputSlot.OutputSideExpression = setVariableExpression;
                if (_expressionTree.RootExpression == assignmentExpression)
                    _expressionTree.RootExpression = setVariableExpression;
            }
            else
                throw _compilationContext.ThrowCompilationError("left side of assignment can only be variable reference for now");
        }

        private void TransformVariableReferencesToGetVariable()
        {
            while (true)
            {
                var variableReferenceExpression = _expressionTree.RootExpression.TraverseSubtree()
                    .OfType<VariableReferenceExpression>()
                    .FirstOrDefault();

                if (variableReferenceExpression == null)
                    break;

                TransformVariableReferenceToGetVariable(variableReferenceExpression);
            }
        }

        private void TransformVariableReferenceToGetVariable(VariableReferenceExpression variableReferenceExpression)
        {
            var getVariableExpression = new GetVariableExpression();
            getVariableExpression.Variable = variableReferenceExpression.Variable;

            getVariableExpression.ReturnOutputSlot = variableReferenceExpression.ReturnOutputSlot;
            getVariableExpression.ReturnOutputSlot.OutputSideExpression = getVariableExpression;
            if (_expressionTree.RootExpression == variableReferenceExpression)
                _expressionTree.RootExpression = getVariableExpression;
        }

        private CompilationContext _compilationContext;
        private ExpressionTree _expressionTree;
    }
}
