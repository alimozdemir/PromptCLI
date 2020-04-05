using System;

namespace Prompt
{
    internal static class ConsoleHelper
    {
        public static void ColoredAction(Action func, ConsoleColor textColor, Nullable<ConsoleColor> backgroundColor = null)
        {
            var (backupTextColor, backupBackgroudColor) = (Console.ForegroundColor, Console.BackgroundColor);

            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor ?? backupBackgroudColor;

            func();

            Console.ForegroundColor = backupTextColor;
            Console.BackgroundColor = backupBackgroudColor;
        }

        public static void Write(string val, ConsoleColor textColor, Nullable<ConsoleColor> backgroundColor = null)
        {
            var (backupTextColor, backupBackgroudColor) = (Console.ForegroundColor, Console.BackgroundColor);

            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor ?? backupBackgroudColor;

            Console.Write(val);

            Console.ForegroundColor = backupTextColor;
            Console.BackgroundColor = backupBackgroudColor;
        }

        public static void WriteLine(string val, ConsoleColor textColor, Nullable<ConsoleColor> backgroundColor = null)
        {
            var (backupTextColor, backupBackgroudColor) = (Console.ForegroundColor, Console.BackgroundColor);

            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor ?? backupBackgroudColor;

            Console.WriteLine(val);

            Console.ForegroundColor = backupTextColor;
            Console.BackgroundColor = backupBackgroudColor;
        }
    }

}