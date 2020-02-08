using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
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
            return _sourceCodePerDocument.TryGetValue(documentPath, out var sourceCode) ? sourceCode : null;
        }
    }

    class SourceCode
    {
        public string FullSourceCode { get; set; }

        public int PositionToCharIdx(Position position)
        {
            int searchingLineIdx = (int)position.Line;
            int searchingCharacterIdx = (int)position.Character;

            int globalCharacterIdx = 0;

            int lineIdx = 0;
            int characterIdx = 0;

            while (true)
            {
                if (lineIdx == searchingLineIdx && characterIdx == searchingCharacterIdx)
                    return globalCharacterIdx;

                if (globalCharacterIdx >= FullSourceCode.Length)
                    return -1;

                var currentChar = FullSourceCode[globalCharacterIdx];

                if (currentChar == '\n')
                {
                    globalCharacterIdx += 1;
                    characterIdx = 0;
                    lineIdx++;
                    continue;
                }

                if (currentChar == '\r' && globalCharacterIdx < FullSourceCode.Length - 1)
                {
                    var nextChar = FullSourceCode[globalCharacterIdx + 1];

                    if (nextChar == '\n')
                    {
                        globalCharacterIdx += 2;
                        characterIdx = 0;
                        lineIdx++;
                        continue;
                    }
                }

                globalCharacterIdx++;
                characterIdx++;
            }
        }
    }
}
