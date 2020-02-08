using Antlr4.Runtime;
using Doshik;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangIR
{
    public static class IRBuilder
    {
        public static CompilationUnit BuildCodeRepresentation(string sourceCode, DoshikExternalApi externalApi, out List<CompilationError> compilationErrors)
        {
            var compilationContext = new CompilationContext()
            {
                ExternalApi = externalApi
            };

            try
            {
                compilationContext.InputStream = new AntlrInputStream(sourceCode);
                compilationContext.Lexer = new DoshikLexer(compilationContext.InputStream);
                compilationContext.TokenStream = new CommonTokenStream(compilationContext.Lexer);
                compilationContext.Parser = new DoshikParser(compilationContext.TokenStream);

                compilationContext.Lexer.RemoveErrorListeners();
                compilationContext.Parser.RemoveErrorListeners();

                var lexerErrorListener = new LexerErrorListener();
                var parserErrorListener = new ParserErrorListener();

                compilationContext.Lexer.AddErrorListener(lexerErrorListener);
                compilationContext.Parser.AddErrorListener(parserErrorListener);

                var antlrCompilationUnit = compilationContext.Parser.compilationUnit();

                if (lexerErrorListener.Errors.Count > 0)
                {
                    var error = lexerErrorListener.Errors.First();
                    throw compilationContext.ThrowCompilationErrorForKnownRange($"SYNTAX: { error.Message }", error.LineIdx, error.CharInLineIdx, null, null);
                }

                if (parserErrorListener.Errors.Count > 0)
                {
                    var error = parserErrorListener.Errors.First();
                    throw compilationContext.ThrowCompilationErrorForKnownRange($"SYNTAX: { error.Message }", error.LineIdx, error.CharInLineIdx, null, null);
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
