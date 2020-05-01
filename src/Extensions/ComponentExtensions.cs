using System;
using System.Collections;
using System.Collections.Generic;

namespace PromptCLI
{
    internal static class ComponentExtensions
    {
        private static Type _genericInput = typeof(Input<>);
        private static Type _genericEnumerable = typeof(IEnumerable<>);
        public static void GetResult(this IComponent component, Type genericType)
        {

        }

        public static Type GetInputType(this ComponentType componentType, Type type) => componentType
            switch {
                ComponentType.Input => _genericInput.MakeGenericType(typeof(string)),
                ComponentType.Checkbox => _genericInput.MakeGenericType(_genericEnumerable.MakeGenericType(type)),
                ComponentType.Select => _genericInput.MakeGenericType(type),
                _ => throw new Exception("Unknown component type")
            };

    }
}