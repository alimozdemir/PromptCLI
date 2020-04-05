

using System;

namespace Prompt
{
    public interface IComponent
    {
        bool IsCompleted { get; set; }
        string Result { get; }
        void Handle(ConsoleKeyInfo act);
        void Draw();
        void SetTopPosition(int top);
        int GetTopPosition();
    }
}
