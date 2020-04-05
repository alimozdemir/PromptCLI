namespace PromptCLI
{
    public class Option<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }

        public Option((string text, T val) vals)
        {
            Text = vals.text;
            Value = vals.val;
        }

        public static implicit operator Option<T>((string text, T val) val)
        {
            return new Option<T>(val);
        }
    }

}