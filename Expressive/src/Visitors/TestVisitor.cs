using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Expressive.AST
{
    class TestVisitor : ExpressiveBaseVisitor<int>
    {
        // public override int VisitChunk(ExpressiveParser.ChunkContext ctx) { return 3; }
        // public override int VisitBlock(ExpressiveParser.BlockContext ctx) { return 3; }
        // public override int VisitStatement_list(ExpressiveParser.Statement_listContext ctx) { return 3; }
        // public override int VisitStatement(ExpressiveParser.StatementContext ctx) { return 3; }
        // public override int VisitSimple_expression(ExpressiveParser.Simple_expressionContext ctx) { return 3; }
        // public override int VisitExpression(ExpressiveParser.ExpressionContext ctx) { return 3; }
        public override int VisitPostfix(ExpressiveParser.PostfixContext ctx)
        {
            base.VisitChildren(ctx);
            if (ctx.children[0].GetText() != "print") { return 3; }
            var s = ctx.children[1].GetChild(1).GetChild(0).GetChild(0).GetText();
            s = s.Trim('\"');
            s = Regex.Unescape(s);
            Console.WriteLine(s);

            return 3;
        }
        // public override int VisitCall(ExpressiveParser.CallContext ctx) { return 3; }
        // public override int VisitIndex(ExpressiveParser.IndexContext ctx) { return 3; }
        // public override int VisitTerrorist_if(ExpressiveParser.Terrorist_ifContext ctx) { return 3; }
        // public override int VisitBinding(ExpressiveParser.BindingContext ctx) { return 3; }
        // public override int VisitBinding_list(ExpressiveParser.Binding_listContext ctx) { return 3; }
        // public override int VisitVariable(ExpressiveParser.VariableContext ctx) { return 3; }
        // public override int VisitFunction_def(ExpressiveParser.Function_defContext ctx) { return 3; }
        // public override int VisitLambda_def(ExpressiveParser.Lambda_defContext ctx) { return 3; }
    }
}