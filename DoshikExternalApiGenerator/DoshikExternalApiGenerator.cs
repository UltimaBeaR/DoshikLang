using System;
using System.Collections.Generic;
using System.Linq;

namespace Doshik
{
    /// <summary>
    /// Генерирует DoshikExternalApi на основе определений нод для языка графов
    /// </summary>
    public class DoshikExternalApiGenerator
    {
        public Action<string> LogWarning { get; set; }
        public Action<string> LogInfo { get; set; }

        public DoshikExternalApi GetOrGenerateCache(string externalApiVersion)
        {
            // Пытаемся получить апи из кэша
            var externalApi = DoshikExternalApiCache.GetCachedApi();

            // Если кэш не найден или версия не соответствует текущей (версия структуры + версия внешних библиотек) - генерируем апи из библиотек и записываем новый кэш
            if (externalApi == null || externalApi.ExternalVersion != externalApiVersion)
            {
                LogInfo?.Invoke("generating external api...");

                externalApi = Generate(externalApiVersion);

                DoshikExternalApiCache.SetApiToCache(externalApi);

                LogInfo?.Invoke("external api generated and saved to cache");
            }

            return externalApi;
        }

        public DoshikExternalApi Generate(string externalApiVersion)
        {
            // Получаем все ноды (типы нод), которые можно использовать в языке графов
            var allNodes = DoshikNodeDefinitionGetter.GetAllNodeDefinitions(new Dictionary<string, Type> { /*{ "VRCUdonUdonBehaviour", typeof(UdonBehaviour) }*/ })
                .GroupBy(x => x.Identifier)
                .Select(x => x.First())
                .OrderBy(x => x.Identifier);

            var constNodes = new List<DoshikNodeDefinition>();
            var typeNodes = new List<DoshikNodeDefinition>();
            var variableNodes = new List<DoshikNodeDefinition>();
            var eventNodes = new List<DoshikNodeDefinition>();
            var methodNodes = new List<DoshikNodeDefinition>();

            // Распределяем ноды по категориям (среди нераспределенных останутся операторы ветвления и т.д. - они нам не нужны)
            foreach (var node in allNodes)
            {
                if (node.Identifier.StartsWith("Const_"))
                    constNodes.Add(node);
                else if (node.Identifier.StartsWith("Type_"))
                    typeNodes.Add(node);
                else if (node.Identifier.StartsWith("Variable_"))
                    variableNodes.Add(node);
                else if (node.Identifier.StartsWith("Event_"))
                    eventNodes.Add(node);
                else if (node.Identifier.Contains(".__"))
                    methodNodes.Add(node);
            }

            var api = new DoshikExternalApi
            {
                ExternalVersion = externalApiVersion,

                Types = new List<DoshikExternalApiType>(),
                Events = new List<DoshikExternalApiEvent>()
            };

            // Обрабатываем все эти ноды по разному, в зависимости от категории, формируя АПИ

            HandleConstNodes(api, constNodes);
            HandleTypeNodes(api, typeNodes);
            HandleVariableNodes(api, variableNodes);

            // Обработка ивентов и методов идет в конце, т.к. там идет референс параметр по дотнет типу (эти типы уже должны быть созданы в апи к этому времени)

            HandleMethodNodes(api, methodNodes);
            HandleEventNodes(api, eventNodes);

            GenerateCodeNames(api);

            return api;
        }

