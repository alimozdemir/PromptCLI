namespace PromptCLI
{
    public class Input : ILine
    {
        private readonly string _regex;
        private readonly string _text;

        public Input(string optionText)
        {
            _text = string.Format("> {0} ", optionText);
            _regex = "^[a-z]";
        }

        public string Text => _text;

        public string Regex => _regex;
        public string Status { get; set; }

        public static implicit operator Input(string val)
        {
            return new Input(val);
        }
    }

}