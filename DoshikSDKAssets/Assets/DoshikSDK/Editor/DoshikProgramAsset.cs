using System;
using System.Collections.Generic;
using VRC.Udon.Common.Interfaces;
using VRC.Udon.Editor;
using Doshik;
using DoshikLangCompiler.Compilation;

namespace DoshikSDK
{
    public class DoshikProgramAsset : UdonProgramAsset
    {
        public string CompilationError { get; private set; }

        public void SetSourceCode(string sourceCode)
        {
            _sourceCode = sourceCode;
        }

        protected override void DoRefreshProgramActions()
        {
            // ToDo: сейчас берет кэш, а если кэша нету, то генерирует новое апи.
            // Нужно найти какое-то событие эдитора - при первом запуске эдитора делать принудительную генерацию апи и ложить ее в кэш (ну или ставить какой-то глобальный флаг что первое
            // получение апи надо сделать с помощью генерации)
            // то есть должна быть гарантия что после перезапуска юнити апи будет пере-генерировано на основе актуального sdk, а не взято из кэша (т.к. там может быть старый sdk)

            var externalApi = DoshikExternalApiCache.GetCachedApi();
            if (externalApi == null)
            {
                var generator = new DoshikExternalApiGenerator
                {
                    LogWarning = (text) =>
                    {
                        UnityEngine.Debug.LogWarning("Doshik: Generate Udon API: " + text);
                    }
                };

                UnityEngine.Debug.Log("Doshik: Generating external api...");

                externalApi = generator.Generate();

                DoshikExternalApiCache.SetApiToCache(externalApi);
            }

            var compiler = new Compiler();

            compiler.ExternalApi = externalApi;

            compiler.SourceCode = _sourceCode;

            try
            {
                var output = compiler.Compile();

                if (output.CompilationErrors == null)
                {
                    var assemblyError = AssembleProgram(output.UdonAssemblyCode);

                    if (assemblyError != null)
                    {
                        CompilationError = "Assembly: " + assemblyError;
                        UnityEngine.Debug.LogError("Doshik: " + CompilationError);

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
                        CompilationError = "Compilation: " + error;
                        UnityEngine.Debug.LogError("Doshik: " + CompilationError);

                        program = null;
                    }
                }
            }
            catch (Exception ex)
            {
                CompilationError = "Unexpected exception while compiling: " + ex.ToString();
                UnityEngine.Debug.LogError("Doshik: " + CompilationError);

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


        private string AssembleProgram(string udonAssembly)
        {
            try
            {
                program = UdonEditorManager.Instance.Assemble(udonAssembly);
                return null;
            }
            catch (Exception e)
            {
                program = null;
                return e.Message;
            }
        }

        private string _sourceCode;
    }
}