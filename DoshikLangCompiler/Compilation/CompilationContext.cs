using DoshikLangCompiler.Compilation.CodeRepresentation;
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

        public DoshikExternalApiType FindExternalApiType(Type dotnetType)
        {
            return ExternalApi.Types.FirstOrDefault(x => x.DotnetType == dotnetType);
        }

        public DoshikExternalApiType FindExternalApiType(string[] codeNameOrIntrinsicTypeName)
        {
            if (codeNameOrIntrinsicTypeName.Length == 1 && IntrinsicTypes.TryGetValue(codeNameOrIntrinsicTypeName[0], out var intrinsicType))
                return FindExternalApiType(intrinsicType);

            return ExternalApi.Types.FirstOrDefault(x => Enumerable.SequenceEqual(x.FullyQualifiedCodeName, codeNameOrIntrinsicTypeName));
        }

        public DoshikExternalApiEvent FindExternalApiEvent(string eventCodeName)
        {
            return ExternalApi.Events.FirstOrDefault(x => x.CodeName == eventCodeName);
        }

        public List<string> CompilationErrors { get; } = new List<string>();
    }
}
