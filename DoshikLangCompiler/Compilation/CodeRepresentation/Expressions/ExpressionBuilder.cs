﻿using DoshikLangCompiler.Compilation.CodeRepresentation.Expressions.Nodes;

namespace DoshikLangCompiler.Compilation.CodeRepresentation.Expressions
{
    public class ExpressionBuilder
    {
        public ExpressionTree Build(ICodeHierarchyNode expressionParent, ExpressionNodeSequence nodeSequence)
        {
            _expressionParent = expressionParent;
            _nodeSequence = nodeSequence;







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






            return null;
        }

        private ExpressionNodeSequence _nodeSequence;
        private ICodeHierarchyNode _expressionParent;
    }
}
