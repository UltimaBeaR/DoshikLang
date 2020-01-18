using System;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangCompiler.Compilation
{
    public class CompilationContext
    {
        public DoshikExternalApi ExternalApi { get; set; }

        public Dictionary<string, Type> IntrinsicTypes { get; } = new Dictionary<string, Type>()
        {
            { "int", typeof(int) },
            { "float", typeof(float) },
            { "bool", typeof(bool) },
            { "string", typeof(string) }
        };

        // ToDo: сделать тут указание строки и столбца в исходном коде, откуда пошла ошибка и возможно чего-то еще
        public void ThrowCompilationError(string errorText)
        {
            CompilationErrors.Add(errorText);

            throw new CompilationErrorException();
        }

        public DoshikExternalApiType FindExternalApiType(string externalApiOrIntrinsicTypeName)
        {
            if (IntrinsicTypes.TryGetValue(externalApiOrIntrinsicTypeName, out var intrinsicType))
            {
                return ExternalApi.Types.FirstOrDefault(x => x.DotnetType == intrinsicType);
            }

            return ExternalApi.Types.FirstOrDefault(x => x.ExternalName == externalApiOrIntrinsicTypeName);
        }

        public List<string> CompilationErrors { get; } = new List<string>();
    }
}
