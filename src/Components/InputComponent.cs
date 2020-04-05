using System;
using System.Text.RegularExpressions;

namespace PromptCLI
{
    public class InputComponent : IComponent
    {
        private readonly Input _input;
        private readonly Range _range;
        private int _cursorPointLeft;
        private int _cursorPointTop;
        private string _regex;
        public Range AvailableRange => _range;

        public string Result => _input.Status;
        public bool IsCompleted { get; set; }

        public InputComponent(Input input)
        {
            _input = input;
            _range = input.Text.Length..;
            _regex = "^[a-zA-Z0-9. _\b]";
            _cursorPointTop = 0;
            _cursorPointLeft = _range.Start.Value;
        }

        public void Clear()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }

        public void Draw()
        {
            Console.WriteLine(_input.Text);
            SetPosition();
        }

        private void SetPosition()
        {
            Console.SetCursorPosition(_cursorPointLeft, _cursorPointTop);
        }

        public void Listener()
        {
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();

                Handle(key);
            }
            while (key.Key != ConsoleKey.Enter);
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
                SetPosition();
                return;
            }

            if (key == ConsoleKey.Backspace) 
            {
                // Go back one character insert a space and go back again.
                Console.Write("\b \b");
                // Remove from status
                _input.Status = _input.Status.Remove(_input.Status.Length - 1);
            } 
            else
            {
                // SetPosition();
                _input.Status += act.KeyChar;
                // SetPosition();
            }
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

        private bool topBound(int top) => top >= 0 && top < 1;
        private bool leftBound(int left) => left >= _range.Start.Value && left <= _range.End.Value;

        private KeyInfo isThisAvailable(char key)
        {
            return Regex.Match(key.ToString(), _regex, RegexOptions.IgnoreCase).Success ?
                KeyInfo.Others : KeyInfo.Unknown;
        }


        public void SetTopPosition(int top)
        {
            _cursorPointTop = top;
        }


        public int GetTopPosition()
        {
            return 1;
        }
    }

}