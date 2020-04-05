namespace PromptCLI
{
    public interface ILine<T>
    {
        string Text { get; }
        T Status { get; set; }
    }
}