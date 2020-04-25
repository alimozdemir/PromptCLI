# PromptCLI

PromptCLI is inspired from inquirer.js and enquirer.js. It is a interactive command line interface library.

![Basics](https://github.com/lyzerk/PromptCLI/raw/master/assets/gifs/basics.gif "Basics")

# Basics

```csharp
var prompt = new Prompt();
prompt.Add(new InputComponent("Project Name", "Project1"));
prompt.Add(new SelectComponent<string>("License Type", new List<string>() { "MIT", "Apache", "GNU" } ));
prompt.Add(new CheckboxComponent<string>("Features", new List<string>() { "Linter", "Router", "Other" }));
prompt.Add(new InputComponent("Description"));
prompt.Begin();
```

## With callback action

You can handle callback action after each step

```csharp
var project = new Project();

var prompt = new Prompt();
prompt.Add(new InputComponent("Project Name", "Project1"))
    .Callback(i => project.ProjectName = i.Status)
    .Add(new SelectComponent<string>("License Type", new List<string>() { "MIT", "Apache", "GNU" } ))
    .Callback(i => project.License = i.Status)
    .Add(new CheckboxComponent<string>("Features", new List<string>() { "Linter", "Router", "Other" }))
    .Callback(i => project.Features = i.Status)
    .Add(new InputComponent("Description"))
    .Callback(i => project.Description = i.Status);
prompt.Begin();
```

# Todo

- Unit tests
- Define data attributes for specify the components
- Fulfill the POCO class with right components

# Contributions

All contributions are welcome if described well.
