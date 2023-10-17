using System.Text;


namespace Expressive
{
	partial class Lexer
	{
		private IContent Content { get; set; }
		private Position CurrentPosition { get; set; }
		public List<Token> TokenList { get; set; }
		public Position TokenBeginPosition { get; set; }

		private bool IsEOF { get { return CurrentPosition.IsEOF; } }
		private char CurrentChar { get { return CurrentPosition.CurrentChar; } }
		private char NextChar { get
		{
			var position = new Position(CurrentPosition);
			position.Advance();
			return position.CurrentChar;
		} }

		private void Advance() => CurrentPosition.Advance();

		public Lexer(IContent content)
		{
			Content = content;
			CurrentPosition = new Position(Content);
			TokenBeginPosition = CurrentPosition;
			TokenList = new List<Token>();
		}


		private Token CreateToken(TokenType type, string? value = null, Position? position = null)
		{
			var token = new Token(type, position ?? new Position(TokenBeginPosition));
			if (value != null)
				token.Value = value;
			return token;
		}

		private Error? PushToken(TokenType type, string? value = null, Position? position = null)
		{
			TokenList.Add(CreateToken(type, value, position));
			return null;
		}

		private void ReduceTokens()
		{
			// Remove Leading Separators
			for (int i = 0; i < TokenList.Count; i++)
			{
				if (TokenList[i].Type != TokenType.Separator) break;
				TokenList.RemoveAt(i);
				i--;
			}

			// Remove Trailing Separators
			for (int i = TokenList.Count - 1; i >= 0; i--)
			{
				if (TokenList[i].Type != TokenType.Separator) break;
				TokenList.RemoveAt(i);
			}

			// Remove Duplicate Separators
			{
				var i = TokenList.Count - 1;
				var removing = false;
				while (i >= 0)
				{
					var isSeparator = TokenList[i].Type == TokenType.Separator;
					if (removing && isSeparator)
					{
						TokenList.RemoveAt(i);
					}
					i--;
					removing = isSeparator;
				}
			}
		}


		public Result Parse()
		{
			CurrentPosition = new Position(Content);
			TokenList = new List<Token>();
			
			while (!IsEOF)
			{
				while (!IsEOF && " \t".Contains(CurrentChar))
					CurrentPosition.Advance();
				
				if (IsEOF) break;

				var error = ParseToken();
				if (error != null) {
					return new Result(error);
				}
			}

			ReduceTokens();

			return new Result(TokenList);
		}


		private Error? ParseIntOrFloatLiteral()
		{
			var builder = new StringBuilder();

			bool isFloat = false;
			bool waitingForFractDigits = false;
			Position dotPosition = CurrentPosition;
			while (!IsEOF && char.IsDigit(CurrentChar))
			{
				if (waitingForFractDigits)
				{
					waitingForFractDigits = false;
					isFloat = true;
					builder.Append('.');
				}

				builder.Append(CurrentChar);
				Advance();

				if (IsEOF) break;

				if (char.IsLetter(CurrentChar))
					return new UnexpectedSymbolError(CurrentPosition);
				
				if (CurrentChar == '.')
				{
					if (NextChar == '.')
						return PushToken(isFloat ? TokenType.Float : TokenType.Int, builder.ToString());

					if (isFloat)
						return PushToken(TokenType.Float, builder.ToString());

					waitingForFractDigits = true;
					dotPosition = new Position(CurrentPosition);
					Advance();
					continue;
				}
			}

			if (!isFloat)
			{
				PushToken(TokenType.Int, builder.ToString());
				if (waitingForFractDigits)
					PushToken(TokenType.Dot, null, dotPosition);
				return null;
			}

			return PushToken(TokenType.Float, builder.ToString());
		}


		private Error? ParseKeywordIdentifierOrBoolean()
		{
			var builder = new StringBuilder();
			builder.Append(CurrentChar);
			Advance();

			while (!IsEOF && (char.IsLetterOrDigit(CurrentChar) || CurrentChar == '_'))
			{
				builder.Append(CurrentChar);
				Advance();
			}

			var s = builder.ToString();

			switch (s) {
				// Keywords
				case "let": return PushToken(TokenType.Let);
				case "global": return PushToken(TokenType.Global);
				case "null": return PushToken(TokenType.Null);

				case "is": return PushToken(TokenType.Is);
				case "in": return PushToken(TokenType.In);

				case "fn": return PushToken(TokenType.Fn);
				case "return": return PushToken(TokenType.Return);

				case "if": return PushToken(TokenType.If);
				case "else": return PushToken(TokenType.Else);

				case "for": return PushToken(TokenType.For);
				case "while": return PushToken(TokenType.While);
				case "break": return PushToken(TokenType.Break);
				case "continue": return PushToken(TokenType.Continue);

				// bool
				case "true":
				case "false":
					return PushToken(TokenType.Bool, s);
			}

			// Identifier
			return PushToken(TokenType.Identifier, s);
		}


		private Error? ParseStringLiteral()
		{
			var builder = new StringBuilder();
			var isSingleQuote = CurrentChar == '\'';
			Advance();
			
			while (true)
			{
				if (CurrentChar == '\\')
				{
					Advance();
					if (IsEOF) return new UnexpectedEOFError(CurrentPosition);
					switch (CurrentChar)
					{
						case '\\':
							builder.Append('\\');
							break;
						
						case 'n':
							builder.Append('\n');
							break;
						
						case 't':
							builder.Append('\t');
							break;

						default:
							builder.Append(CurrentChar);
							break;
					}
					Advance();
					continue;
				}

				if ((!isSingleQuote && CurrentChar == '\"') || (isSingleQuote && CurrentChar == '\''))
				{
					Advance();
					break;
				}
				
				builder.Append(CurrentChar);
				Advance();
				if (IsEOF) return new UnexpectedEOFError(CurrentPosition);
			}

			return PushToken(TokenType.String, builder.ToString());
		}