        private void GenerateCodeNames(DoshikExternalApi api)
        {
            foreach (var apiEvent in api.Events)
            {
                var name = apiEvent.ExternalName;
                // remove first "_"
                name = name.Remove(0, 1);
                name = FirstLetterToUpperCase(name);

                apiEvent.CodeName = name;
            }

            foreach (var apiType in api.Types)
            {
                string firstNamespaceSegment = null;
                string name = apiType.ExternalName;

                bool handled;

                handled = TryHandleTypePrefix("System", "System", ref firstNamespaceSegment, ref name);
                if (!handled)
                    handled = TryHandleTypePrefix("UnityEngine", "UnityEngine", ref firstNamespaceSegment, ref name);
                if (!handled)
                    handled = TryHandleTypePrefix("VRCSDK3Components", "VRCSDK3Components", ref firstNamespaceSegment, ref name);
                if (!handled)
                    handled = TryHandleTypePrefix("VRCSDKBase", "VRCSDKBase", ref firstNamespaceSegment, ref name);
                if (!handled)
                    handled = TryHandleTypePrefix("VRCUdon", "VRCUdon", ref firstNamespaceSegment, ref name);

                if (firstNamespaceSegment == null)
                {
                    apiType.FullyQualifiedCodeName = new string[] { name };
                }
                else
                {
                    apiType.FullyQualifiedCodeName = new string[] { firstNamespaceSegment, name };
                }

                foreach (var method in apiType.Methods)
                {
                    // Пока сохраняем оригинальные имена методов (потом возможно я переименую имена, связанные с операторами, чтобы из кода это выглядело как-то посимпатичнее)
                    method.CodeName = method.ExternalName;
                }
            }
        }

        private bool TryHandleTypePrefix(string externalNamePrefix, string correspondingFirstNamespaceSegment, ref string firstNamespaceSegment, ref string name)
        {
            if (name.StartsWith(externalNamePrefix))
            {
                firstNamespaceSegment = correspondingFirstNamespaceSegment;
                name = name.Remove(0, externalNamePrefix.Length);

                return true;
            }

            return false;
        }

        private void HandleConstNodes(DoshikExternalApi api, List<DoshikNodeDefinition> nodes)
        {
            foreach (var node in nodes)
            {
                bool CheckIsValid()
                {
                    if (node.InputParameters.Length != 1 || node.OutputParameters.Length != 1)
                        return false;

                    if (node.InputParameters[0].Type != node.OutputParameters[0].Type || node.InputParameters[0].Type != node.Type)
                        return false;

                    return true;
                }

                // Игнорируем тип null
                if (node.Identifier == "Const_Null")
                    continue;

                var externalTypeName = node.Identifier.Remove(0, "Const_".Length);

                if (!CheckIsValid())
                {
                    LogWarning?.Invoke("invalid const node");                    
                    continue;
                }

                var apiType = GetOrCreateApiType(api, externalTypeName);

                apiType.DeclaredAsConstNode = true;

                if (apiType.DotnetTypeString == null)
                {
                    apiType.DotnetTypeString = GetDotnetTypeAsString(apiType, node);
                }

                if (apiType.DotnetTypeString != GetDotnetTypeAsString(apiType, node))
                    LogWarning?.Invoke("different dotnet types for single node type");
            }
        }

        private void HandleTypeNodes(DoshikExternalApi api, List<DoshikNodeDefinition> nodes)
        {
            foreach (var node in nodes)
            {
                bool CheckIsValid()
                {
                    if (node.InputParameters.Length != 0 || node.OutputParameters.Length != 1)
                        return false;

                    if (node.OutputParameters[0].Type != typeof(Type))
                        return false;

                    return true;
                }

                var externalTypeName = node.Identifier.Remove(0, "Type_".Length);

                if (!CheckIsValid())
                {
                    LogWarning?.Invoke("invalid type node");
                    continue;
                }

                var apiType = GetOrCreateApiType(api, externalTypeName);

                apiType.DeclaredAsTypeNode = true;

                if (apiType.DotnetTypeString == null)
                    apiType.DotnetTypeString = GetDotnetTypeAsString(apiType, node);

                if (apiType.DotnetTypeString != GetDotnetTypeAsString(apiType, node))
                    LogWarning?.Invoke("different dotnet types for single node type");
            }
        }

