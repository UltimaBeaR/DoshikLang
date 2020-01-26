﻿using Antlr4.Runtime.Misc;
using DoshikLangCompiler.Compilation.CodeRepresentation;

namespace DoshikLangCompiler.Compilation.Visitors
{
    public class MethodBlockCreationVisitor : CompilationContextVisitorBase<object>
    {
        public MethodBlockCreationVisitor(CompilationContext compilationContext, ICodeHierarchyNode currentNode, Scope currentScope)
            : base(compilationContext)
        {
            _currentNode = currentNode;
            _currentScope = currentScope;
        }

        public static void Apply(CompilationContext compilationContext, MethodDeclaration methodDeclaration)
        {
            methodDeclaration.BodyBlock = (BlockOfStatements)methodDeclaration.AntlrBody.Accept(
                new MethodBlockCreationVisitor(compilationContext, methodDeclaration, methodDeclaration.Parameters.Scope)
            );
        }

        // возвращает BlockOfStatements
        public override object VisitBlock([NotNull] DoshikParser.BlockContext context)
        {
            var block = new BlockOfStatements(_currentNode, _currentScope);

            _currentNode = block;

            // Заходим в scope блока
            _currentScope = block.Scope;

            foreach (var blockStatementCtx in context.blockStatement())
            {
                var statement = (Statement)Visit(blockStatementCtx);
                block.Statements.Add(statement);
            }

            // Покидаем scope блока
            _currentScope = _currentScope.ParentScope;
            return block;
        }

        // возвращает Statement
        public override object VisitBlockStatement([NotNull] DoshikParser.BlockStatementContext context)
        {
            var localVariableDeclarationCtx = context.localVariableDeclaration();
            if (localVariableDeclarationCtx != null)
            {
                return (LocalVariableDeclarationStatement)Visit(localVariableDeclarationCtx);
            }

            // ToDo: сделать остальные виды statement блоков (но сначала видимо надо сделать expression в части инициализации локальной переменной)

            return null;
        }

        // возвращает LocalVariableDeclarationStatement
        public override object VisitLocalVariableDeclaration([NotNull] DoshikParser.LocalVariableDeclarationContext context)
        {
            var statement = new LocalVariableDeclarationStatement(_currentNode);

            statement.Variable = new Variable(statement);
            statement.Variable.Type = GetTypeNameVisitor.Apply(_compilationContext, context.typeType());

            _declaringVariable = statement.Variable;

            _currentExpressionParent = statement;

            // Он инициализирует _declaringVariable
            Visit(context.variableDeclarator());

            _currentScope.Variables.Add(statement.Variable.Name, statement.Variable);

            return statement;
        }

        // Ничего не возвращает, просто инициализирует _declaringVariable
        public override object VisitVariableDeclarator([NotNull] DoshikParser.VariableDeclaratorContext context)
        {
            _declaringVariable.Name = context.variableName.Text;

            if (_currentScope.FindVariableByName(_declaringVariable.Name) != null)
                _compilationContext.ThrowCompilationError($"variable { _declaringVariable.Name } is already defined");

            var variableInitializerCtx = context.variableInitializer();

            // если != null значит инициализтор задан - иначе это просто объявление переменной без инициализации
            if (variableInitializerCtx != null)
            {
                // продолжаем изменять _declaringVariable
                Visit(variableInitializerCtx);
            }

            return null;
        }

        // Ничего не возвращает, просто инициализирует _declaringVariable (часть справа от присваивания)
        public override object VisitVariableInitializer([NotNull] DoshikParser.VariableInitializerContext context)
        {
            var expressionCtx = context.expression();

            if (expressionCtx != null)
            {
                // ToDo: тут я поидее должнен выполнить expression, проверить что он возвращает такой же тип как и объявленный в переменной и после этого
                // надо добавить дополнительный стейтмент в список стейтментов у которого будет внутри expression с присвоением переменной
                // то есть создать новый expression и в левую часть ему референс на объявляемую переменную а в правую - полученное только что выражение инициализации переменной.
                // а при визите обычного expression statement я буду такой же стейтмент делать полностью из кода

                var expression = ExpressionCreationVisitor.Apply(_compilationContext, _currentExpressionParent, expressionCtx);
            }
            else
                _compilationContext.ThrowCompilationError("variables can only be initialized using expressions (no special initializer support yet)");

            return null;
        }

        // Сюда устанавливается (заранее) родитель для возможного expression-а который встретится в самом стейтменте или его части (например for init часть цикла)
        private ICodeHierarchyNode _currentExpressionParent;

        private Variable _declaringVariable;

        private ICodeHierarchyNode _currentNode;
        private Scope _currentScope;
    }
}
