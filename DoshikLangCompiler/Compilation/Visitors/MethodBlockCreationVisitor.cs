using Antlr4.Runtime.Misc;
using DoshikLangCompiler.Compilation.CodeRepresentation;
using DoshikLangCompiler.Compilation.CodeRepresentation.Expressions;
using System.Collections.Generic;

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

            var foundType = GetTypeNameVisitor.Apply(_compilationContext, context.typeType());
            foundType.ThrowIfNotFound(_compilationContext);
            statement.Variable.Type = foundType.DataType;

            _declaringVariable = statement.Variable;

            _currentExpressionParent = statement;

            // Он инициализирует _declaringVariable
            Visit(context.variableDeclarator());

            // Добавляем переменную ПОСЛЕ выполнения инициализации, таким образом при выполнении выражения инициализации эта переменная еще будет недоступна для референеса
            // то есть нельзя будет сделать так int a = a + 1;
            // этим отличается инициализация переменной от обычного присваивания, где референсить присваемую переменную можно
            _currentScope.Variables.Add(statement.Variable.Name, statement.Variable);

            statement.Initializer = _declaringVariableInitializerExpression;

            return statement;
        }

        // Ничего не возвращает, просто инициализирует _declaringVariable и _declaringVariableInitializerExpression
        public override object VisitVariableDeclarator([NotNull] DoshikParser.VariableDeclaratorContext context)
        {
            _declaringVariable.Name = context.variableName.Text;

            // ToDo: Тут надо искать переменную не везде а только в текущем scope плюс в родительях, исключая все что выше определения метода.
            // то есть переменную заданную в параметрах переопределить нельзя, НО должно быть возможно переопределить переменную, заданную в "текущем классе"(то есть глобальную). Ее потом надо сделать чтобы
            // можно было вызывать по this.variable
            if (_currentScope.FindVariableByName(_declaringVariable.Name) != null)
                throw _compilationContext.ThrowCompilationError($"variable { _declaringVariable.Name } is already defined");

            var variableInitializerCtx = context.variableInitializer();

            // если != null значит инициализтор задан
            if (variableInitializerCtx != null)
            {
                // продолжаем изменять _declaringVariable
                _declaringVariableInitializerExpression = (ExpressionTree)Visit(variableInitializerCtx);
            }
            else
            {
                // иначе это просто объявление переменной без инициализации

                _declaringVariableInitializerExpression = null;
            }

            return null;
        }

        // Возвращает ExpressionTree
        public override object VisitVariableInitializer([NotNull] DoshikParser.VariableInitializerContext context)
        {
            var expressionCtx = context.expression();

            if (expressionCtx != null)
            {
                var expressionTree = ExpressionCreationVisitor.Apply(_compilationContext, _currentExpressionParent, expressionCtx);

                if (_declaringVariable.Type != expressionTree.RootExpression.ReturnOutputSlot.Type)
                    throw _compilationContext.ThrowCompilationError("declaring variable type differs from initialization expression return type");

                return expressionTree;
            }
            else
                throw _compilationContext.ThrowCompilationError("variables can only be initialized using expressions (no special initializer support yet)");
        }

        // Сюда устанавливается (заранее) родитель для возможного expression-а который встретится в самом стейтменте или его части (например for init часть цикла)
        private ICodeHierarchyNode _currentExpressionParent;

        private Variable _declaringVariable;
        private ExpressionTree _declaringVariableInitializerExpression;

        private ICodeHierarchyNode _currentNode;
        private Scope _currentScope;
    }
}
