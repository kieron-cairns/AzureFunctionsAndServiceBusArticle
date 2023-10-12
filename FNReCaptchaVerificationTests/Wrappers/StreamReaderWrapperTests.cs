using FNReCaptchaVerification.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerificationTests.Wrappers
{
    public class StreamReaderWrapperTests
    {
        [Fact]
        public async Task ReadToEndAsync_ReturnsCorrectString()
        {
            var testString = "Test Content";
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(testString));
            var streamReaderWrapper = new StreamReaderWrapper(memoryStream);

            var result = await streamReaderWrapper.ReadToEndAsync();

            Assert.Equal(testString, result);
        }
    }
}
