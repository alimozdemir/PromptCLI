using System;
using PromptCLI;
using Xunit;
using Moq;

namespace PromptCLITests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var console = Mock.Of<IConsoleBase>();

            InputComponent component = new InputComponent("Text", console);
            ConsoleKeyInfo e = new ConsoleKeyInfo('e', ConsoleKey.E, false, false, false);
            component.Handle(e);

            Assert.Equal(component.Result.Status, "e");
        }
    }
}
