using System;

namespace PromptCLI
{
    public class InputComponent : ComponentBase<string>
    {
        private readonly string _defaultValue;
        private readonly Input<string> _input;
        private Action<string> _callback;

        public override ComponentType ComponentType => ComponentType.Input;
        public override Action<string> CallbackAction => _callback;
        public Range Range => _range;
        public override Input<string> Result => _input;
        public override bool IsCompleted { get; set; }

        public InputComponent(Input<string> input, IConsoleBase console,  string defaultValue = default)
            : base(console)
        {
            _defaultValue = defaultValue;
            _input = input;
            _regex = "^[a-zA-Z0-9. _\b]";
        }

        public InputComponent(Input<string> input, string defaultValue = default)
            :this(input, ConsoleBase.Default, defaultValue)
        {
        }

        public override void Draw(bool defaultValue = true)
        {
            int startPoint = _config.Cursor.Length + _input.Text.Length + 1;
            Console.Write(_config.Cursor, _config.CursorColor);
            Console.Write(_input.Text, _config.QuestionColor);

            if (defaultValue && !string.IsNullOrEmpty(_defaultValue))
            {
                var format = string.Format(" ({0})", _defaultValue);
                Console.Write(format, ConsoleColor.DarkGray);
            }

            _range = startPoint..;
            ResetCursorToBegining();
        }

        private void ResetCursorToBegining()
        {
            _cursorPointLeft = _range.Start.Value;
            SetPosition();
        }

        private void Reset()
        {
            Console.ClearLine(_cursorPointTop, _cursorPointLeft);
            SetPosition();
        }

        public override void Handle(ConsoleKeyInfo act)
        {
            // Special for each component
            var (result, key) = IsKeyAvailable(act);
            if (result == KeyInfo.Unknown)
            {
                SetPosition();
                return;
            }
            else if (result == KeyInfo.Direction)
            {
                SetPosition();
                return;
            }

            if (string.IsNullOrEmpty(_input.Status) && _defaultValue != default)
            {
                _cursorPointLeft = Console.CursorLeft;
                Reset();
            }

            if (key == ConsoleKey.Backspace)
            {
                // Go back one character insert a space and go back again.
                GoBack();
                // Remove from status
                _input.Status = _input.Status.Remove(_input.Status.Length - 1);
            }
            else
            {
                _input.Status += act.KeyChar;
            }
        }


        public override void SetTopPosition(int top)
        {
            _cursorPointTop = top;
            _maxTop = top + 1;
        }

        public override void Complete()
        {
            // if no input detected, then set the result into the input.status
            if (string.IsNullOrEmpty(_input.Status) && !string.IsNullOrEmpty(_defaultValue))
            {
                _input.Status = _defaultValue;
            }
            // Clear the current line
            _cursorPointLeft = 0;
            Console.ClearLine(_cursorPointTop);
            SetPosition();
            
            // Write the result 
            Console.Write(_input.Text, _config.QuestionColor);
            Console.Write(" > ");
            Console.WriteLine(_input.Status, _config.AnswerColor);

            CallbackAction?.Invoke(this.Result.Status);
        }

        public override int GetTopPosition() => 1;

        public override void Bind(IPrompt prompt) => _prompt = prompt;

        public override IPrompt Callback(Action<string> callback)
        {
            _callback = callback;
            return _prompt;
        }
    }

}