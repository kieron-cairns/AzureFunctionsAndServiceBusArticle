using FNMailSender.Interfaces;
using FNMailSender.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNMailSenderTests.WrapperClasses
{
    public class AzureSecretClientWrapperTests
    {
        [Fact]
        public async Task GetSecretAsync_ReturnsExpectedSecret()
        {
            // Arrange
            var mockKeyVaultWrapper = new Mock<IAzureKeyVaultWrapper>();
            var testSecretName = "TestSecret";
            var testSecretValue = "TestValue";
            mockKeyVaultWrapper.Setup(c => c.GetSecretAsync(It.Is<string>(s => s == testSecretName)))
                               .ReturnsAsync(testSecretValue);

            var azureSecretClientWrapper = new AzureSecretClientWrapper(mockKeyVaultWrapper.Object);

            // Act
            var result = await azureSecretClientWrapper.GetSecretAsync(testSecretName);

            // Assert
            Assert.Equal(testSecretValue, result);
        }
    }
}
