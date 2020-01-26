using Antlr4.Runtime.Misc;
using DoshikLangCompiler.Compilation.CodeRepresentation;
using System;

namespace DoshikLangCompiler.Compilation.Visitors
{
    public class GetTypeNameVisitor : CompilationContextVisitorBase<GetTypeNameVisitor.FoundType>
    {
        public GetTypeNameVisitor(CompilationContext compilationContext)
            : base(compilationContext)
        {
        }

        /// <summary>
        /// Возвращает тип. Может вернуться void
        /// </summary>
        public static FoundType Apply(CompilationContext compilationContext, DoshikParser.TypeTypeOrVoidContext context)
        {
            return context.Accept(new GetTypeNameVisitor(compilationContext));
        }

        /// <summary>
        /// Возвращает тип. void в этом случае не может вернуться
        /// </summary>
        public static FoundType Apply(CompilationContext compilationContext, DoshikParser.TypeTypeContext context)
        {
            return context.Accept(new GetTypeNameVisitor(compilationContext));
        }

        public override FoundType VisitTypeTypeOrVoid([NotNull] DoshikParser.TypeTypeOrVoidContext context)
        {
            if (context.VOID() != null)
            {
                return new FoundType()
                {
                    SourceText = context.GetText(),
                    DataType = new DataType() { IsVoid = true }
                };
            }

            return Visit(context.typeType());
        }

        public override FoundType VisitTypeType([NotNull] DoshikParser.TypeTypeContext context)
        {
            var sourceTypeText = context.GetText();

            if (sourceTypeText.Contains("[]"))
            {
                // Пока не поддерживаем [] в названии типа (вместо этого используем тип System::Int32Array и т.д. для этого)
                return new FoundType()
                {
                    SourceText = sourceTypeText,
                    DataType = null
                };
            }

            // Получаем "::"
            var scopeResolutionOperatorString = DoshikParser.DefaultVocabulary.GetLiteralName(DoshikParser.SCOPE_RESOLUTION).Trim('\'');

            // Разделяем полное имя типа по "::" (ToDo: по хорошему это надо делать не вручную а используя методы визитора, заходя в каждое под-выражения, но это было лень делать)
            var codeName = sourceTypeText.Split(new string[] { scopeResolutionOperatorString }, StringSplitOptions.None);

            // ToDo: если появятся using statements то тут надо будет учитывать также то что на уровне файла мог быть определен using какого-нибуль неймспейса
            // и тип тогда надо будет искать не только по type но type с учетом этого namespace
            var externalApiType = _compilationContext.FindExternalApiType(codeName);

            if (externalApiType == null)
            {
                // Тип не найден
                return new FoundType()
                {
                    SourceText = sourceTypeText,
                    DataType = null
                };
            }

            // Тип найден
            return new FoundType()
            {
                SourceText = sourceTypeText,
                DataType = new DataType() { ExternalType = externalApiType }
            };
        }

        /// <summary>
        /// Найденный тип + дополнительные данные о том где и как он был найден (может понадобится для правильного построения ошибок)
        /// </summary>
        public class FoundType
        {
            /// <summary>
            /// Исходный код, по которому найден этот тип
            /// </summary>
            public string SourceText { get; set; }

            /// <summary>
            /// null, если тип не был найден
            /// </summary>
            public DataType DataType { get; set; }

            public void ThrowIfNotFound(CompilationContext _compilationContext)
            {
                if (DataType == null)
                {
                    throw _compilationContext.ThrowCompilationError("type " + SourceText + " is undefined");
                }
            }
        }
    }
}
