using System;
using PromptCLI;

namespace PromptCLITests
{
    public static class TextExtensions
    {
        public static void InvokeHandle(this IComponent component, ConsoleKey key)
        {
            ConsoleKeyInfo e = new ConsoleKeyInfo((char)key, key, false, false, false);
            component.Handle(e);
        }
    }

}