using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;

namespace DoshikLangIR
{
    public class ParserErrorListener : IAntlrErrorListener<IToken>
    {
        public ParserErrorListener()
        {
            Errors = new List<CompilationError>();
        }

        public List<CompilationError> Errors { get; set; }

        public void SyntaxError(
            TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
            int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add(new CompilationError { Message = msg, LineIdx = line - 1, CharInLineIdx = charPositionInLine });
        }
    }
}