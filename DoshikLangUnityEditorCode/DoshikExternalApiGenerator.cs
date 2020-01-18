using DoshikLangCompiler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangUnityEditor
{
    /// <summary>
    /// Генерирует DoshikExternalApi на основе определений нод для языка графов
    /// </summary>
    public class DoshikExternalApiGenerator
    {
        public Action<string> LogWarning { get; set; }

        public DoshikExternalApi Generate()
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

            return api;
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

                if (apiType.DotnetType == null)
                    apiType.DotnetType = node.Type;

                if (apiType.DotnetType != node.Type)
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

                if (apiType.DotnetType == null)
                    apiType.DotnetType = node.Type;

                if (apiType.DotnetType != node.Type)
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

                if (apiType.DotnetType == null)
                    apiType.DotnetType = node.Type;

                if (apiType.DotnetType != node.Type)
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

                if (apiType.DotnetType == null)
                    apiType.DotnetType = node.Type;

                if (apiType.DotnetType != node.Type)
                    LogWarning?.Invoke("different dotnet types for single node type");

                var apiMethod = GetOrCreateMethod(apiType, externalMethodName);

                bool methodOverloadCreated;
                var apiMethodOverload = GetOrCreateMethodOverload(apiMethod, externalMethodSignatureName, out methodOverloadCreated);

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

                    var isInstanceParameter = nodeInputParameterIdx == 0 && nodeParameter.Name == "instance" && apiType == parameterApiType;

                    if (isInstanceParameter)
                    {
                        apiMethodOverload.IsStatic = false;
                    }
                    else
                    {
                        apiMethodOverload.InParameters.Add(string.IsNullOrEmpty(nodeParameter.Name) ? ("_" + nodeInputParameterIdx.ToString()) : nodeParameter.Name, parameterApiType);
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
                        apiMethodOverload.ExtraOutParameters.Add(string.IsNullOrEmpty(nodeParameter.Name) ? ("_" + nodeOutputParameterIdx.ToString()) : nodeParameter.Name, parameterApiType);
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

                var apiEvent = GetOrCreateApiEvent(api, externalEventName);

                if (apiEvent.Parameters.Count > 0)
                {
                    LogWarning?.Invoke("event already defined");
                    continue;
                }

                for (var nodeOutputParameterIdx = 0; nodeOutputParameterIdx < node.OutputParameters.Length; nodeOutputParameterIdx++)
                {
                    var nodeParameter = node.OutputParameters[nodeOutputParameterIdx];

                    var apiType = GetOrCreateApiType(api, nodeParameter.Type);

                    apiEvent.Parameters.Add(string.IsNullOrEmpty(nodeParameter.Name) ? ("_" + nodeOutputParameterIdx.ToString()) : nodeParameter.Name, apiType);
                }
            }
        }

        private static DoshikExternalApiType GetOrCreateApiType(DoshikExternalApi api, string externalName)
        {
            // Не сохраняем Ref типы как отдельные типы - окончание Ref добавляется когда это параметр с ref / out модификатором
            if (externalName.EndsWith("Ref"))
                externalName = externalName.Remove(externalName.Length - "Ref".Length, "Ref".Length);

            var apiType = api.Types.Find(x => x.ExternalName == externalName);

            if (apiType == null)
            {
                apiType = new DoshikExternalApiType { ExternalName = externalName, Methods = new List<DoshikExternalApiTypeMethod>() };
                api.Types.Add(apiType);
            }

            return apiType;
        }

        private DoshikExternalApiType GetOrCreateApiType(DoshikExternalApi api, Type dotnetType)
        {
            var externalName = MakeExternalNameFromDotnetType(dotnetType);

            var apiType = GetOrCreateApiType(api, externalName);

            if (apiType.DotnetType == null)
                apiType.DotnetType = dotnetType;

            if (apiType.DotnetType != dotnetType)
                LogWarning?.Invoke("different dotnet types for single node type");

            return apiType;
        }

        private static DoshikExternalApiTypeMethod GetOrCreateMethod(DoshikExternalApiType apiType, string externalName)
        {
            var apiMethod = apiType.Methods.Find(x => x.ExternalName == externalName);

            if (apiMethod == null)
            {
                apiMethod = new DoshikExternalApiTypeMethod { ExternalName = externalName, Overloads = new List<DoshikExternalApiTypeMethodOverload>() };
                apiType.Methods.Add(apiMethod);
            }

            return apiMethod;
        }

        private static DoshikExternalApiTypeMethodOverload GetOrCreateMethodOverload(DoshikExternalApiTypeMethod apiMethod, string externalName, out bool created)
        {
            created = false;

            var apiMethodOverload = apiMethod.Overloads.Find(x => x.ExternalName == externalName);

            if (apiMethodOverload == null)
            {
                apiMethodOverload = new DoshikExternalApiTypeMethodOverload
                {
                    ExternalName = externalName,
                    InParameters = new Dictionary<string, DoshikExternalApiType>(),
                    ExtraOutParameters = new Dictionary<string, DoshikExternalApiType>()
                };

                apiMethod.Overloads.Add(apiMethodOverload);

                created = true;
            }

            return apiMethodOverload;
        }

        private static DoshikExternalApiEvent GetOrCreateApiEvent(DoshikExternalApi api, string externalName)
        {
            var apiEvent = api.Events.Find(x => x.ExternalName == externalName);

            if (apiEvent == null)
            {
                apiEvent = new DoshikExternalApiEvent { ExternalName = externalName, Parameters = new Dictionary<string, DoshikExternalApiType>() };
                api.Events.Add(apiEvent);
            }

            return apiEvent;
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
    }
}
