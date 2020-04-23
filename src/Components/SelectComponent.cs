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

        public T Result => _selects[_selectedIndex].Value;
        public bool IsCompleted { get; set; }
        private int _selectedIndex = -1;

        public SelectComponent(Input<T> input, List<Option<T>> selects)
        {
            _input = input;
            _selects = selects;
            _range = 1..2;
            _regex = "^[ ]";
        }

        public void Complete()
        {
            for (int i = 0; i < _selects.Count + 1; i++)
            {
                Console.ClearLine(_offsetTop + i);
            }
            
            _cursorPointLeft = 0;
            _cursorPointTop = _offsetTop;
            SetPosition();

            Console.Write(_input.Text);
            Console.Write(" > ");
            Console.Write(Result.ToString(), ConsoleColor.Cyan);
            Console.WriteLine();
        }
        public void Draw(bool defaultValue = true)
        {
            Console.Write(prefix, ConsoleColor.Green);
            Console.Write(_input.Text);
            Console.WriteLine();

            foreach (var item in _selects)
            {
                Console.WriteLine(string.Format("( ) {0}", item.Text));
            }

            setNew(0);
        }

        public void Handle(ConsoleKeyInfo act)
        {
            // Special for each component
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

            SetPosition();
            clearOld();
            setNew(index);
        }

        private void clearOld()
        {
            int tempTop = _cursorPointTop;
            _cursorPointTop = _offsetTop + 1 + _selectedIndex;
            SetPosition();
            Console.Write(' ');
            _cursorPointTop = tempTop;
            SetPosition();
        }

        private void setNew(int index)
        {
            int tempTop = _cursorPointTop;
            _selectedIndex = index;
            _cursorPointTop = _offsetTop + 1 + _selectedIndex;
            SetPosition();
            Console.Write('•', ConsoleColor.DarkRed);
            _cursorPointTop = tempTop;
            SetPosition();
        }

        public void SetTopPosition(int top)
        {
            _offsetTop = top;
            _cursorPointTop = top + 1; // offset 1 for input at the begining
            _cursorPointLeft = _range.Start.Value;
            _maxTop = _selects.Count + 1;
        }

        public int GetTopPosition()
        {
            return 1;
        }


    }

}