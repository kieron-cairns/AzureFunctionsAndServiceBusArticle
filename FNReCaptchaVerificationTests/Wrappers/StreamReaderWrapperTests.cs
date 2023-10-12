using FNReCaptchaVerification.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerificationTests.Wrappers
{
    public class StreamReaderWrapperTests
    {
        [Fact]
        public async Task ReadToEndAsync_ReturnsExpectedContent()
        {
            // Arrange
            var sampleContent = "Sample Stream Content";
            var mockStreamReader = new Mock<StreamReader>(Stream.Null); // Stream.Null is just a placeholder since we'll mock the method call

            mockStreamReader.Setup(sr => sr.ReadToEndAsync()).ReturnsAsync(sampleContent);

            // Here we use a little trick: We'll substitute the private _streamReader field of StreamReaderWrapper with our mock
            var wrapper = new StreamReaderWrapper(Stream.Null); // Stream.Null is a placeholder, since the mock will intercept the call
            typeof(StreamReaderWrapper).GetField("_streamReader", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(wrapper, mockStreamReader.Object);

            // Act
            var content = await wrapper.ReadToEndAsync();

            // Assert
            Assert.Equal(sampleContent, content);
        }
    }
}
