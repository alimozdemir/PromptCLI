using System;
using System.Runtime.CompilerServices;

namespace PromptCLI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class InputAttribute : Attribute, IComponentAttribute
    {
        private InputComponent _component;
        public IComponent Component => _component;

        public InputAttribute(string text, string defaultValue = null, [CallerMemberName]string propertyName = null)
        {
            _component = new InputComponent(text, defaultValue);
        }

        public object GetResult()
        {
            return _component.Result.Status;
        }
    }
}