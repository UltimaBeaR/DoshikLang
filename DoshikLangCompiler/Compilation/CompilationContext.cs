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

        public List<string> CompilationErrors { get; } = new List<string>();

        // ToDo: сделать тут указание строки и столбца в исходном коде, откуда пошла ошибка и возможно чего-то еще
        public CompilationErrorException ThrowCompilationError(string errorText)
        {
            CompilationErrors.Add(errorText);

            throw new CompilationErrorException();
        }

        public string GetApiTypeFullCodeName(DoshikExternalApiType type)
        {
            return string.Join("::", type.FullyQualifiedCodeName);
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

        /// <summary>
        /// Найти "лучшую перегрузку" метода
        /// </summary>
        public FoundOverload FindBestMethodOverload(
            bool isStatic,
            DoshikExternalApiType type, string methodName,
            List<DoshikExternalApiType> inParameters, List<DoshikExternalApiType> outParameters)
        {
            // ToDo: потом нужно как-то сделать нахождение перегрузки метода с учетом даункаста(учитывая правила даункаста встроенных типов и implicit методов определенных у некоторых типов)
            // а пока перегрузка будет находиться только если ЯВНО указывать конкретный тип параметров.
            // то есть в случае если не получается найти метод - нужно сделать явный тайпкаст в коде.

            var result = new FoundOverload();

            var suitableOverloads = new List<DoshikExternalApiTypeMethodOverload>();

            foreach (var method in type.Methods)
            {
                if (method.CodeName == methodName)
                {
                    foreach (var overload in method.Overloads)
                    {
                        if (isStatic == overload.IsStatic)
                        {
                            suitableOverloads.Add(overload);
                        }
                    }
                }
            }

            result.OverloadCount = suitableOverloads.Count;

            foreach (var overload in suitableOverloads)
            {
                if (DoParametersMatchOverload(inParameters, outParameters, overload))
                {
                    result.BestOverload = overload;
                    break;
                }
            }

            return result;
        }

        private bool DoParametersMatchOverload(List<DoshikExternalApiType> inParameters, List<DoshikExternalApiType> outParameters, DoshikExternalApiTypeMethodOverload overload)
        {
            if (overload.InParameters.Count != inParameters.Count || overload.ExtraOutParameters.Count != outParameters.Count)
                return false;

            for (int parameterIdx = 0; parameterIdx < inParameters.Count; parameterIdx++)
            {
                if (inParameters[parameterIdx] != overload.InParameters[parameterIdx].Type)
                    return false;
            }

            for (int parameterIdx = 0; parameterIdx < outParameters.Count; parameterIdx++)
            {
                if (outParameters[parameterIdx] != overload.ExtraOutParameters[parameterIdx].Type)
                    return false;
            }

            return true;
        }

        public class FoundOverload
        {
            // Если 0, значит не найдено ни одной перегрузки с таким названием (не все из них подходят под заданные параметры)
            // Число учитывает фильтр по статическим/instance методам но не учитывает фильтр по типам параметров
            public int OverloadCount { get; set; }

            // Лучшая перегрузка метода (может быть 0, если она не найдена)
            public DoshikExternalApiTypeMethodOverload BestOverload { get; set; }
        }
    }
}
