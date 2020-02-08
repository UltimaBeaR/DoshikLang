using Doshik;
using DoshikLangIR;
using System;
using System.Collections.Generic;

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

            var compilationUnit = IRBuilder.BuildCodeRepresentation(SourceCode, ExternalApi, out var compilationErrors);

            // ToDo: надо бы сделать какой то опциональный вывод лога со всеми данными из compilationUnit в этом месте
            // а еще лучше сделать какой-то визуализатор конечных нодов (особенно expression-ов), т.к. там дальше пойдет куча трасформаций и оптимизаций и без видения
            // графа очень сложно понять что происходит

            if (compilationUnit != null)
            {
                // Процесс генерации кода уже не должен валидировать исходный код (описываемый в структуре CompilationUnit)
                // Процесс генерации кода просто генерирует assembly код под target платформу (в данном случае это всегда udon assembly)
                // Все compile-time ошибки, которые могли быть должны быть обработаны ДО входа в этот метод.
                // В этом случае весь код, кроме вызова этого метода можно будет переиспользовать для анализаторов языка. Например сделать language server для подсветки кода
                // в vs code. Можно потом оформить библиотеку для создания CompilationUnit объекта отдельно и использовать ее тут же, просто послее нее будет вызываться эта генерация asm кода
                var code = (new CodeGenerator(compilationUnit, true)).GenerateCode();

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

        public List<CompilationError> CompilationErrors { get; set; }
    }
}
