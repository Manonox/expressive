using System.Runtime.InteropServices;

namespace Expressive
{
	public class Token {
		public TokenType Type { get; set; }
		public string Value { get; set; }
		public Position Position { get; set; }


		public Token(TokenType type, Position position, string value = "")
		{
			Type = type; Position = position; Value = value;
		}

        public override string ToString()
        {
            var s = base.ToString() + '<' + Type + '>';
			if (Value.Length > 0)
				s += '[' + Value + ']';
			return s;
        }
	}

	public enum TokenType {
		EOF,

		// Common
		Identifier, // alpha_numeric_123
		Dot, Comma, // . ,
		Separator, // ;
		Assignment, // =
		LeftParenthesis, RightParenthesis, // ( )
		LeftBrace, RightBrace, // { }
		LeftBracket, RightBracket, // [ ]
		Colon, RightArrow, // : ->

		// Literals
		Bool, Number, String,

		// Keywords
		Let, Global,
		Is, In, As,
		Fn, Return,
		If, Else,
		For, While, Loop,
		Break, Continue,


		// Operators
		Not, Or, And, // ! || &&
		
		Add, Sub, // + -
		Mul, Div, // * /
		Mod, // %
		Pow, // **

		OrAssignment, AndAssignment, // ||=, &&=

		AddAssignment, SubAssignment, // += -=
		MulAssignment, DivAssignment, // *= /=
		ModAssignment, // %=
		PowAssignment, // **=

		BitwiseNot, BitwiseOr, BitwiseAnd, BitwiseXor, // ~ | & ^
		BitwiseLeftShift, BitwiseRightShift, // << >>

		BitwiseOrAssignment, BitwiseAndAssignment, BitwiseXorAssignment, // |= &= ^=
		BitwiseLeftShiftAssignment, BitwiseRightShiftAssignment, // <<= >>=
		// BitwiseRightShiftAssignmentSomething, // >>>=


		// Comparison

		Equals, NotEquals, // == !=
		LessThan, LessThanOrEquals, // < <=
		GreaterThan, GreaterThanOrEquals, // > >=


		// Other
		
		Range, // ..
		InclusiveRange, // ..=
	}
}