namespace PromptCLI
{
    public class Option : ILine
    {
        private readonly string _regex;
        private readonly string _text;

        public Option(string optionText)
        {
            _text = string.Format("( ) {0} ", optionText);
            _regex = "^[ ]";
        }

        public Option((string optionText, string val) vals)
        {
        }

        public string Text => _text;

        public string Regex => _regex;
        public bool Status { get; set; }

        public static implicit operator Option(string val)
        {
            return new Option(val);
        }
    }

}