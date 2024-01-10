// using Expressive.AST;

// namespace Expressive
// {
// 	partial class Parser
// 	{
//         public class Result
//         {
// 			public bool IsError { get; set; }
// 			public Node? Node { get; set; }
// 			public Error? Error { get; set; }



// 			public Result(Node node)
// 			{
// 				IsError = false;
// 				Node = node;
// 			}

// 			public Result(Position position, string message)
// 			{
// 				IsError = true;
// 				Error = new SyntaxError(position, message);
// 			}
// 		}
//     }
// }