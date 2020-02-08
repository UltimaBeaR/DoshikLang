using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var source = _documentsSourceCode.GetDocumentSourceCode(documentPath);

            var sourceCode = source.FullSourceCode;

            var typeName = FindTypeNameReverse(source.PositionToCharIdx(request.Position), source.FullSourceCode);

            var items = externalApi.Types
                .Where(type => type.DeclaredAsConstNode || type.DeclaredAsTypeNode || type.DeclaredAsVariableNode)
                .Select(type => (label: MakeTypeCodename(type), methodCount: type.Methods.Count, type: type))
                .OrderByDescending(x => x.methodCount)
                .ThenBy(x => x.label)
                .Select(data =>
                {
                    return new CompletionItem
                    {
                        Label = data.label,
                        Kind = CompletionItemKind.Class,
                        TextEdit = new TextEdit
                        {
                            NewText = MakeMethodsComment(data.type),
                            Range = new Range(
                                new Position
                                {
                                    Line = request.Position.Line,
                                    Character = request.Position.Character - typeName.Length
                                }, new Position
                                {
                                    Line = request.Position.Line,
                                    Character = request.Position.Character
                                }
                            )
                        }
                    };
                })
                .ToArray();

            var result =  new CompletionList(items, isIncomplete: items.Length == 0);

            return Task.FromResult(result);
        }

        private string FindTypeNameReverse(int charIdx, string sourceCode)
        {
            if (charIdx < 0)
                return "";

            var sb = new StringBuilder();

            int currentCharIdx = charIdx - 1;

            while (true)
            {
                if (currentCharIdx == -1)
                    break;

                if (!_typeNameRegex.IsMatch(sourceCode[currentCharIdx].ToString()))
                    break;

                sb.Append(sourceCode[currentCharIdx].ToString());

                currentCharIdx--;
            }

            return new string(sb.ToString().Reverse().ToArray());
        }

        private string MakeMethodsComment(Doshik.DoshikExternalApiType type)
        {
            var typeName = MakeTypeCodename(type);

            var sb = new StringBuilder();

            sb.Append(typeName);

            sb.AppendLine();
            sb.AppendLine();

            sb.AppendLine("// ######## " + typeName + " ########");

            if (type.Methods.Count > 0)
            {
                sb.AppendLine("//");

                for (var methodIdx = 0; methodIdx < type.Methods.Count; methodIdx++)
                {
                    var method = type.Methods[methodIdx];

                    foreach (var overload in method.Overloads)
                    {
                        sb.AppendLine("// " + MakeMethodString(overload));
                    }

                    if (methodIdx != type.Methods.Count - 1)
                        sb.AppendLine("//");
                }

                sb.AppendLine("//");
            }

            sb.AppendLine("// ################");

            return sb.ToString();
        }

        private string MakeMethodString(Doshik.DoshikExternalApiTypeMethodOverload overload)
        {
            var sb = new StringBuilder();

            if (overload.IsStatic)
                sb.Append("static ");

            sb.Append((overload.OutParameterType == null ? "void" : MakeTypeCodename(overload.OutParameterType)) + " ");

            sb.Append(overload.Method.CodeName + "(");

            var parameters = new List<string>();

            foreach (var parameter in overload.InParameters)
            {
                parameters.Add(MakeTypeCodename(parameter.Type) + " " + (parameter.Name ?? "[noname]"));
            }

            foreach (var parameter in overload.ExtraOutParameters)
            {
                parameters.Add("out " + MakeTypeCodename(parameter.Type) + " " + (parameter.Name ?? "[noname]"));
            }

            sb.Append(string.Join(", ", parameters) + ")");

            return sb.ToString();
        }

        private string MakeTypeCodename(Doshik.DoshikExternalApiType type)
        {
            return string.Join("::", type.FullyQualifiedCodeName);
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

        private readonly static Regex _typeNameRegex = new Regex("[_0-9a-zA-Z:]{1}", RegexOptions.Compiled);

        private CompletionCapability _capability;
    }
}