        private void HandleVariableNodes(DoshikExternalApi api, List<DoshikNodeDefinition> nodes)
        {
            foreach (var node in nodes)
            {
                bool CheckIsValid()
                {
                    if (node.InputParameters.Length != 5 || node.OutputParameters.Length != 0)
                        return false;

                    if (node.InputParameters[0].Type != node.Type)
                        return false;

                    if (node.InputParameters[1].Name != "name" || node.InputParameters[1].Type != typeof(string))
                        return false;
                    if (node.InputParameters[2].Name != "public" || node.InputParameters[2].Type != typeof(bool))
                        return false;
                    if (node.InputParameters[3].Name != "synced" || node.InputParameters[3].Type != typeof(bool))
                        return false;
                    if (node.InputParameters[4].Name != "syncMode" || node.InputParameters[4].Type != typeof(string))
                        return false;

                    return true;
                }

                var externalTypeName = node.Identifier.Remove(0, "Variable_".Length);

                if (!CheckIsValid())
                {
                    LogWarning?.Invoke("invalid variable node");
                    continue;
                }

                var apiType = GetOrCreateApiType(api, externalTypeName);

                apiType.DeclaredAsVariableNode = true;

                if (apiType.DotnetTypeString == null)
                    apiType.DotnetTypeString = GetDotnetTypeAsString(apiType, node);

                if (apiType.DotnetTypeString != GetDotnetTypeAsString(apiType, node))
                    LogWarning?.Invoke("different dotnet types for single node type");
            }
        }

        private void HandleMethodNodes(DoshikExternalApi api, List<DoshikNodeDefinition> nodes)
        {
            foreach (var node in nodes)
            {
                bool CheckIsValid()
                {
                    return true;
                }

                const string typeAndFullSignatureSeparatorString = ".__";
                const string methodAndSignatureSeparatorString = "__";

                var typeAndFullSignatureSeparator = node.Identifier.IndexOf(typeAndFullSignatureSeparatorString);
                var externalTypeName = node.Identifier.Substring(0, typeAndFullSignatureSeparator);
                var fullSignature = node.Identifier.Substring(typeAndFullSignatureSeparator + typeAndFullSignatureSeparatorString.Length, node.Identifier.Length - (typeAndFullSignatureSeparator + typeAndFullSignatureSeparatorString.Length));

                var methodAndSignatureSeparator = fullSignature.IndexOf(methodAndSignatureSeparatorString);
                var externalMethodName = fullSignature.Substring(0, methodAndSignatureSeparator);
                var externalMethodSignatureName = fullSignature.Substring(methodAndSignatureSeparator + methodAndSignatureSeparatorString.Length, fullSignature.Length - (methodAndSignatureSeparator + methodAndSignatureSeparatorString.Length));

                if (!CheckIsValid())
                {
                    LogWarning?.Invoke("invalid method node");
                    continue;
                }

                var apiType = GetOrCreateApiType(api, externalTypeName);

                if (apiType.DotnetTypeString == null)
                    apiType.DotnetTypeString = GetDotnetTypeAsString(apiType, node);

                if (apiType.DotnetTypeString != GetDotnetTypeAsString(apiType, node))
                    LogWarning?.Invoke("different dotnet types for single node type");

                var apiMethod = GetOrCreateMethod(apiType, externalMethodName);

                bool methodOverloadCreated;
                // externalTypeName в данном случае не всегда будет равно apiType.ExternalName
                var apiMethodOverload = GetOrCreateMethodOverload(apiMethod, externalTypeName, externalMethodSignatureName, out methodOverloadCreated);

                if (!methodOverloadCreated)
                {
                    LogWarning?.Invoke("method overload already defined");
                    continue;
                }

                apiMethodOverload.IsStatic = true;

                for (var nodeInputParameterIdx = 0; nodeInputParameterIdx < node.InputParameters.Length; nodeInputParameterIdx++)
                {
                    var nodeParameter = node.InputParameters[nodeInputParameterIdx];
                     
                    var parameterApiType = GetOrCreateApiType(api, nodeParameter.Type);

                    var isInstanceParameter = false;
                    
                    if (nodeInputParameterIdx == 0 && nodeParameter.Name == "instance")
                    {
                        // ToDo: раньше я проверял еще на apiType == parameterApiType для того чтобы уточнить что это именно instance метод,
                        // но для интерфейсов, которые имплементятся в udonbehaviour-е почему-то первый параметр имеет тип UnityEngine.Object,
                        // а не тип интерфейса (который указан в текущем apiType), по этому я оставляю только проверку по имени первого параметра
                        // есть еще варианты с проверкой по extern-сигнатуры, т.к. для instance методов в ней не указывается instance параметр,
                        // но там вроде тоже конвенция нестабильно соблюдается, так что не хочется на нее опираться.

                        isInstanceParameter = true;
                    }

                    if (isInstanceParameter)
                    {
                        apiMethodOverload.IsStatic = false;
                    }
                    else
                    {
                        apiMethodOverload.InParameters.Add(new DoshikExternalApiMethodParameter()
                        {
                            Name = string.IsNullOrEmpty(nodeParameter.Name) ? ("_" + nodeInputParameterIdx.ToString()) : nodeParameter.Name,
                            Type = parameterApiType
                        });
                    }
                }

                for (var nodeOutputParameterIdx = 0; nodeOutputParameterIdx < node.OutputParameters.Length; nodeOutputParameterIdx++)
                {
                    var nodeParameter = node.OutputParameters[nodeOutputParameterIdx];

                    var parameterApiType = GetOrCreateApiType(api, nodeParameter.Type);

                    var isMain = nodeOutputParameterIdx == 0 && string.IsNullOrEmpty(nodeParameter.Name);

                    if (isMain)
                    {
                        apiMethodOverload.OutParameterType = parameterApiType;
                    }
                    else
                    {
                        apiMethodOverload.ExtraOutParameters.Add(new DoshikExternalApiMethodParameter()
                        {
                            Name = string.IsNullOrEmpty(nodeParameter.Name) ? ("_" + nodeOutputParameterIdx.ToString()) : nodeParameter.Name,
                            Type = parameterApiType
                        });
                    }
                }
            }
        }

