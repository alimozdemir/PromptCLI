using System;
using System.Collections.Generic;
using System.Threading;
using PromptCLI;

namespace basics
{
    class Program
    {
        static void Main(string[] args)
        {
            var prompt = new Prompt();
            prompt.Add(new InputComponent("Project Name", "Project1"));
            prompt.Add(new SelectComponent<string>("License Type", new List<string>() { "MIT", "Apache", "GNU" } ));
            prompt.Add(new CheckboxComponent<string>("Features", new List<string>() { "Linter", "Router", "Other" }));
            prompt.Add(new InputComponent("Description"));
            prompt.Begin();
        }
    }
}
