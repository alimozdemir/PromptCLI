using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PromptCLI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class BaseAttribute : Attribute, IComponentAttribute
    {
        public abstract Type Type { get; }
        public abstract IComponent Component { get; }

        public abstract object GetResult();

        public abstract void SetCallback(PropertyInfo prop, object @class);


        /// <summary>
        /// Reference https://stackoverflow.com/a/22942598/2107255
        /// </summary>
        /// <param name="items"></param>
        /// <param name="type"></param>
        /// <param name="performConversion"></param>
        /// <returns></returns>
        public object ConvertList(List<object> items, Type type)
        {
            var enumerableType = typeof(System.Linq.Enumerable);
            var castMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.Cast)).MakeGenericMethod(type);
            var toListMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.ToList)).MakeGenericMethod(type);

            IEnumerable<object> itemsToCast = items.Select(item => Convert.ChangeType(item, type));

            var castedItems = castMethod.Invoke(null, new[] { itemsToCast });

            return toListMethod.Invoke(null, new[] { castedItems });
        }
    }
}