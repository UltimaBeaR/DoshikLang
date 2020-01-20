using Antlr4.Runtime.Misc;
using DoshikLangCompiler.Compilation.CodeRepresentation;
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
            _currentScope = block.Scope;

            foreach (var blockStatementCtx in context.blockStatement())
            {
                var statement = (Statement)Visit(blockStatementCtx);
                block.Statements.Add(statement);
            }

            return block;
        }

        // возвращает Statement
        public override object VisitBlockStatement([NotNull] DoshikParser.BlockStatementContext context)
        {
            // ToDo: продолжать тут

            return null;
        }

        private ICodeHierarchyNode _currentNode;
        private Scope _currentScope;
    }
}
