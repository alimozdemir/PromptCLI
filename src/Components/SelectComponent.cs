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

        public Action<Input<T>> CallbackAction { get; private set; }
        public Input<T> Result => _input; // _selects[_selectedIndex].Value;
        public bool IsCompleted { get; set; }
        private int _selectedIndex = -1;

        public SelectComponent(Input<T> input, List<Option<T>> selects)
        {
            _input = input;
            _selects = selects;
            _range = 1..2;
            _regex = "^[ ]";
        }

        public void Draw(bool defaultValue = true)
        {
            Console.Write(prefix, ConsoleColor.Green);
            Console.WriteLine(_input.Text);

            foreach (var item in _selects)
            {
                Console.WriteLine(string.Format("( ) {0}", item.Text));
            }

            _selectedIndex = 0;
            toggle(0);
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
            toggle();
            _selectedIndex = index;
            toggle(index);
        }
        private void toggle(int index = -1)
        {
            int tempTop = _cursorPointTop;
            _cursorPointTop = _offsetTop + 1 + _selectedIndex;

            SetPosition();

            if (index > -1) 
                Console.Write('•', ConsoleColor.DarkRed);
            else
                Console.Write(' ');
            
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

        public void Complete()
        {
            _input.Status = _selects[_selectedIndex].Value;

            // Clear all drawed lines and set the cursor into component start position
            for (int i = 0; i < _selects.Count + 1; i++)
            {
                Console.ClearLine(_offsetTop + i);
            }
            
            _cursorPointLeft = 0;
            _cursorPointTop = _offsetTop;
            SetPosition();

            // Write the result
            Console.Write(_input.Text);
            Console.Write(" > ");
            Console.WriteLine(Result.ToString(), ConsoleColor.Cyan);

            CallbackAction(this.Result);
        }
        
        public void Bind(Prompt prompt)
        {
            _prompt = prompt;
        }

        public Prompt Callback(Action<Input<T>> callback)
        {
            CallbackAction = callback;
            return _prompt;
        }
    }
}