namespace PromptCLI
{
    public class TestClass
    {
        [Input("What is the project name ?")]
        public string ProjectName { get; set; }


        [Input("License Type ?")]
        public string License { get; set; }
    }
}