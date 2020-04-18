using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PromptCLI
{
    public class CheckboxComponent<T> : ComponentBase, IComponent<IEnumerable<T>>
    {
        private readonly Input<IEnumerable<T>> _input;
        private readonly List<Option<T>> _selects;
        private readonly bool[] _status;
        public Range Range => _range;

        public IEnumerable<T> Result => _status.Where(i => i).Select((i, index) => _selects[index].Value);
        public bool IsCompleted { get; set; }

        public CheckboxComponent(Input<IEnumerable<T>> input, List<Option<T>> selects)
        {
            _input = input;
            _selects = selects;
            _range = 1..2;
            _regex = "^[ ]";
            _status = new bool[_selects.Count];
        }


        public void Draw(bool defaultValue = true)
        {
            ConsoleHelper.Write(prefix, ConsoleColor.Green);
            Console.Write(_input.Text);
            Console.WriteLine();

            foreach(var item in _selects)
            {
                Console.WriteLine("( ) {0}", item.Text);
            }

            SetPosition();
        }

        public void Handle(ConsoleKeyInfo act)
        {
            // Special for each component
            var (result, key) = isKeyAvailable(act);
            if (result == KeyInfo.Unknown)
            {
                SetPosition();
                Console.Write(' ');
                SetPosition();
                return;
            }
            else if (result == KeyInfo.Direction)
            {
                Direction(key);
                return;
            }

            var index = _cursorPointTop - _offsetTop - 1;

            SetPosition();
            _status[index] = !_status[index];
            ConsoleHelper.Write(_status[index] ? '•' : ' ', ConsoleColor.DarkRed);
            SetPosition();
        }

        public void SetTopPosition(int top)
        {
            _offsetTop = top;
            _cursorPointTop = top + 1; // offset 1 for input at the begining
            _cursorPointLeft = _range.Start.Value;
            _maxTop = _offsetTop + _selects.Count + 1;
        }

        public int GetTopPosition()
        {
            return 1;
        }

        public void Complete()
        {
            for (int i = 0; i < _selects.Count + 1; i++)
            {
                ConsoleHelper.ClearLine(_offsetTop + i);
            }
            
            _cursorPointLeft = 0;
            _cursorPointTop = _offsetTop;
            SetPosition();

            Console.Write(_input.Text);
            Console.Write(" > ");
            ConsoleHelper.Write(string.Join(",", Result), ConsoleColor.Cyan);
            Console.WriteLine();
        }
    }

}