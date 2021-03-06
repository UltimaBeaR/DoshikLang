﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DoshikLangIR;
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
        public TextDocumentSyncHandler(ILanguageServer server, DocumentsSourceCode documentsSourceCode, DoshikExternalApiProvider externalApiProvider)
        {
            _server = server;
            _documentsSourceCode = documentsSourceCode;
            _externalApiProvider = externalApiProvider;
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
            _documentsSourceCode.UpdateDocumentSourceCode(request.TextDocument.Uri.ToString(), new SourceCode { FullSourceCode = request.TextDocument.Text });

            UpdateDiagnostics(request.TextDocument.Uri);

            return Unit.Task;
        }

        public Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken token)
        {
            UpdateDiagnostics(request.TextDocument.Uri, true);

            return Unit.Task;
        }

        public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken token)
        {
            var documentPath = request.TextDocument.Uri.ToString();
            var text = request.ContentChanges.FirstOrDefault()?.Text;

            _documentsSourceCode.UpdateDocumentSourceCode(documentPath, new SourceCode { FullSourceCode = text });

            UpdateDiagnostics(request.TextDocument.Uri);

            return Unit.Task;
        }

        public Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken token)
        {
            UpdateDiagnostics(request.TextDocument.Uri);

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

        private void UpdateDiagnostics(Uri documentUri, bool clear = false)
        {
            var diagnostics = new List<Diagnostic>();

            if (!clear)
            {
                var externalApi = _externalApiProvider.GetExternalApi();

                if (externalApi != null)
                {
                    var documentPath = documentUri.ToString();

                    IRBuilder.BuildCodeRepresentation(
                        _documentsSourceCode.GetDocumentSourceCode(documentPath).FullSourceCode,
                        externalApi,
                        out var compilationErrors
                    );

                    if (compilationErrors != null)
                    {
                        foreach (var compilationError in compilationErrors)
                        {
                            var diagnostic = new Diagnostic();

                            diagnostic.Severity = DiagnosticSeverity.Error;

                            diagnostic.Message = compilationError.Message;

                            diagnostic.Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range()
                            {
                                Start = new Position(compilationError.LineIdx, compilationError.CharInLineIdx),
                                End = new Position(
                                    compilationError.LineIdxTo == null ? compilationError.LineIdx : compilationError.LineIdxTo.Value,
                                    compilationError.CharInLineIdxTo == null ? compilationError.CharInLineIdx : compilationError.CharInLineIdxTo.Value
                                )
                            };

                            diagnostics.Add(diagnostic);
                        }
                    }
                }
                else
                {
                    var diagnostic = new Diagnostic();

                    diagnostic.Severity = DiagnosticSeverity.Error;

                    diagnostic.Message = "Doshik external Api cache was not found. Try to update doshik script from unity3d, that shoud rebuild cache";

                    diagnostic.Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range()
                    {
                        Start = new Position(0, 0),
                        End = new Position(0, 0)
                    };

                    diagnostics.Add(diagnostic);   
                }
            }

            _server.Document.PublishDiagnostics(new PublishDiagnosticsParams() { Uri = documentUri, Diagnostics = new Container<Diagnostic>(diagnostics) });
        }

        private readonly ILanguageServer _server;
        private readonly DocumentsSourceCode _documentsSourceCode;
        private readonly DoshikExternalApiProvider _externalApiProvider;

        private SynchronizationCapability _capability;
    }
}