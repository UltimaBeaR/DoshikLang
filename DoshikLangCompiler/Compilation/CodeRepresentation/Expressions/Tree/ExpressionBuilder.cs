using System;
using DoshikLangCompiler.Compilation.CodeRepresentation.Expressions.Nodes;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace DoshikLangCompiler.Compilation.CodeRepresentation.Expressions.Tree
{
    public class ExpressionBuilder
    {
        public ExpressionTree Build(CompilationContext compilationContext, ICodeHierarchyNode expressionParent, ExpressionNodeSequence nodeSequence)
        {
            _compilationContext = compilationContext;

            _nodeSequence = nodeSequence;

            _tree = new ExpressionTree(expressionParent);

            _sequence = new List<IExpression>();

            for (int sequenceIndex = 0; sequenceIndex < _nodeSequence.Sequence.Count; sequenceIndex++)
            {
                var expressionNode = _nodeSequence.Sequence[sequenceIndex];

                var expression = HandleNode(expressionNode);

                _sequence.Add(expression);
            }

            _tree.RootExpression = _sequence.LastOrDefault();

            (new ExpressionTransformer()).Transform(_compilationContext, _tree);

            return _tree;
        }

        private IExpression HandleNode(IExpressionNode node)
        {
            if (node is ParenthesisExpressionNode parenthesisExpressionNode)
                return HandleParenthesisExpressionNode(parenthesisExpressionNode);
            else if (node is IdentifierExpressionNode identifierExpressionNode)
                return HandleIdentifierExpressionNode(identifierExpressionNode);
            else if (node is LiteralExpressionNode literalExpressionNode)
                return HandleLiteralExpressionNode(literalExpressionNode);
            else if (node is DotExpressionNode dotExpressionNode)
                return HandleDotExpressionNode(dotExpressionNode);
            else if (node is TypeDotExpressionNode typeDotExpressionNode)
                return HandleTypeDotExpressionNode(typeDotExpressionNode);
            else if (node is TypecastExpressionNode typecastExpressionNode)
                return HandleTypecastExpressionNode(typecastExpressionNode);
            else if (node is AssignmentExpressionNode assignmentExpressionNode)
                return HandleAssignmentExpressionNode(assignmentExpressionNode);
            else if (node is DefaultOfTypeExpressionNode defaultOfTypeExpressionNode)
                return HandleDefaultOfTypeExpressionNode(defaultOfTypeExpressionNode);
            else if (node is UnaryPrefixExpressionNode unaryPrefixExpressionNode)
                return HandleUnaryPrefixExpressionNode(unaryPrefixExpressionNode);
            else if (node is NotExpressionNode notExpressionNode)
                return HandleNotExpressionNode(notExpressionNode);
            else if (node is MultiplicationExpressionNode multiplicationExpressionNode)
                return HandleMultiplicationExpressionNode(multiplicationExpressionNode);
            else if (node is AdditionExpressionNode additionExpressionNode)
                return HandleAdditionExpressionNode(additionExpressionNode);
            else if (node is RelativeExpressionNode relativeExpressionNode)
                return HandleRelativeExpressionNode(relativeExpressionNode);
            else if (node is EqualsExpressionNode equalsExpressionNode)
                return HandleEqualsExpressionNode(equalsExpressionNode);
            else if (node is AndExpressionNode andExpressionNode)
                return HandleAndExpressionNode(andExpressionNode);
            else if (node is OrExpressionNode orExpressionNode)
                return HandleOrExpressionNode(orExpressionNode);

            throw new NotImplementedException();
        }

        private IExpression HandleParenthesisExpressionNode(ParenthesisExpressionNode node)
        {
            return FindExpressionByExpressionNode(node.Expression, false);
        }

        private IExpression HandleIdentifierExpressionNode(IdentifierExpressionNode node)
        {
            if (node.IsLeftOfDotExpression)
            {
                // Если нода с идентификатором упоминалась как левая часть dot выражения, значит нужно в ПЕРВУЮ ОЧЕРЕДЬ проверить не является ли
                // этот идентификатор названием типа

                var foundType = _compilationContext.TypeLibrary.FindTypeByCodeNameString(node.Identifier);
                if (foundType.DataType != null)
                {
                    return new TypeHolderDummyExpression() { Type = foundType.DataType };
                }
            }

            var result = new VariableReferenceExpression();

            var scope = _tree.FindNearestScopeOwner().Scope;

            var variable = scope.FindVariableByName(node.Identifier);

            if (variable == null)
                throw _compilationContext.ThrowCompilationError($"variable { node.Identifier } is not defined");

            result.Variable = variable;

            // Определяем выходное значение
            result.ReturnOutputSlot = new ExpressionSlot(result.Variable.Type, result);

            return result;
        }

        private IExpression HandleLiteralExpressionNode(LiteralExpressionNode node)
        {
            var result = new ConstantValueExpression();
            Type dotnetType;

            switch (node.LiteralType)
            {
                case LiteralExpressionNode.LiteralTypeOption.Int:
                    {
                        var literalValue = node.LiteralValue;

                        if (int.TryParse(literalValue, NumberStyles.None, CultureInfo.InvariantCulture,  out int intResult))
                        {
                            dotnetType = typeof(int);
                            result.DotnetValue = intResult;
                        }
                        else if (long.TryParse(literalValue, NumberStyles.None, CultureInfo.InvariantCulture, out long longResult))
                        {
                            dotnetType = typeof(long);
                            result.DotnetValue = longResult;
                        }
                        else
                            throw _compilationContext.ThrowCompilationError("integer constant is too large");
                    }
                    break;
                case LiteralExpressionNode.LiteralTypeOption.IntHex:
                    {
                        // Удаляем '0x'
                        var literalValue = node.LiteralValue.Remove(0, 2);

                        if (int.TryParse(literalValue, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out int intResult))
                        {
                            dotnetType = typeof(int);
                            result.DotnetValue = intResult;
                        }
                        else if (long.TryParse(literalValue, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out long longResult))
                        {
                            dotnetType = typeof(long);
                            result.DotnetValue = longResult;
                        }
                        else
                            throw _compilationContext.ThrowCompilationError("integer constant is too large");
                    }
                    break;
                case LiteralExpressionNode.LiteralTypeOption.Float:
                    {
                        var literalValue = node.LiteralValue;
                        bool isFloatLiteral;

                        if (literalValue.EndsWith("f"))
                        {
                            isFloatLiteral = true;
                            literalValue = literalValue.Remove(literalValue.Length - 1, 1);
                        }
                        else
                        {
                            isFloatLiteral = false;

                            if (literalValue.EndsWith("d"))
                            {
                                literalValue = literalValue.Remove(literalValue.Length - 1, 1);
                            }
                        }

                        if (isFloatLiteral)
                        {
                            if (float.TryParse(literalValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float floatResult))
                            {
                                dotnetType = typeof(float);
                                result.DotnetValue = floatResult;

                            }
                            else
                                throw _compilationContext.ThrowCompilationError("cannot parse float literal");
                        }
                        else
                        {
                            if (double.TryParse(literalValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double doubleResult))
                            {
                                dotnetType = typeof(double);
                                result.DotnetValue = doubleResult;

                            }
                            else
                                throw _compilationContext.ThrowCompilationError("cannot parse double literal");
                        }
                    }
                    break;
                case LiteralExpressionNode.LiteralTypeOption.String:
                    {
                        dotnetType = typeof(string);

                        var literalValue = node.LiteralValue;

                        // Удаляем кавычки по краям

                        literalValue = literalValue.Remove(0, 1);
                        literalValue = literalValue.Remove(literalValue.Length - 1, 1);

                        // unescape

                        // ToDo: в этом месте делаем undescape, то есть находим в строке обратный слеш плюс код и заменяем эти места на соответствующие символы (например \n меняем на символ новой строки и тд)
                        // В парсере это не определено и не будет определено

                        result.DotnetValue = literalValue;
                    }
                    break;
                case LiteralExpressionNode.LiteralTypeOption.Bool:
                    {
                        dotnetType = typeof(bool);
                        result.DotnetValue = bool.Parse(node.LiteralValue);
                    }
                    break;
                case LiteralExpressionNode.LiteralTypeOption.Null:
                    {
                        dotnetType = typeof(object);
                        result.DotnetValue = null;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            result.ValueType = _compilationContext.TypeLibrary.FindTypeByDotnetType(dotnetType);

            _compilationContext.CompilationUnit.AddConstant(result.ValueType, result.DotnetValue);

            // Определяем выходное значение
            result.ReturnOutputSlot = new ExpressionSlot(result.ValueType, result);

            return result;
        }

        private IExpression HandleDotExpressionNode(DotExpressionNode node)
        {
            if (node.RightIdentifier != null)
                throw _compilationContext.ThrowCompilationError("properties are not supported yet");

            var left = FindExpressionByExpressionNode(node.Left, false);

            if (left is TypeHolderDummyExpression typeHolder)
            {
                return CreateStaticMethodCallExpression(typeHolder.Type, node.RightMethodCallData);
            }
            else
            {
                throw _compilationContext.ThrowCompilationError("instance method calls are not supported yet");
            }
        }

        private IExpression HandleTypeDotExpressionNode(TypeDotExpressionNode node)
        {
            if (node.RightIdentifier != null)
                throw _compilationContext.ThrowCompilationError("static properties are not supported yet");

            return CreateStaticMethodCallExpression(node.LeftType, node.RightMethodCallData);
        }

        private IExpression HandleTypecastExpressionNode(TypecastExpressionNode node)
        {
            var result = new TypecastExpression();

            result.Type = node.Type;

            result.Expression = FindExpressionByExpressionNode(node.Expression, false).ReturnOutputSlot;
            result.Expression.InputSideExpression = result;
            result.InputSlots.Add(result.Expression);

            // Определяем выходное значение
            result.ReturnOutputSlot = new ExpressionSlot(result.Type, result);

            return result;
        }

        private IExpression HandleAssignmentExpressionNode(AssignmentExpressionNode node)
        {
            var result = new AssignmentExpression();
            result.Operator = node.Operator;

            result.Left = FindExpressionByExpressionNode(node.Left, true).ReturnOutputSlot;
            result.Left.InputSideExpression = result;
            result.InputSlots.Add(result.Left);

            result.Right = FindExpressionByExpressionNode(node.Right, false).ReturnOutputSlot;
            result.Right.InputSideExpression = result;
            result.InputSlots.Add(result.Right);

            // Определяем выходное значение
            result.ReturnOutputSlot = new ExpressionSlot(_compilationContext.TypeLibrary.FindVoid(), result);

            return result;
        }

        private IExpression HandleDefaultOfTypeExpressionNode(DefaultOfTypeExpressionNode node)
        {
            var result = new ConstantValueExpression();

            result.ValueType = node.Type;

            // Добавляем константу (если еще нет), означающую дефолтное значение этого типа. null - значит что возьмется значение null из кода (не реальное значение dotnet объекта),
            // а это значит что будет просто дефолтное значение этого типа
            // ToDo: надо удостовериться что все дефолтные значения безопасны для копирования.
            // То есть проверить что не будет такого что дефолтное значение это какая нибудь ссылка с готовым объектом
            // (тогда можно будет изменить его через вызов метода и это сломает константу для всех мест где ее используют)
            _compilationContext.CompilationUnit.AddConstant(result.ValueType, null);

            // Определяем выходное значение
            result.ReturnOutputSlot = new ExpressionSlot(result.ValueType, result);

            return result;
        }

        private IExpression HandleUnaryPrefixExpressionNode(UnaryPrefixExpressionNode node)
        {
            switch (node.Prefix)
            {
                case UnaryPrefixExpressionNode.PrefixOption.Minus:
                    return CreateStaticMethodCallExpressionForUnaryOperator("op_UnaryMinus", node.Expression);
                case UnaryPrefixExpressionNode.PrefixOption.Plus:
                    // В случае операции унарного плюса возвращаем исходное выражение всегда (то есть плюс без последствий можно применить к чему угодно)
                    return FindExpressionByExpressionNode(node.Expression, false);

                // ToDo: Increment, Decrement

                default:
                    throw new NotImplementedException();
            }
        }

        private IExpression HandleNotExpressionNode(NotExpressionNode node)
        {
            return CreateStaticMethodCallExpressionForUnaryOperator("op_UnaryNegation", node.Expression);
        }

        private IExpression HandleMultiplicationExpressionNode(MultiplicationExpressionNode node)
        {
            // ToDo: тут и в других перегрузках операторов: при формировании external api нужно бы разобрать методы по группам: Простой метод, конструктор, перегрузка оператора (+, - и тд)
            // тогда можно было бы тут искать не по имени метода а по группе и среди них уже искать перегрузки по типам параметров (причем тут бы тогда то статический метод или нет определялось бы
            // в зависимости от флага того метод статический или нет)

            switch (node.Operator)
            {
                case MultiplicationExpressionNode.OperatorOption.Multiply:
                    // ToDo: для UnityEngine типов тут есть op_Multiply (надо поменяь методы чтобы мог принимать несколько методов и искал перегрузки во всех них)
                    return CreateStaticMethodCallExpressionForBinaryOperator("op_Multiplication", node.Left, node.Right);
                case MultiplicationExpressionNode.OperatorOption.Divide:
                    return CreateStaticMethodCallExpressionForBinaryOperator("op_Division", node.Left, node.Right);
                case MultiplicationExpressionNode.OperatorOption.Mod:
                    // ToDo: пока не нашел метода для этого, однако на feedback.vrchat.com есть выполненный тикет где написано что добавили какой то метод remainer - вроде как это оно и есть
                    throw _compilationContext.ThrowCompilationError("mod operator is not implemented yet");
                default:
                    throw new NotImplementedException();
            }
        }

        private IExpression HandleAdditionExpressionNode(AdditionExpressionNode node)
        {
            switch (node.Operator)
            {
                case AdditionExpressionNode.OperatorOption.Plus:
                    return CreateStaticMethodCallExpressionForBinaryOperator("op_Addition", node.Left, node.Right);
                case AdditionExpressionNode.OperatorOption.Minus:
                    return CreateStaticMethodCallExpressionForBinaryOperator("op_Subtraction", node.Left, node.Right);
                default:
                    throw new NotImplementedException();
            }
        }

        private IExpression HandleRelativeExpressionNode(RelativeExpressionNode node)
        {
            switch (node.Operator)
            {
                case RelativeExpressionNode.OperatorOption.Greater:
                    return CreateStaticMethodCallExpressionForBinaryOperator("op_GreaterThan", node.Left, node.Right);
                case RelativeExpressionNode.OperatorOption.Lesser:
                    return CreateStaticMethodCallExpressionForBinaryOperator("op_LessThan", node.Left, node.Right);
                case RelativeExpressionNode.OperatorOption.GreaterOrEquals:
                    return CreateStaticMethodCallExpressionForBinaryOperator("op_GreaterThanOrEqual", node.Left, node.Right);
                case RelativeExpressionNode.OperatorOption.LesserOrEquals:
                    return CreateStaticMethodCallExpressionForBinaryOperator("op_LessThanOrEqual", node.Left, node.Right);
                default:
                    throw new NotImplementedException();
            }
        }

        private IExpression HandleEqualsExpressionNode(EqualsExpressionNode node)
        {
            switch (node.Operator)
            {
                case EqualsExpressionNode.OperatorOption.Equals:
                    return CreateStaticMethodCallExpressionForBinaryOperator("op_Equality", node.Left, node.Right);
                case EqualsExpressionNode.OperatorOption.NotEquals:
                    return CreateStaticMethodCallExpressionForBinaryOperator("op_Inequality", node.Left, node.Right);
                default:
                    throw new NotImplementedException();
            }
        }

        private IExpression HandleAndExpressionNode(AndExpressionNode node)
        {
            return CreateStaticMethodCallExpressionForBinaryOperator("op_ConditionalAnd", node.Left, node.Right);
        }

        private IExpression HandleOrExpressionNode(OrExpressionNode node)
        {
            return CreateStaticMethodCallExpressionForBinaryOperator("op_ConditionalOr", node.Left, node.Right);
        }
        
        private IExpression CreateStaticMethodCallExpressionForUnaryOperator(string methodName, IExpressionNode operand)
        {
            var operandExpression = FindExpressionByExpressionNode(operand, false);

            var methodCallData = new MethodCallExpressionNodeData
            {
                Name = methodName
            };

            methodCallData.Parameters.Add(new MethodCallParameterExpressionNodeData { Expression = operand });

            return CreateStaticMethodCallExpression(operandExpression.ReturnOutputSlot.Type, methodCallData);
        }

        private IExpression CreateStaticMethodCallExpressionForBinaryOperator(string methodName, IExpressionNode left, IExpressionNode right)
        {
            var leftExpression = FindExpressionByExpressionNode(left, false);

            var methodCallData = new MethodCallExpressionNodeData
            {
                Name = methodName
            };

            methodCallData.Parameters.Add(new MethodCallParameterExpressionNodeData { Expression = left });
            methodCallData.Parameters.Add(new MethodCallParameterExpressionNodeData { Expression = right });

            return CreateStaticMethodCallExpression(leftExpression.ReturnOutputSlot.Type, methodCallData);
        }

        private IExpression CreateStaticMethodCallExpression(DataType type, MethodCallExpressionNodeData methodCallData)
        {
            var result = new StaticMethodCallExpression();

            if (methodCallData.TypeArguments.Count > 0)
                throw _compilationContext.ThrowCompilationError("method type arguments are not supported");

            var findOverloadPararmeters = new List<TypeLibrary.FindOverloadParameter>(methodCallData.Parameters.Count);

            // Проходим по всем параметрам у вызова метода
            foreach (var methodCallParameter in methodCallData.Parameters)
            {
                // Добавляем input слот к текущему выражению (он уже существует как output у выражения, соответствующего этому параметру)

                var slot = FindExpressionByExpressionNode(methodCallParameter.Expression, false).ReturnOutputSlot;

                slot.InputSideExpression = result;
                result.InputSlots.Add(slot);

                findOverloadPararmeters.Add(new TypeLibrary.FindOverloadParameter { IsOut = methodCallParameter.IsOut, Type = slot.Type.ExternalType });
            }

            var foundOverload = _compilationContext.TypeLibrary.FindBestMethodOverload(true, type.ExternalType, methodCallData.Name, findOverloadPararmeters);

            if (foundOverload.OverloadCount == 0)
                throw _compilationContext.ThrowCompilationError($"static method { methodCallData.Name } not found in { _compilationContext.TypeLibrary.GetApiTypeFullCodeName(type.ExternalType) } type");

            if (foundOverload.BestOverload == null)
            {
                // ToDo: в сообщении указывать сигнатуру перегрузки которая искалась и все сигнатуры которые есть
                throw _compilationContext.ThrowCompilationError($"overload for static method { methodCallData.Name } not found in { _compilationContext.TypeLibrary.GetApiTypeFullCodeName(type.ExternalType) } type, but found { foundOverload.OverloadCount } other overloads for this method");
            }

            result.MethodOverload = foundOverload.BestOverload;

            // Определяем выходное значение

            var returnValueIsVoid = result.MethodOverload.OutParameterType == null;

            var returnValueType = returnValueIsVoid
                ? _compilationContext.TypeLibrary.FindVoid()
                : _compilationContext.TypeLibrary.FindByExternalType(result.MethodOverload.OutParameterType);

            result.ReturnOutputSlot = new ExpressionSlot(returnValueType, result);

            foreach (var outParameter in result.MethodOverload.ExtraOutParameters)
            {
                result.AdditionalOutputSlots.Add(new ExpressionSlot(_compilationContext.TypeLibrary.FindByExternalType(outParameter.Type), result));
            }

            return result;
        }

        // Находит ранее определенный IExpression в _sequence по соответствующему ему ноду из _nodeSequence
        private IExpression FindExpressionByExpressionNode(IExpressionNode node, bool canReturnVoid = true)
        {
            var foundIndex = _nodeSequence.Sequence.FindIndex(x => x == node);

            if (foundIndex >= 0)
            {
                var expression = _sequence[foundIndex];

                // проверка на null, т.к могут быть dummy ноды (временные), они могут не иметь слотов вообще
                if (!canReturnVoid && (expression.ReturnOutputSlot != null && expression.ReturnOutputSlot.Type.IsVoid))
                    throw _compilationContext.ThrowCompilationError("expression cannot return void");

                return expression;
            }

            return null;
        }

        private CompilationContext _compilationContext;

        private ExpressionNodeSequence _nodeSequence;
        private List<IExpression> _sequence;

        private ExpressionTree _tree;
    }
}
