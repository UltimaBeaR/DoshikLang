﻿using Doshik;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangIR
{
    public class TypeLibrary
    {
        public TypeLibrary(CompilationContext compilationContext)
        {
            _compilationContext = compilationContext;

            AllTypes.Add(new DataType { IsVoid = true, ExternalType = null });
        }

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

        public DataType FindByExternalTypeName(string externalTypeName)
        {
            if (externalTypeName == null)
                return null;

            return AllTypes.FirstOrDefault(x => x.ExternalType?.ExternalName == externalTypeName);
        }

        public DataType FindByKnownType(KnownType type)
        {
            return FindByExternalTypeName(_knownTypeData[type].ExternalTypeName);
        }

        public DataType FindTypeByFullyQualifiedExternalTypeCodeNameOrIntrinsicTypeName(string[] fullyQualifiedExternalTypeCodeNameOrIntrinsiTypeName)
        {
            if (fullyQualifiedExternalTypeCodeNameOrIntrinsiTypeName.Length == 1)
            {
                var foundIntrinsicValue = _knownTypeData.Values.FirstOrDefault(x => x.IntrinsicCodeName == fullyQualifiedExternalTypeCodeNameOrIntrinsiTypeName[0]);

                if (foundIntrinsicValue != null)
                    return FindByExternalTypeName(foundIntrinsicValue.ExternalTypeName);
            }

            return AllTypes.FirstOrDefault(x => x.ExternalType != null && Enumerable.SequenceEqual(x.ExternalType.FullyQualifiedCodeName, fullyQualifiedExternalTypeCodeNameOrIntrinsiTypeName));
        }

        public FoundType FindTypeByCodeNameString(string codeNameString)
        {
            if (codeNameString.Contains("[]"))
            {
                // Пока не поддерживаем [] в названии типа (вместо этого используем тип System::Int32Array и т.д. для этого)
                return new FoundType()
                {
                    SourceText = codeNameString,
                    DataType = null
                };
            }

            // Получаем "::"
            var scopeResolutionOperatorString = DoshikParser.DefaultVocabulary.GetLiteralName(DoshikParser.SCOPE_RESOLUTION).Trim('\'');

            // Разделяем полное имя типа по "::"
            var codeName = codeNameString.Split(new string[] { scopeResolutionOperatorString }, StringSplitOptions.None);

            // ToDo: если появятся using statements то тут надо будет учитывать также то что на уровне файла мог быть определен using какого-нибуль неймспейса
            // и тип тогда надо будет искать не только по type но type с учетом этого namespace
            var dataType = FindTypeByFullyQualifiedExternalTypeCodeNameOrIntrinsicTypeName(codeName);

            if (dataType == null)
            {
                // Тип не найден
                return new FoundType()
                {
                    SourceText = codeNameString,
                    DataType = null
                };
            }

            // Тип найден
            return new FoundType()
            {
                SourceText = codeNameString,
                DataType = dataType
            };
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

        private static Dictionary<KnownType, IntrinsicTypeData> _knownTypeData { get; } = new Dictionary<KnownType, IntrinsicTypeData>()
        {
            { KnownType.Boolean, new IntrinsicTypeData { IntrinsicCodeName = "bool", ExternalTypeName = "SystemBoolean" } },
            { KnownType.Byte, new IntrinsicTypeData { IntrinsicCodeName = "byte", ExternalTypeName = "SystemByte" } },
            { KnownType.SByte, new IntrinsicTypeData { IntrinsicCodeName = "sbyte", ExternalTypeName = "SystemSByte" } },
            { KnownType.Char, new IntrinsicTypeData { IntrinsicCodeName = "char", ExternalTypeName = "SystemChar" } },
            { KnownType.Decimal, new IntrinsicTypeData { IntrinsicCodeName = "decimal", ExternalTypeName = "SystemDecimal" } },
            { KnownType.Double, new IntrinsicTypeData { IntrinsicCodeName = "double", ExternalTypeName = "SystemDouble" } },
            { KnownType.Single, new IntrinsicTypeData { IntrinsicCodeName = "float", ExternalTypeName = "SystemSingle" } },
            { KnownType.Int32, new IntrinsicTypeData { IntrinsicCodeName = "int", ExternalTypeName = "SystemInt32" } },
            { KnownType.UInt32, new IntrinsicTypeData { IntrinsicCodeName = "uint", ExternalTypeName = "SystemUInt32" } },
            { KnownType.Int64, new IntrinsicTypeData { IntrinsicCodeName = "long", ExternalTypeName = "SystemInt64" } },
            { KnownType.UInt64, new IntrinsicTypeData { IntrinsicCodeName = "ulong", ExternalTypeName = "SystemUInt64" } },
            { KnownType.Int16, new IntrinsicTypeData { IntrinsicCodeName = "short", ExternalTypeName = "SystemInt16" } },
            { KnownType.UInt16, new IntrinsicTypeData { IntrinsicCodeName = "ushort", ExternalTypeName = "SystemUInt16" } },
            { KnownType.Object, new IntrinsicTypeData { IntrinsicCodeName = "object", ExternalTypeName = "SystemObject" } },
            { KnownType.String, new IntrinsicTypeData { IntrinsicCodeName = "string", ExternalTypeName = "SystemString" } },

            { KnownType.Type, new IntrinsicTypeData { IntrinsicCodeName = null, ExternalTypeName = "SystemType" } },
        };

        private class IntrinsicTypeData
        {
            public string IntrinsicCodeName { get; set; }
            public string ExternalTypeName { get; set; }
        }

        private CompilationContext _compilationContext;
    }

    public enum KnownType
    {
        Boolean,
        Byte,
        SByte,
        Char,
        Decimal,
        Double,
        Single,
        Int32,
        UInt32,
        Int64,
        UInt64,
        Int16,
        UInt16,
        Object,
        String,

        Type
    }
}
