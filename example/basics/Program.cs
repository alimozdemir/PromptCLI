using System;
using System.Collections.Generic;
using PromptCLI;

namespace basics
{
    class Program
    {
        static void Main(string[] args)
        {
            var prompt = new Prompt();
            prompt.Add(new InputComponent("Project Name ?"));
            prompt.Add(new SelectComponent("Which Router ?", new List<Option>() { "Vue Router", "Angular Router", "React Router" }));
            prompt.Add(new InputComponent("License ?"));
            prompt.Begin();
        }
    }
}
