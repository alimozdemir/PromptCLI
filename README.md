# PromptCLI

PromptCLI is inspired from inquirer.js and enquirer.js. It is a interactive command line interface library.

# Basics

```csharp
var prompt = new Prompt();
prompt.Add(new InputComponent("Project Name", "Project1"));
prompt.Add(new SelectComponent<string>("License Type", new List<Option<string>>() { ("MIT", "MIT"), ("Apache", "Apache"), ("GNU", "GNU") }));
prompt.Add(new CheckboxComponent<int>("Features", new List<Option<int>>() { ("Linter", 1), ("Router", 2), ("Other", 3) }));
prompt.Add(new InputComponent("Description"));
prompt.Begin();
```

## With callback action


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
    .Callback(i => project.Description = i.Text);
prompt.Begin();
```

# Todo

- Define data attributes for specify the components
- Fullfil the POCO class with right components


# Contributions
