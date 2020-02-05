using Antlr4.Runtime.Misc;

namespace DoshikLangIR
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

            // Помечаем текущий блок и заходим в его scope

            _currentNode = block;
            _currentScope = block.Scope;

            foreach (var blockStatementCtx in context.statementInBlock())
            {
                var statement = (Statement)Visit(blockStatementCtx);
                block.Statements.Add(statement);
            }

            // Возвращаем предыдущий блок и заходим в рдоительский scope
            _currentScope = _currentScope.ParentScope;
            _currentNode = _currentNode.Parent;

            return block;
        }

        // возвращает Statement
        public override object VisitStatementInBlock([NotNull] DoshikParser.StatementInBlockContext context)
        {
            var localVariableDeclarationCtx = context.localVariableDeclaration();
            var statementCtx = context.statement();
            if (localVariableDeclarationCtx != null)
                return (LocalVariableDeclarationStatement)Visit(localVariableDeclarationCtx);
            else if (statementCtx != null)
                return (Statement)Visit(statementCtx);
            else
                throw new System.NotImplementedException();
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

            if (statement.Initializer == null && IsInsideOfLoop())
            {
                // Если инициализатор не задан (то есть переменная объявлена без инициализирующего выражения)
                // и при этом мы находимся в теле цикла
                // то создаем неявное инициализирующее выражение = default(T), где T = statement.Variable.Type
                statement.Initializer = ExpressionBuilder.BuildDefaultOfType(_compilationContext, _currentExpressionParent, statement.Variable.Type);

                // это нужно, чтобы при объявлении локальной переменной в теле цикла при любой итерации цикла она имела дефолтное значение, если
                // ей не был указан инициализатор, иначе получится что ее значение будет переиспользоватья с предыдущей итерации

                // впринципе можно в любом случае делать этот искусственный инициализатор (даже если не в цикле), но вроде как если переменная
                // не находится в цикле то ее значение всегда будет дефолтным, если не было инициализатора
                // возможно это изменится при реализации рекурсивных юзерских функций
            }

            return statement;
        }

        // возвращает ExpressionStatement
        public override object VisitExpressionStatement([NotNull] DoshikParser.ExpressionStatementContext context)
        {
            var statement = new ExpressionStatement(_currentNode);

            statement.Expression = ExpressionCreationVisitor.Apply(_compilationContext, statement, context.expression());

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

        // возвращает Statement (BlockOfStatements)
        public override object VisitSubBlockStatement([NotNull] DoshikParser.SubBlockStatementContext context)
        {
            return Visit(context.block());
        }

        // возвращает Statement (IfStatement)
        public override object VisitIfStatement([NotNull] DoshikParser.IfStatementContext context)
        {
            var statement = new IfStatement(_currentNode);

            _currentExpressionParent = statement;

            statement.Condition = ExpressionCreationVisitor.Apply(_compilationContext, _currentExpressionParent, context.condition);

            if (statement.Condition.RootExpression.ReturnOutputSlot.Type != _compilationContext.TypeLibrary.FindTypeByDotnetType(typeof(bool)))
                throw _compilationContext.ThrowCompilationError("condition must evaluate to bool value");

            statement.TrueStatement = (Statement)Visit(context.trueBody);

            if (context.falseBody != null)
            {
                statement.FalseStatement = (Statement)Visit(context.falseBody);
            }

            return statement;
        }

        public override object VisitWhileLoopStatement([NotNull] DoshikParser.WhileLoopStatementContext context)
        {
            var statement = new WhileStatement(_currentNode);

            _currentExpressionParent = statement;

            statement.Condition = ExpressionCreationVisitor.Apply(_compilationContext, _currentExpressionParent, context.condition);

            if (statement.Condition.RootExpression.ReturnOutputSlot.Type != _compilationContext.TypeLibrary.FindTypeByDotnetType(typeof(bool)))
                throw _compilationContext.ThrowCompilationError("condition must evaluate to bool value");

            _isInLoopCounter++;

            statement.BodyStatement = (Statement)Visit(context.body);

            _isInLoopCounter--;

            return statement;
        }

        public override object VisitBreakStatement([NotNull] DoshikParser.BreakStatementContext context)
        {
            if (!IsInsideOfLoop())
                throw _compilationContext.ThrowCompilationError("break operator can be used only inside of loop body");

            return new BreakStatement(_currentNode);
        }

        public override object VisitContinueStatement([NotNull] DoshikParser.ContinueStatementContext context)
        {
            if (!IsInsideOfLoop())
                throw _compilationContext.ThrowCompilationError("continue operator can be used only inside of loop body");

            return new ContinueStatement(_currentNode);
        }

        public override object VisitNopStatement([NotNull] DoshikParser.NopStatementContext context)
        {
            return new EmptyStatement(_currentNode);
        }

        private bool IsInsideOfLoop()
        {
            return _isInLoopCounter > 0;
        }

        // Сюда устанавливается (заранее) родитель для возможного expression-а который встретится в самом стейтменте или его части (например for init часть цикла)
        private ICodeHierarchyNode _currentExpressionParent;

        private Variable _declaringVariable;
        private ExpressionTree _declaringVariableInitializerExpression;

        private ICodeHierarchyNode _currentNode;
        private Scope _currentScope;

        // инкрементируется каждый раз перед входом в тело цикла
        // и декрементируется при выходе из тела цикла
        // используется для проверки - находимся ли мы сейчас в цикле с учетом вложенностей. Если == 0, значит НЕ в цикле.
        private int _isInLoopCounter = 0;
    }
}
