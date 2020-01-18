using Antlr4.Runtime.Misc;
using System.Linq;

namespace DoshikLangCompiler.Compilation.Visitors
{
    public class GetTypeNameVisitor : CompilationContextVisitorBase<string>
    {
        public GetTypeNameVisitor(CompilationContext compilationContext)
            : base(compilationContext)
        {
        }

        public static string Apply(CompilationContext compilationContext, DoshikParser.TypeTypeContext context)
        {
            return context.Accept(new GetTypeNameVisitor(compilationContext));
        }

        public override string VisitTypeType([NotNull] DoshikParser.TypeTypeContext context)
        {
            var type = context.GetText();

            var externalApiType = _compilationContext.FindExternalApiType(type);

            if (externalApiType == null)
            {
                _compilationContext.ThrowCompilationError("type " + type + " is undefined");
            }

            return externalApiType.ExternalName;
        }
    }
}