		private Error? ParseToken()
		{
			TokenBeginPosition = new Position(CurrentPosition);

			if (char.IsDigit(CurrentChar))
				return ParseIntOrFloatLiteral();
			
			if (char.IsLetter(CurrentChar) || CurrentChar == '_')
				return ParseKeywordIdentifierOrBoolean();

			if (CurrentChar == '\"' || CurrentChar == '\'')
				return ParseStringLiteral();


			switch (CurrentChar)
			{
				case '\n':
				case ';':
					Advance();
					return PushToken(TokenType.Separator);


				case '(':
					Advance();
					return PushToken(TokenType.LeftParenthesis);
				case ')':
					Advance();
					return PushToken(TokenType.RightParenthesis);
				case '{':
					Advance();
					return PushToken(TokenType.LeftBrace);
				case '}':
					Advance();
					return PushToken(TokenType.RightBrace);
				case '[':
					Advance();
					return PushToken(TokenType.LeftBracket);
				case ']':
					Advance();
					return PushToken(TokenType.RightBracket);

				case ':':
					Advance();
					return PushToken(TokenType.Colon);


				case '=':
					Advance();
					if (CurrentChar == '=')
					{
						Advance();
						return PushToken(TokenType.Equals);
					}
					return PushToken(TokenType.Assignment);

				case '!':
					Advance();
					if (CurrentChar == '=')
					{
						Advance();
						return PushToken(TokenType.NotEquals);
					}
					return PushToken(TokenType.Not);

				case '&':
					Advance();
					if (CurrentChar == '&')
					{
						Advance();
						return PushToken(TokenType.And);
					}
					if (CurrentChar == '=')
					{
						Advance();
						return PushToken(TokenType.BitwiseAndAssignment);
					}
					return PushToken(TokenType.BitwiseAnd);

				case '|':
					Advance();
					if (CurrentChar == '|')
					{
						Advance();
						return PushToken(TokenType.Or);
					}
					if (CurrentChar == '=')
					{
						Advance();
						return PushToken(TokenType.BitwiseOrAssignment);
					}
					return PushToken(TokenType.BitwiseOr);
				
				case '^':
					Advance();
					if (CurrentChar == '=')
					{
						Advance();
						return PushToken(TokenType.BitwiseXorAssignment);
					}
					return PushToken(TokenType.BitwiseXor);

				case '<':
					Advance();
					if (CurrentChar == '<')

					{
						Advance();
						if (CurrentChar == '=')
						{
							Advance();
							return PushToken(TokenType.BitwiseLeftShiftAssignment);
						}
						return PushToken(TokenType.BitwiseLeftShift);
					}

					if (CurrentChar == '=')
					{
						Advance();
						return PushToken(TokenType.LessThanOrEquals);
					}

					return PushToken(TokenType.LessThan);
				
				case '>':
					Advance();
					if (CurrentChar == '>')
					{
						Advance();
						if (CurrentChar == '=')
						{
							Advance();
							return PushToken(TokenType.BitwiseRightShiftAssignment);
						}
						return PushToken(TokenType.BitwiseRightShift);
					}
					
					if (CurrentChar == '=')
					{
						Advance();
						return PushToken(TokenType.GreaterThanOrEquals);
					}

					return PushToken(TokenType.GreaterThan);


				case '.':
					Advance();
					if (CurrentChar == '.')
					{
						Advance();
						return PushToken(TokenType.Range);
					}
					return PushToken(TokenType.Dot);


				case '+':
					Advance();
					if (CurrentChar == '=')
					{
						Advance(); 
						return PushToken(TokenType.AddAssignment);
					}

					if (CurrentChar == '+')
					{
						Advance();
						return PushToken(TokenType.Increment);
					}

					return PushToken(TokenType.Add);


				case '-':
					Advance();
					if (CurrentChar == '=')
					{
						Advance();
						return PushToken(TokenType.SubAssignment);
					}

					if (CurrentChar == '-')
					{
						Advance();
						return PushToken(TokenType.Decrement);
					}

					if (CurrentChar == '>')
					{
						Advance();
						return PushToken(TokenType.RightArrow);
					}

					return PushToken(TokenType.Sub);
				

				case '*':
					Advance();
					if (CurrentChar == '*')
					{
						Advance();
						if (CurrentChar == '=')
						{
							Advance();
							return PushToken(TokenType.PowAssignment);
						}
						return PushToken(TokenType.Pow);
					}

					if (CurrentChar == '=')
					{
						Advance();
						return PushToken(TokenType.MulAssignment);
					}

					return PushToken(TokenType.Mul);
				

				case '/':
					Advance();

					if (CurrentChar == '/') // Comment
					{
						Advance();
						while (CurrentChar != '\n')
							Advance();
						return null;
					}

					if (CurrentChar == '*') /* Comment */
					{
						int depth = 1;
						while (!IsEOF && depth > 0)
						{
							Advance();
							var nextChar = NextChar;
							if (CurrentChar == '/' && nextChar == '*')
							{
								Advance();
								Advance();
								depth++;
							}
							if (CurrentChar == '*' && nextChar == '/')
							{
								Advance();
								Advance();
								depth--;
							}
						}
						return null;
					}

					if (CurrentChar == '=')
					{
						return PushToken(TokenType.DivAssignment);
					}
					return PushToken(TokenType.Div);
			}

			return new UnexpectedSymbolError(CurrentPosition);
		}
	}
}
