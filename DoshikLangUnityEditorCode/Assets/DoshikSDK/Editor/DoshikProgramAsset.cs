using DoshikLangCompiler.Compilation;
using DoshikSDK.ExternalApi;
using System;
using System.Collections.Generic;
using VRC.Udon.Common.Interfaces;
using VRC.Udon.Editor;

namespace DoshikSDK
{
    public class DoshikProgramAsset : UdonProgramAsset
    {
        public void SetSourceCode(string sourceCode)
        {
            _sourceCode = sourceCode;
        }

        protected override void DoRefreshProgramActions()
        {
            if (_externalApi == null)
            {
                var generator = new DoshikExternalApiGenerator
                {
                    LogWarning = (text) =>
                    {
                    // Warning при генерировании api
                }
                };

                _externalApi = generator.Generate();
            }

            var compiler = new Compiler();

            compiler.ExternalApi = _externalApi;

            compiler.SourceCode = _sourceCode;

            try
            {
                var output = compiler.Compile();

                if (output.CompilationErrors == null)
                {
                    AssembleProgram(output.UdonAssemblyCode);

                    if (_assemblyError != null)
                    {
                        // Ошибка в assembly

                        program = null;
                    }
                    else
                    {
                        base.DoRefreshProgramActions();

                        ApplyDefaultValuesToHeap(output.DefaultHeapValues);
                    }
                }
                else
                {
                    foreach (var error in output.CompilationErrors)
                    {
                        // Ошибка компиляции

                        program = null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Неожиданное исключение при компиляции

                program = null;
            }
        }

        protected void ApplyDefaultValuesToHeap(Dictionary<string, (object value, Type type)> heapDefaultValues)
        {
            IUdonSymbolTable symbolTable = program?.SymbolTable;
            IUdonHeap heap = program?.Heap;
            if (symbolTable == null || heap == null)
            {
                return;
            }

            foreach (KeyValuePair<string, (object value, Type type)> defaultValue in heapDefaultValues)
            {
                if (!symbolTable.HasAddressForSymbol(defaultValue.Key))
                {
                    continue;
                }

                uint symbolAddress = symbolTable.GetAddressFromSymbol(defaultValue.Key);
                (object value, Type declaredType) = defaultValue.Value;
                if (typeof(UnityEngine.Object).IsAssignableFrom(declaredType))
                {
                    if (value != null && !declaredType.IsInstanceOfType(value))
                    {
                        heap.SetHeapVariable(symbolAddress, null, declaredType);
                        continue;
                    }
                    if ((UnityEngine.Object)value == null)
                    {
                        heap.SetHeapVariable(symbolAddress, null, declaredType);
                        continue;
                    }
                }

                if (value != null)
                {
                    if (!declaredType.IsInstanceOfType(value))
                    {
                        value = declaredType.IsValueType ? Activator.CreateInstance(declaredType) : null;
                    }
                }

                heap.SetHeapVariable(symbolAddress, value, declaredType);
            }
        }


        private void AssembleProgram(string udonAssembly)
        {
            try
            {
                program = UdonEditorManager.Instance.Assemble(udonAssembly);
                _assemblyError = null;
            }
            catch (Exception e)
            {
                program = null;
                _assemblyError = e.Message;
            }
        }

        private string _sourceCode;
        private string _assemblyError;

        // ToDo: сделать кэширование этого всего через сериализацию (возможно не тут, а где-то отдельно)
        private static DoshikExternalApi _externalApi;
    }
}