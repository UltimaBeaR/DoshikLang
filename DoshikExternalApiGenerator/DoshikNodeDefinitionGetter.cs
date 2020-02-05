using System;
using System.Collections.Generic;
using System.Linq;
using VRC.Udon.EditorBindings;
using VRC.Udon.Graph.Interfaces;
using VRC.Udon.UAssembly.Assembler;

namespace Doshik
{
    public static class DoshikNodeDefinitionGetter
    {
        /// <summary>
        /// В typesForResolver надо передавать как минимум {"VRCUdonUdonBehaviour", typeof(UdonBehaviour)}
        /// </summary>
        public static DoshikNodeDefinition[] GetAllNodeDefinitions(IDictionary<string, Type> typesForResolver)
        {
            var udonEditorInterface = new UdonEditorInterface();
            udonEditorInterface.AddTypeResolver(new AnyTypeResolver(typesForResolver));

            var rootRegistries = udonEditorInterface.GetNodeRegistries();

            return GetNodeDefinitions(rootRegistries.Values, new INodeRegistry[0]).ToArray();
        }

        private static IEnumerable<DoshikNodeDefinition> GetNodeDefinitions(IEnumerable<INodeRegistry> subRegistries, INodeRegistry[] parentRegistriesPath)
        {
            if (subRegistries == null)
                yield break;

            foreach (var subRegistry in subRegistries)
            {
                var registryPath = parentRegistriesPath.Concat(new INodeRegistry[] { subRegistry }).ToArray();

                foreach (var nodeDefinition in subRegistry.GetNodeDefinitions())
                {
                    yield return new DoshikNodeDefinition(
                        nodeDefinition,
                        registryPath.ToArray() //< копия
                    );
                }

                foreach (var nodeDefinition in GetNodeDefinitions(subRegistry.GetNodeRegistries().Values, registryPath))
                    yield return nodeDefinition;
            }
        }

        private class AnyTypeResolver : BaseTypeResolver
        {
            public AnyTypeResolver(IDictionary<string, Type> types)
            {
                _types = types.ToDictionary(x => x.Key, x => x.Value);
            }

            protected override Dictionary<string, Type> Types => _types;

            private readonly Dictionary<string, Type> _types;
        }
    }
}
