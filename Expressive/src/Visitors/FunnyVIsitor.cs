using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Tree.Xpath;


namespace Expressive.AST
{
    public class StatementVisitor : ExpressiveBaseVisitor<StatementVisitor.Result>
    {
        public ExpressiveParser Parser { get; set; }
        public VM Vm { get; set; }

        public override Result VisitLet_statement([NotNull] ExpressiveParser.Let_statementContext context)
        {
            var binding = XPath.FindAll(context, "/*/binding", Parser).First();
            var name = binding.GetChild(0).GetText();
            
            var expression = XPath.FindAll(context, "/*/expression", Parser).First();
            var expression_visitor = new ExpressionVisitor() { Parser = Parser, Vm = Vm };
            var value = expression.Accept(expression_visitor);

            Vm.variables.Remove(name);
            Vm.variables.Add(name, value);
            return new();
        }

        public override Result VisitAssign_statement([NotNull] ExpressiveParser.Assign_statementContext context)
        {
            var variable = XPath.FindAll(context, "/*/variable", Parser).First();
            var name = variable.GetChild(0).GetText();
            if (!Vm.variables.ContainsKey(name))
                throw new Exception();
            
            var expression = XPath.FindAll(context, "/*/expression", Parser).First();
            var expression_visitor = new ExpressionVisitor() { Parser = Parser, Vm = Vm };
            var value = expression.Accept(expression_visitor);

            Vm.variables.Remove(name);
            Vm.variables.Add(name, value);
            return new();
        }

        public override Result VisitCompound_statement([NotNull] ExpressiveParser.Compound_statementContext context)
        {
            var variable = XPath.FindAll(context, "/*/variable", Parser).First();
            var name = variable.GetChild(0).GetText();
            var value1_maybe = Vm.variables.GetValueOrDefault(name);
            if (value1_maybe == null)
                throw new Exception();
            var value1 = value1_maybe ?? new VM.Value();

            var expression = XPath.FindAll(context, "/*/expression", Parser).First();
            var expression_visitor = new ExpressionVisitor() { Parser = Parser, Vm = Vm };
            var value2 = expression.Accept(expression_visitor);

            var op_tok = ((ITerminalNode)context.GetChild(1).GetChild(0)).Symbol.Type;
            VM.Value value;
            switch (op_tok)
            {
                case ExpressiveLexer.ADD_ASSIGNMENT:
                    value = value1 + value2;
                    break;
                case ExpressiveLexer.SUB_ASSIGNMENT:
                    value = value1 - value2;
                    break;
                case ExpressiveLexer.MUL_ASSIGNMENT:
                    value = value1 * value2;
                    break;
                case ExpressiveLexer.DIV_ASSIGNMENT:
                    value = value1 / value2;
                    break;
                case ExpressiveLexer.OR_ASSIGNMENT:
                    value = VM.Value.Or(value1, value2);
                    break;
                case ExpressiveLexer.AND_ASSIGNMENT:
                    value = VM.Value.And(value1, value2);
                    break;

                default:
                    throw new Exception();
            }

            Vm.variables.Remove(name);
            Vm.variables.Add(name, value);
            return new();
        }

        public override Result VisitIf_else_statement([NotNull] ExpressiveParser.If_else_statementContext context)
        {
            var expression = XPath.FindAll(context, "/*/expression", Parser).First();
            var expression_visitor = new ExpressionVisitor() { Parser = Parser, Vm = Vm };
            var value = expression.Accept(expression_visitor);
            if (value.IsFalsy())
                return new();
            
            return base.VisitIf_else_statement(context);
        }

        public override Result VisitExpression([NotNull] ExpressiveParser.ExpressionContext context)
        {
            var expression_visitor = new ExpressionVisitor() { Parser = Parser, Vm = Vm };
            _ = context.GetChild(0).Accept(expression_visitor);
            return new();
        }

        public class Result {}
    }

    public class ExpressionVisitor : ExpressiveBaseVisitor<VM.Value>
    {
        public ExpressiveParser Parser { get; set; }
        public VM Vm { get; set; }

