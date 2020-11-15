using System;
using System.Collections.Generic;
using System.Reflection;

namespace PromptCLI
{
    public interface IPrompt
    {
        IComponentPrompt<T> Add<T>(ComponentBase<T> comp);
        void Add(ComponentBase comp);
        T Run<T>() where T : class, new();
        void Begin();
    }

    public class Prompt : IPrompt
    {
        private readonly Queue<ComponentBase> _components;
        private ComponentBase _currentComponent;
        private int _offsetTop;
        private readonly PromptConfig _config;

        public Prompt()
            : this(new PromptConfig())
        {
        }

        public Prompt(PromptConfig config)
        {
            _components = new Queue<ComponentBase>();
            _offsetTop = 0;
            _config = config;
        }

        public IComponentPrompt<T> Add<T>(ComponentBase<T> comp)
        {
            comp.Bind(this);
            _components.Enqueue(comp);
            return comp;
        }

        public void Add(ComponentBase comp)
        {
            _components.Enqueue(comp);
        }

        private void AddClassProperties<T>(T poco) where T : class
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

            /* enable later
            if (prop.PropertyType != attr.PropertyType)
                throw new Exception($"{prop.Name} is not valid with {attr.PropertyType}-{prop.PropertyType}");
            */

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
                
                _currentComponent.SetConfig(_config);

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

        public T Run<T>() where T : class, new()
        {
            var result = new T();

            AddClassProperties(result);

            Begin();

            return result;
        }

    }

}