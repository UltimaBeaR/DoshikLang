using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using System.Threading;
using System.Threading.Tasks;

namespace DoshikLanguageServer.Handlers
{
    internal class CompletionHandler : ICompletionHandler
    {
        public CompletionHandler(ILanguageServer router, BufferManager bufferManager)
        {
            _router = router;
            _bufferManager = bufferManager;
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
            var documentPath = request.TextDocument.Uri.ToString();
            var buffer = _bufferManager.GetBuffer(documentPath);

            var sourceCode = buffer.Data;

            var result =  new CompletionList(new CompletionItem[] {
                new CompletionItem
                {
                    Label = "UnityEngine::Transform.Rotate(UnityEngine::Vector3 axis, float angle)",
                    Kind = CompletionItemKind.Method,
                    TextEdit = new TextEdit
                    {
                        NewText = "Rotate(",
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
                }
            }, isIncomplete: true);

            return Task.FromResult(result);
        }

        private readonly ILanguageServer _router;
        private readonly BufferManager _bufferManager;

        private readonly DocumentSelector _documentSelector = new DocumentSelector(
            new DocumentFilter()
            {
                Pattern = "**/*.doshik"
            }
        );

        private CompletionCapability _capability;
    }
}