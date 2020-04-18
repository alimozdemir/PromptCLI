using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PromptCLI
{
    public abstract class ComponentBase
    {

        protected int _cursorPointLeft, _offsetLeft, _maxLeft;
        protected int _cursorPointTop, _offsetTop, _maxTop;
        protected string _regex;
        protected Range _range;

        protected void Direction(ConsoleKey key)
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
        protected void SetPosition()
        {
            Console.SetCursorPosition(_cursorPointLeft, _cursorPointTop);
        }

        private bool topBound(int top) => top > _offsetTop && top < _offsetTop + _maxTop;
        private bool leftBound(int left) => left >= _range.Start.Value && left <= _range.End.Value;



        protected KeyInfo isThisAvailable(char key)
        {
            return Regex.Match(key.ToString(), _regex, RegexOptions.IgnoreCase).Success ?
                KeyInfo.Others : KeyInfo.Unknown;
        }
        protected (KeyInfo, ConsoleKey) isKeyAvailable(ConsoleKeyInfo act) =>
            act.Key switch
            {
                ConsoleKey.UpArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.DownArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.RightArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.LeftArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.Backspace => (KeyInfo.Others, act.Key),
                _ => (isThisAvailable(act.KeyChar), act.Key)
            };
    }
}