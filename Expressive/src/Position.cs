using System.Reflection.Metadata.Ecma335;

namespace Expressive
{
	public class Position
	{
		public IContent Content { get; private set; }

		public int Index { get; private set; } = 0;
		public int Line { get; private set; } = 0;
		public int Column { get; private set; } = 0;
        public char CurrentChar { get; private set; }
        public bool IsEOF { get; private set; } = true;

        public Position(IContent content)
        {
            Content = content;
            Index = -1;
            Line = 1;
            Column = 0;
            if (Content.Length != 0)
                Advance();
        }

		public Position(Position other)
        {
            Content = other.Content;
            Index = other.Index; Line = other.Line; Column = other.Column;
            IsEOF = other.IsEOF; CurrentChar = other.CurrentChar;
        }

		public override string ToString()
		{
			return Content.ToString() + ":" + Line + ":" + Column;
		}

        static private bool IsSkippedChar(char c) {
            if (!char.IsControl(c)) return false;
            if (c == '\n') return false;
            if (c == '\t') return false;
            return true;
        }

		public void Advance()
		{
            AdvanceIndex();
            while (!IsEOF && IsSkippedChar(CurrentChar)) {
                AdvanceIndex();
            }
		}

        private void AdvanceIndex() {
            Index++;
            IncrementLineColumn();
            IsEOF = Index >= Content.Length;
            CurrentChar = Content.GetChar(Index);
        }


        private void IncrementLineColumn() {
            if (IsEOF) return;
            if (IsSkippedChar(CurrentChar)) return;

            if (CurrentChar == '\n') {
                Column = 1;
                Line++;
                return;
            }

            Column++;
        }
	}
}
