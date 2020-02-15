using Doshik;
using DoshikLangCompiler;
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
                    if (variable.Value is ConcreteValueDefaultHeapValue concreteValue)
                        Console.WriteLine(variable.Key + " = [value] '" + concreteValue.Value.ToString() + "' (" + concreteValue.Type.FullName + ")");
                    else if (variable.Value is TypeAsStringDefaultHeapValue typeAsString)
                        Console.WriteLine(variable.Key + " = [type] '" + typeAsString.TypeAsString + "'");
                    else
                        Console.WriteLine(variable.Key + " = ???");
                }
            }
            else
            {
                Console.WriteLine("compilation ERRORS:");

                foreach (var error in output.CompilationErrors)
                {
                    Console.WriteLine($"{ error.LineIdx + 1 }, { error.CharInLineIdx + 1 }: { error.Message }");
                }
            }

            Console.ReadLine();
        }
    }
}
