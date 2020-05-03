# PromptCLI

![Nuget](https://img.shields.io/nuget/v/PromptCLI) [![Azure DevOps builds](https://img.shields.io/azure-devops/build/almozdmr/PromptCLI/1)](https://dev.azure.com/almozdmr/PromptCLI/_build?definitionId=1&_a=summary) ![Coverage](https://img.shields.io/azure-devops/coverage/almozdmr/PromptCLI/1)

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

## Attributes

```csharp
public class TestClass
{
    [Input("Project Name ?")]
    public string ProjectName { get; set; }

    [Select(typeof(string), "License Type ?", "MIT", "GNU", "Apache")]
    public string License { get; set; }

    [Checkbox(typeof(int), "Level ?", 1, 2, 3, 4)]
    public IEnumerable<int> Level { get; set; }

    [Input("Briefly explain ?")]
    public string Description { get; set; }
}

static void Main(string[] args)
{
    var obj = new TestClass();
    var prompt = new Prompt();

    prompt.AddClass(obj);

    prompt.Begin();
}

```


## Callback Action

You can handle callback action after each step

```csharp
var project = new Project();

var prompt = new Prompt();
prompt.Add(new InputComponent("Project Name", "Project1"))
    .Callback(i => project.ProjectName = i)
    .Add(new SelectComponent<string>("License Type", new List<string>() { "MIT", "Apache", "GNU" } ))
    .Callback(i => project.License = i)
    .Add(new CheckboxComponent<string>("Features", new List<string>() { "Linter", "Router", "Other" }))
    .Callback(i => project.Features = i)
    .Add(new InputComponent("Description"))
    .Callback(i => project.Description = i);
prompt.Begin();
```

# Todo

- Unit tests
- Define data attributes for specify the components
- Linked list implementation on Prompt class

# Contributions

All contributions are welcome if well described.
