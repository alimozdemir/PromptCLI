

using System;

namespace PromptCLI
{
    public interface IComponent
    {
        bool IsCompleted { get; set; }
        void Handle(ConsoleKeyInfo act);
        void Draw();
        void SetTopPosition(int top);
        int GetTopPosition();
        void Done();
    }
    public interface IComponent<out T> : IComponent
    {
        T Result { get; }
    }
}
