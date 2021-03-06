﻿using Antlr4.Runtime.Misc;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangIR
{
    public class CompilationUnitCreationVisitor : CompilationContextVisitorBase<object>
    {
        public CompilationUnitCreationVisitor(CompilationContext compilationContext)
            : base(compilationContext)
        {
        }

        public static void Apply(CompilationContext compilationContext, DoshikParser.CompilationUnitContext antlrContext)
        {
            antlrContext.Accept(new CompilationUnitCreationVisitor(compilationContext));
        }

        public override object VisitCompilationUnit([NotNull] DoshikParser.CompilationUnitContext context)
        {
            _compilationContext.CompilationUnit = new CompilationUnit(_compilationContext.ExternalApi);

            var memberDeclarations = context.memberDeclaration();

            foreach (var memberDeclaration in memberDeclarations)
            {
                Visit(memberDeclaration);
            }

            return null;
        }

        public override object VisitMemberDeclaration([NotNull] DoshikParser.MemberDeclarationContext context)
        {
            _compilationContext.SetParsingAntlrContext(context);

            if (context.fieldDeclaration() != null)
                Visit(context.fieldDeclaration());

            if (context.methodDeclaration() != null)
                Visit(context.methodDeclaration());

            return null;
        }

        public override object VisitFieldDeclaration([NotNull] DoshikParser.FieldDeclarationContext context)
        {
            _compilationContext.SetParsingAntlrContext(context);

            var scope = _compilationContext.CompilationUnit.Scope;

            var variable = new CompilationUnitVariable(_compilationContext.CompilationUnit);

            variable.IsPublic = context.PUBLIC() != null;

            var foundType = GetTypeNameVisitor.Apply(_compilationContext, context.typeType());
            foundType.ThrowIfNotFound(_compilationContext);
            variable.Type = foundType.DataType;

            (var variableName, var variableInitializer) = ((string, DoshikParser.VariableInitializerContext))Visit(context.variableDeclarator());

            variable.Name = variableName;

            if (variableInitializer != null)
            {
                // ToDo: потом можно сделать инициализаторы полей. Прикол тут в том что их нельзя инициализировать также
                // как локальные переменные в statement-ах, потому что тут нет порядка выполнения операций, а значит
                // в инициализирующем выражении первой переменной может быть зареференшена вторвая переменная а в инициализации второй переменной референс на первую
                // таким образом будет circular reference. И такие вещи нужно определять, для этого нужно сортировать эти определения переменных и инициализировать их
                // в порядке начиная от меньшего количества референсов на другие переменные в инициализаторе до больших + трекать как то circular референсы. 
                // Из-за того что тут такой гимор, я решил пока не делать инициализаторы (инициализировать переменные все равно можно будет вручную на событии Start или как там его)
                throw _compilationContext.ThrowCompilationError($"field initializer is not supported yet");
            }

            if (scope.FindVariableByName(variable.Name, true) != null)
                throw _compilationContext.ThrowCompilationError($"variable { variable.Name } is already defined");

            scope.Variables[variable.Name] = variable;

            return null;
        }

        // возвращает (string variableName, DoshikParser.VariableInitializerContext variableInitializer)
        public override object VisitVariableDeclarator([NotNull] DoshikParser.VariableDeclaratorContext context)
        {
            _compilationContext.SetParsingAntlrContext(context);

            var variableName = context.variableName.Text;
            var variableInitializer = context.variableInitializer();

            return (variableName, variableInitializer);
        }

        public override object VisitMethodDeclaration([NotNull] DoshikParser.MethodDeclarationContext context)
        {
            _compilationContext.SetParsingAntlrContext(context);

            var methodIsEvent = context.EVENT() != null;
            var methodReturnTypeCtx = context.typeTypeOrVoid();
            var methodCodeName = context.methodName.Text;
            var methodParemtersCtx = context.formalParameters();
            var methodBodyCtx = context.block();

            // Пока не поддерживаю кастомные функции (не ивенты)
            if (!methodIsEvent)
                throw _compilationContext.ThrowCompilationError($"custom functions is not supported yet");

            // Если это event

            HandleEventDeclaration(methodReturnTypeCtx, methodCodeName, methodParemtersCtx, methodBodyCtx);

            return null;
        }

        private void HandleEventDeclaration(
            DoshikParser.TypeTypeOrVoidContext eventReturnTypeCtx,
            string eventCodeName,
            DoshikParser.FormalParametersContext eventParametersCtx,
            DoshikParser.BlockContext eventBodyCtx
        )
        {
            var eventDeclaration = new EventDeclaration(_compilationContext.CompilationUnit);
            eventDeclaration.Parameters = new MethodDeclarationParameters(eventDeclaration, _compilationContext.CompilationUnit.Scope);

            _currentMethodDeclaration = eventDeclaration;

            eventDeclaration.Name = eventCodeName;

            var foundType = GetTypeNameVisitor.Apply(_compilationContext, eventReturnTypeCtx);
            foundType.ThrowIfNotFound(_compilationContext);
            eventDeclaration.ReturnTypeOrVoid = foundType.DataType;

            eventDeclaration.ExternalEvent = _compilationContext.FindExternalApiEventByCodeName(eventDeclaration.Name);

            if (_compilationContext.CompilationUnit.Events.ContainsKey(eventDeclaration.Name))
                throw _compilationContext.ThrowCompilationError($"event handler { eventDeclaration.Name } is already defined");

            eventDeclaration.Parameters.Parameters.AddRange((List<MethodDeclarationParameter>)Visit(eventParametersCtx));

            eventDeclaration.AntlrBody = eventBodyCtx;

            if (eventDeclaration.IsCustom)
                ValidateCustomEvent(eventDeclaration);
            else
                ValidateBuiltInEvent(eventDeclaration);

            _compilationContext.CompilationUnit.Events[eventDeclaration.Name] = eventDeclaration;
        }

        private void ValidateCustomEvent(EventDeclaration eventDeclaration)
        {
            // Кастомные события (во всяком случае сейчас) не должны иметь параметров и возвращаемый тип должен быть void
            // также кастомные события не могут называться так же как external имена стандартных событий, т.к. при экспорте будут использоваться codename этих событий
            // в отлчиии от стандартных, у которых при экспорте используются externalname

            var externalEvent = _compilationContext.FindExternalApiEventByExternalName(eventDeclaration.Name);

            if (externalEvent != null)
                throw _compilationContext.ThrowCompilationError($"custom event name cannot match to internal built-in event name. if you are looking for built-in event, it should be named " + externalEvent.CodeName + ", not " + externalEvent.ExternalName);

            if (!eventDeclaration.ReturnTypeOrVoid.IsVoid || eventDeclaration.Parameters.Parameters.Count > 0)
                throw _compilationContext.ThrowCompilationError($"custom events should not have any parameters and output type must be void");
        }

        private void ValidateBuiltInEvent(EventDeclaration eventDeclaration)
        {
            // Не знаю, бывают ли случаи когда событие может что-то возвращать.
            // Если увижу пример - сниму это ограничение (тогда в этом месте надо будет проверять что возвращаемый тип конкретного события является тем что описан в апи)
            // Вообще в апи там есть ивенты с output параметрами но похоже что это не возвращаемое значение, а out params
            if (!eventDeclaration.ReturnTypeOrVoid.IsVoid)
                throw _compilationContext.ThrowCompilationError($"event's return type must be void, but declared as { eventDeclaration.ReturnTypeOrVoid }");

            // проверяем что параметры соответствуют указанным в external api

            var declarationParamsAreOk =
                eventDeclaration.ExternalEvent.InParameters.Count == eventDeclaration.Parameters.Parameters.Count
                && !eventDeclaration.Parameters.Parameters.Any(x => x.IsOutput); //< don't support out params for built-in events yet

            if (declarationParamsAreOk)
            {
                for (int paramIdx = 0; paramIdx < eventDeclaration.ExternalEvent.InParameters.Count; paramIdx++)
                {
                    var externalApiParam = eventDeclaration.ExternalEvent.InParameters[paramIdx];
                    var declarationParam = eventDeclaration.Parameters.Parameters[paramIdx];

                    // Проверяем только типы, название параметров может отличаться от заданного в api
                    if (declarationParam.Variable.Type.ExternalType.ExternalName != externalApiParam.Type.ExternalName)
                    {
                        declarationParamsAreOk = false;
                        break;
                    }
                }
            }

            if (!declarationParamsAreOk)
                throw _compilationContext.ThrowCompilationError($"declared event parameters doesn't match to predefined built-in event signature");
        }

        // возвращает List<MethodDeclarationParameter>
        public override object VisitFormalParameters([NotNull] DoshikParser.FormalParametersContext context)
        {
            _compilationContext.SetParsingAntlrContext(context);

            var parameterList = context.formalParameterList();

            if (parameterList == null)
                return new List<MethodDeclarationParameter>(0);

            return Visit(parameterList);
        }

        // возвращает List<MethodDeclarationParameter>
        public override object VisitFormalParameterList([NotNull] DoshikParser.FormalParameterListContext context)
        {
            _compilationContext.SetParsingAntlrContext(context);

            var formalParameters = context.formalParameter();

            return formalParameters.Select(x => (MethodDeclarationParameter)Visit(x)).ToList();
        }

        // возвращает MethodDeclarationParameter
        public override object VisitFormalParameter([NotNull] DoshikParser.FormalParameterContext context)
        {
            _compilationContext.SetParsingAntlrContext(context);

            var parameter = new MethodDeclarationParameter(_currentMethodDeclaration.Parameters);

            var scope = parameter.Parent.Scope;

            parameter.IsOutput = context.OUT() != null;

            var foundType = GetTypeNameVisitor.Apply(_compilationContext, context.typeType());
            foundType.ThrowIfNotFound(_compilationContext);

            var variable = new Variable(parameter)
            {
                Type = foundType.DataType,
                Name = context.parameterName.Text
            };

            if (scope.FindVariableByName(variable.Name, true) != null)
                throw _compilationContext.ThrowCompilationError($"parameter { variable.Name } is already defined");

            scope.Variables[variable.Name] = variable;

            parameter.Variable = variable;

            return parameter;
        }

        private MethodDeclaration _currentMethodDeclaration;
    }
}