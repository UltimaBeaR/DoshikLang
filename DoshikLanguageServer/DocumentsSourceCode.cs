using System.Collections.Concurrent;

namespace DoshikLanguageServer
{
    class DocumentsSourceCode
    {
        private ConcurrentDictionary<string, SourceCode> _sourceCodePerDocument = new ConcurrentDictionary<string, SourceCode>();

        public void UpdateDocumentSourceCode(string documentPath, SourceCode sourceCode)
        {
            _sourceCodePerDocument.AddOrUpdate(documentPath, sourceCode, (k, v) => sourceCode);
        }

        public SourceCode GetDocumentSourceCode(string documentPath)
        {
            return _sourceCodePerDocument.TryGetValue(documentPath, out var buffer) ? buffer : null;
        }
    }

    class SourceCode
    {
        public string FullSourceCode { get; set; }
    }
}
