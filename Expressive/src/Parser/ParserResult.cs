namespace Expressive
{
	partial class Parser
	{
        public class Result
        {
			public bool IsError { get; set; }
			public Error? Error { get; set; }

			public Result()
			{
				IsError = false;
			}

			public Result(Error error)
			{
				Error = error;
				IsError = true;
			}

			public Result Register(Result other)
			{
				if (other.IsError) {
					IsError = true;
					Error = other.Error;
				}

				return this;
			}

			public Result Fail(Position position, string message)
			{
				IsError = true;
				Error = new SyntaxError(position, message);
				return this;
			}
		}
    }
}