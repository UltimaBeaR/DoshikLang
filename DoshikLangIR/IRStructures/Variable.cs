﻿namespace DoshikLangIR
{
    public class Variable
    {
        public Variable(IVariableDeclarator declarator)
        {
            Declarator = declarator;
        }

        public DataType Type { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Ссылка на место, где переменная была объявлена
        /// </summary>
        public IVariableDeclarator Declarator { get; private set; }
    }

    /// <summary>
    /// Место в иерархии, в котором определена переменная.
    /// Это может быть глобальный объект CompilationUnt (если переменная объявлена глобально, то есть на уровне всего текущего компонента).
    /// Также это может быть параметр в методе/событии, отдельный statement (если это локальная переменная в теле метода/события)
    /// а также часть синтаксической конструкции (инициализирующая часть в блоке for / foreach, там где можно объявлять переменные)
    /// </summary>
    public interface IVariableDeclarator : ICodeHierarchyNode
    {
    }
}
