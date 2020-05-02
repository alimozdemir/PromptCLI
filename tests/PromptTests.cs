using System;
using PromptCLI;
using Xunit;
using Moq;
using System.Linq;
using System.Collections.Generic;

namespace PromptCLITests
{
    public class PromptTests
    {
        [Fact]
        public void Add_InputComponent_Binded()
        {
            var prompt = new Prompt();
            var comp = new InputComponent("Q");
            var result = prompt.Add(comp);

            Assert.Equal(prompt, comp.Callback(It.IsAny<Action<string>>()));
        }

        [Fact]
        public void Begin_()
        {
            var prompt = new Prompt();
            var comp = new Mock<IComponent<string>>();
            var result = prompt.Add(comp.Object);

            // prompt.Begin();
        }
    }
}
