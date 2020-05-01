using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PromptCLI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CheckboxAttribute : Attribute, IComponentAttribute
    {
        // we need generic attribute to better support
        // https://github.com/dotnet/roslyn/pull/26337
        public IComponent Component { get; private set;}

        private readonly Type _type, _genericType;

        public CheckboxAttribute(Type type, string text, params object[] vals)
        {
            // create the component instance
            var nonGenericType = typeof(CheckboxComponent<>);
            _genericType = nonGenericType.MakeGenericType(type);
            this.Component = (IComponent)Activator.CreateInstance(_genericType, text, vals.ToList());

            _type = type;
        }

        public object GetResult()
        {
            var resultProperty = _genericType.GetProperty("Result");
            var data = resultProperty.GetValue(this.Component);

            var resultGenericType = this.Component.ComponentType.GetInputType(_type);
            var statusProperty = resultGenericType.GetProperty("Status");

            return statusProperty.GetValue(data);
        }
    }
}