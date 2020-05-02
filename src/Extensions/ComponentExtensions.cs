using System;
using System.Collections;
using System.Collections.Generic;

namespace PromptCLI
{
    internal static class ComponentExtensions
    {
        private static Type _genericInput = typeof(Input<>);
        private static Type _genericEnumerable = typeof(IEnumerable<>);

        public static Type GetInputType(this ComponentType componentType, Type type) => 
            _genericInput.MakeGenericType(componentType.GetTType(type));
        
        public static Type GetTType(this ComponentType componentType, Type type) => componentType
            switch {
                ComponentType.Input => typeof(string),
                ComponentType.Checkbox => _genericEnumerable.MakeGenericType(type),
                ComponentType.Select => type,
                _ => throw new Exception("Unknown component type")
            };
    }
}