        private void HandleEventNodes(DoshikExternalApi api, List<DoshikNodeDefinition> nodes)
        {
            foreach (var node in nodes)
            {
                bool CheckIsValid()
                {
                    if (node.InputParameters.Length != 0)
                        return false;

                    return true;
                }

                // Игнорируем кастомные события
                if (node.Identifier == "Event_Custom")
                    continue;

                var externalEventName = "_" + FirstLetterToLowerCase(node.Identifier.Remove(0, "Event_".Length));

                if (!CheckIsValid())
                {
                    LogWarning?.Invoke("invalid event node");
                    continue;
                }

                bool eventCreated;
                var apiEvent = GetOrCreateApiEvent(api, externalEventName, out eventCreated);

                if (!eventCreated)
                {
                    LogWarning?.Invoke("event already defined");
                    continue;
                }

                for (var nodeOutputParameterIdx = 0; nodeOutputParameterIdx < node.OutputParameters.Length; nodeOutputParameterIdx++)
                {
                    var nodeParameter = node.OutputParameters[nodeOutputParameterIdx];

                    var apiType = GetOrCreateApiType(api, nodeParameter.Type);

                    apiEvent.InParameters.Add(new DoshikExternalApiMethodParameter()
                    {
                        Name = string.IsNullOrEmpty(nodeParameter.Name) ? ("_" + nodeOutputParameterIdx.ToString()) : nodeParameter.Name,
                        Type = apiType
                    });
                }
            }
        }

        private static DoshikExternalApiType GetOrCreateApiType(DoshikExternalApi api, string externalName)
        {
            // Не сохраняем Ref типы как отдельные типы - окончание Ref добавляется когда это параметр с ref / out модификатором
            if (externalName.EndsWith("Ref"))
                externalName = externalName.Remove(externalName.Length - "Ref".Length, "Ref".Length);

            // хз почему так, но видимо так надо (по другому не работает). В коде графов видел похожие замены
            switch (externalName)
            {
                case "VRCUdonCommonInterfacesIUdonEventReceiver": externalName = "VRCUdonUdonBehaviour"; break;
                case "VRCUdonCommonInterfacesIUdonEventReceiverArray": externalName = "VRCUdonUdonBehaviourArray"; break;
            }

            var apiType = api.Types.Find(x => x.ExternalName == externalName);

            if (apiType == null)
            {
                apiType = new DoshikExternalApiType
                {
                    ExternalName = externalName,
                    Methods = new List<DoshikExternalApiTypeMethod>()
                };

                api.Types.Add(apiType);
            }

            return apiType;
        }

