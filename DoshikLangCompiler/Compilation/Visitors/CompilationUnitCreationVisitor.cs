using Antlr4.Runtime.Misc;

namespace DoshikLangCompiler.Compilation.Visitors
{
    public class CompilationUnitCreationVisitor : CompilationContextVisitorBase<object>
    {
        public CompilationUnitCreationVisitor(CompilationContext compilationContext)
            : base(compilationContext)
        {
        }

        public static CompilationUnit Apply(CompilationContext compilationContext, DoshikParser.CompilationUnitContext context)
        {
            return (CompilationUnit)context.Accept(new CompilationUnitCreationVisitor(compilationContext));
        }

        public override object VisitCompilationUnit([NotNull] DoshikParser.CompilationUnitContext context)
        {
            _compilationUnit = new CompilationUnit();

            var memberDeclarations = context.memberDeclaration();

            foreach (var memberDeclaration in memberDeclarations)
            {
                Visit(memberDeclaration);
            }

            return _compilationUnit;
        }

        public override object VisitMemberDeclaration([NotNull] DoshikParser.MemberDeclarationContext context)
        {
            if (context.fieldDeclaration() != null)
                Visit(context.fieldDeclaration());

            if (context.methodDeclaration() != null)
                Visit(context.methodDeclaration());

            return null;
        }

        public override object VisitFieldDeclaration([NotNull] DoshikParser.FieldDeclarationContext context)
        {
            var variable = new Variable();

            variable.IsPublic = context.PUBLIC() != null;

            variable.Type = GetTypeNameVisitor.Apply(_compilationContext, context.typeType());

            _compilationUnit.Variables.Add(variable);

            return null;
        }

        public override object VisitMethodDeclaration([NotNull] DoshikParser.MethodDeclarationContext context)
        {
            return null;
        }

        private CompilationUnit _compilationUnit;
    }
}