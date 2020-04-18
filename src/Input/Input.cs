namespace PromptCLI
{
    public class Input<T> : ILine<T>
    {
        private readonly string _text;

        public Input(string optionText)
        {
            _text = optionText;
        }

        public string Text => _text;
        public T Status { get; set; }

        public static implicit operator Input<T>(string val)
        {
            return new Input<T>(val);
        }
    }

}