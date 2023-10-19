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
            return Position.ToString() + ": " + "Unexpected symbol '" + Position.CurrentChar + "'";
        }
    }

    public class SyntaxError : Error
    {
        public string Message { get; set; }
        public SyntaxError(Position position, string message) : base(position) { Message = message; }
        public override string ToString()
        {
            return Position.ToString() + ": " + Message;
        }
    }

    public class NotImplementedError : Error
    {
        public NotImplementedError(Position position) : base(position) { }
        public override string ToString()
        {
            return Position.ToString() + ": " + "Not implemented";
        }
    }

    public class UnexpectedEOFError : Error
    {
        public UnexpectedEOFError(Position position) : base(position) { }
        public override string ToString()
        {
            return Position.ToString() + ": " + "Unexpected end of file";
        }
    }
}