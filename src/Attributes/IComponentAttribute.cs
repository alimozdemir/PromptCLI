using System;
using System.Runtime.CompilerServices;

namespace PromptCLI
{
    public interface IComponentAttribute
    {
        IComponent Component { get; }
        object GetResult();
    }
}