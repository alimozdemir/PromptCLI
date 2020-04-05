namespace Prompt
{
    public interface ILine
    {
        string Text { get; }
        string Regex { get; }
    }
}