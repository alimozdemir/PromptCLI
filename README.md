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

# Advanced