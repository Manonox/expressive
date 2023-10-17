using System.Security.Cryptography.X509Certificates;

namespace Expressive
{
	public class StringContent: IContent
	{
		public string Name { get; } = "string";
		public int Length { get; }

		private string Data { get; }

        public StringContent(string s)
		{
			Data = s;
			Length = Data.Length;
        }

		public char GetChar(int index)
		{
			if (index >= Length) return '\0';
			return Data[index];
		}

		public override string ToString()
		{
            return "[\"" + Name + "\"]";
		}
	}
}