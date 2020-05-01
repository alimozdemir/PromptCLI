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


        [Fact]
        public void Draw_InputText_Write()
        {
            var console = new Mock<IConsoleBase>();
            const string text = "Question";

            InputComponent component = new InputComponent(text, console.Object);
            component.Draw();

            // make sure the input is wrote
            console.Verify(i => i.Write(text), Times.Once);
            // and make sure the start length must be higher than text length
            Assert.True(component.Range.Start.Value > text.Length);
        }

        
        [Fact]
        public void Draw_InputTextWithDefault_Write()
        {
            var console = new Mock<IConsoleBase>();
            const string text = "Question", defaultVal = "DefaultValue";

            InputComponent component = new InputComponent(text, console.Object, defaultVal);
            component.Draw();

            // make sure the input is wrote
            string defaultFormattedVal = string.Format(" ({0})", defaultVal);
            console.Verify(i => i.Write(text), Times.Once);
            console.Verify(i => i.Write(defaultFormattedVal, It.IsAny<ConsoleColor>(), null), Times.Once);
            // and make sure the start length must be higher than text length
            Assert.True(component.Range.Start.Value > text.Length);
            // also, the cursor should be setted into range start
            Assert.Equal(component.Range.Start.Value, component.CursorLeft);
        }
        
        [Fact]
        public void SetTopPosition_CustomInput_Set()
        {
            var console = new Mock<IConsoleBase>();
            const int val = 10;
            
            InputComponent component = new InputComponent("Input text", console.Object);
            component.SetTopPosition(val);

            Assert.Equal(val, component.CursorTop);
        }

        [Fact]
        public void Complete_CursorReset_Done()
        {
            var console = new Mock<IConsoleBase>();
            
            InputComponent component = new InputComponent("Input text", console.Object);
            component.Complete();
            Assert.Equal(0, component.CursorLeft);
        }

        [Fact]
        public void Complete_ClearLineCurrentTop_Cleared()
        {
            var console = new Mock<IConsoleBase>();
            const int top = 10;
            InputComponent component = new InputComponent("Input text", console.Object);
            component.SetTopPosition(top);

            component.Complete();
            
            console.Verify(i => i.ClearLine(top, 0), Times.Once);
        }

        
        [Fact]
        public void Complete_InputText_Write()
        {
            var console = new Mock<IConsoleBase>();
            const string text = "Input text";
            InputComponent component = new InputComponent(text, console.Object);

            component.Complete();
            console.Verify(i => i.Write(text), Times.Once);
        }

        
        [Fact]
        public void Complete_InputStatus_Same()
        {
            var console = new Mock<IConsoleBase>();
            const string text = "Input text";
            InputComponent component = new InputComponent(text, console.Object);

            ConsoleKeyInfo e = new ConsoleKeyInfo('e', ConsoleKey.E, false, false, false);
            component.Handle(e);

            component.Complete();
            Assert.Equal("e", component.Result.Status);
        }

        [Fact]
        public void Complete_DefaultValueInputStatus_Same()
        {
            var console = new Mock<IConsoleBase>();
            const string text = "Input text", defaultVal = "DefaultVal";
            InputComponent component = new InputComponent(text, console.Object, defaultVal);

            component.Complete();
            Assert.Equal(defaultVal, component.Result.Status);
        }

        
        [Fact]
        public void Complete_DefaultValueInputStatus_Override()
        {
            var console = new Mock<IConsoleBase>();
            const string text = "Input text", defaultVal = "DefaultVal";
            InputComponent component = new InputComponent(text, console.Object, defaultVal);

            ConsoleKeyInfo e = new ConsoleKeyInfo('e', ConsoleKey.E, false, false, false);
            component.Handle(e);

            component.Complete();
            Assert.Equal("e", component.Result.Status);
        }
        
        [Fact]
        public void Complete_Callback_Invoked()
        {
            var console = new Mock<IConsoleBase>();
            var mockCallback = new Mock<Action<Input<string>>>();

            const string text = "Input text", defaultVal = "DefaultVal";
            InputComponent component = new InputComponent(text, console.Object, defaultVal);

            component.Callback(mockCallback.Object);

            component.Complete();

            mockCallback.Verify(i => i.Invoke(component.Result));
        }

        
        [Fact]
        public void Bind_Prompt_Binded()
        {
            var console = new Mock<IConsoleBase>();
            var mockPrompt = new Mock<Prompt>();

            const string text = "Input text", defaultVal = "DefaultVal";
            InputComponent component = new InputComponent(text, console.Object, defaultVal);

            component.Bind(mockPrompt.Object);

            var result = component.Callback(It.IsAny<Action<Input<string>>>());

            Assert.Equal(mockPrompt.Object, result);
        }

    }
}
