using System;
using System.Text.RegularExpressions;

namespace PromptCLI
{
    public abstract class ComponentBase : IComponent
    {
        private readonly IConsoleBase _console;

        protected IConsoleBase Console => _console;

        protected ComponentBase()
            : this(ConsoleBase.Default)
        {
        }

        protected ComponentBase(IConsoleBase console)
        {
            _console = console;
        }


        protected int _cursorPointLeft, _offsetLeft, _maxLeft;
        protected int _cursorPointTop, _offsetTop, _maxTop;
        protected string _regex;
        protected Range _range;
        protected IPrompt _prompt;

        protected PromptConfig _config 
        { 
            get 
            {
                // for test cases and individual uses
                if (__config == null)
                    __config = PromptConfig.Default;

                return __config;
            }
            set 
            {
                __config = value;
            }
        }

        private PromptConfig __config;

        private bool topBound(int top) => top > _offsetTop && top < _offsetTop + _maxTop;
        private bool leftBound(int left) => left >= _range.Start.Value && left <= _range.End.Value;
        private KeyInfo isThisAvailable(char key) => 
            Regex.Match(key.ToString(), _regex, RegexOptions.IgnoreCase).Success ? KeyInfo.Others : KeyInfo.Unknown;

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
        protected void SetPosition() => _console.SetPosition(_cursorPointLeft, _cursorPointTop);
        protected (KeyInfo, ConsoleKey) IsKeyAvailable(ConsoleKeyInfo act) =>
            act.Key switch
            {
                ConsoleKey.UpArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.DownArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.RightArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.LeftArrow => (KeyInfo.Direction, act.Key),
                ConsoleKey.Backspace => (KeyInfo.Others, act.Key),
                _ => (isThisAvailable(act.KeyChar), act.Key)
            };

        protected void ClearCurrentPosition() => _console.ClearCurrentPosition(_cursorPointLeft, _cursorPointTop);

        protected void GoBack() => _console.GoBack();

        protected void WriteCurrent(char val, ConsoleColor? textColor = null) => _console.WritePreservePosition(val, _cursorPointLeft, _cursorPointTop, textColor);

        internal void SetConfig(PromptConfig config)
        {
            _config = config;
        }

        #region Abstracts
        public abstract ComponentType ComponentType { get; }

        public abstract bool IsCompleted { get; set; }
        public int CursorLeft => _cursorPointLeft;
        public int CursorTop => _cursorPointTop;

        public abstract void Complete();

        public abstract void Draw(bool defaultValue = true);

        public abstract int GetTopPosition();

        public abstract void Handle(ConsoleKeyInfo act);

        public abstract void SetTopPosition(int top);

        #endregion

    }

    public abstract class ComponentBase<T> : ComponentBase, IComponent<T>
    {
        protected ComponentBase(): base(ConsoleBase.Default)
        {
        }
        
        protected ComponentBase(IConsoleBase console): base(console)
        {
        }


        public abstract Input<T> Result { get; }

        public abstract Action<T> CallbackAction { get; }

        public abstract void Bind(IPrompt prompt);

        public abstract IPrompt Callback(Action<T> callback);
    }
}