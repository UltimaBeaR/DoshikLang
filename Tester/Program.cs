using DoshikLangCompiler.Compilation;
using DoshikLangUnityEditor;
using System;
using System.Linq;

namespace Tester
{
    class Program
    {
        static void Main()
        {




            var source =
@"
float angle = 100500;


public int test = 5;

private string test2 = ""sdfasdfasdfsdf"";

event void _update()
{
    // GetComponent<T> это системная функция, ее вызов превращается в определение глобальной переменной
    // заданного типа, у которого инициализирующее значение это this. При этом это не обязательно компонент - это может быть любой тип у которого дефолтное значение
    // может быть this
    UnityEngineTransform thisTransform = GetComponent<UnityEngineTransform>();

            
    // ToDo: Надо сделать оператор new, т.к. у многих типов есть методы конструкторы.
    // возможно метод new const не нужен вообще, т.к. начальные значения нужны только для примитивных типов (литералы), остальное все можно создавать через
    // вызовы конструкторов

    thisTransform.Rotate(
        new UnityEngineVector3((float)0, (float)1, (float)0),
        (float)angle
    );
}

UnityEngineVector3 someVector;

";

            Console.WriteLine(source);
            Console.WriteLine();

            var compiler = new Compiler();

            var generator = new DoshikExternalApiGenerator
            {
                LogWarning = (text) => { Console.WriteLine("WARNING! " + text); }
            };

            Console.WriteLine("generating external api...");

            // ToDo: можно засериалазировать сгенерированное апи куда нибудь и присваивать тут результат десериализации из файла, чтобы каждый раз не генерировать его, т.к. это долго
            compiler.ExternalApi = generator.Generate();

            compiler.SourceCode = source;

            Console.WriteLine("compiling...");

            var output = compiler.Compile();


            Console.WriteLine("compiled. code:");
            Console.WriteLine(output.UdonAssemblyCode);

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
