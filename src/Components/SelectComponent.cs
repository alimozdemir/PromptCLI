using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PromptCLI
{
    public class SelectComponent<T> : ComponentBase, IComponent<T>
    {
        private readonly Input<T> _input;
        private readonly List<Option<T>> _selects;
        public Range Range => _range;

        public T Result => _selects[_cursorPointTop - _offsetTop - 1].Value;
        public bool IsCompleted { get; set; }

        public SelectComponent(Input<T> input, List<Option<T>> selects)
        {
            _input = input;
            _selects = selects;
            _range = 1..2;
            _regex = "^[a-zA-Z0-9. _\b]";
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
            ConsoleHelper.Write(Result.ToString(), ConsoleColor.Cyan);
            Console.WriteLine();
        }
        public void Draw(bool defaultValue = true)
        {
            ConsoleHelper.Write(prefix, ConsoleColor.Green);
            Console.Write(_input.Text);
            Console.WriteLine();

            foreach (var item in _selects)
            {
                Console.WriteLine(item.Text);
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
                return;
            }
            else if (result == KeyInfo.Direction)
            {
                Direction(key);
                return;
            }

            var index = _cursorPointTop - _offsetTop - 1;

            SetPosition();
            Console.ForegroundColor = ConsoleColor.Green;
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


    }

}