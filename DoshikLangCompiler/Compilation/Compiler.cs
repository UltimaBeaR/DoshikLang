using Antlr4.Runtime;
using DoshikLangCompiler.Compilation.Visitors;
using DoshikLangCompiler.UAssemblyGeneration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangCompiler.Compilation
{
    public class Compiler
    {
        // ToDo: для работы аналога reflection-а (вызов функций по динамически задаваемому имени, из переменной например) нужно всю информацию о внешнем апи (DoshikExternalApi)
        // хранить в heap-е assembly, а там много данных. Возможно потом сделаю, пока без этого




        // ToDo: сделать способ указания конструкторов типов в зависимости от строкового названия типа (например UnityEngineVector3) и какого-то литерального вида начального значения
        // то есть надо в компилятор передавать фабрику которая будет создавать экземпляры этих типов по запросу
        // 
        // Также нужно таким же образом задавать извне стандартные варианты функций и типов, которые можно объявлять в коде. Т.к. по сути это внешня зависимость с зависмостью на UnityEngine, а компилятор не должен от этого всего зависеть.
        // То есть весь доступный АПИ (функции и типы которые можно использовать) должен быть задан извне перед комплияцией


        /*
         * ToDo: На основе полученных извне (из юнити) данных из DoshikNodeDefinition можно их преобразовать в входную структуру компилятора:
         * 
         * 1) Список всех доступных типов (некий репозиторий).
         *      У каждого типа будет идентификатор (то как он референсится в параметрах, возвращаемых значениях методов, а также в самой части типа метода)
         *      также будет dotnet Type
         *      еще возможно будут другие свойства и флаги в зависимости от того где этот тип упоминается (он может упомнаться как тип от которого вызывается метод, как Type_, как Const_, как Variable_)
         *      список методов, которые можно применить в этому типу (они получаются из нодов, у которых Id начинается содержит ".__", Они щас логируются в methods.txt)
         *      у каждого метода в списке будет определен выходной параметр(параметры, но пока таких случаев не видел, вроде их несколько сделано для других случаев, например ивенты и бранч(там true/false выходные))
         *      и набор именованных входных параметров. также можно разбить эти методы на перегрузки операторов и т.д. (чтобы в языке сразу делать по ним операторы)
         *      
         *      то есть должна возможность компилятору по .net типу узнавать определение типа, доступное компилятору (с id типа и списком методов внутри доступных для графа) так и наоборот
         *      по id типа получить .net тип.
         *      
         *      также нужно определить видимо вручную, возможно захардкоженную внутри - фабрику изначальных значений для разных типов. например для int/string/float - это из 1го строкового значения (литерала)
         *      можно создать экземпляр .net типа. еще есть всякие Const_ графы, там есть всякие сложные контролы для обозначения начальных данных, например колор пикер. тоже надо решить как это в коде будет
         *      
         *      дальше еще в коде должно быть определено как будет работа с this. Я думаю что везде где можно юзать this просто в этом месте надо будет явно писать
         *      типа ThisGetComponent<Transform>.Rotate(axis, ange) - это будет то же самое что в графах Transform.Rotate(this, axis, angle).
         *      тогда можно будет другие объекты, Vector3 например юзать через переменная точка, то есть экземпляр будет через точку передаваться там где он доступен.
         *      доступен ли какому-то методу экземпляр, либо это чисто статический метод можно определять по первой переменной instance - если она есть значит это instance метод (ну или как то так)
         *      
         * 
         *      то что получается из Node объектов, но не относится ни к чему (методы, Const_, Type_, Variable_, Event_) - можно никак не обрабатывать. это всякие операторы ветвления и тд. их уже я буду сам делать
         *      вручную
         */


        /// <summary>
        /// Исходник на языке дошика
        /// </summary>
        public string SourceCode { get; set; }

        /// <summary>
        /// Определения доступного извне АПИ (вшнешние типы и методы - то же самое, что доступно в языке графов)
        /// </summary>
        public DoshikExternalApi ExternalApi { get; set; }

        public CompilerOutput Compile()
        {
            var output = new CompilerOutput();

            var compilationContext = new CompilationContext()
            {
                ExternalApi = ExternalApi
            };

            try
            {
                var inputStream = new AntlrInputStream(SourceCode);
                var lexer = new DoshikLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new DoshikParser(tokenStream);

                lexer.RemoveErrorListeners();
                parser.RemoveErrorListeners();

                var lexerErrorListener = new LexerErrorListener();
                var parserErrorListener = new ParserErrorListener();

                lexer.AddErrorListener(lexerErrorListener);
                parser.AddErrorListener(parserErrorListener);

                var antlrCompilationUnit = parser.compilationUnit();

                if (lexerErrorListener.Errors.Count > 0)
                {
                    compilationContext.ThrowCompilationError($"Error in lexer: { lexerErrorListener.Errors.First() }");
                }

                if (parserErrorListener.Errors.Count > 0)
                {
                    compilationContext.ThrowCompilationError($"Error in parser: { parserErrorListener.Errors.First() }");
                }

                var compilationUnit = CompilationUnitCreationVisitor.Apply(compilationContext, antlrCompilationUnit);

                var code = GenerateCode(compilationUnit);

                output.UdonAssemblyCode = code.UdonAssemblyCode;
                output.DefaultHeapValues = code.DefaultHeapValues;
            }
            catch (CompilationErrorException)
            {
                output.CompilationErrors = compilationContext.CompilationErrors;
            }

            return output;
        }

        private UAssemblyBuilderCode GenerateCode(CompilationUnit compilationUnit)
        {
            // Процесс генерации кода уже не должен валидировать исходный код (описываемый в структуре CompilationUnit)
            // Процесс генерации кода просто генерирует assembly код под target платформу (в данном случае это всегда udon assembly)
            // Все compile-time ошибки, которые могли быть должны быть обработаны ДО входа в этот метод.
            // В этом случае весь код, кроме вызова этого метода можно будет переиспользовать для анализаторов языка. Например сделать language server для подсветки кода
            // в vs code. Можно потом оформить библиотеку для создания CompilationUnit объекта отдельно и использовать ее тут же, просто послее нее будет вызываться эта генерация asm кода

            var assemblyBuilder = new UAssemblyBuilder();

            foreach (var variable in compilationUnit.Variables.Values.OrderBy(x => !x.IsPublic).ThenBy(x => x.Name))
            {
                assemblyBuilder.AddVariable(variable.IsPublic, "global__" + variable.Name, variable.Type);
            }

            // Сначала просто добавляем все ивенты, чтобы было их определение
            foreach (var eventHandler in compilationUnit.Events.Values.OrderBy(x => x.Name))
            {
                assemblyBuilder.AddOrGetEvent(eventHandler.Name);
            }

            // Генерируем код внутри каждого из ивентов, попутно генерируя переменные
            foreach (var eventHandler in compilationUnit.Events.Values.OrderBy(x => x.Name))
            {
                var eventBodyEmitter = assemblyBuilder.AddOrGetEvent(eventHandler.Name);

                eventBodyEmitter.JUMP_absoluteAddress(UAssemblyBuilder.maxCodeAddress);
            }

            return assemblyBuilder.MakeCode(true);
        }
    }

    public class CompilerOutput
    {
        /// <summary>
        /// Код на языке udon assembly. Это не код виртуальной машины а именно комманды на языке udon assembly
        /// </summary>
        public string UdonAssemblyCode { get; set; }

        /// <summary>
        /// Начальные значения переменных, объявленных в heap. Во всяком случае сейчас их невозможно объявить в коде, только извне.
        /// </summary>
        public Dictionary<string, (object value, Type type)> DefaultHeapValues { get; set; }

        public List<string> CompilationErrors { get; set; }
    }
}
