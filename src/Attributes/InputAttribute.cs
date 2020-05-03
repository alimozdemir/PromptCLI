using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PromptCLI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class InputAttribute : BaseAttribute
    {
        private InputComponent _component;
        public override IComponent Component => _component;
        public override ComponentType Type => ComponentType.Input;
        public override Type PropertyType => typeof(string);

        public InputAttribute(string text, string defaultValue = null)
        {
            _component = new InputComponent(text, defaultValue);
        }
        
        public override void SetCallback(PropertyInfo prop, object @class)
        {
            _component.Callback((input) => prop.SetValue(@class, input));
        }
    }
}