using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace Expressive
{
	partial class Parser
	{
        private int Index { get; set; }
        private List<Token> Tokens { get; set; }
        private Token CurrentToken { get; set; }
        private Stack<int> SavedIndices { get; }


        private readonly HashSet<TokenType> unaryOps = new HashSet<TokenType> () {
            TokenType.Sub, TokenType.Not
        };

        private readonly HashSet<TokenType> binaryOps = new HashSet<TokenType> () {
            TokenType.Add, TokenType.Sub,
            TokenType.Mul, TokenType.Div,
            TokenType.Mod,
            TokenType.Pow,
            TokenType.BitwiseOr, TokenType.BitwiseAnd, TokenType.BitwiseXor,
            TokenType.BitwiseLeftShift, TokenType.BitwiseRightShift,
            
            TokenType.Or, TokenType.And,
            TokenType.Equals, TokenType.NotEquals,
            TokenType.LessThan, TokenType.LessThanOrEquals,
            TokenType.GreaterThan, TokenType.GreaterThanOrEquals,

            TokenType.Range,
        };

        private readonly HashSet<TokenType> assignmentOps = new HashSet<TokenType> () {
            TokenType.Assignment,

            TokenType.AddAssignment, TokenType.SubAssignment,
            TokenType.MulAssignment, TokenType.DivAssignment,
            TokenType.ModAssignment,
            TokenType.PowAssignment,

            TokenType.BitwiseOrAssignment, TokenType.BitwiseAndAssignment, TokenType.BitwiseXorAssignment,
            TokenType.BitwiseLeftShiftAssignment, TokenType.BitwiseRightShiftAssignment,
        };


        private TokenType CurrentTokenType { get {
            return CurrentToken.Type;
        } }

        private Position CurrentPosition { get {
            return CurrentToken.Position;
        } }
        
        public Parser(List<Token> tokens)
        {
            CurrentToken = tokens[0];
            Tokens = tokens;
            SavedIndices = new Stack<int>();
        }

        private void Advance() 
        {
            Index += 1;
            CurrentToken = Tokens[Index];
        }

        private TokenType? LookAhead(int i)
        {
            return Tokens.Count > Index + i ? Tokens[Index + i].Type : null;
        }
        
        private int Save()
        {
            return Index;
        }

        private void Load(int index)
        {
            Index = index - 1;
            Advance();
        }

        private bool IsEOF()
        {
            return CurrentTokenType == TokenType.EOF;
        }

        public Result Parse()
        {
            Index = -1; Advance();
            var result = new Result();
            result.Register(Chunk());
            return result;
        }

        private Result Chunk()
        {
            var result = new Result();
            
            result.Register(StatementList());

            return result;
        }

        private Result Block()
        {
            var result = new Result();
            var leftBracePosition = CurrentToken.Position;
            if (CurrentTokenType != TokenType.LeftBrace)
                return result.Fail(CurrentPosition, "Expected '{'");
            Advance();
            while (CurrentTokenType == TokenType.Separator)
                Advance();
            if (CurrentTokenType != TokenType.RightBrace)
            {
                result.Register(Chunk());
                if (result.IsError) return result;
                while (CurrentTokenType == TokenType.Separator)
                    Advance();
            }
            if (CurrentTokenType != TokenType.RightBrace)
                return result.Fail(CurrentPosition, "Expected '}' (to close '{' at " + leftBracePosition + ")");
            Advance();
            return result;
        }
        
        private Result StatementList()
        {
            var result = new Result();
            
            while (!IsEOF()) {
                var statementResult = Statement();
                if (statementResult.IsError)
                    return statementResult;
                
                result.Register(statementResult);
                
                if (CurrentTokenType == TokenType.RightBrace)
                    return result;

                if (CurrentTokenType != TokenType.Separator)
                    return result.Fail(CurrentPosition, "Expected ';' or a newline");
                
                Advance();
                if (CurrentTokenType == TokenType.RightBrace)
                    return result;
                
                while (CurrentTokenType == TokenType.Separator)
                {
                    Advance();
                    if (CurrentTokenType == TokenType.RightBrace)
                        return result;
                }
            }

            return result;
        }

        private Result Statement()
        {
            var result = new Result();
            switch (CurrentTokenType)
            {
                case TokenType.Let:
                    Advance();
                    if (CurrentTokenType == TokenType.Global)
                    {
                        Advance();
                    }

                    result.Register(BindingList());
                    if (result.IsError) return result;

                    if (CurrentTokenType == TokenType.Assignment)
                    {
                        Advance();
                        result.Register(ExpressionList());
                    }
                    return result;
                
                case TokenType.If:
                    Advance();
                    result.Register(Expression());
                    if (result.IsError) return result;
                    result.Register(Block());
                    if (result.IsError) return result;
                    while (CurrentTokenType == TokenType.Else) {
                        Advance();
                        if (CurrentTokenType == TokenType.If)
                        {
                            Advance();
                            result.Register(Expression());
                            if (result.IsError) return result;
                            result.Register(Block());
                            if (result.IsError) return result;
                            continue;
                        }

                        result.Register(Block());
                        break;
                    }
                    return result;
                

                case TokenType.Loop:
                    Advance();
                    return result.Register(Block());

                case TokenType.While:
                    Advance();
                    result.Register(Expression());
                    if (result.IsError) return result;
                    result.Register(Block());
                    return result;
                
                case TokenType.For:
                    Advance();
                    result.Register(BindingList());
                    if (result.IsError) return result;
                    if (CurrentTokenType != TokenType.In)
                        return result.Fail(CurrentPosition, "Expected 'in'");
                    Advance();
                    result.Register(Expression());
                    if (result.IsError) return result;
                    result.Register(Block());
                    return result;
                
                case TokenType.Break:
                    Advance();
                    return result;

                case TokenType.Continue:
                    Advance();
                    return result;

                
                case TokenType.Fn:
                    Advance();
                    result.Register(FunctionName());
                    if (result.IsError) return result;
                    result.Register(FunctionBody());
                    return result;
                
                case TokenType.Return:
                    Advance();
                    result.Register(Expression());
                    return result;


                case TokenType.LeftBrace:
                    return result.Register(Block());
            }

            // Check if it's assignment-type op
            var secondTokenType = LookAhead(1);
            if (secondTokenType.HasValue && assignmentOps.Contains(secondTokenType.Value))
            {
                result.Register(Variable());
                if (result.IsError) return result;
                result.Register(AssignmentOp());
                if (result.IsError) return result;
                result.Register(Expression());
                if (result.IsError) return result.Fail(CurrentPosition, "Expected right-hand side expression, got '" + CurrentPosition.CurrentChar + "'");
                return result;
            }

            // Else, try function call
            var position = CurrentPosition;
            var functionCallChainResult = FunctionCallChain();
            if (!functionCallChainResult.IsError)
                return functionCallChainResult;
            return result.Fail(CurrentPosition, "Expected statement");
        }

        private Result Variable()
        {
            var result = new Result();
            if (CurrentTokenType != TokenType.Identifier)
                return result.Fail(CurrentPosition, "Expected a variable identifier");
            Advance();
            return result;
        }

        private Result Binding()
        {
            var result = new Result();
            if (CurrentTokenType != TokenType.Identifier)
                return result.Fail(CurrentPosition, "Expected an identifier");
            Advance();
            if (CurrentTokenType != TokenType.Colon)
                return result;
            
            Advance();
            if (CurrentTokenType != TokenType.Identifier)
                return result.Fail(CurrentPosition, "Expected a type identifier");
            Advance();
            return result;
        }

        private Result BindingList()
        {
            var result = new Result();
            result.Register(Binding());
            if (result.IsError) return result;
            while (CurrentTokenType == TokenType.Comma)
            {
                Advance();
                result.Register(Binding());
                if (result.IsError) return result;
            }
            return result;
        }

        private Result Expression()
        {
            var result = new Result();
            if (unaryOps.Contains(CurrentTokenType))
            {
                result.Register(UnaryOp());
                if (result.IsError) return result;
                result.Register(Expression());
                if (result.IsError) return result;
            }
            else
            {
                result.Register(BaseExpression());
                if (result.IsError) return result;
            }

            while (binaryOps.Contains(CurrentTokenType))
            {
                result.Register(BinaryOp());
                if (result.IsError) return result;
                result.Register(Expression());
                if (result.IsError) return result;
            }

            return result;
        }

        private Result BaseExpression()
        {
            var result = new Result();
            switch (CurrentTokenType)
            {
                case TokenType.Bool:
                case TokenType.Int:
                case TokenType.Float:
                case TokenType.String:
                    Advance();
                    return result;

                case TokenType.Fn:
                    Advance();
                    result.Register(FunctionBody());
                    return result;
                
                case TokenType.If:
                    result.Register(IfElseExpression());
                    return result;
            }

            var index = Save();
            var functionCallChainResult = FunctionCallChain();
            if (!functionCallChainResult.IsError)
                return result.Register(functionCallChainResult);
            Load(index);

            return result.Register(PrefixExpression());
        }

        private Result PrefixExpression()
        {
            var result = new Result();
            if (CurrentTokenType == TokenType.LeftParenthesis)
            {
                var leftParenthesisPosition = CurrentToken.Position;
                Advance();
                result.Register(Expression());
                if (result.IsError) return result;

                if (CurrentTokenType != TokenType.RightParenthesis)
                    return result.Fail(CurrentPosition, "Expected ')' (to close '(' at " + leftParenthesisPosition + ")");
                Advance();
                return result;
            }

            return result.Register(Variable());
        }

        private Result IfElseExpression()
        {
            var result = new Result();
            if (CurrentTokenType != TokenType.If)
                return result.Fail(CurrentPosition, "Expected 'if'");
            Advance();
            result.Register(Expression());
            if (result.IsError) return result;
            result.Register(BlockExpression());
            if (result.IsError) return result;

            while (CurrentTokenType == TokenType.Else)
            {
                Advance();
                if (CurrentTokenType == TokenType.If)
                    result.Register(IfElseExpression());
                return result.Register(BlockExpression());
            }

            return result.Fail(CurrentPosition, "Expected 'else'");
        }

        private Result BlockExpression()
        {
            var result = new Result();
            var leftBracePosition = CurrentToken.Position;
            if (CurrentTokenType != TokenType.LeftBrace)
                return result.Fail(CurrentPosition, "Expected '{'");
            Advance();
            result.Register(result);
            if (result.IsError) return result;
            if (CurrentTokenType != TokenType.LeftBrace)
                return result.Fail(CurrentPosition, "Expected '}' (to close '{' at " + leftBracePosition + ")");
            Advance();
            return result;
        }


        private Result ExpressionList()
        {
            var result = new Result();
            result.Register(Expression());
            if (result.IsError) return result;
            while (CurrentTokenType == TokenType.Comma)
            {
                Advance();
                result.Register(Expression());
                if (result.IsError) return result;
            }
            return result;
        }
        
        private Result FunctionCallChain()
        {
            var result = new Result();
            result.Register(PrefixExpression());
            if (result.IsError) return result;

            result.Register(FunctionArgs());

            while (true)
            {
                var index = Save();
                var functionArgsResult = FunctionArgs();
                if (functionArgsResult.IsError)
                {
                    Load(index);
                    break;
                }
                result.Register(functionArgsResult);
            }

            return result;
        }

        private Result FunctionArgs()
        {
            var result = new Result();
            var leftParenthesisPosition = CurrentToken.Position;
            if (CurrentTokenType != TokenType.LeftParenthesis)
                return result.Fail(CurrentPosition, "Expected '('");
            Advance();
            
            var index = Save();
            var expressionListResult = ExpressionList();
            if (!expressionListResult.IsError)
                result.Register(expressionListResult);
            else
                Load(index);
            
            if (CurrentTokenType != TokenType.RightParenthesis)
                return result.Fail(CurrentPosition, "Expected ')' (to close '(' at " + leftParenthesisPosition + ")");
            Advance();
            return result;
        }

        private Result FunctionName()
        {
            var result = new Result();
            if (CurrentTokenType != TokenType.Identifier)
                return result.Fail(CurrentPosition, "Expected function name identifier");
            Advance();
            return result;
        }

        private Result FunctionBody()
        {
            var result = new Result();
            var leftParenthesisPosition = CurrentToken.Position;
            if (CurrentTokenType != TokenType.LeftParenthesis)
                return result.Fail(CurrentPosition, "Expected '('");
            Advance();
            
            if (CurrentTokenType == TokenType.Identifier)
            {
                result.Register(BindingList());
                if (result.IsError) return result;
            }

            if (CurrentTokenType != TokenType.RightParenthesis)
                return result.Fail(CurrentPosition, "Expected ')' (to close '(' at " + leftParenthesisPosition + ")");
            Advance();
            result.Register(Block());
            if (result.IsError) return result;
            return result;
        }


        private Result BinaryOp()
        {
            switch (CurrentTokenType)
            {
                case TokenType.Add:
                case TokenType.Sub:
                case TokenType.Mul:
                case TokenType.Div:
                case TokenType.Mod:
                case TokenType.Pow:
                case TokenType.BitwiseOr:
                case TokenType.BitwiseAnd:
                case TokenType.BitwiseXor:
                case TokenType.BitwiseLeftShift:
                case TokenType.BitwiseRightShift:
                
                case TokenType.Or:
                case TokenType.And:
                case TokenType.Equals:
                case TokenType.NotEquals:
                case TokenType.LessThan:
                case TokenType.LessThanOrEquals:
                case TokenType.GreaterThan:
                case TokenType.GreaterThanOrEquals:

                case TokenType.Range:
                    Advance();
                    return new Result();
            }

            return new Result(new SyntaxError(CurrentPosition, "Expected a binary operation"));
            // '+', '-', '*', '/', '%', '**', '|', '&', '^', ...
        }

        private Result AssignmentOp()
        {
            var result = new Result();
            if (CurrentTokenType == TokenType.Assignment)
            {
                Advance();
                return result;
            }

            result.Register(CompoundOp());
            return result;
        }

        private Result CompoundOp()
        {
            switch (CurrentTokenType)
            {
                case TokenType.AddAssignment:
                case TokenType.SubAssignment:
                case TokenType.MulAssignment:
                case TokenType.DivAssignment:
                case TokenType.ModAssignment:
                case TokenType.PowAssignment:

                case TokenType.BitwiseOrAssignment:
                case TokenType.BitwiseAndAssignment:
                case TokenType.BitwiseXorAssignment:
                case TokenType.BitwiseLeftShiftAssignment:
                case TokenType.BitwiseRightShiftAssignment:
                    Advance();
                    return new Result();
            }
            return new Result(new SyntaxError(CurrentPosition, "Expected a compound operation"));
        }

        private Result UnaryOp()
        {
            switch (CurrentTokenType)
            {
                case TokenType.Sub:
                case TokenType.Not:
                    return new Result();
            }
            return new Result(new SyntaxError(CurrentPosition, "Expected a unary operation"));
        }
    }
}