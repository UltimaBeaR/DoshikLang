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
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="DoshikParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public interface IDoshikParserListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.compilationUnit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCompilationUnit([NotNull] DoshikParser.CompilationUnitContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.compilationUnit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCompilationUnit([NotNull] DoshikParser.CompilationUnitContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.memberDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMemberDeclaration([NotNull] DoshikParser.MemberDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.memberDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMemberDeclaration([NotNull] DoshikParser.MemberDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.methodDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodDeclaration([NotNull] DoshikParser.MethodDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.methodDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodDeclaration([NotNull] DoshikParser.MethodDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.typeTypeOrVoid"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeTypeOrVoid([NotNull] DoshikParser.TypeTypeOrVoidContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.typeTypeOrVoid"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeTypeOrVoid([NotNull] DoshikParser.TypeTypeOrVoidContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.fieldDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldDeclaration([NotNull] DoshikParser.FieldDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.fieldDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldDeclaration([NotNull] DoshikParser.FieldDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.variableDeclarator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableDeclarator([NotNull] DoshikParser.VariableDeclaratorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.variableDeclarator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableDeclarator([NotNull] DoshikParser.VariableDeclaratorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.variableInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableInitializer([NotNull] DoshikParser.VariableInitializerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.variableInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableInitializer([NotNull] DoshikParser.VariableInitializerContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.arrayInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArrayInitializer([NotNull] DoshikParser.ArrayInitializerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.arrayInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArrayInitializer([NotNull] DoshikParser.ArrayInitializerContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.classOrInterfaceType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassOrInterfaceType([NotNull] DoshikParser.ClassOrInterfaceTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.classOrInterfaceType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassOrInterfaceType([NotNull] DoshikParser.ClassOrInterfaceTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.typeArgument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeArgument([NotNull] DoshikParser.TypeArgumentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.typeArgument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeArgument([NotNull] DoshikParser.TypeArgumentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.formalParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFormalParameters([NotNull] DoshikParser.FormalParametersContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.formalParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFormalParameters([NotNull] DoshikParser.FormalParametersContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.formalParameterList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFormalParameterList([NotNull] DoshikParser.FormalParameterListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.formalParameterList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFormalParameterList([NotNull] DoshikParser.FormalParameterListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.formalParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFormalParameter([NotNull] DoshikParser.FormalParameterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.formalParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFormalParameter([NotNull] DoshikParser.FormalParameterContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteral([NotNull] DoshikParser.LiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteral([NotNull] DoshikParser.LiteralContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.integerLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIntegerLiteral([NotNull] DoshikParser.IntegerLiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.integerLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIntegerLiteral([NotNull] DoshikParser.IntegerLiteralContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlock([NotNull] DoshikParser.BlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlock([NotNull] DoshikParser.BlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.blockStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlockStatement([NotNull] DoshikParser.BlockStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.blockStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlockStatement([NotNull] DoshikParser.BlockStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.localVariableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLocalVariableDeclaration([NotNull] DoshikParser.LocalVariableDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.localVariableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLocalVariableDeclaration([NotNull] DoshikParser.LocalVariableDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] DoshikParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] DoshikParser.StatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.forControl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterForControl([NotNull] DoshikParser.ForControlContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.forControl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitForControl([NotNull] DoshikParser.ForControlContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.forInit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterForInit([NotNull] DoshikParser.ForInitContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.forInit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitForInit([NotNull] DoshikParser.ForInitContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.parExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParExpression([NotNull] DoshikParser.ParExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.parExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParExpression([NotNull] DoshikParser.ParExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.expressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpressionList([NotNull] DoshikParser.ExpressionListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.expressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpressionList([NotNull] DoshikParser.ExpressionListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodCall([NotNull] DoshikParser.MethodCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodCall([NotNull] DoshikParser.MethodCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.methodCallParams"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodCallParams([NotNull] DoshikParser.MethodCallParamsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.methodCallParams"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodCallParams([NotNull] DoshikParser.MethodCallParamsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.methodCallParam"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodCallParam([NotNull] DoshikParser.MethodCallParamContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.methodCallParam"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodCallParam([NotNull] DoshikParser.MethodCallParamContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.newCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNewCall([NotNull] DoshikParser.NewCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.newCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNewCall([NotNull] DoshikParser.NewCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.newConstCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNewConstCall([NotNull] DoshikParser.NewConstCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.newConstCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNewConstCall([NotNull] DoshikParser.NewConstCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.newConstCallParams"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNewConstCallParams([NotNull] DoshikParser.NewConstCallParamsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.newConstCallParams"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNewConstCallParams([NotNull] DoshikParser.NewConstCallParamsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.newConstCallParam"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNewConstCallParam([NotNull] DoshikParser.NewConstCallParamContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.newConstCallParam"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNewConstCallParam([NotNull] DoshikParser.NewConstCallParamContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.constExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConstExpression([NotNull] DoshikParser.ConstExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.constExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConstExpression([NotNull] DoshikParser.ConstExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.constArrayInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConstArrayInitializer([NotNull] DoshikParser.ConstArrayInitializerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.constArrayInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConstArrayInitializer([NotNull] DoshikParser.ConstArrayInitializerContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>primaryExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrimaryExpression([NotNull] DoshikParser.PrimaryExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>primaryExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrimaryExpression([NotNull] DoshikParser.PrimaryExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>newCallExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNewCallExpression([NotNull] DoshikParser.NewCallExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>newCallExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNewCallExpression([NotNull] DoshikParser.NewCallExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>dotExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDotExpression([NotNull] DoshikParser.DotExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>dotExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDotExpression([NotNull] DoshikParser.DotExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>additionExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAdditionExpression([NotNull] DoshikParser.AdditionExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>additionExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAdditionExpression([NotNull] DoshikParser.AdditionExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>unaryPrefixExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUnaryPrefixExpression([NotNull] DoshikParser.UnaryPrefixExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>unaryPrefixExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUnaryPrefixExpression([NotNull] DoshikParser.UnaryPrefixExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>newConstCallExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNewConstCallExpression([NotNull] DoshikParser.NewConstCallExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>newConstCallExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNewConstCallExpression([NotNull] DoshikParser.NewConstCallExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>methodCallExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodCallExpression([NotNull] DoshikParser.MethodCallExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>methodCallExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodCallExpression([NotNull] DoshikParser.MethodCallExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>assignmentExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignmentExpression([NotNull] DoshikParser.AssignmentExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>assignmentExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignmentExpression([NotNull] DoshikParser.AssignmentExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>bracketsExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBracketsExpression([NotNull] DoshikParser.BracketsExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>bracketsExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBracketsExpression([NotNull] DoshikParser.BracketsExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>notExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNotExpression([NotNull] DoshikParser.NotExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>notExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNotExpression([NotNull] DoshikParser.NotExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>multiplicationExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMultiplicationExpression([NotNull] DoshikParser.MultiplicationExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>multiplicationExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMultiplicationExpression([NotNull] DoshikParser.MultiplicationExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>unaryPostfixExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUnaryPostfixExpression([NotNull] DoshikParser.UnaryPostfixExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>unaryPostfixExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUnaryPostfixExpression([NotNull] DoshikParser.UnaryPostfixExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>relativeExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRelativeExpression([NotNull] DoshikParser.RelativeExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>relativeExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRelativeExpression([NotNull] DoshikParser.RelativeExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ifElseExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfElseExpression([NotNull] DoshikParser.IfElseExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ifElseExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfElseExpression([NotNull] DoshikParser.IfElseExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>orExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOrExpression([NotNull] DoshikParser.OrExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>orExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOrExpression([NotNull] DoshikParser.OrExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>andExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAndExpression([NotNull] DoshikParser.AndExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>andExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAndExpression([NotNull] DoshikParser.AndExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>equalsExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEqualsExpression([NotNull] DoshikParser.EqualsExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>equalsExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEqualsExpression([NotNull] DoshikParser.EqualsExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>typecastExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypecastExpression([NotNull] DoshikParser.TypecastExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>typecastExpression</c>
	/// labeled alternative in <see cref="DoshikParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypecastExpression([NotNull] DoshikParser.TypecastExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenthesisExpression([NotNull] DoshikParser.ParenthesisExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenthesisExpression([NotNull] DoshikParser.ParenthesisExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>literalExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteralExpression([NotNull] DoshikParser.LiteralExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>literalExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteralExpression([NotNull] DoshikParser.LiteralExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>identifierExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdentifierExpression([NotNull] DoshikParser.IdentifierExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>identifierExpression</c>
	/// labeled alternative in <see cref="DoshikParser.primary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdentifierExpression([NotNull] DoshikParser.IdentifierExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.typeType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeType([NotNull] DoshikParser.TypeTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.typeType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeType([NotNull] DoshikParser.TypeTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.primitiveType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrimitiveType([NotNull] DoshikParser.PrimitiveTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.primitiveType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrimitiveType([NotNull] DoshikParser.PrimitiveTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="DoshikParser.typeArguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeArguments([NotNull] DoshikParser.TypeArgumentsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="DoshikParser.typeArguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeArguments([NotNull] DoshikParser.TypeArgumentsContext context);
}
