//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Programs\DoshikLang\DoshikLangAntlr\DoshikParser.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IDoshikParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public partial class DoshikParserBaseListener : IDoshikParserListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.compilationUnit"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCompilationUnit([NotNull] DoshikParser.CompilationUnitContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.compilationUnit"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCompilationUnit([NotNull] DoshikParser.CompilationUnitContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.memberDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMemberDeclaration([NotNull] DoshikParser.MemberDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.memberDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMemberDeclaration([NotNull] DoshikParser.MemberDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.methodDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodDeclaration([NotNull] DoshikParser.MethodDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.methodDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodDeclaration([NotNull] DoshikParser.MethodDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.typeTypeOrVoid"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeTypeOrVoid([NotNull] DoshikParser.TypeTypeOrVoidContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.typeTypeOrVoid"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeTypeOrVoid([NotNull] DoshikParser.TypeTypeOrVoidContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.fieldDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFieldDeclaration([NotNull] DoshikParser.FieldDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.fieldDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFieldDeclaration([NotNull] DoshikParser.FieldDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.variableDeclarator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableDeclarator([NotNull] DoshikParser.VariableDeclaratorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.variableDeclarator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableDeclarator([NotNull] DoshikParser.VariableDeclaratorContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.variableInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableInitializer([NotNull] DoshikParser.VariableInitializerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.variableInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableInitializer([NotNull] DoshikParser.VariableInitializerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.arrayInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArrayInitializer([NotNull] DoshikParser.ArrayInitializerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.arrayInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArrayInitializer([NotNull] DoshikParser.ArrayInitializerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.classOrInterfaceType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassOrInterfaceType([NotNull] DoshikParser.ClassOrInterfaceTypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.classOrInterfaceType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassOrInterfaceType([NotNull] DoshikParser.ClassOrInterfaceTypeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.typeArgument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeArgument([NotNull] DoshikParser.TypeArgumentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.typeArgument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeArgument([NotNull] DoshikParser.TypeArgumentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.formalParameters"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFormalParameters([NotNull] DoshikParser.FormalParametersContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.formalParameters"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFormalParameters([NotNull] DoshikParser.FormalParametersContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.formalParameterList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFormalParameterList([NotNull] DoshikParser.FormalParameterListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.formalParameterList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFormalParameterList([NotNull] DoshikParser.FormalParameterListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.formalParameter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFormalParameter([NotNull] DoshikParser.FormalParameterContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.formalParameter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFormalParameter([NotNull] DoshikParser.FormalParameterContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLiteral([NotNull] DoshikParser.LiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLiteral([NotNull] DoshikParser.LiteralContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.integerLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIntegerLiteral([NotNull] DoshikParser.IntegerLiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.integerLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIntegerLiteral([NotNull] DoshikParser.IntegerLiteralContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlock([NotNull] DoshikParser.BlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlock([NotNull] DoshikParser.BlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.blockStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlockStatement([NotNull] DoshikParser.BlockStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.blockStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlockStatement([NotNull] DoshikParser.BlockStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.localVariableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLocalVariableDeclaration([NotNull] DoshikParser.LocalVariableDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.localVariableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLocalVariableDeclaration([NotNull] DoshikParser.LocalVariableDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] DoshikParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] DoshikParser.StatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.forControl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterForControl([NotNull] DoshikParser.ForControlContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.forControl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitForControl([NotNull] DoshikParser.ForControlContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.forInit"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterForInit([NotNull] DoshikParser.ForInitContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.forInit"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitForInit([NotNull] DoshikParser.ForInitContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.parExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParExpression([NotNull] DoshikParser.ParExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.parExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParExpression([NotNull] DoshikParser.ParExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.expressionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpressionList([NotNull] DoshikParser.ExpressionListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.expressionList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpressionList([NotNull] DoshikParser.ExpressionListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.methodCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodCall([NotNull] DoshikParser.MethodCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.methodCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodCall([NotNull] DoshikParser.MethodCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.methodCallParams"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodCallParams([NotNull] DoshikParser.MethodCallParamsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.methodCallParams"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodCallParams([NotNull] DoshikParser.MethodCallParamsContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.methodCallParam"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodCallParam([NotNull] DoshikParser.MethodCallParamContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.methodCallParam"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodCallParam([NotNull] DoshikParser.MethodCallParamContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.newCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNewCall([NotNull] DoshikParser.NewCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.newCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNewCall([NotNull] DoshikParser.NewCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>primaryExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrimaryExpression([NotNull] DoshikParser.PrimaryExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>primaryExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrimaryExpression([NotNull] DoshikParser.PrimaryExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>newCallExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNewCallExpression([NotNull] DoshikParser.NewCallExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>newCallExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNewCallExpression([NotNull] DoshikParser.NewCallExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>dotExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDotExpression([NotNull] DoshikParser.DotExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>dotExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDotExpression([NotNull] DoshikParser.DotExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>additionExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAdditionExpression([NotNull] DoshikParser.AdditionExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>additionExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAdditionExpression([NotNull] DoshikParser.AdditionExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>unaryPrefixExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterUnaryPrefixExpression([NotNull] DoshikParser.UnaryPrefixExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>unaryPrefixExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitUnaryPrefixExpression([NotNull] DoshikParser.UnaryPrefixExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>methodCallExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodCallExpression([NotNull] DoshikParser.MethodCallExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>methodCallExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodCallExpression([NotNull] DoshikParser.MethodCallExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>assignmentExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAssignmentExpression([NotNull] DoshikParser.AssignmentExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>assignmentExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAssignmentExpression([NotNull] DoshikParser.AssignmentExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>bracketsExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBracketsExpression([NotNull] DoshikParser.BracketsExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>bracketsExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBracketsExpression([NotNull] DoshikParser.BracketsExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>notExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNotExpression([NotNull] DoshikParser.NotExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>notExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNotExpression([NotNull] DoshikParser.NotExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>multiplicationExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMultiplicationExpression([NotNull] DoshikParser.MultiplicationExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>multiplicationExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMultiplicationExpression([NotNull] DoshikParser.MultiplicationExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>unaryPostfixExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterUnaryPostfixExpression([NotNull] DoshikParser.UnaryPostfixExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>unaryPostfixExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitUnaryPostfixExpression([NotNull] DoshikParser.UnaryPostfixExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>typeDotExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeDotExpression([NotNull] DoshikParser.TypeDotExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>typeDotExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeDotExpression([NotNull] DoshikParser.TypeDotExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>relativeExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRelativeExpression([NotNull] DoshikParser.RelativeExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>relativeExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRelativeExpression([NotNull] DoshikParser.RelativeExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ifElseExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfElseExpression([NotNull] DoshikParser.IfElseExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ifElseExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfElseExpression([NotNull] DoshikParser.IfElseExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>orExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOrExpression([NotNull] DoshikParser.OrExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>orExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOrExpression([NotNull] DoshikParser.OrExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>andExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAndExpression([NotNull] DoshikParser.AndExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>andExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAndExpression([NotNull] DoshikParser.AndExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>equalsExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEqualsExpression([NotNull] DoshikParser.EqualsExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>equalsExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEqualsExpression([NotNull] DoshikParser.EqualsExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>typecastExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypecastExpression([NotNull] DoshikParser.TypecastExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>typecastExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypecastExpression([NotNull] DoshikParser.TypecastExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParenthesisExpression([NotNull] DoshikParser.ParenthesisExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParenthesisExpression([NotNull] DoshikParser.ParenthesisExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>literalExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLiteralExpression([NotNull] DoshikParser.LiteralExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>literalExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLiteralExpression([NotNull] DoshikParser.LiteralExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>identifierExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIdentifierExpression([NotNull] DoshikParser.IdentifierExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>identifierExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIdentifierExpression([NotNull] DoshikParser.IdentifierExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.typeType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeType([NotNull] DoshikParser.TypeTypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.typeType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeType([NotNull] DoshikParser.TypeTypeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.primitiveType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrimitiveType([NotNull] DoshikParser.PrimitiveTypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.primitiveType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrimitiveType([NotNull] DoshikParser.PrimitiveTypeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.typeArguments"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypeArguments([NotNull] DoshikParser.TypeArgumentsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.typeArguments"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypeArguments([NotNull] DoshikParser.TypeArgumentsContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
