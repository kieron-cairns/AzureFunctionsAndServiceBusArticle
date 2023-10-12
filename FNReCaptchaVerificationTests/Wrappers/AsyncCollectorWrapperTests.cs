using FNReCaptchaVerification.Utilities;
using Microsoft.Azure.WebJobs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerificationTests.Wrappers
{
    public class AsyncCollectorWrapperTests
    {
        [Fact]
        public async Task AddAsync_CallsUnderlyingCollector()
        {
            // Arrange
            var mockCollector = new Mock<IAsyncCollector<string>>();
            var collectorWrapper = new AsyncCollectorWrapper<string>(mockCollector.Object);
            var message = "Test Message";

            // Act
            await collectorWrapper.AddAsync(message);

            // Assert
            mockCollector.Verify(c => c.AddAsync(message, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task FlushAsync_CallsUnderlyingCollector()
        {
            // Arrange
            var mockCollector = new Mock<IAsyncCollector<string>>();
            var collectorWrapper = new AsyncCollectorWrapper<string>(mockCollector.Object);

            // Act
            await collectorWrapper.FlushAsync();

            // Assert
            mockCollector.Verify(c => c.FlushAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
