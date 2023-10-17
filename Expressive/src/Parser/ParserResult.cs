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
		}
    }
}