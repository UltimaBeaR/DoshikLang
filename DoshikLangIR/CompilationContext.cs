using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Doshik;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangIR
{
    public class CompilationContext
    {
        public CompilationContext()
        {
            TypeLibrary = new TypeLibrary(this);
        }

        public AntlrInputStream InputStream { get; set; }
        public DoshikLexer Lexer { get; set; }
        public CommonTokenStream TokenStream { get; set; }
        public DoshikParser Parser { get; set; }

        public DoshikExternalApi ExternalApi { get { return _externalApi; } set { _externalApi = value; TypeLibrary.UpdateExternalApiTypes(); } }

        public TypeLibrary TypeLibrary { get; private set; }

        public CompilationUnit CompilationUnit { get; set; }

        public List<CompilationError> CompilationErrors { get; } = new List<CompilationError>();

        public CompilationErrorException ThrowCompilationErrorForKnownRange(string message, int lineIdx, int charInLineIdx, int? lineIdxTo, int? charInLineIdxTo)
        {
            CompilationErrors.Add(new CompilationError
            {
                Message = message,
                LineIdx = lineIdx,
                CharInLineIdx = charInLineIdx,
                LineIdxTo = lineIdxTo,
                CharInLineIdxTo = charInLineIdxTo
            });

            throw new CompilationErrorException();
        }

        public CompilationErrorException ThrowCompilationError(string message)
        {
            if (_parsingAntlrContext != null)
            {
                var interval = _parsingAntlrContext.SourceInterval;
                var fromToken = TokenStream.Get(interval.a);
                var toToken = TokenStream.Get(interval.b);

                throw ThrowCompilationErrorForKnownRange(message, fromToken.Line - 1, fromToken.Column, toToken.Line - 1, toToken.Column);
            }

            throw ThrowCompilationErrorForKnownRange(message, 0, 0, null, null);
        }

        public DoshikExternalApiEvent FindExternalApiEvent(string eventCodeName)
        {
            return ExternalApi.Events.FirstOrDefault(x => x.CodeName == eventCodeName);
        }

        /// <summary>
        /// Устанавливает текущее поддерево anlr-а которое парсится в данный момент.
        /// Используется для того чтобы установить область в которой произошла ошибка, при генерации ошибки
        /// </summary>
        public void SetParsingAntlrContext(IParseTree context)
        {
            _parsingAntlrContext = context;
        }

        /// <summary>
        /// Резервирует текущий parsing context, чтобы потом восстановить через pop
        /// </summary>
        public void PushParsingContext()
        {
            _reservedParsingAntlrContextStack.Push(_parsingAntlrContext);
            _parsingAntlrContext = null;
        }

        /// <summary>
        /// Возвращает ранее зарезервированный parsing context
        /// </summary>
        public void PopParsingContext()
        {
            _parsingAntlrContext = _reservedParsingAntlrContextStack.Pop();
        }

        private IParseTree _parsingAntlrContext = null;
        private Stack<IParseTree> _reservedParsingAntlrContextStack = new Stack<IParseTree>();

        private DoshikExternalApi _externalApi;
    }
}
