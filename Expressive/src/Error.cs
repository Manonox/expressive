using System.Net.Mime;

namespace Expressive
{
    abstract public class Error
    {
        public Position Position { get; set; } = new Position(new StringContent(""));
        abstract public override string ToString();

        public Error(Position position) {
            Position = position;
        }
    }

    public class UnexpectedSymbolError : Error
    {
        public UnexpectedSymbolError(Position position) : base(position) { }
        public override string ToString()
        {
            return "Unexpected symbol '" + Position.CurrentChar + "' at " + Position.ToString();
        }
    }

    public class SyntaxError : Error
    {
        public string Message { get; set; }
        public SyntaxError(Position position, string message) : base(position) { Message = message; }
        public override string ToString()
        {
            return Message + " at " + Position.ToString();
        }
    }

    public class UnexpectedEOFError : Error
    {
        public UnexpectedEOFError(Position position) : base(position) { }
        public override string ToString()
        {
            return "Unexpected end of file at " + Position.ToString();
        }
    }
}