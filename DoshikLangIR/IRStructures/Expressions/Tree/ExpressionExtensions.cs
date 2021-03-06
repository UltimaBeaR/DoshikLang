﻿using System.Collections.Generic;

namespace DoshikLangIR
{
    public static class ExpressionExtensions
    {
        public static IEnumerable<IExpression> TraverseSubtree(this IExpression expression)
        {
            foreach (var inputSlot in expression.InputSlots)
            {
                foreach (var inputSubtreeExpression in inputSlot.OutputSideExpression.TraverseSubtree())
                {
                    yield return inputSubtreeExpression;
                }
            }

            yield return expression;
        }
    }
}
