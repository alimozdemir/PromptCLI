using System;
using System.Collections.Generic;
using PromptCLI;

namespace callback
{
    class Project
    {
        public string ProjectName { get; set; }
        public string License { get; set; }
        public IEnumerable<string> Features { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Project Name: {ProjectName}\nLicense: {License}\nFeatures: {string.Join(",", Features)}\nDescription: {Description}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var project = new Project();

            var prompt = new Prompt();
            prompt.Add(new InputComponent("Project Name", "Project1"))
                .Callback(i => project.ProjectName = i.Status)
                .Add(new SelectComponent<string>("License Type", new List<string>() { "MIT", "Apache", "GNU" } ))
                .Callback(i => project.License = i.Status)
                .Add(new CheckboxComponent<string>("Features", new List<string>() { "Linter", "Router", "Other" }))
                .Callback(i => project.Features = i.Status)
                .Add(new InputComponent("Description"))
                .Callback(i => project.Description = i.Text);
            prompt.Begin();

            Console.WriteLine();
            Console.WriteLine("***** The result *****");
            Console.WriteLine(project);

        }
    }
}
