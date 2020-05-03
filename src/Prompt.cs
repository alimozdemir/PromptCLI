using System;
using System.Collections.Generic;
using System.Reflection;

namespace PromptCLI
{
    public interface IPrompt
    {
        IComponentPrompt<T> Add<T>(IComponent<T> comp);
        void Add(IComponent comp);
        void AddClass<T>(T poco) where T : class;
        void Begin();
    }

    public class Prompt : IPrompt
    {
        private readonly Queue<IComponent> _components;
        private IComponent _currentComponent;
        private int _offsetTop;
        public Prompt()
        {
            _components = new Queue<IComponent>();
            _offsetTop = 0;
        }

        public IComponentPrompt<T> Add<T>(IComponent<T> comp)
        {
            comp.Bind(this);
            _components.Enqueue(comp);
            return comp;
        }

        public void Add(IComponent comp)
        {
            _components.Enqueue(comp);
        }

        public void AddClass<T>(T poco) where T : class
        {
            var type = typeof(T);
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                SetPropertyCallback(prop, poco);
            }
        }

        private void SetPropertyCallback<T>(PropertyInfo prop, T poco)
        {
            if (!(Attribute.GetCustomAttribute(prop, typeof(BaseAttribute)) is BaseAttribute attr))
                return;

            this.Add(attr.Component);

            attr.SetCallback(prop, poco);
        }

        private void Clear()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }

        public void Begin()
        {
            this.Clear();
            ConsoleKeyInfo key;
            while (_components.Count > 0)
            {
                _currentComponent = _components.Dequeue();

                _currentComponent.SetTopPosition(_offsetTop);
                _currentComponent.Draw();
                do
                {
                    key = Console.ReadKey();

                    _currentComponent.Handle(key);
                }
                while (key.Key != ConsoleKey.Enter);

                _currentComponent.Complete();

                _offsetTop += _currentComponent.GetTopPosition();

                Console.SetCursorPosition(0, _offsetTop);
            }
        }

    }

}