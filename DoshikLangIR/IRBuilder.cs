using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangIR
{
    public static class IRBuilder
    {
        public static CompilationUnit BuildCodeRepresentation(string sourceCode, DoshikExternalApi externalApi, out List<string> compilationErrors)
        {
            var compilationContext = new CompilationContext()
            {
                ExternalApi = externalApi
            };

            try
            {
                var inputStream = new AntlrInputStream(sourceCode);
                var lexer = new DoshikLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new DoshikParser(tokenStream);

                lexer.RemoveErrorListeners();
                parser.RemoveErrorListeners();

                var lexerErrorListener = new LexerErrorListener();
                var parserErrorListener = new ParserErrorListener();

                lexer.AddErrorListener(lexerErrorListener);
                parser.AddErrorListener(parserErrorListener);

                var antlrCompilationUnit = parser.compilationUnit();

                if (lexerErrorListener.Errors.Count > 0)
                {
                    throw compilationContext.ThrowCompilationError($"Error in lexer: { lexerErrorListener.Errors.First() }");
                }

                if (parserErrorListener.Errors.Count > 0)
                {
                    throw compilationContext.ThrowCompilationError($"Error in parser: { parserErrorListener.Errors.First() }");
                }

                // Высокоуровневая обработка - создается корневая струкура CompilationUnit и заполняются объявления (declarations) на уровне CompilationUnit - то есть
                // переменные и ивенты. Непосредственно императивные куски кода (statements, expressions) еще не обрабатываются
                CompilationUnitCreationVisitor.Apply(compilationContext, antlrCompilationUnit);

                // Обходим все объявленные события и генерируем код для их implementation части (то есть обрабатываем statement-ы внутри тела обработчиков событий)
                foreach (var eventHandler in compilationContext.CompilationUnit.Events.Values.OrderBy(x => x.Name))
                {
                    MethodBlockCreationVisitor.Apply(compilationContext, eventHandler);
                }

                compilationErrors = null;
                return compilationContext.CompilationUnit;
            }
            catch (CompilationErrorException)
            {
                compilationContext.CompilationUnit = null;
                compilationErrors = compilationContext.CompilationErrors;
                return null;
            }
        }
    }
}
