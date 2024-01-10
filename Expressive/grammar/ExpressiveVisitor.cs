//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from x://coding//gamedev//other//expressive//Expressive//grammar//Expressive.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="ExpressiveParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IExpressiveVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.chunk"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitChunk([NotNull] ExpressiveParser.ChunkContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlock([NotNull] ExpressiveParser.BlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.statement_list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement_list([NotNull] ExpressiveParser.Statement_listContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] ExpressiveParser.StatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.let_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLet_statement([NotNull] ExpressiveParser.Let_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.assign_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssign_statement([NotNull] ExpressiveParser.Assign_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.compound_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompound_statement([NotNull] ExpressiveParser.Compound_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.if_else_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIf_else_statement([NotNull] ExpressiveParser.If_else_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.for_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFor_statement([NotNull] ExpressiveParser.For_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.while_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhile_statement([NotNull] ExpressiveParser.While_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.loop_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLoop_statement([NotNull] ExpressiveParser.Loop_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.return_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReturn_statement([NotNull] ExpressiveParser.Return_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.simple_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSimple_expression([NotNull] ExpressiveParser.Simple_expressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] ExpressiveParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.postfix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPostfix([NotNull] ExpressiveParser.PostfixContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.call"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCall([NotNull] ExpressiveParser.CallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.index"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIndex([NotNull] ExpressiveParser.IndexContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.terrorist_if"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTerrorist_if([NotNull] ExpressiveParser.Terrorist_ifContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.binding"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinding([NotNull] ExpressiveParser.BindingContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.binding_list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinding_list([NotNull] ExpressiveParser.Binding_listContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.variable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariable([NotNull] ExpressiveParser.VariableContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.function_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunction_def([NotNull] ExpressiveParser.Function_defContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.lambda_def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLambda_def([NotNull] ExpressiveParser.Lambda_defContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExpressiveParser.compound_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompound_op([NotNull] ExpressiveParser.Compound_opContext context);
}
