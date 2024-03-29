namespace Expressive
{
	public class FileContent: IContent
	{
		public string Name { get; }
		public int Length { get; }

		public string Data { get; }

        public FileContent(string path)
		{
			Name = Path.GetFileName(path);
			Data = File.ReadAllText(path);
			Data = Data.ReplaceLineEndings("\n");
			Length = Data.Length;
        }

		public char GetChar(int index)
		{
			return Data[index];
		}

		public override string ToString()
		{
            return "[\"" + Name + "\"]";
		}
	}
}