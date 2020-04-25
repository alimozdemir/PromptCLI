using System;
using System.Collections.Generic;

namespace PromptCLI
{
    public class Prompt
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