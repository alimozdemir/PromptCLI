using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PromptCLI
{
    public class SelectComponent : IComponent
    {
        private readonly Input _input;
        private readonly List<Option> _selects;
        private readonly Range _range;
        private int _cursorPointLeft;
        private int _cursorPointTop, _offsetTop;
        private string _regex;
        public Range AvailableRange => _range;

        public string Result => string.Join(",", _selects.Where(i => i.Status).Select(i => i.Text));
        public bool IsCompleted { get; set; }

        public SelectComponent(Input input, List<Option> selects)
        {
            _input = input;
            _selects = selects;
            _range = 1..2;
            _regex = "^[a-zA-Z0-9. _\b]";
        }

        public void Clear()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }
        public void Done()
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
            ConsoleHelper.Write(Result, ConsoleColor.Cyan);
            Console.WriteLine();
        }
        public void Draw()
        {
            Console.WriteLine(_input.Text);

            foreach (var item in _selects)
            {
                Console.WriteLine(item.Text);
            }

            SetPosition();
        }

        private void SetPosition()
        {
            Console.SetCursorPosition(_cursorPointLeft, _cursorPointTop);
        }

        private (KeyInfo, ConsoleKey) isKeyAvailable(ConsoleKeyInfo act) =>
            act.Key switch
            {
                ConsoleKey.UpArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.DownArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.RightArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.LeftArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.Backspace => (KeyInfo.Others, act.Key),
                _ => (isThisAvailable(act.KeyChar), act.Key)
            };

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
            var currentLine = _selects[index];


            SetPosition();
            currentLine.Status = !currentLine.Status;
            Console.Write(currentLine.Status ? '•' : ' ');
            SetPosition();
        }

        private void Direction(ConsoleKey key)
        {
            var (left, top) = (_cursorPointLeft, _cursorPointTop);

            if (key == ConsoleKey.UpArrow)
                top -= 1;
            else if (key == ConsoleKey.DownArrow)
                top += 1;

            // bound check special
            var isOk = leftBound(left) && topBound(top);
            if (!isOk)
                return;

            _cursorPointLeft = left;
            _cursorPointTop = top;

            SetPosition();
        }

        private bool topBound(int top) => top > _offsetTop && top < _offsetTop + _selects.Count + 1;
        private bool leftBound(int left) => left >= _range.Start.Value && left <= _range.End.Value;

        private KeyInfo isThisAvailable(char key)
        {
            return Regex.Match(key.ToString(), _regex, RegexOptions.IgnoreCase).Success ?
                KeyInfo.Others : KeyInfo.Unknown;
        }

        public void SetTopPosition(int top)
        {
            _offsetTop = top;
            _cursorPointTop = top + 1; // offset 1 for input at the begining
            _cursorPointLeft = _range.Start.Value;
        }

        public int GetTopPosition()
        {
            return 1;
        }


    }

}