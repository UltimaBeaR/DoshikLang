namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    // ToDo: это пока просто заготовка под объект с последовательностью / деревом выражений. Неясно пока каким он точно будет, но точно ясно что будет конвертироваться из ExpressionNode дерева
    public class Expression : ICodeHierarchyNode
    {
        public Expression(ICodeHierarchyNode parent)
        {
            Parent = parent;
        }

        public ICodeHierarchyNode Parent { get; private set; }
    }
}
