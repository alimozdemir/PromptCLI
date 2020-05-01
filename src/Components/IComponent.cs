

using System;

namespace PromptCLI
{
    public interface IComponent
    {
        bool IsCompleted { get; set; }
        int CursorTop { get; }
        int CursorLeft { get; }
        void Handle(ConsoleKeyInfo act);
        void Draw(bool defaultValue = true);
        void SetTopPosition(int top);
        int GetTopPosition();
        void Complete();
    }

    public interface IComponentPrompt<T>
    {
        Action<Input<T>> CallbackAction { get;}
        void Bind(IPrompt prompt);
        IPrompt Callback(Action<Input<T>> callback);
    }

    public interface IComponent<T> : IComponent, IComponentPrompt<T>
    {
        Input<T> Result { get; }
    }
}
