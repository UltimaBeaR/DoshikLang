using DoshikLangUnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tester
{
    public class NodesLogger
    {
        public static void LogRegistries()
        {
            TestLogger.Begin();

            var allNodeDefinitions = DoshikNodeDefinitionGetter.GetAllNodeDefinitions(new Dictionary<string, Type> { /*{ "VRCUdonUdonBehaviour", typeof(UdonBehaviour) }*/ })
                .GroupBy(x => x.Identifier)
                .Select(x => x.First())
                .OrderBy(x => x.Identifier);

            foreach (var nodeDefinition in allNodeDefinitions)
            {
                var logName = "undefined";

                if (nodeDefinition.Identifier.StartsWith("Const_"))
                    logName = "const";
                else if (nodeDefinition.Identifier.StartsWith("Type_"))
                    logName = "type";
                else if (nodeDefinition.Identifier.StartsWith("Variable_"))
                    logName = "variable";
                else if (nodeDefinition.Identifier.StartsWith("Event_"))
                    logName = "event";
                else if (nodeDefinition.Identifier.Contains(".__"))
                    logName = "method";

                var nodeName = nodeDefinition.Name ?? "[null]";
                var nodeType = nodeDefinition.Type?.FullName ?? "[null]";

                TestLogger.LogLine(logName, nodeDefinition.Identifier);

                TestLogger.LogLine(logName, "  label: " + nodeName);

                TestLogger.LogLine(logName, "  type: " + nodeType);

                if (nodeDefinition.InputParameters.Length > 0)
                {
                    TestLogger.LogLine(logName, "");
                    TestLogger.LogLine(logName, "  in");
                }

                foreach (var input in nodeDefinition.InputParameters)
                {
                    var name = input.Name == null ? "[null]" : "\"" + input.Name + "\"";
                    var type = input.Type == null ? "[null]" : input.Type.FullName;

                    TestLogger.LogLine(logName, "    " + name + ": " + type);
                }

                if (nodeDefinition.OutputParameters.Length > 0)
                {
                    TestLogger.LogLine(logName, "");
                    TestLogger.LogLine(logName, "  out");
                }

                foreach (var output in nodeDefinition.OutputParameters)
                {
                    var name = output.Name == null ? "[null]" : "\"" + output.Name + "\"";
                    var type = output.Type == null ? "[null]" : output.Type.FullName;

                    TestLogger.LogLine(logName, "    " + name + ": " + type);
                }

                TestLogger.LogLine(logName, "");
            }

            TestLogger.End();
        }
    }
}
