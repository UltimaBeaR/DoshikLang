using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;

namespace DoshikLangCompiler.Compilation
{
    public class ParserErrorListener : IAntlrErrorListener<IToken>
    {
        public ParserErrorListener()
        {
            Errors = new List<string>();
        }

        public List<string> Errors { get; set; }

        public void SyntaxError(
            TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
            int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add(msg);
        }
    }
}