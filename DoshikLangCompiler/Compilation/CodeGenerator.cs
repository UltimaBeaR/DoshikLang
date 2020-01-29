using DoshikLangCompiler.Compilation.CodeRepresentation;
using DoshikLangCompiler.Compilation.CodeRepresentation.Expressions;
using DoshikLangCompiler.UAssemblyGeneration;
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

            for (int constantIdx = 0; constantIdx < _compilationUnit.Constants.Count; constantIdx++)
            {
                var constant = _compilationUnit.Constants[constantIdx];

                var name = "constant_" + constantIdx.ToString();

                _constantNames.Add((constant, name));
                _assemblyBuilder.AddVariable(false, name, constant.Type.ExternalType.ExternalName, constant.DotnetValue);
            }

            foreach (var variable in _compilationUnit.Scope.Variables.Values.Cast<CompilationUnitVariable>().Where(x => x.IsPublic).OrderBy(x => x.Name))
            {
                var name = MakeVariableName(variable, _compilationUnit);

                _variableNames.Add(variable, name);
                _assemblyBuilder.AddVariable(true, name, variable.Type.ExternalType.ExternalName);
            }

            foreach (var variable in _compilationUnit.Scope.Variables.Values.Cast<CompilationUnitVariable>().Where(x => !x.IsPublic).OrderBy(x => x.Name))
            {
                var name = MakeVariableName(variable, _compilationUnit);

                _variableNames.Add(variable, name);
                _assemblyBuilder.AddVariable(false, name, variable.Type.ExternalType.ExternalName);
            }

            // Сначала просто добавляем все ивенты, чтобы было их определение
            foreach (var eventHandler in _compilationUnit.Events.Values.OrderBy(x => x.ExternalEvent.ExternalName))
            {
                _assemblyBuilder.AddOrGetEvent(eventHandler.ExternalEvent.ExternalName);
            }

            // Генерируем код внутри каждого из предопределенных ивентов, попутно генерируя переменные
            foreach (var eventHandler in _compilationUnit.Events.Values.Where(x => !x.IsCustom).OrderBy(x => x.ExternalEvent.ExternalName))
            {
                _currentEventBodyEmitter = _assemblyBuilder.AddOrGetEvent(eventHandler.ExternalEvent.ExternalName);

                // ToDo: как-то обработать параметры в методе. Вроде как под них есть захардкоженные названия переменных, которые нужно объявить?

                var statements = eventHandler.BodyBlock.Statements;

                foreach (var statement in statements)
                {
                    GenerateCodeForStatement(statement);
                }

                _currentEventBodyEmitter.JUMP_absoluteAddress(UAssemblyBuilder.maxCodeAddress);
            }

            return _assemblyBuilder.MakeCode(_humanReadable);
        }

        private void GenerateCodeForStatement(Statement statement)
        {
            if (statement is LocalVariableDeclarationStatement localVariableDeclarationStatement)
                GenerateCodeForLocalVariableDeclarationStatement(localVariableDeclarationStatement);
            else if (statement is ExpressionStatement expressionStatement)
                GenerateCodeForExpressionStatement(expressionStatement);
            else
                throw new NotImplementedException();
        }

        private void GenerateCodeForLocalVariableDeclarationStatement(LocalVariableDeclarationStatement statement)
        {
            // Объявляем локальную переменную

            var name = MakeVariableName(statement.Variable, statement);

            _variableNames.Add(statement.Variable, name);
            _assemblyBuilder.AddVariable(false, name, statement.Variable.Type.ExternalType.ExternalName);

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

        private void GenerateCodeForExpressionStatement(ExpressionStatement statement)
        {
            GenerateCodeForExpression(statement.Expression.RootExpression, true);
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
                GenerateCodeForStaticMethodCallExpression(staticMethodCallExpression);
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
            var name = _constantNames.First(x => x.constant.Equals(expression.ValueType, expression.DotnetValue)).name;

            _currentEventBodyEmitter.PUSH_varableName(name);
        }

        private void GenerateCodeForStaticMethodCallExpression(StaticMethodCallExpression expression)
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

            foreach (var outputSlot in expression.AdditionalOutputSlots)
            {
                var outputVariableName = AddTemporaryVariable(outputSlot.Type.ExternalType);
                _currentEventBodyEmitter.PUSH_varableName(outputVariableName);
            }

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
            _assemblyBuilder.AddVariable(false, name, externalType.ExternalName);

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
                if (!_constantNames.Any(x => x.name == prefix) && !_variableNames.Values.Contains(prefix) && !_temporaryVariableNames.Any(x => x == prefix))
                    return prefix;
            }

            int index = 0;
            string name;
            while (true)
            {
                name = prefix + index;

                if (!_constantNames.Any(x => x.name == name) && !_variableNames.Values.Contains(name) && !_temporaryVariableNames.Any(x => x == name))
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

        // уникальные имена для констант. constant значения нельзя сравнивать по ссылке. Нужно вызывать Equals
        private List<(Constant constant, string name)> _constantNames = new List<(Constant constant, string name)>();

        // Ключи можно сравнивать по ссылке
        private Dictionary<Variable, string> _variableNames = new Dictionary<Variable, string>();

        // Временные переменные
        private List<string> _temporaryVariableNames = new List<string>();

        private UAssemblyEventBodyEmitter _currentEventBodyEmitter;
        private UAssemblyBuilder _assemblyBuilder;
        private CompilationUnit _compilationUnit;
        private bool _humanReadable;
    }
}
