using Antlr4.Runtime.Misc;

namespace DoshikLangCompiler.Compilation.Visitors
{
    public class GetTypeNameVisitor : CompilationContextVisitorBase<TypeLibrary.FoundType>
    {
        public GetTypeNameVisitor(CompilationContext compilationContext)
            : base(compilationContext)
        {
        }

        /// <summary>
        /// Возвращает тип. Может вернуться void
        /// </summary>
        public static TypeLibrary.FoundType Apply(CompilationContext compilationContext, DoshikParser.TypeTypeOrVoidContext context)
        {
            return context.Accept(new GetTypeNameVisitor(compilationContext));
        }

        /// <summary>
        /// Возвращает тип. void в этом случае не может вернуться
        /// </summary>
        public static TypeLibrary.FoundType Apply(CompilationContext compilationContext, DoshikParser.TypeTypeContext context)
        {
            return context.Accept(new GetTypeNameVisitor(compilationContext));
        }

        public override TypeLibrary.FoundType VisitTypeTypeOrVoid([NotNull] DoshikParser.TypeTypeOrVoidContext context)
        {
            if (context.VOID() != null)
            {
                return new TypeLibrary.FoundType()
                {
                    SourceText = context.GetText(),
                    DataType = _compilationContext.TypeLibrary.FindVoid()
                };
            }

            return Visit(context.typeType());
        }

        public override TypeLibrary.FoundType VisitTypeType([NotNull] DoshikParser.TypeTypeContext context)
        {
            return _compilationContext.TypeLibrary.FindTypeByCodeNameString(context.GetText());
        }
    }
}
