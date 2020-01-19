using Antlr4.Runtime.Misc;

namespace DoshikLangCompiler.Compilation.Visitors
{
    public class GetTypeNameVisitor : CompilationContextVisitorBase<string>
    {
        public GetTypeNameVisitor(CompilationContext compilationContext)
            : base(compilationContext)
        {
        }

        /// <summary>
        /// Возвращает имя типа в виде строки. Если вернулся null - значит это не тип, а ключевое слово void
        /// </summary>
        public static string Apply(CompilationContext compilationContext, DoshikParser.TypeTypeOrVoidContext context)
        {
            return context.Accept(new GetTypeNameVisitor(compilationContext));
        }

        /// <summary>
        /// Возвращает имя типа в виде строки. null не возвращается
        /// </summary>
        public static string Apply(CompilationContext compilationContext, DoshikParser.TypeTypeContext context)
        {
            return context.Accept(new GetTypeNameVisitor(compilationContext));
        }

        public override string VisitTypeTypeOrVoid([NotNull] DoshikParser.TypeTypeOrVoidContext context)
        {
            if (context.VOID() != null)
            {
                // В случае void возвращаем null (это единственный случай когда возвращается null)
                return null;
            }

            return Visit(context.typeType());
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
