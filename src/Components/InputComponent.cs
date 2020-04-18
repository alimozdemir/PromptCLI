using System;
using System.Text.RegularExpressions;

namespace PromptCLI
{
    public class InputComponent : ComponentBase, IComponent<string>
    {
        private readonly Input<string> _input;
        public Range Range => _range;

        public string Result => _input.Status;
        public bool IsCompleted { get; set; }

        public InputComponent(Input<string> input)
        {
            _input = input;
            _range = input.Text.Length..;
            _regex = "^[a-zA-Z0-9. _\b]";
            _cursorPointTop = 0;
            _cursorPointLeft = _range.Start.Value;
        }

        public void Draw()
        {
            Console.WriteLine(_input.Text);
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


        public void SetTopPosition(int top)
        {
            _cursorPointTop = top;
            _maxTop = top + 1;
        }


        public int GetTopPosition()
        {
            return 1;
        }

        public void Complete()
        {
        }
    }

}