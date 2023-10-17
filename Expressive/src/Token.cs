using System.Runtime.InteropServices;

namespace Expressive
{
	class Token {
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
		// Common
		Identifier,
		Dot, // .
		Separator, // ; \n
		Assignment, // =
		LeftParenthesis, RightParenthesis, // ( )
		LeftBrace, RightBrace, // { }
		LeftBracket, RightBracket, // [ ]
		Colon, RightArrow, // : ->


		// Literals
		Bool, Int, Float, String,


		// Keywords
		Let, Global, Null,
		Is, In,
		Fn, Return,
		If, Else,
		For, While,
		Break, Continue,


		// Operators
		Not, Or, And, // != || &&
		

		// Math
		
		Add, Sub, // + -
		Mul, Div, // * /
		Pow, // **
		Increment, Decrement, // ++ --

		AddAssignment, SubAssignment, // += -=
		MulAssignment, DivAssignment, // *= /=
		PowAssignment, // **=


		// Bit

		BitwiseOr, BitwiseAnd, BitwiseXor, // | & ^
		BitwiseLeftShift, BitwiseRightShift, // << >>

		BitwiseOrAssignment, BitwiseAndAssignment, BitwiseXorAssignment, // |= &= ^=
		BitwiseLeftShiftAssignment, BitwiseRightShiftAssignment, // <<= >>=


		// Comparison

		Equals, NotEquals, // == !=
		LessThan, LessThanOrEquals, // < <=
		GreaterThan, GreaterThanOrEquals, // > >=


		// Other
		
		Range, // ..
	}
}