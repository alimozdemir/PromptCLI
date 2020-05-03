using System;
using System.Collections.Generic;
using System.Threading;
using PromptCLI;

namespace attributes
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj = new TestClass();
            var prompt = new Prompt();

            prompt.AddClass(obj);

            prompt.Begin();

            Console.WriteLine();

            Console.WriteLine("*********");

            Console.WriteLine("---- ProjectName:" + obj.ProjectName);
            Console.WriteLine("---- License:" + obj.License);
            Console.WriteLine("---- Level:" + string.Join(",", obj.Level));
            Console.WriteLine("---- Description:" + obj.Description);
        }


        public class TestClass
        {
            [Input("What is the project name ?")]
            public string ProjectName { get; set; }

            [Select(typeof(int), "License Type ?", "MIT", "GNU", "Apache")]
            public string License { get; set; }

            [Checkbox(typeof(int), "Level ?", 1, 2, 3, 4)]
            public IEnumerable<int> Level { get; set; }

            [Input("Briefly explain ?")]
            public string Description { get; set; }
            
            public string NonInput { get; set; }
        }

    }
}
