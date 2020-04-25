

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

    public interface IComponentPrompt<T>
    {
        Action<Input<T>> CallbackAction { get;}
        void Bind(Prompt prompt);
        Prompt Callback(Action<Input<T>> callback);
    }

    public interface IComponent<T> : IComponent, IComponentPrompt<T>
    {
        Input<T> Result { get; }
    }
}
