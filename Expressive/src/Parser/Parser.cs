// using System.ComponentModel;
// using System.Data;
// using System.Diagnostics.CodeAnalysis;
// using System.Dynamic;
// using System.Linq.Expressions;
// using System.Reflection.Metadata.Ecma335;
// using System.Security.Cryptography.X509Certificates;
// using System.Text;
// using Expressive.AST;


// namespace Expressive
// {
// 	partial class Parser
// 	{
//         private AST.Node Node { get; set; } = new AST.StatementListNode();
//         private int Index { get; set; }
//         private List<Token> Tokens { get; set; }
//         private Token CurrentToken { get; set; }

//         private TokenType CurrentTokenType { get {
//             return CurrentToken.Type;
//         } }

//         private Position CurrentPosition { get {
//             return CurrentToken.Position;
//         } }
        
//         public Parser(List<Token> tokens)
//         {
//             CurrentToken = tokens[0];
//             Tokens = tokens;
//         }

//         private void Advance() 
//         {
//             Index += 1;
//             CurrentToken = Tokens[Index];
//         }

//         private bool IsEOF()
//         {
//             return CurrentTokenType == TokenType.EOF;
//         }

//         public Result Parse()
//         {
//             Index = -1; Advance();
            
//             try
//             {
//                 Chunk();
//                 return new Result(Node);
//             }
//             catch (ParserException exception)
//             {
//                 return new Result(new Position(CurrentPosition), exception.Message);
//             }
//         }

//         public void Chunk()
//         {
//             var node = new StatementListNode();
//             StatementList(node);
//             Node = node;
//         }

//         public void StatementList(StatementListNode node)
//         {
//             switch (CurrentTokenType)
//             {
//                 case TokenType.RightBrace:
//                 case TokenType.EOF:
//                     return;
                
//                 case TokenType.Let:
//                 case TokenType.If:
//                 case TokenType.For:
//                 case TokenType.While:
//                 case TokenType.Loop:
//                 case TokenType.Identifier:
//                 case TokenType.Fn:
//                 case TokenType.LeftBrace:
//                     Statement(node);
//                     StatementList(node);
//                     return;
//             }

//             throw new ParserException("doodoo");
//         }

//         public void Statement(StatementListNode node)
//         {
//             switch (CurrentTokenType)
//             {
//                 case TokenType.Let:
//                     LetStatement(node);
//                     return;
                

//             }
//         }


//         public void LetStatement(StatementListNode node)
//         {
//             var let_node = new LetNode
//             {
//                 IsGlobal = LocalGlobalBinding(),
//                 Binding = Binding(),
//                 Expression = Expression()
//             };
            
//             node.Children.Add(let_node);
//         }

//         public void LocalGlobalBinding()
//         {
//             switch (CurrentTokenType)
//             {
//                 case TokenType.Identifier:
//                     Binding()
//             }
//         }

//         public BindingNode Binding()
//         {
//             if (CurrentTokenType != TokenType.Identifier)
//                 throw
//         }

//         public ExpressionNode Expression()
//         {

//         }


//         [Serializable]
//         public class ParserException : Exception
//         {
//             public ParserException() { }
//             public ParserException(string message) : base(message) { }
//         }
//     }
// }