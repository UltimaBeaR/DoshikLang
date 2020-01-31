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









            /* ToDo: Способ построения
             * существует постпроцессинг этих нод.
             * 
             * 
             * постпроцессинг - это преобразования одних выражений в другие. Например можно сделать временный элемент expression tree который будет обозначать /= а в итоговом варианте обрабатывать дерево
             * и перестраивать его так чтобы там были две отдельные операции для / (вызов метода operator_div) и для = (assign)
             * 
             * идем по порядку по sequence и преобразовываем объекты в выражения expression tree при этом порядок при обходе итогового expression tree Должен быть таким же как при обходе sequence
             * бОльшая часть Node-ов преобразуется в вызовы методов. И по ходу построения дерева нужно также создавать слоты соответствующие для элементов дерева и соединять элементы дерева этими слотами
             * 
             * после построения expressionTree его нужно валидировать и вывалить ошибки компиляции если что-то не так - т.к. некоторые вещи нельзя валидировать сразу, по ходу построения этого дерева
             * 
             * 
             * кейсы с обходом определенных нод при построении кода (assign, ifelse) будут разрешаться уже при генерации assembly по этому дереву
             * 
             * 
             */







            // ToDo: а может ли результат одного expression-а использоваться в нескольких местах? поидее должно такое уметь, то есть у каждого выражения (элемента дерева) на каждый
            // слот должен быть внутри не один Input а несколько (массив) Input-ов. То есть каждый, кто использует слот как Input должен прописаться туда. То есть если пустой массив инпутов
            // - значит этот слот никем не используется (он возвращается кем то но не используется)

            // ToDo: проходимся по всему _nodeSequence и создаем параллельную ExpressionSequence
            // для примера int result = a + 1;
            // это будет сначала первая нода VariableReferenceExpression вторая ConstantValueExpression
            // При их построении они создают свои output слоты и задают там тип + референс на себя как Output часть (input запоняет тот кто соединяется потом с этим слотом (он может быть не использованным, в этом случае - не задается)
            // а третья - MethodCallExpression / StaticCallExpression - чтобы определить какой именно там метод, нужно взять
            // левое выражение (можно определить из параллельного _nodeSequence получив по индексу параметра готовый Expression (в этом случае это будет VariableReferenceExpression)
            // далее зная тип выходного слота этой VariableReferenceExpression можно из него найти перегрузку метода op_Addition (или как он там зовется) подходящую для второго параметра (в этом случае тоже int32 - там литерал интовый)

            // Смысл последовательности, что все элементы которые идут ДО текущего они референсятся как-то в текущем и уже полностью обработаны. По этому можно спокойно на них референсится.
            // Это работает как с nodeSequence так и с той последовательностью выражений что я щас собираюсь тут построить.
            // В конечном итоге просто возвращается последний элемент этой последовательности и засовывается в ExpressionTree как rootExpression

            // ExpressionSlot  - при обработке выражений очередное выражение всегда создает свои Output слоты (создает эти объекты с 0ля) и заполняет Тип и ссылку на само выражение.
            // дальше каждое выражение при заполнении своих Input-ов НЕ создает с 0ля слот а ищет уже готовый заданный как Output его параметров. и при этом ДОПИСЫВАЕТ туда себя как input.



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
            if (node is ParenthesisExpressionNode)
                return null;
            else if (node is IdentifierExpressionNode identifierExpressionNode)
                return HandleIdentifierExpressionNode(identifierExpressionNode);
            else if (node is LiteralExpressionNode literalExpressionNode)
                return HandleLiteralExpressionNode(literalExpressionNode);
            else if (node is DotExpressionNode dotExpressionNode)
                return HandleDotExpressionNode(dotExpressionNode);
            else if (node is TypeDotExpressionNode typeDotExpressionNode)
                return HandleTypeDotExpressionNode(typeDotExpressionNode);
            else if (node is AdditionExpressionNode additionExpressionNode)
                return HandleAdditionExpressionNode(additionExpressionNode);
            else if (node is TypecastExpressionNode typecastExpressionNode)
                return HandleTypecastExpressionNode(typecastExpressionNode);
            else if (node is AssignmentExpressionNode assignmentExpressionNode)
                return HandleAssignmentExpressionNode(assignmentExpressionNode);
            else if (node is DefaultOfTypeExpressionNode defaultOfTypeExpressionNode)
                return HandleDefaultOfTypeExpressionNode(defaultOfTypeExpressionNode);

            throw new NotImplementedException();
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

        private IExpression HandleAdditionExpressionNode(AdditionExpressionNode node)
        {
            // ToDo: сделать
            // тут надо найти определенный статический метод op_Addition у типа, который определен в выходной ноде левого параметра
            // и возвратить его вызов

            // ToDo: при формировании external api нужно бы разобрать методы по группам: Простой метод, конструктор, перегрузка оператора (+, - и тд)
            // тогда можно было бы тут искать не по имени метода а по группе и среди них уже искать перегрузки по типам параметров (причем тут бы тогда то статический метод или нет определялось бы
            // в зависимости от флага того метод статический или нет)
            // этот код можно тоже сделать универсальным на многие операторы

            var result = new StaticMethodCallExpression();

            var leftExpression = FindExpressionByExpressionNode(node.Left, false);
            var rightExpression = FindExpressionByExpressionNode(node.Right, false);








            // Если при поиске метода перегрузки был найден статический метод - то тут надо вызывать CreateStaticMethodCallExpression()




            return result;
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
