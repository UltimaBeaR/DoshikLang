using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;

namespace DoshikLanguageServer.Handlers
{
    class TextDocumentSyncHandler : ITextDocumentSyncHandler
    {
        public TextDocumentSyncHandler(ILanguageServer router, BufferManager bufferManager)
        {
            _router = router;
            _bufferManager = bufferManager;
        }

        public void SetCapability(SynchronizationCapability capability)
        {
            _capability = capability;
        }

        public TextDocumentAttributes GetTextDocumentAttributes(Uri uri)
        {
            return new TextDocumentAttributes(uri, "doshik");
        }

        public Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken token)
        {
            _bufferManager.UpdateBuffer(request.TextDocument.Uri.ToString(), new SomeData { Data = request.TextDocument.Text });

            return Unit.Task;
        }

        public Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken token)
        {
            return Unit.Task;
        }

        public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken token)
        {
            var documentPath = request.TextDocument.Uri.ToString();
            var text = request.ContentChanges.FirstOrDefault()?.Text;

            _bufferManager.UpdateBuffer(documentPath, new SomeData { Data = text });

            _router.Window.LogInfo($"Updated buffer for document: {documentPath} text:\n{text}");

            return Unit.Task;
        }

        public Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken token)
        {
            return Unit.Task;
        }

        TextDocumentRegistrationOptions IRegistration<TextDocumentRegistrationOptions>.GetRegistrationOptions()
        {
            return new TextDocumentRegistrationOptions()
            {
                DocumentSelector = _documentSelector,
            };
        }

        TextDocumentChangeRegistrationOptions IRegistration<TextDocumentChangeRegistrationOptions>.GetRegistrationOptions()
        {
            return new TextDocumentChangeRegistrationOptions()
            {
                DocumentSelector = _documentSelector,
                SyncKind = TextDocumentSyncKind.Full
            };
        }

        TextDocumentSaveRegistrationOptions IRegistration<TextDocumentSaveRegistrationOptions>.GetRegistrationOptions()
        {
            return new TextDocumentSaveRegistrationOptions()
            {
                DocumentSelector = _documentSelector,
                IncludeText = true
            };
        }

        private readonly DocumentSelector _documentSelector = new DocumentSelector(
            new DocumentFilter()
            {
                Pattern = "**/*.doshik"
            }
        );

        private readonly ILanguageServer _router;
        private readonly BufferManager _bufferManager;

        private SynchronizationCapability _capability;
    }
}