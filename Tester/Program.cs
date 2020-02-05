using Doshik;
using DoshikLangCompiler.Compilation;
using System;
using System.IO;

namespace Tester
{
    class Program
    {
        static void Main()
        {
            var projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\"));
            var sourceFilePath = Path.Combine(projectDir, "test.doshik");

            var source = File.ReadAllText(sourceFilePath);

            Console.WriteLine(source);
            Console.WriteLine();

            var externalApi = DoshikExternalApiCache.GetCachedApi();
            if (externalApi == null)
            {
                var generator = new DoshikExternalApiGenerator
                {
                    LogWarning = (text) => { Console.WriteLine("WARNING! " + text); }
                };

                Console.WriteLine("generating external api...");

                externalApi = generator.Generate();

                DoshikExternalApiCache.SetApiToCache(externalApi);
            }

            var compiler = new Compiler()
            {
                ExternalApi = externalApi
            };

            compiler.SourceCode = source;

            Console.WriteLine("compiling...");

            var output = compiler.Compile();

            if (output.CompilationErrors == null)
            {
                Console.WriteLine("compiled.");

                Console.WriteLine("code:");
                Console.WriteLine();
                Console.WriteLine("###############################################################");
                Console.WriteLine(output.UdonAssemblyCode);
                Console.WriteLine("###############################################################");

                Console.WriteLine();
                Console.WriteLine("variable default values:");
                Console.WriteLine();
                foreach (var variable in output.DefaultHeapValues)
                {
                    Console.WriteLine(variable.Key + " = " + variable.Value.value.ToString() + " (" + variable.Value.type.FullName + ")");
                }
            }
            else
            {
                Console.WriteLine("compilation ERRORS:");

                foreach (var error in output.CompilationErrors)
                {
                    Console.WriteLine(error);
                }
            }

            Console.ReadLine();

            //NodesLogger.LogRegistries();


            // ToDo: не все типы есть среди тех что указываются в сигнатурах методов (в типах параметров)
            // например ListT -> List<UnityEngine.Object> который идет входным параметром в одном из методов.
            // также есть в ивенте.
            // но т.к на основе сигнатуры нельзя точно определить assembly имя типа, т.к. разный порядок аргументов (из-за ref параметров)
            // то хз че тут делать. Возможно стоит эти имена из dotnet имен генерировать (возможно графы так и делают, им же надо как то в heap Обозначить тип переменной входной для передачи ее во входные параметр)
            // можно посмотреть как там происходит.
            // еще возможно что там работает логика даункаста в другие типы, например ListT не определено, но определен IEnumerableT (тоже от UnityEngine.Object) - может оно должно в этих случаях
            // всегда само как то даункастится в этот тип? тогда возможно и во входных параметрах в heap будет определен IEnumerableT. Вобщем надо поисследовать




            //var allTypes = api.Types
            //    .OrderBy(x => x.ExternalName)
            //    .ToArray();

            //TestLogger.Begin();

            //foreach (var type in allTypes)
            //{
            //    TestLogger.LogLine("all_types", type.ExternalName);
            //}

            //TestLogger.End();

            //Console.ReadKey();
        }

        /*void RenderMethod()
        {
            for (int inputIdx = 0; inputIdx < nodeDefinition.inputs.Length; ++inputIdx)
                state.instructions.Add(new UdonGraphCompiler.InstructionContainer()
                {
                    type = "PUSH",
                    parameter = inputDataNames[inputIdx]
                });
            bool isNoReturnParam = nodeDefinition.fullName.EndsWith("Void");
            for (
                int outputParamIdx = isNoReturnParam ? 0 : nodeDefinition.outputs.Length - 1;
                (isNoReturnParam ? (outputParamIdx < nodeDefinition.outputs.Length ? 1 : 0) : (outputParamIdx >= 0 ? 1 : 0)) != 0;
                outputParamIdx = isNoReturnParam ? outputParamIdx + 1 : outputParamIdx - 1
            )
            {
                bool flag2 = isNoReturnParam || nodeDefinition.outputs.Length > 1 && outputParamIdx == 0;
                Type output = nodeDefinition.outputs[outputParamIdx];
                if (!(output == typeof(void)) && !(output == (Type)null))
                {
                    UdonGraphCompiler.DataObjectContainer dataObjectContainer = new UdonGraphCompiler.DataObjectContainer()
                    {
                        type = output
                    };
                    if (state.uidToProperty.ContainsKey((node.uid, output.Name, outputParamIdx, false)))
                    {
                        dataObjectContainer.identifier = state.uidToProperty[(node.uid, output.Name, outputParamIdx, false)];
                    }
                    else
                    {
                        dataObjectContainer.identifier = !(string.IsNullOrEmpty(returnName) | flag2) ? returnName : UdonGraphCompiler.GenerateUniqueIdentifier(output.Name, state.compilerData);
                        state.uidToProperty.Add((node.uid, output.Name, outputParamIdx, false), dataObjectContainer.identifier);
                    }
                    if (string.IsNullOrEmpty(returnName) | flag2)
                    {
                        state.compilerData.Add(dataObjectContainer);
                        state.instructions.Add(new UdonGraphCompiler.InstructionContainer()
                        {
                            type = "PUSH",
                            parameter = dataObjectContainer.identifier
                        });
                    }
                    else
                        state.instructions.Add(new UdonGraphCompiler.InstructionContainer()
                        {
                            type = "PUSH",
                            parameter = returnName
                        });
                    str1 = dataObjectContainer.identifier;
                }
            }
            state.instructions.Add(new UdonGraphCompiler.InstructionContainer()
            {
                type = "EXTERN",
                parameter = "\"" + node.fullName + "\""
            });
        }*/
    }
}
