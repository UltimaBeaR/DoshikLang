﻿using Doshik;
using DoshikLangCompiler.UAssemblyGeneration;
using DoshikLangIR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangCompiler.Compilation
{
    public class CodeGenerator
    {
        public CodeGenerator(CompilationUnit compilationUnit, bool humanReadable)
        {
            _compilationUnit = compilationUnit;
            _humanReadable = humanReadable;
        }

        public UAssemblyBuilderCode GenerateCode()
        {
            _assemblyBuilder = new UAssemblyBuilder();

            // Добавляем переменные, заданные для ивентов в список зарезервированных, некоторые из них могут использоваться, некоторые могут
            // не использоваться, но они должны быть зарезервированы, чтобы эти имена не занял никто кроме параметров ивентов
            foreach (var eventHandler in _compilationUnit.ExternalApi.Events)
            {
                foreach (var eventParameter in eventHandler.InParameters)
                {
                    _reservedVariableNames.Add(MakeEventParameterVariableName(eventHandler.ExternalName, eventParameter.Name));
                }
            }

            for (int constantIdx = 0; constantIdx < _compilationUnit.Constants.Count; constantIdx++)
            {
                var constant = _compilationUnit.Constants[constantIdx];

                var name = "constant_" + constantIdx.ToString();

                _constantNames.Add((constant, name));

                if (constant.IsThis)
                    _assemblyBuilder.AddVariableAsThisDefaultValue(false, name, constant.Type.ExternalType.ExternalName);
                else if (constant.DotnetTypeString != null)
                    _assemblyBuilder.AddVariableAsTypeDefaultValue(false, name, constant.Type.ExternalType.ExternalName, constant.DotnetTypeString);
                else
                    _assemblyBuilder.AddVariableAsDefaultValue(false, name, constant.Type.ExternalType.ExternalName, constant.DotnetValue);
            }

            // Объявляем переменные для всех используемых built-in ивентов
            foreach (var eventHandler in _compilationUnit.Events.Values.Where(x => !x.IsCustom))
            {
                if (eventHandler.ExternalEvent.InParameters.Count != eventHandler.Parameters.Parameters.Count)
                    throw new Exception("external event params doesn't match actual event params");

                for (var paramIdx = 0; paramIdx < eventHandler.ExternalEvent.InParameters.Count; paramIdx++)
                {
                    var externalParameter = eventHandler.ExternalEvent.InParameters[paramIdx];
                    var parameter = eventHandler.Parameters.Parameters[paramIdx];

                    var name = MakeEventParameterVariableName(eventHandler.ExternalEvent.ExternalName, externalParameter.Name);

                    _variableNames.Add(parameter.Variable, name);
                    _assemblyBuilder.AddVariableAsDefaultValue(false, name, parameter.Variable.Type.ExternalType.ExternalName, null);
                }
            }

            foreach (var variable in _compilationUnit.Scope.Variables.Values.Cast<CompilationUnitVariable>().Where(x => x.IsPublic).OrderBy(x => x.Name))
            {
                var name = MakeVariableName(variable, _compilationUnit);

                _variableNames.Add(variable, name);
                _assemblyBuilder.AddVariableAsDefaultValue(true, name, variable.Type.ExternalType.ExternalName, null);
            }

            foreach (var variable in _compilationUnit.Scope.Variables.Values.Cast<CompilationUnitVariable>().Where(x => !x.IsPublic).OrderBy(x => x.Name))
            {
                var name = MakeVariableName(variable, _compilationUnit);

                _variableNames.Add(variable, name);
                _assemblyBuilder.AddVariableAsDefaultValue(false, name, variable.Type.ExternalType.ExternalName, null);
            }

            // Сначала просто добавляем все ивенты, чтобы было их определение
            foreach (var eventHandler in _compilationUnit.Events.Values.OrderBy(x => x.Name).ThenBy(x => x.IsCustom))
            {
                _assemblyBuilder.AddOrGetEvent(GetEventAssemblyName(eventHandler));
            }

            // Генерируем код внутри каждого из предопределенных ивентов, попутно генерируя переменные
            foreach (var eventHandler in _compilationUnit.Events.Values)
            {
                _currentEventBodyEmitter = _assemblyBuilder.AddOrGetEvent(GetEventAssemblyName(eventHandler));

                GenerateCodeForBlockOfStatements(eventHandler.BodyBlock);

                _currentEventBodyEmitter.JUMP_absoluteAddress(UAssemblyBuilder.maxCodeAddress);
            }

            return _assemblyBuilder.MakeCode(_humanReadable);
        }

        private string GetEventAssemblyName(EventDeclaration eventDeclaration)
        {
            return eventDeclaration.IsCustom
                ? eventDeclaration.Name
                : eventDeclaration.ExternalEvent.ExternalName;
        }

        private void GenerateCodeForStatement(Statement statement)
        {
            if (statement is LocalVariableDeclarationStatement localVariableDeclarationStatement)
                GenerateCodeForLocalVariableDeclarationStatement(localVariableDeclarationStatement);
            else if (statement is BlockOfStatements blockOfStatements)
                GenerateCodeForBlockOfStatements(blockOfStatements);
            else if (statement is ExpressionStatement expressionStatement)
                GenerateCodeForExpressionStatement(expressionStatement);
            else if (statement is IfStatement ifStatement)
                GenerateCodeForIfStatement(ifStatement);
            else if (statement is WhileStatement whileStatement)
                GenerateCodeForWhileStatement(whileStatement);
            else if (statement is BreakStatement breakStatement)
                GenerateCodeForBreakStatement(breakStatement);
            else if (statement is ContinueStatement continueStatement)
                GenerateCodeForContinueStatement(continueStatement);
            else if (statement is EmptyStatement emptyStatement)
                GenerateCodeForEmptyStatement(emptyStatement);
            else
                throw new NotImplementedException();
        }

        private void GenerateCodeForLocalVariableDeclarationStatement(LocalVariableDeclarationStatement statement)
        {
            // Объявляем локальную переменную

            var name = MakeVariableName(statement.Variable, statement);

            _variableNames.Add(statement.Variable, name);
            _assemblyBuilder.AddVariableAsDefaultValue(false, name, statement.Variable.Type.ExternalType.ExternalName, null);

            // В случае если у объявления переменной присутствует инициализирующее выражение (его может и не быть)
            if (statement.Initializer != null)
            {
                // Генерируем код для выражения инициализации только что объявленной переменной (все что после знака присвоения)
                GenerateCodeForExpression(statement.Initializer.RootExpression);

                // Копируем результат выполнения выражения инициализатора. Результат должен быть, т.к. инициализатор не может возвращать void

                // 1ый аргумент (PUSH) для COPY это результат выполнения инициализирующего выражения (выражение должно было вызвать PUSH с результатом своего выполнения)
                _currentEventBodyEmitter.PUSH_varableName(name);
                _currentEventBodyEmitter.COPY();
            }
        }

        private void GenerateCodeForBlockOfStatements(BlockOfStatements statement)
        {
            var subStatements = statement.Statements;

            foreach (var subStatement in subStatements)
                GenerateCodeForStatement(subStatement);
        }

        private void GenerateCodeForExpressionStatement(ExpressionStatement statement)
        {
            GenerateCodeForExpression(statement.Expression.RootExpression, true);
        }

        private void GenerateCodeForIfStatement(IfStatement statement)
        {
            var labelIndex = GetUniqueGlobalLabelPostfix();

            var labelAfterTrue = "after_true" + labelIndex;
            var labelAfterFalse = "after_false" + labelIndex;

            // Генерируем выражение условия. результатом будет значение в стеке типа bool, означающее, нужно ли выполнять true ветку (TrueStatement)
            GenerateCodeForExpression(statement.Condition.RootExpression);

            // В случае false в стеке (условие НЕ выполнено), прыгаем на метку с кодом после true блока (то есть это else ветка, либо за пределы условия, если else нет)
            // Также убирается (pop) значение со стека
            _currentEventBodyEmitter.JUMP_IF_FALSE_globalLabel(labelAfterTrue);

            GenerateCodeForStatement(statement.TrueStatement);

            if (statement.FalseStatement != null)
                _currentEventBodyEmitter.JUMP_globalLabel(labelAfterFalse);

            _currentEventBodyEmitter.NOP().GlobalLabel = labelAfterTrue;

            if (statement.FalseStatement != null)
            {
                GenerateCodeForStatement(statement.FalseStatement);

                _currentEventBodyEmitter.NOP().GlobalLabel = labelAfterFalse;
            }

            // ToDo: пока сделал косо криво через nop - по хорошему тут надо делать Jump на метку после последней инструкции в falsestatement, 
            // но так как она еще не создана, то нельзя задать ей label (можно на уровне event body emitter сделать возможность задавать globalLabel + оффсет от него)
            // можно также в NOP передавать какой-то флаг, что он может быть оптимизирован и в генерации итоговой сборки можно такие nop-ы удалять, но при этом 
            // делать так что лейбел который был у этого Nop-а будет разрешаться на адрес следующей инструкции (учесть кейс что следующей инструкцией также может быть такой же Nop либо вообще отстутствие инструкции)
            // (в этом случае нужно разрешать адрес до первой свободной инструкции либо ставить maxAddress)
        }

        private void GenerateCodeForWhileStatement(WhileStatement statement)
        {
            // Нужно завести стек в который перед входом в тело цикла ложить значения лейбелов labelBeforeCondition и labelAfterBody
            // для того чтобы знать куда прыгать внутри тела цикла по break; и continue;
            // а при выходе нужно удалять значение со стека

            var labelIndex = GetUniqueGlobalLabelPostfix();

            var labelBeforeCondition = "before_condition" + labelIndex;
            var labelAfterBody = "after_body" + labelIndex;

            _currentEventBodyEmitter.NOP().GlobalLabel = labelBeforeCondition;

            // Генерируем выражение условия. результатом будет значение в стеке типа bool, означающее, нужно ли выполнять тело цикла (BodyStatement)
            GenerateCodeForExpression(statement.Condition.RootExpression);

            // В случае false в стеке (условие продолжения цикла НЕ выполнено), прыгаем на метку с кодом после тела цикла
            // Также убирается (pop) значение со стека
            _currentEventBodyEmitter.JUMP_IF_FALSE_globalLabel(labelAfterBody);

            PushBreakContinueJumpLabel(labelAfterBody, labelBeforeCondition);

            GenerateCodeForStatement(statement.BodyStatement);

            PopBreakContinueJumpLabel();

            // По завершении тела цикла прыгаем назад на условие (в итоге проверка условия выполнится опять)
            _currentEventBodyEmitter.JUMP_globalLabel(labelBeforeCondition);

            _currentEventBodyEmitter.NOP().GlobalLabel = labelAfterBody;
        }

        private void GenerateCodeForBreakStatement(BreakStatement statement)
        {
            _currentEventBodyEmitter.JUMP_globalLabel(GetBreakContinueJumpLabel().BreakLabel);
        }

        private void GenerateCodeForContinueStatement(ContinueStatement statement)
        {
            _currentEventBodyEmitter.JUMP_globalLabel(GetBreakContinueJumpLabel().ContinueLabel);
        }

        private void GenerateCodeForEmptyStatement(EmptyStatement statement)
        {
            // Ничего не делаем
        }

        private void GenerateCodeForExpression(IExpression expression, bool removeExpressionResult = false)
        {
            if (expression is GetVariableExpression getVariableExpression)
                GenerateCodeForGetVariableExpression(getVariableExpression);
            else if (expression is SetVariableExpression setVariableExpression)
                GenerateCodeForSetVariableExpression(setVariableExpression);
            else if (expression is ConstantValueExpression constantValueExpression)
                GenerateCodeForConstantValueExpression(constantValueExpression);
            else if (expression is StaticMethodCallExpression staticMethodCallExpression)
                GenerateCodeForMethodCallExpression(staticMethodCallExpression);
            else if (expression is InstanceMethodCallExpression instanceMethodCallExpression)
                GenerateCodeForMethodCallExpression(instanceMethodCallExpression);
            else if (expression is TypecastExpression typecastExpression)
                GenerateCodeForTypecastExpression(typecastExpression);
            else
                throw new NotImplementedException();

            if (removeExpressionResult)
            {
                // Если removeExpressionResult значит нужно удалить результат последнего выполненного выражения в expression tree (последний push)
                // так как его никто не будет использовать, а стек должен сводиться к 0лю (итоговое кол-во push-ей должно быть покрыто таким же количеством pop-ов, явных или неявных)

                if (!expression.ReturnOutputSlot.Type.IsVoid)
                {
                    // Удаляем последнюю push инструкцию (она должна быть, если возвращаемый результат из выражения это не void)
                    _currentEventBodyEmitter.Instructions.RemoveAt(_currentEventBodyEmitter.Instructions.Count - 1);
                }
            }
        }

        private void GenerateCodeForGetVariableExpression(GetVariableExpression expression)
        {
            var name = _variableNames[expression.Variable];

            _currentEventBodyEmitter.PUSH_varableName(name);
        }

        private void GenerateCodeForSetVariableExpression(SetVariableExpression expression)
        {
            // Генерируем код выражения, которое будет присваиваться
            GenerateCodeForExpression(expression.Expression.OutputSideExpression);

            // пушим переменную, в которую будет присвоение

            var name = _variableNames[expression.Variable];
            _currentEventBodyEmitter.PUSH_varableName(name);

            // Копируем (то есть присваиваем)
            _currentEventBodyEmitter.COPY();
        }

        private void GenerateCodeForConstantValueExpression(ConstantValueExpression expression)
        {
            var constant = new Constant()
            {
                Type = expression.ValueType,
                IsThis = expression.IsThis,
                DotnetTypeString = expression.DotnetTypeString,
                DotnetValue = expression.DotnetValue
            };

            var name = _constantNames.First(x => Constant.Equals(x.constant, constant)).name;

            _currentEventBodyEmitter.PUSH_varableName(name);
        }

        private void GenerateCodeForMethodCallExpression(IMethodCallExpression expression)
        {
            // Проходим по всем in параметрам (кроме out и return слотов) и (в порядке их следования!) выполняем генерацию кода для них
            foreach (var inputSlot in expression.InputSlots)
            {
                GenerateCodeForExpression(inputSlot.OutputSideExpression);
            }

            var returnValueIsVoid = expression.ReturnOutputSlot.Type.IsVoid;
            string returnVariableName = null;

            if (!returnValueIsVoid)
            {
                returnVariableName = AddTemporaryVariable(expression.ReturnOutputSlot.Type.ExternalType);
                _currentEventBodyEmitter.PUSH_varableName(returnVariableName);
            }

            // Для out параметров код не генерируем (они также присутствуют в input-е), они просто передаются в инпут как обычные параметры, но после
            // отработки метода в них, так же как и в return параметр присвоятся новые значения.

            _currentEventBodyEmitter.EXTERN_externMethodSignature(expression.MethodOverload.GetFullExternalName());

            if (!returnValueIsVoid)
            {
                // Пушим еще раз, т.к. extern сделал pop (чтобы в результате вызова этого expression-а остался запушенный результат на стеке)
                _currentEventBodyEmitter.PUSH_varableName(returnVariableName);
            }
        }

        private void GenerateCodeForTypecastExpression(TypecastExpression expression)
        {
            // Генерируем код выражения, которые кастится к заданному типу (в результате будет push переменной)
            GenerateCodeForExpression(expression.Expression.OutputSideExpression);

            // Добавляем и пушим временную переменную c типом, в который нужно затайпкастить

            var outputVariableName = AddTemporaryVariable(expression.Type.ExternalType);
            _currentEventBodyEmitter.PUSH_varableName(outputVariableName);

            // Копируем во временную переменную (при этом автоматически производится тайпкаст к типу переменной, в которую идет копирование)
            _currentEventBodyEmitter.COPY();

            // Пушим еще раз временную переменную, как результат всей операции, т.к. copy сделала pop своих аргументов
            _currentEventBodyEmitter.PUSH_varableName(outputVariableName);
        }

        // создает временную переменную для вычислений.
        private string AddTemporaryVariable(DoshikExternalApiType externalType)
        {
            // ToDo: временные переменные как минимум нужны для возвращения результат выполнения методов в выражениях а также для out параметров
            // Нужно придумать какой-то механизм переиспользования переменных с одним и тем же типом. Нужно понять как удобнее определить момент когда временная переменная перестает быть нужна коду
            // то есть легко определить когда нужно ее создавать - но не очень понятно когда она перестает быть нужна, чтобы ее можно было после этого момента переиспользовать
            // пока буду без оптимизаций - просто каждый раз добавлять новую переменную и все

            var name = MakeVariableName();

            _temporaryVariableNames.Add(name);
            _assemblyBuilder.AddVariableAsDefaultValue(false, name, externalType.ExternalName, null);

            return name;
        }

        /// <summary>
        /// Находит имя переменной, которое еще не было использовано в assembly
        /// </summary>
        private string MakeVariableName(Variable variable = null, ICodeHierarchyNode declarationPlace = null)
        {
            string prefix = MakeVariableNamePrefix(variable, declarationPlace, out bool tryUseWithoutNumber);

            if (tryUseWithoutNumber)
            {
                if (!IsVariableNameRegistered(prefix))
                    return prefix;
            }

            int index = 0;
            string name;
            while (true)
            {
                name = prefix + index;

                if (!IsVariableNameRegistered(name))
                    return name;

                index++;
            }
        }

        private string MakeVariableNamePrefix(Variable variable, ICodeHierarchyNode declarationPlace, out bool tryUseWithoutNumber)
        {
            tryUseWithoutNumber = false;

            if (variable == null)
                return "temp_";

            if (variable is CompilationUnitVariable compilationUnitVariable && compilationUnitVariable.IsPublic)
            {
                // В случае глобальной переменной - пытаемся заюзать ее имя без всяких префиксов и номеров, т.к. это имя будет экспортироваться и видно извне
                tryUseWithoutNumber = true;
                return compilationUnitVariable.Name;
            }
            
            if (_humanReadable)
            {
                var prefix = "";

                var methodDeclaration = declarationPlace.FindNearestParentOfType<MethodDeclaration>();
                if (methodDeclaration != null)
                {
                    if (methodDeclaration is EventDeclaration eventDeclaration)
                    {
                        prefix += "local_" + (eventDeclaration.IsCustom ? "custom_" : "") + "event_" + eventDeclaration.Name + "_";
                    }
                }
                else
                {
                    prefix += "global_private_";
                }

                prefix += variable.Name + "_";

                return prefix;
            }

            return "var_";
        }

        private bool IsVariableNameRegistered(string variableName)
        {
            return
                _constantNames.Any(x => x.name == variableName) ||
                _variableNames.Values.Contains(variableName) ||
                _temporaryVariableNames.IndexOf(variableName) >= 0 ||
                _reservedVariableNames.IndexOf(variableName) >= 0;
        }

        private string GetUniqueGlobalLabelPostfix()
        {
            var result = "_" + _globalLabelUniqueCounter.ToString();
            _globalLabelUniqueCounter++;

            return result;
        }

        private void PushBreakContinueJumpLabel(string breakLabel, string continueLabel)
        {
            _breakContinueJumpLabels.Push(new BreakContinueJumpLabel { BreakLabel = breakLabel, ContinueLabel = continueLabel });
        }

        private void PopBreakContinueJumpLabel()
        {
            _breakContinueJumpLabels.Pop();
        }

        private BreakContinueJumpLabel GetBreakContinueJumpLabel()
        {
            return _breakContinueJumpLabels.Peek();
        }

        private string MakeEventParameterVariableName(string eventExternalName, string parameterName)
        {
            // remove first "_"
            string name = eventExternalName.Remove(0, 1);

            name += FirstLetterToUpperCase(parameterName);

            return name;
        }

        private static string FirstLetterToUpperCase(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            char[] chars = str.ToCharArray();
            chars[0] = char.ToUpperInvariant(chars[0]);
            return new string(chars);
        }

        // уникальные имена для констант. constant значения нельзя сравнивать по ссылке. Нужно вызывать Equals
        private List<(Constant constant, string name)> _constantNames = new List<(Constant constant, string name)>();

        // Ключи можно сравнивать по ссылке
        private Dictionary<Variable, string> _variableNames = new Dictionary<Variable, string>();

        // Временные переменные
        private List<string> _temporaryVariableNames = new List<string>();

        // Список зарезервированных имен переменных (используется щас как минимум под параметры встроенных ивентов)
        private List<string> _reservedVariableNames = new List<string>();

        // Используется для генерации уникальных имен меток для JUMP-ов
        private int _globalLabelUniqueCounter = 0;

        // Стек с глобальными метками для операторов break/continue, чтобы знать куда прыгать при генерации кода внутри цикла (с учетом вложенности циклов)
        private Stack<BreakContinueJumpLabel> _breakContinueJumpLabels = new Stack<BreakContinueJumpLabel>();

        private UAssemblyEventBodyEmitter _currentEventBodyEmitter;
        private UAssemblyBuilder _assemblyBuilder;
        private CompilationUnit _compilationUnit;
        private bool _humanReadable;

        private class BreakContinueJumpLabel
        {
            public string BreakLabel { get; set; }
            public string ContinueLabel { get; set; }
        }
    }
}
