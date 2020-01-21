using Antlr4.Runtime.Misc;
using DoshikLangCompiler.Compilation.CodeRepresentation;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangCompiler.Compilation.Visitors
{
    public class CompilationUnitCreationVisitor : CompilationContextVisitorBase<object>
    {
        public CompilationUnitCreationVisitor(CompilationContext compilationContext)
            : base(compilationContext)
        {
        }

        public static CompilationUnit Apply(CompilationContext compilationContext, DoshikParser.CompilationUnitContext antlrContext)
        {
            return (CompilationUnit)antlrContext.Accept(new CompilationUnitCreationVisitor(compilationContext));
        }

        // возвращает CompilationUnit
        public override object VisitCompilationUnit([NotNull] DoshikParser.CompilationUnitContext context)
        {
            _compilationUnit = new CompilationUnit();

            var memberDeclarations = context.memberDeclaration();

            foreach (var memberDeclaration in memberDeclarations)
            {
                Visit(memberDeclaration);
            }

            return _compilationUnit;
        }

        public override object VisitMemberDeclaration([NotNull] DoshikParser.MemberDeclarationContext context)
        {
            if (context.fieldDeclaration() != null)
                Visit(context.fieldDeclaration());

            if (context.methodDeclaration() != null)
                Visit(context.methodDeclaration());

            return null;
        }

        public override object VisitFieldDeclaration([NotNull] DoshikParser.FieldDeclarationContext context)
        {
            var scope = _compilationUnit.Scope;

            var variable = new CompilationUnitVariable(_compilationUnit);

            variable.IsPublic = context.PUBLIC() != null;

            variable.Type = GetTypeNameVisitor.Apply(_compilationContext, context.typeType());

            (var variableName, var variableInitializer) = ((string, DoshikParser.VariableInitializerContext))Visit(context.variableDeclarator());

            variable.Name = variableName;

            variable.AntlrInitializer = variableInitializer;

            if (scope.Variables.ContainsKey(variable.Name))
                _compilationContext.ThrowCompilationError($"variable { variable.Name } is already defined");

            scope.Variables[variable.Name] = variable;

            return null;
        }

        // возвращает (string variableName, DoshikParser.VariableInitializerContext variableInitializer)
        public override object VisitVariableDeclarator([NotNull] DoshikParser.VariableDeclaratorContext context)
        {
            var variableName = context.variableName.Text;
            var variableInitializer = context.variableInitializer();

            return (variableName, variableInitializer);
        }

        public override object VisitMethodDeclaration([NotNull] DoshikParser.MethodDeclarationContext context)
        {
            var isEvent = context.EVENT() != null;

            // Пока не поддерживаю кастомные функции (не ивенты)
            if (!isEvent)
                _compilationContext.ThrowCompilationError($"custom functions is not supported yet");

            // Если это event

            var eventDeclaration = new MethodDeclaration(_compilationUnit);
            eventDeclaration.Parameters = new MethodDeclarationParameters(eventDeclaration, _compilationUnit.Scope);

            _currentMethodDeclaration = eventDeclaration;

            eventDeclaration.ReturnTypeOrVoid = GetTypeNameVisitor.Apply(_compilationContext, context.typeTypeOrVoid());

            // Не знаю, бывают ли случаи когда событие может что-то возвращать.
            // Если увижу пример - сниму это ограничение (тогда в этом месте надо будет проверять что возвращаемый тип конкретного события является тем что описан в апи)
            // Вообще в апи там есть ивенты с output параметрами но похоже что это не возвращаемое значение, а out params
            if (eventDeclaration.ReturnTypeOrVoid != null)
                _compilationContext.ThrowCompilationError($"event's return type must be void, but declared as { eventDeclaration.ReturnTypeOrVoid }");

            eventDeclaration.Name = context.methodName.Text;

            var externalApiEvent = _compilationContext.FindExternalApiEvent(eventDeclaration.Name);

            eventDeclaration.IsCustom = externalApiEvent == null;

            if (eventDeclaration.IsCustom)
                _compilationContext.ThrowCompilationError($"custom events is not supported yet. event name must be one of predefined names");

            if (_compilationUnit.Events.ContainsKey(eventDeclaration.Name))
                _compilationContext.ThrowCompilationError($"event handler { eventDeclaration.Name } is already defined");

            eventDeclaration.Parameters.Parameters.AddRange((List<MethodDeclarationParameter>)Visit(context.formalParameters()));

            if (!eventDeclaration.IsCustom)
            {
                // Если ивент не кастомный, проверяем что параметры соответствуют указанным в external api

                var declarationParamsAreOk =
                    externalApiEvent.InParameters.Count == eventDeclaration.Parameters.Parameters.Count
                    && !eventDeclaration.Parameters.Parameters.Any(x => x.IsOutput); //< don't support out params for built-in events yet

                if (declarationParamsAreOk)
                {
                    for (int paramIdx = 0; paramIdx < externalApiEvent.InParameters.Count; paramIdx++)
                    {
                        var externalApiParam = externalApiEvent.InParameters[paramIdx];
                        var declarationParam = eventDeclaration.Parameters.Parameters[paramIdx];

                        // Проверяем только типы, название параметров может отличаться от заданного в api
                        if (declarationParam.Variable.Type != externalApiParam.Type.ExternalName)
                        {
                            declarationParamsAreOk = false;
                            break;
                        }
                    }
                }

                if (!declarationParamsAreOk)
                    _compilationContext.ThrowCompilationError($"declared event parameters doesn't match to predefined built-in event { externalApiEvent.ExternalName } signature");
            }

            eventDeclaration.AntlrBody = context.block();

            _compilationUnit.Events[eventDeclaration.Name] = eventDeclaration;

            return null;
        }

        // возвращает List<MethodDeclarationParameter>
        public override object VisitFormalParameters([NotNull] DoshikParser.FormalParametersContext context)
        {
            var parameterList = context.formalParameterList();

            if (parameterList == null)
                return new List<MethodDeclarationParameter>(0);

            return Visit(parameterList);
        }

        // возвращает List<MethodDeclarationParameter>
        public override object VisitFormalParameterList([NotNull] DoshikParser.FormalParameterListContext context)
        {
            var formalParameters = context.formalParameter();

            return formalParameters.Select(x => (MethodDeclarationParameter)Visit(x)).ToList();
        }

        // возвращает MethodDeclarationParameter
        public override object VisitFormalParameter([NotNull] DoshikParser.FormalParameterContext context)
        {
            var parameter = new MethodDeclarationParameter(_currentMethodDeclaration.Parameters);

            var scope = parameter.Parent.Scope;

            parameter.IsOutput = context.OUT() != null;

            var variable = new Variable(parameter)
            {
                Type = GetTypeNameVisitor.Apply(_compilationContext, context.typeType()),
                Name = context.parameterName.Text
            };

            if (scope.Variables.ContainsKey(variable.Name))
                _compilationContext.ThrowCompilationError($"parameter { variable.Name } is already defined");

            scope.Variables[variable.Name] = variable;

            parameter.Variable = variable;

            return parameter;
        }

        private MethodDeclaration _currentMethodDeclaration;

        private CompilationUnit _compilationUnit;
    }
}