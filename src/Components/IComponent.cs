

using System;

namespace PromptCLI
{
    public interface IComponent
    {
        bool IsCompleted { get; set; }
        void Handle(ConsoleKeyInfo act);
        void Draw(bool defaultValue = true);
        void SetTopPosition(int top);
        int GetTopPosition();
        void Complete();
    }

    public interface IComponent<out T> : IComponent
    {
        T Result { get; }
    }
}
