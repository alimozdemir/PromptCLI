
using System;
using System.Reflection;

namespace PromptCLI
{
    public class PocoBuilder
    {
        public void Run<T>(T poco, Prompt prompt)
        {
            var type = typeof(T);
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                PropertyCheck(prop, poco, prompt);
            }
        }     

        private void PropertyCheck<T>(PropertyInfo prop, T poco, Prompt prompt)
        {
            var attr = (BaseAttribute)Attribute.GetCustomAttribute(prop, typeof(BaseAttribute));
            
            if (attr == null)
                return;

            prompt.Add(attr.Component);

            attr.SetCallback(prop, poco);
        }
    }
}