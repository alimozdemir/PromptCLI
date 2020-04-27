using System;
using PromptCLI;
using Xunit;
using Moq;

namespace PromptCLITests
{
    public class InputComponentTests
    {
        [Fact]
        public void Handle_PressAChar_Pushed()
        {
            var console = Mock.Of<IConsoleBase>();

            InputComponent component = new InputComponent("Text", console);
            ConsoleKeyInfo e = new ConsoleKeyInfo('e', ConsoleKey.E, false, false, false);
            component.Handle(e);

            Assert.Equal("e", component.Result.Status);
        }
        
        [Fact]
        public void Handle_PressTwoChars_Pushed()
        {
            var console = Mock.Of<IConsoleBase>();

            InputComponent component = new InputComponent("Text", console);
            ConsoleKeyInfo e = new ConsoleKeyInfo('p', ConsoleKey.P, false, false, false);
            component.Handle(e);

            e = new ConsoleKeyInfo('e', ConsoleKey.E, false, false, false);
            component.Handle(e);

            Assert.Equal("pe", component.Result.Status);
        }

        
        
        [Fact]
        public void Handle_NotValidKey_Unknown()
        {
            var console = Mock.Of<IConsoleBase>();

            InputComponent component = new InputComponent("Text", console);
            ConsoleKeyInfo e = new ConsoleKeyInfo('*', ConsoleKey.Multiply, false, false, false);
            component.Handle(e);

            Assert.True(string.IsNullOrEmpty(component.Result.Status));
        }

        
        [Fact]
        public void Handle_NotValidKeyWithAChar_Unknown()
        {
            var console = Mock.Of<IConsoleBase>();

            InputComponent component = new InputComponent("Text", console);

            ConsoleKeyInfo e = new ConsoleKeyInfo('e', ConsoleKey.E, false, false, false);
            component.Handle(e);

            e = new ConsoleKeyInfo('*', ConsoleKey.Multiply, false, false, false);
            component.Handle(e);

            Assert.Equal("e", component.Result.Status);
        }

        
        
        [Fact]
        public void Handle_Direction_Unknown()
        {
            var console = Mock.Of<IConsoleBase>();

            InputComponent component = new InputComponent("Text", console);
            ConsoleKeyInfo e = new ConsoleKeyInfo((char)ConsoleKey.UpArrow, ConsoleKey.UpArrow, false, false, false);
            component.Handle(e);

            Assert.True(string.IsNullOrEmpty(component.Result.Status));
        }


        [Fact]
        public void HandleDefault_PressAChar_CursorPositionChanged()
        {
            var console = new Mock<IConsoleBase>();
            int cursorPoint = 30;
            console.SetupGet(i => i.CursorLeft).Returns(cursorPoint);

            InputComponent component = new InputComponent("Text", console.Object, "DefaultValue");
            ConsoleKeyInfo e = new ConsoleKeyInfo('h', ConsoleKey.H, false, false, false);
            component.Handle(e);

            e = new ConsoleKeyInfo((char)ConsoleKey.RightArrow, ConsoleKey.RightArrow, false, false, false);
            component.Handle(e);

            Assert.Equal(cursorPoint, component.CursorLeft);
        }

        [Fact]
        public void Handle_Backspace_RemoveChar()
        {
            var console = Mock.Of<IConsoleBase>();

            InputComponent component = new InputComponent("Text", console, "DefaultValue");
            ConsoleKeyInfo e = new ConsoleKeyInfo('h', ConsoleKey.H, false, false, false);
            component.Handle(e);
            e = new ConsoleKeyInfo('e', ConsoleKey.H, false, false, false);
            component.Handle(e);
            e = new ConsoleKeyInfo('l', ConsoleKey.H, false, false, false);
            component.Handle(e);

            e = new ConsoleKeyInfo((char)ConsoleKey.Backspace, ConsoleKey.Backspace, false, false, false);
            component.Handle(e);

            Assert.Equal("he", component.Result.Status);
        }
    }
}
