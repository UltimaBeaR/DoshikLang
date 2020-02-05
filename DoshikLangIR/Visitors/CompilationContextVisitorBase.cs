namespace DoshikLangIR
{
    public abstract class CompilationContextVisitorBase<Result> : DoshikParserBaseVisitor<Result>
    {
        public CompilationContextVisitorBase(CompilationContext compilationContext)
        {
            _compilationContext = compilationContext;
        }

        protected CompilationContext _compilationContext;
    }
}
