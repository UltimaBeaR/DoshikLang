using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;

namespace DoshikLangIR
{
    public class LexerErrorListener : IAntlrErrorListener<int>
    {
        public LexerErrorListener()
        {
            Errors = new List<string>();
        }

        public List<string> Errors { get; set; }

        public void SyntaxError(
            TextWriter output, IRecognizer recognizer, int offendingSymbol, int line,
            int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add(msg);
        }
    }
}