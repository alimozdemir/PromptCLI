using System;
using System.Collections.Generic;
using System.Linq;

namespace PromptCLI
{
    public class CheckboxComponent<T> : ComponentBase<IEnumerable<T>>
    {
        private readonly Input<IEnumerable<T>> _input;
        private readonly List<T> _selects;
        private readonly bool[] _status;
        private Action<IEnumerable<T>> _callback;

        public override ComponentType ComponentType => ComponentType.Checkbox;
        public override Action<IEnumerable<T>> CallbackAction => _callback;

        public Range Range => _range;

        public override Input<IEnumerable<T>> Result => _input;
        public override bool IsCompleted { get; set; }


        public CheckboxComponent(Input<IEnumerable<T>> input, List<T> selects, IConsoleBase console) 
            : base(console)
        {
            _input = input;
            _selects = selects;
            _range = 1..2;
            _regex = "^[ ]";
            _status = new bool[_selects.Count];
        }

        public CheckboxComponent(Input<IEnumerable<T>> input, List<T> selects)
            :this(input, selects, ConsoleBase.Default)
        {
        }


        public override void Draw(bool defaultValue = true)
        {
            Console.Write(_config.Cursor, _config.CursorColor);
            Console.WriteLine(_input.Text, _config.QuestionColor);

            foreach(var item in _selects)
            {
                Console.WriteLine(string.Format("[ ] {0}", item));
            }

            SetPosition();
        }

        public override void Handle(ConsoleKeyInfo act)
        {
            var index = _cursorPointTop - _offsetTop - 1;
            var (result, key) = IsKeyAvailable(act);
            if (result == KeyInfo.Unknown)
            {
                ClearCurrentPosition();
                // if it is checked before the the unknown char. Re-check it.
                Check();
                return;
            }
            else if (result == KeyInfo.Direction)
            {
                Direction(key);
                return;
            }

            void Check() => WriteCurrent(_status[index] ? '•' : ' ', ConsoleColor.DarkRed);

            _status[index] = !_status[index];
            Check();
        }

        public override void SetTopPosition(int top)
        {
            _offsetTop = top;
            _cursorPointTop = top + 1; // offset 1 for input at the begining
            _cursorPointLeft = _range.Start.Value;
            _maxTop = _selects.Count + 1;
        }

        public override int GetTopPosition() => 1;

        public override void Complete()
        {
            _input.Status = _status.Select((i, index) => (status:i, index)).Where(i => i.status).Select(i => _selects[i.index]);
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
            Console.WriteLine(string.Join(",", Result.Status), _config.AnswerColor);

            CallbackAction?.Invoke(this.Result.Status);
        }

        public override void Bind(IPrompt prompt) => _prompt = prompt;

        public override IPrompt Callback(Action<IEnumerable<T>> callback)
        {
            _callback = callback;
            return _prompt;
        }
    }

}