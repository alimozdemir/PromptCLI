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
        public override ComponentType Type => ComponentType.Checkbox;
        private object _fullComponent;
        private PropertyInfo _prop;

        private object _class;
        private readonly Type _type, _genericType;

        public CheckboxAttribute(Type type, string text, params object[] vals)
        {
            var componentResultType = this.Type.GetTType(type);
            var inputType = typeof(Input<>).MakeGenericType(componentResultType);
            var input = Activator.CreateInstance(inputType, text);
            // create the component instance
            var nonGenericType = typeof(CheckboxComponent<>);
            _genericType = nonGenericType.MakeGenericType(type);

            _fullComponent = Activator.CreateInstance(_genericType, input, ConvertList(vals, type));
            this.Component = (IComponent)_fullComponent;
            _type = type;
        }

        public override void SetCallback(PropertyInfo prop, object @class)
        {
            var componentResultType = this.Type.GetTType(_type);
            var callbackActionGeneric = typeof(Action<>).MakeGenericType(componentResultType);

            _prop = prop;
            _class = @class;

            var setter = typeof(CheckboxAttribute).GetMethod("Callback").MakeGenericMethod(componentResultType);

            var del = Delegate.CreateDelegate(callbackActionGeneric, this, setter);

            var set = _genericType.GetMethod("Callback");
            set.Invoke(_fullComponent, new object [] { del });
        }

        public void Callback<T>(T val)
        {
            _prop.SetValue(@_class, val);
        }
    }
}