namespace Expressive
{
	partial class Lexer
	{
        public class Result
                {
			public bool IsError { get; set; }
			public List<Token> Tokens { get; set; } = new List<Token>();
			public Error? Error { get; set; }

			public Result(List<Token> tokens)
			{
				Tokens = tokens;
				IsError = false;
			}

			public Result(Error error)
			{
				Error = error;
				IsError = true;
			}
		}
    }
}