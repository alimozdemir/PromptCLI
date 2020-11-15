using System.Reflection;

namespace PromptCLI
{
    public interface IComponentAttribute
    {
        ComponentBase Component { get; }
        void SetCallback(PropertyInfo prop, object @class);
    }
}