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
            /*var prompt = new Prompt();
            prompt.Add(new InputComponent("Project Name", "Project1"));
            prompt.Add(new SelectComponent<string>("License Type", new List<string>() { "MIT", "Apache", "GNU" } ));
            prompt.Add(new CheckboxComponent<string>("Features", new List<string>() { "Linter", "Router", "Other" }));
            prompt.Add(new InputComponent("Description"));
            prompt.Begin();*/
            
            var obj = new TestClass();
            var prompt = new Prompt();

            prompt.AddPoco(obj);

            prompt.Begin();

            Console.WriteLine("*********");

            Console.WriteLine("---- ProjectName:" + obj.ProjectName);
            Console.WriteLine("---- License:" + obj.License);
            Console.WriteLine("---- Level:" + string.Join(",", obj.Level));
        }


        public class TestClass
        {
            [Input("What is the project name ?")]
            public string ProjectName { get; set; }

            [Select(typeof(string), "License Type ?", "MIT", "GNU", "Apache")]
            public string License { get; set; }
            
            [Checkbox(typeof(int), "Level ?", 1,2,3,4)]
            public IEnumerable<int> Level { get; set; }
        }
    }
}
