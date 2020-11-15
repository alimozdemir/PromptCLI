using System;

namespace PromptCLI
{
    public interface IConsoleBase
    {
        void SetPosition(int left, int top);
        void ClearCurrentPosition(int left, int top);
        void ClearLine(int top, int startLeft = 0);
        void GoBack();
        void WritePreservePosition(char val, int left, int top, ConsoleColor? textColor = null);
        void Write(char val);
        void Write(string val);
        void Write(char val, ConsoleColor textColor, ConsoleColor? backgroundColor = null);
        void Write(string val, ConsoleColor textColor, ConsoleColor? backgroundColor = null);

        void WriteLine(string val);
        void WriteLine(string val, ConsoleColor textColor, ConsoleColor? backgroundColor = null);
        void WriteLine();

        int CursorLeft { get;} 
    }

    public class ConsoleBase : IConsoleBase
    {
        public static IConsoleBase Default { get; } = new ConsoleBase();
        private const string _goBackConst = "\b \b";

        private ConsoleBase()
        {
        }

        public void ClearLine(int top, int startLeft = 0)
        {
            Console.SetCursorPosition(startLeft, top);
            for (int i = startLeft; i < Console.WindowWidth; i++)
            {
                Console.Write(' ');
            }
        }

        public void WritePreservePosition(char val, int left, int top, ConsoleColor? textColor = null)
        {
            SetPosition(left, top);
            Write(val, textColor.GetValueOrDefault());
            SetPosition(left, top);
        }

        public void ClearCurrentPosition(int left, int top)
        {
            WritePreservePosition(' ', left, top);
        }

        public void SetPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        public int CursorLeft => Console.CursorLeft;

        public void GoBack()
        {
            // Go back one character insert a space and go back again.
            Console.Write(_goBackConst);
        }
        private void Compose(Action f, ConsoleColor textColor, ConsoleColor? backgroundColor = null)
        {
            var (backupTextColor, backupBackgroudColor) = (Console.ForegroundColor, Console.BackgroundColor);

            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor ?? backupBackgroudColor;

            f();

            Console.ForegroundColor = backupTextColor;
            Console.BackgroundColor = backupBackgroudColor;
        }

        public void Write(char val, ConsoleColor textColor, ConsoleColor? backgroundColor = null)
        {
            Compose(() => Console.Write(val), textColor, backgroundColor);
        }

        public void Write(string val, ConsoleColor textColor, ConsoleColor? backgroundColor = null)
        {
            Compose(() => Console.Write(val), textColor, backgroundColor);
        }

        public void WriteLine(string val, ConsoleColor textColor, ConsoleColor? backgroundColor = null)
        {
            Compose(() => Console.WriteLine(val), textColor, backgroundColor);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void Write(char val)
        {
            Console.Write(val);
        }

        public void Write(string val)
        {
            Console.Write(val);
        }

        public void WriteLine(string val)
        {
            Console.WriteLine(val);
        }

    }
}