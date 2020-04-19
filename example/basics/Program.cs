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
            prompt.Add(new CheckboxComponent<int>("Which Router ?", new List<Option<int>>() { ("Vue Router", 1), ("Angular Router", 2), ("React Router", 3) }));
            prompt.Add(new SelectComponent<int>("Which Router ?", new List<Option<int>>() { ("Vue Router", 1), ("Angular Router", 2), ("React Router", 3) }));
            prompt.Add(new InputComponent("Project Name ?"));
            prompt.Begin();
        }
    }
}
