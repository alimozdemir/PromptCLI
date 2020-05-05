using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PromptCLI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SelectAttribute : BaseAttribute
    {
        // we need generic attribute for better support
        // https://github.com/dotnet/roslyn/pull/26337
        public override IComponent Component { get; }
        public override ComponentType Type => ComponentType.Select;
        public override Type PropertyType => _type;
        private object _fullComponent;
        private PropertyInfo _prop;

        private object _class;
        private readonly Type _type, _genericType;

        public SelectAttribute(Type type, string text, params object[] vals)
        {
            // implicit operator does not work with reflection, therefore we have to create input by ourselves
            var inputType = typeof(Input<>).MakeGenericType(type);
            var input = Activator.CreateInstance(inputType, text);

            // create the component instance
            var nonGenericType = typeof(SelectComponent<>);
            _genericType = nonGenericType.MakeGenericType(type);

            _fullComponent = Activator.CreateInstance(_genericType, input, ConvertList(vals, type));

            this.Component = (IComponent)_fullComponent;
            _type = type;
        }

        public override void SetCallback(PropertyInfo prop, object @class)
        {
            var callbackActionGeneric = typeof(Action<>).MakeGenericType(_type);

            _prop = prop;
            _class = @class;

            var setter = typeof(SelectAttribute).GetMethod("Callback").MakeGenericMethod(_type);

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