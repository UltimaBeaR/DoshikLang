using DoshikLangCompiler.Compilation.CodeRepresentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangCompiler.Compilation
{
    public class TypeLibrary
    {
        public TypeLibrary(CompilationContext compilationContext)
        {
            _compilationContext = compilationContext;

            AllTypes.Add(new DataType { IsVoid = true, ExternalType = null });
        }

        public Dictionary<string, Type> IntrinsicTypes { get; } = new Dictionary<string, Type>()
        {
            { "int", typeof(int) },
            { "float", typeof(float) },
            { "bool", typeof(bool) },
            { "string", typeof(string) },
            { "object", typeof(object) }
        };

        public List<DataType> AllTypes { get; } = new List<DataType>();

        public string GetApiTypeFullCodeName(DoshikExternalApiType type)
        {
            return string.Join("::", type.FullyQualifiedCodeName);
        }

        public DataType FindVoid()
        {
            return AllTypes.FirstOrDefault(x => x.IsVoid == true);
        }

        public DataType FindByExternalType(DoshikExternalApiType externalType)
        {
            if (externalType == null)
                return null;

            return AllTypes.FirstOrDefault(x => x.ExternalType == externalType);
        }

        public DataType FindTypeByDotnetType(Type dotnetType)
        {
            return AllTypes.FirstOrDefault(x => x.ExternalType != null && x.ExternalType.DotnetType == dotnetType);
        }

        public DataType FindTypeByFullyQualifiedExternalTypeCodeNameOrIntrinsiTypeName(string[] fullyQualifiedExternalTypeCodeNameOrIntrinsiTypeName)
        {
            if (fullyQualifiedExternalTypeCodeNameOrIntrinsiTypeName.Length == 1 && IntrinsicTypes.TryGetValue(fullyQualifiedExternalTypeCodeNameOrIntrinsiTypeName[0], out var intrinsicType))
                return FindTypeByDotnetType(intrinsicType);

            return AllTypes.FirstOrDefault(x => x.ExternalType != null && Enumerable.SequenceEqual(x.ExternalType.FullyQualifiedCodeName, fullyQualifiedExternalTypeCodeNameOrIntrinsiTypeName));
        }

        /// <summary>
        /// Найти "лучшую перегрузку" метода
        /// </summary>
        public FindOverloadResult FindBestMethodOverload(
            bool isStatic,
            DoshikExternalApiType type, string methodName,
            List<FindOverloadParameter> parameters)
        {
            // ToDo: потом нужно как-то сделать нахождение перегрузки метода с учетом даункаста(учитывая правила даункаста встроенных типов и implicit методов определенных у некоторых типов)
            // а пока перегрузка будет находиться только если ЯВНО указывать конкретный тип параметров.
            // то есть в случае если не получается найти метод - нужно сделать явный тайпкаст в коде.

            var result = new FindOverloadResult();

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

            // Распределяем параметры в один из списков - входной или выходной

            var inParameters = new List<DoshikExternalApiType>();
            var outParameters = new List<DoshikExternalApiType>();
            var outParametersAreInTheEndOfSequence = true;

            foreach (var parameter in parameters)
            {
                if (parameter.IsOut)
                    outParameters.Add(parameter.Type);
                else
                {
                    if (outParameters.Count > 0)
                    {
                        outParametersAreInTheEndOfSequence = false;
                        break;
                    }

                    inParameters.Add(parameter.Type);
                }
            }

            // У языка нет ограничения на то, должны ли out параметры быть в конце или out параметр может быть например в середине списка параметров
            // Но в external api out параметры всегда идут в конце после in параметров, по этому для этого случая (поиск перегрузки именно для external api, а не для каких-то локальных методов)
            // будем проверять что входные и выходные параметры не перемешиваются - а если перемешиваются то значит перегрузку такую не нашли (то есть НЕ находим в этом случае перегрузку)
            if (outParametersAreInTheEndOfSequence)
            {

                foreach (var overload in suitableOverloads)
                {
                    if (DoParametersMatchOverload(inParameters, outParameters, overload))
                    {
                        result.BestOverload = overload;
                        break;
                    }
                }
            }

            return result;
        }

        public void UpdateExternalApiTypes()
        {
            if (_compilationContext.ExternalApi != null)
            {
                foreach (var externalType in _compilationContext.ExternalApi.Types)
                {
                    if (AllTypes.Find(x => x.ExternalType == externalType) == null)
                        AllTypes.Add(new DataType { IsVoid = false, ExternalType = externalType });
                }
            }
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

        public class FindOverloadParameter
        {
            public bool IsOut { get; set; }
            public DoshikExternalApiType Type { get; set; }
        }

        public class FindOverloadResult
        {
            // Если 0, значит не найдено ни одной перегрузки с таким названием (не все из них подходят под заданные параметры)
            // Число учитывает фильтр по статическим/instance методам но не учитывает фильтр по типам параметров
            public int OverloadCount { get; set; }

            // Лучшая перегрузка метода (может быть 0, если она не найдена)
            public DoshikExternalApiTypeMethodOverload BestOverload { get; set; }
        }

        private CompilationContext _compilationContext;
    }
}