        public override VM.Value VisitExpression([NotNull] ExpressiveParser.ExpressionContext context)
        {
            if (context.ChildCount == 3)
            {
                var operand1 = context.GetChild(0).Accept(this);
                var operand2 = context.GetChild(2).Accept(this);

                var op_tok = ((ITerminalNode)context.GetChild(1)).Symbol.Type;
                switch (op_tok)
                {
                    case ExpressiveLexer.ADD:
                        return operand1 + operand2;
                    case ExpressiveLexer.SUB:
                        return operand1 - operand2;
                    case ExpressiveLexer.MUL:
                        return operand1 * operand2;
                    case ExpressiveLexer.DIV:
                        return operand1 / operand2;

                    case ExpressiveLexer.OR:
                        return VM.Value.Or(operand1, operand2);
                    case ExpressiveLexer.AND:
                        return VM.Value.And(operand1, operand2);

                    case ExpressiveLexer.EQUALS:
                        return VM.Value.Eq(operand1, operand2);
                    case ExpressiveLexer.NOT_EQUALS:
                        return VM.Value.NotEq(operand1, operand2);
                }
            }

            return base.VisitExpression(context);
        }

        public override VM.Value VisitSimple_expression([NotNull] ExpressiveParser.Simple_expressionContext context)
        {

            var text = context.GetChild(0).GetText();

            // 🩼
            if (text == "nil") return new VM.Value();
            if (text == "false") return new VM.Value(false);
            if (text == "true") return new VM.Value(true);

            var tok = ((ITerminalNode)context.GetChild(0)).Symbol.Type;
            switch (tok)
            {
                case ExpressiveLexer.NIL:
                    return new VM.Value();
                
                case ExpressiveLexer.FALSE:
                    return new VM.Value(false);
                    
                case ExpressiveLexer.TRUE:
                    return new VM.Value(true);
                
                case ExpressiveLexer.INT:
                    return new VM.Value(int.Parse(text));

                case ExpressiveLexer.FLOAT:
                    return new VM.Value(float.Parse(text));
                
                case ExpressiveLexer.STRING:
                    return new VM.Value(Regex.Unescape(text.Substring(1, text.Length - 2)));
                
                case ExpressiveLexer.IDENTIFIER:
                    if (!Vm.variables.ContainsKey(text))
                        throw new Exception();
                    return Vm.variables.GetValueOrDefault(text, new VM.Value());
                
                case ExpressiveLexer.LEFT_PARENTHESIS:
                    return context.GetChild(1).Accept(this);
            }
            
            return new VM.Value();
        }

        public override VM.Value VisitPostfix([NotNull] ExpressiveParser.PostfixContext context)
        {
            var expression = context.GetChild(0).Accept(this);
            
            for (int i = 1; i < context.ChildCount; i++)
            {
                var call_or_index = context.GetChild(i);
                var visitor = new CallAndIndexVisitor() { Parser = Parser, Vm = Vm };
                var result = call_or_index.Accept(visitor);
                if (result.Type == CallAndIndexVisitor.Result.OpType.Index)
                    throw new NotImplementedException();
                if (expression.Type != VM.ValueType.Function)
                    throw new Exception();
                expression.Call(result.Args);
            }
            return new VM.Value();
        }
    }

    public class CallAndIndexVisitor : ExpressiveBaseVisitor<CallAndIndexVisitor.Result>
    {
        public ExpressiveParser Parser { get; set; }
        public VM Vm { get; set; }

        public override Result VisitCall([NotNull] ExpressiveParser.CallContext context)
        {
            var expression_visitor = new ExpressionVisitor() { Parser = Parser, Vm = Vm };
            var expressions = XPath.FindAll(context, "/*/expression", Parser);
            var args = expressions.Select(x => x.Accept(expression_visitor)).ToList();
            return new() { Type = Result.OpType.Call, Args = args };
        }

        public override Result VisitIndex([NotNull] ExpressiveParser.IndexContext context)
        {
            var expression_visitor = new ExpressionVisitor() { Parser = Parser, Vm = Vm };
            var expressions = XPath.FindAll(context, "/*/expression", Parser);
            var args = expressions.Select(x => x.Accept(expression_visitor)).ToList();
            return new() { Type = Result.OpType.Index, Args = args };
        }

        public class Result
        {
            public OpType Type { get; set; }
            public List<VM.Value> Args { get; set; } = new();

            public enum OpType
            {
                Call,
                Index
            }
        }
    }
}
