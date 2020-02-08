using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DoshikLanguageServer.Handlers
{
    internal class CompletionHandler : ICompletionHandler
    {
        public CompletionHandler(ILanguageServer server, DocumentsSourceCode documentsSourceCode, DoshikExternalApiProvider externalApiProvider)
        {
            _server = server;
            _documentsSourceCode = documentsSourceCode;
            _externalApiProvider = externalApiProvider;
        }

        public void SetCapability(CompletionCapability capability)
        {
            _capability = capability;
        }

        public CompletionRegistrationOptions GetRegistrationOptions()
        {
            return new CompletionRegistrationOptions
            {
                DocumentSelector = _documentSelector,
                ResolveProvider = false
            };
        }

        public Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
        {
            var externalApi = _externalApiProvider.GetExternalApi();

            var documentPath = request.TextDocument.Uri.ToString();
            var buffer = _documentsSourceCode.GetDocumentSourceCode(documentPath);

            var sourceCode = buffer.FullSourceCode;

            var items = externalApi.Types
                .Where(type => type.DeclaredAsConstNode || type.DeclaredAsTypeNode || type.DeclaredAsVariableNode)
                .Select(type =>
                {
                    var codeName = string.Join("::", type.FullyQualifiedCodeName);

                    return new CompletionItem
                    {
                        Label = codeName,
                        Kind = CompletionItemKind.Class,
                        TextEdit = new TextEdit
                        {
                            NewText = codeName,
                            Range = new Range(
                                new Position
                                {
                                    Line = request.Position.Line,
                                    Character = request.Position.Character
                                }, new Position
                                {
                                    Line = request.Position.Line,
                                    Character = request.Position.Character
                                }
                            )
                        }
                    };
                })
                .OrderBy(x => x.Label)
                .ToArray();

            var result =  new CompletionList(items, isIncomplete: false);

            return Task.FromResult(result);
        }

        private readonly ILanguageServer _server;
        private readonly DocumentsSourceCode _documentsSourceCode;
        private readonly DoshikExternalApiProvider _externalApiProvider;

        private readonly DocumentSelector _documentSelector = new DocumentSelector(
            new DocumentFilter()
            {
                Pattern = "**/*.doshik"
            }
        );

        private CompletionCapability _capability;
    }
}