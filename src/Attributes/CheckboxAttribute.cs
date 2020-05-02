using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PromptCLI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CheckboxAttribute : BaseAttribute
    {
        // we need generic attribute to better support
        // https://github.com/dotnet/roslyn/pull/26337
        public override IComponent Component { get; }
        public override Type Type => _type;
        private object _fullComponent;
        private PropertyInfo _prop;

        private object _class;
        private readonly Type _type, _genericType;

        public CheckboxAttribute(Type type, string text, params object[] vals)
        {
            var componentResultType = ComponentType.Checkbox.GetTType(type);
            var inputType = typeof(Input<>).MakeGenericType(componentResultType);
            var input = Activator.CreateInstance(inputType, text);
            // create the component instance
            var nonGenericType = typeof(CheckboxComponent<>);
            _genericType = nonGenericType.MakeGenericType(type);

            var listType = typeof(List<>).MakeGenericType(type);

            _fullComponent = Activator.CreateInstance(_genericType, input, ConvertList(vals.ToList(), type));
            this.Component = (IComponent)_fullComponent;
            _type = type;
        }

        public override object GetResult()
        {
            var resultProperty = _genericType.GetProperty("Result");
            var data = resultProperty.GetValue(this.Component);

            var resultGenericType = this.Component.ComponentType.GetInputType(_type);
            var statusProperty = resultGenericType.GetProperty("Status");

            return statusProperty.GetValue(data);
        }

        public override void SetCallback(PropertyInfo prop, object @class)
        {
            var componentResultType = this.Component.ComponentType.GetTType(_type);
            var callbackActionGeneric = typeof(Action<>).MakeGenericType(componentResultType);

            _prop = prop;
            _class = @class;

            var setter = typeof(CheckboxAttribute).GetMethod("Callback").MakeGenericMethod(componentResultType);

            var d = Delegate.CreateDelegate(callbackActionGeneric, this, setter);

            var set = _genericType.GetMethod("Callback");
            set.Invoke(_fullComponent, new object [] { d });
        }

        public void Callback<T>(T val)
        {
            _prop.SetValue(@_class, val);
        }
    }
}