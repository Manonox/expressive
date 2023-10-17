namespace Expressive
{
    public interface IContent {
        public string Name { get; }
        public int Length { get; }
        public char GetChar(int index);
    }
}