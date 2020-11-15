using System;
using System.Collections.Generic;

namespace PromptCLI
{
    public class SelectComponent<T> : ComponentBase<T>
    {
        private readonly Input<T> _input;
        private readonly IList<T> _selects;
        public Range Range => _range;

        public override ComponentType ComponentType => ComponentType.Select;
        public override Action<T> CallbackAction => _callback;
        public override Input<T> Result => _input; // _selects[_selectedIndex].Value;
        public override bool IsCompleted { get; set; }
        private int _selectedIndex = -1;
        private Action<T> _callback;

        public SelectComponent(Input<T> input, IList<T> selects, IConsoleBase console)
            : base(console)
        {
            _input = input;
            _selects = selects;
            _range = 1..2;
            _regex = "^[ ]";
        }

        public SelectComponent(Input<T> input, IList<T> selects)
            : this(input, selects, ConsoleBase.Default)
        {
        }

        public override void Draw(bool defaultValue = true)
        {
            Console.Write(_config.Cursor,  _config.CursorColor);
            Console.WriteLine(_input.Text, _config.QuestionColor);

            foreach (var item in _selects)
            {
                Console.WriteLine(string.Format("( ) {0}", item));
            }

            // default toggled value's index
            Toggle(1);
        }

        private void ChangeSelected(int index)
        {
            _selectedIndex = index;
            _input.Status = _selects[_selectedIndex];
        }

        public override void Handle(ConsoleKeyInfo act)
        {
            var (result, key) = IsKeyAvailable(act);
            if (result == KeyInfo.Unknown)
            {
                ClearCurrentPosition();
                return;
            }
            else if (result == KeyInfo.Direction)
            {
                Direction(key);
                return;
            }

            var index = _cursorPointTop - _offsetTop - 1;

            ClearOldPosition();
            Toggle(index);
        }

        private void Toggle(int index)
        {
            ChangeSelected(index);
            
            int tempTop = _cursorPointTop;
            _cursorPointTop = _offsetTop + 1 + _selectedIndex;

            SetPosition();            
            Console.Write('•', ConsoleColor.DarkRed);
            _cursorPointTop = tempTop;
            SetPosition();
        }

        private void ClearOldPosition()
        {
            int tempTop = _cursorPointTop;
            _cursorPointTop = _offsetTop + 1 + _selectedIndex;

            SetPosition();

            Console.Write(' ');
            
            _cursorPointTop = tempTop;
            SetPosition();
        }

        public override void SetTopPosition(int top)
        {
            _offsetTop = top;
            _cursorPointTop = top + 1; // offset 1 for input at the begining
            _cursorPointLeft = _range.Start.Value;
            _maxTop = _selects.Count + 1;
        }

        public override void Complete()
        {
            // Clear all drawed lines and set the cursor into component start position
            for (int i = 0; i < _selects.Count + 1; i++)
            {
                Console.ClearLine(_offsetTop + i);
            }
            
            _cursorPointLeft = 0;
            _cursorPointTop = _offsetTop;
            SetPosition();

            // Write the result
            Console.Write(_input.Text, _config.QuestionColor);
            Console.Write(" > ");
            Console.WriteLine(Result.Status.ToString(), _config.AnswerColor);

            CallbackAction?.Invoke(this.Result.Status);
        }

        public override int GetTopPosition() => 1;

        public override void Bind(IPrompt prompt) => _prompt = prompt;

        public override IPrompt Callback(Action<T> callback)
        {
            _callback = callback;
            return _prompt;
        }
    }
}