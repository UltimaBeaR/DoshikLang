using System;
using System.Linq;
using VRC.Udon.Graph;
using VRC.Udon.Graph.Interfaces;

namespace Doshik
{
    public class DoshikNodeDefinition
    {
        public DoshikNodeDefinition(UdonNodeDefinition nodeDefinition, INodeRegistry[] registryPath)
        {
            NodeDefinition = nodeDefinition;
            RegistryPath = registryPath;

            Name = NodeDefinition.name;
            Identifier = NodeDefinition.fullName;
            Type = NodeDefinition.type;

            InputParameters = new ParameterInfo[Math.Max(NodeDefinition.inputs?.Length ?? 0, NodeDefinition.inputNames?.Length ?? 0)];
            for (int i = 0; i < InputParameters.Length; i++)
            {
                InputParameters[i] = new ParameterInfo()
                {
                    Name = i < (NodeDefinition.inputNames?.Length ?? 0) ? NodeDefinition.inputNames[i] : null,
                    Type = i < (NodeDefinition.inputs?.Length ?? 0) ? NodeDefinition.inputs[i] : null
                };
            }

            OutputParameters = new ParameterInfo[Math.Max(NodeDefinition.outputs.Length, NodeDefinition.outputNames.Length)];
            for (int i = 0; i < OutputParameters.Length; i++)
            {
                OutputParameters[i] = new ParameterInfo()
                {
                    Name = i < (NodeDefinition.outputNames?.Length ?? 0) ? NodeDefinition.outputNames[i] : null,
                    Type = i < (NodeDefinition.outputs?.Length ?? 0) ? NodeDefinition.outputs[i] : null
                };
            }
        }

        public UdonNodeDefinition NodeDefinition { get; private set; }
        public INodeRegistry[] RegistryPath { get; private set; }

        public string Name { get; set; }
        public string Identifier { get; set; }
        public Type Type { get; set; }

        public ParameterInfo[] InputParameters { get; set; }
        public ParameterInfo[] OutputParameters { get; set; }

        public string RegistryPathToString()
        {
            if (RegistryPath.Length == 0)
                return "";

            return string.Join(" => ", RegistryPath.Select(x => x.GetType().Name.Replace("NodeRegistry", "")));
        }

        public class ParameterInfo
        {
            public string Name { get; set; }
            public Type Type { get; set; }
        }
    }
}
