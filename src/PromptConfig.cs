using System;

namespace PromptCLI
{
    public class PromptConfig
    {
        public static PromptConfig Default { get; } = new PromptConfig();
        public ConsoleColor QuestionColor { get; set; } = ConsoleColor.Magenta;
        public ConsoleColor AnswerColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor CursorColor { get; set; } = ConsoleColor.Red;
    }

}