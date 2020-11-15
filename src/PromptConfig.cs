using System;

namespace PromptCLI
{
    public class PromptConfig
    {
        public static PromptConfig Default { get; } = new PromptConfig();
        public ConsoleColor QuestionColor { get; } = ConsoleColor.Gray;
        public ConsoleColor AnswerColor { get; } = ConsoleColor.Cyan;
        public ConsoleColor CursorColor { get; } = ConsoleColor.Red;
        public string Cursor { get; } = "> ";
    }

}