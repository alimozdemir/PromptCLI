using System.Reflection;

namespace PromptCLI
{
    public interface IComponentAttribute
    {
        IComponent Component { get; }
        void SetCallback(PropertyInfo prop, object @class);
    }
}