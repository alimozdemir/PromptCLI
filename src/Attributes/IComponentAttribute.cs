using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PromptCLI
{
    public interface IComponentAttribute
    {
        IComponent Component { get; }
        object GetResult();
        void SetCallback(PropertyInfo prop, object @class);
    }
}