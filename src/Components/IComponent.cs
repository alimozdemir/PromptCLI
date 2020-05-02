

using System;

namespace PromptCLI
{
    public enum ComponentType
    {
        Input,
        Checkbox,
        Select
    }
    public interface IComponent
    {
        ComponentType ComponentType { get; }
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
        Action<T> CallbackAction { get;}
        void Bind(IPrompt prompt);
        IPrompt Callback(Action<T> callback);
    }

    public interface IComponent<T> : IComponent, IComponentPrompt<T>
    {
        Input<T> Result { get; }
    }
}