        private DoshikExternalApiType GetOrCreateApiType(DoshikExternalApi api, Type dotnetType)
        {
            var externalName = MakeExternalNameFromDotnetType(dotnetType);

            var apiType = GetOrCreateApiType(api, externalName);

            if (apiType.DotnetTypeString == null)
                apiType.DotnetTypeString = GetDotnetTypeAsString(dotnetType);

            if (apiType.DotnetTypeString != GetDotnetTypeAsString(dotnetType))
                LogWarning?.Invoke("different dotnet types for single node type");

            return apiType;
        }

        private static DoshikExternalApiTypeMethod GetOrCreateMethod(DoshikExternalApiType apiType, string externalName)
        {
            var apiMethod = apiType.Methods.Find(x => x.ExternalName == externalName);

            if (apiMethod == null)
            {
                apiMethod = new DoshikExternalApiTypeMethod { Type = apiType, ExternalName = externalName, Overloads = new List<DoshikExternalApiTypeMethodOverload>() };
                apiType.Methods.Add(apiMethod);
            }

            return apiMethod;
        }

        private static DoshikExternalApiTypeMethodOverload GetOrCreateMethodOverload(DoshikExternalApiTypeMethod apiMethod, string externalTypeName, string externalName, out bool created)
        {
            created = false;

            var apiMethodOverload = apiMethod.Overloads.Find(x => x.ExternalName == externalName);

            if (apiMethodOverload == null)
            {
                apiMethodOverload = new DoshikExternalApiTypeMethodOverload
                {
                    Method = apiMethod,
                    ExternalTypeName = externalTypeName,
                    ExternalName = externalName,
                    InParameters = new List<DoshikExternalApiMethodParameter>(),
                    ExtraOutParameters = new List<DoshikExternalApiMethodParameter>()
                };

                apiMethod.Overloads.Add(apiMethodOverload);

                created = true;
            }

            return apiMethodOverload;
        }

        private static DoshikExternalApiEvent GetOrCreateApiEvent(DoshikExternalApi api, string externalName, out bool created)
        {
            created = false;

            var apiEvent = api.Events.Find(x => x.ExternalName == externalName);

            if (apiEvent == null)
            {
                apiEvent = new DoshikExternalApiEvent
                {
                    ExternalName = externalName,
                    InParameters = new List<DoshikExternalApiMethodParameter>()
                };

                api.Events.Add(apiEvent);

                created = true;
            }

            return apiEvent;
        }

        private static string FirstLetterToUpperCase(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            char[] chars = str.ToCharArray();
            chars[0] = char.ToUpperInvariant(chars[0]);
            return new string(chars);
        }

        private static string FirstLetterToLowerCase(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            char[] chars = str.ToCharArray();
            chars[0] = char.ToLowerInvariant(chars[0]);
            return new string(chars);
        }

        private static string MakeExternalNameFromDotnetType(Type dotnetType)
        {
            return ConstTypeNameFromType(dotnetType).Replace("[]", "Array");
        }

        private static string ConstTypeNameFromType(Type t)
        {
            if (t == (Type)null)
                t = typeof(object);
            return t.FullName != null ? t.FullName.Replace(".", "").Replace("+", "") : "null";
        }

        private static string GetDotnetTypeAsString(DoshikExternalApiType apiType, DoshikNodeDefinition node)
        {
            if (apiType.ExternalName == "VRCUdonUdonBehaviour")
                return "VRC.Udon.UdonBehaviour, VRC.Udon";
            else if (apiType.ExternalName == "VRCUdonUdonBehaviourArray")
                return "VRC.Udon.UdonBehaviour[], VRC.Udon";

            return GetDotnetTypeAsString(node.Type);
        }

        private static string GetDotnetTypeAsString(Type type)
        {
            return type.AssemblyQualifiedName;
        }
    }
}
