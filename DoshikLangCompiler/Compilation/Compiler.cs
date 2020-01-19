using Antlr4.Runtime;
using DoshikLangCompiler.Compilation.CodeRepresentation;
using DoshikLangCompiler.Compilation.Visitors;
using DoshikLangCompiler.UAssemblyGeneration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoshikLangCompiler.Compilation
{
    public class Compiler
    {
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

            var compilationUnit = BuildCodeRepresentation(out var compilationErrors);

            if (compilationUnit != null)
            {
                var code = GenerateCode(compilationUnit);

                output.UdonAssemblyCode = code.UdonAssemblyCode;
                output.DefaultHeapValues = code.DefaultHeapValues;

                return output;
            }
            else
            {
                output.CompilationErrors = compilationErrors;
                return output;
            }
        }

        private CompilationUnit BuildCodeRepresentation(out List<string> compilationErrors)
        {
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

                compilationErrors = null;
                return compilationUnit;
            }
            catch (CompilationErrorException)
            {
                compilationErrors = compilationContext.CompilationErrors;
                return null;
            }
        }

        private UAssemblyBuilderCode GenerateCode(CompilationUnit compilationUnit)
        {
            // Процесс генерации кода уже не должен валидировать исходный код (описываемый в структуре CompilationUnit)
            // Процесс генерации кода просто генерирует assembly код под target платформу (в данном случае это всегда udon assembly)
            // Все compile-time ошибки, которые могли быть должны быть обработаны ДО входа в этот метод.
            // В этом случае весь код, кроме вызова этого метода можно будет переиспользовать для анализаторов языка. Например сделать language server для подсветки кода
            // в vs code. Можно потом оформить библиотеку для создания CompilationUnit объекта отдельно и использовать ее тут же, просто послее нее будет вызываться эта генерация asm кода

            var assemblyBuilder = new UAssemblyBuilder();

            foreach (var variable in compilationUnit.Scope.Variables.Values.Cast<CompilationUnitVariable>().OrderBy(x => !x.IsPublic).ThenBy(x => x.Name))
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
