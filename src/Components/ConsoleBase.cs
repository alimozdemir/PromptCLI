using System;

namespace PromptCLI
{
    public interface IConsoleBase
    {
        void SetPosition(int left, int top);
        void ClearCurrentPosition(int left, int top);
        void ClearCurrentLine(int top, int startLeft = 0);
        void GoBack();
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

        public void ClearCurrentLine(int top, int startLeft = 0)
        {
            Console.SetCursorPosition(startLeft, top);
            for (int i = startLeft; i < Console.WindowWidth; i++)
            {
                Console.Write(' ');
            }
        }

        public void ClearCurrentPosition(int left, int top)
        {
            SetPosition(left, top);
            Console.Write(' ');
            SetPosition(left, top);
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

        public void Write(char val, ConsoleColor textColor, ConsoleColor? backgroundColor = null)
        {
            var (backupTextColor, backupBackgroudColor) = (Console.ForegroundColor, Console.BackgroundColor);

            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor ?? backupBackgroudColor;

            Console.Write(val);

            Console.ForegroundColor = backupTextColor;
            Console.BackgroundColor = backupBackgroudColor;
        }

        public void Write(string val, ConsoleColor textColor, ConsoleColor? backgroundColor = null)
        {
            var (backupTextColor, backupBackgroudColor) = (Console.ForegroundColor, Console.BackgroundColor);

            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor ?? backupBackgroudColor;

            Console.Write(val);

            Console.ForegroundColor = backupTextColor;
            Console.BackgroundColor = backupBackgroudColor;
        }

        public void WriteLine(string val, ConsoleColor textColor, ConsoleColor? backgroundColor = null)
        {
            var (backupTextColor, backupBackgroudColor) = (Console.ForegroundColor, Console.BackgroundColor);

            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor ?? backupBackgroudColor;

            Console.WriteLine(val);

            Console.ForegroundColor = backupTextColor;
            Console.BackgroundColor = backupBackgroudColor;
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