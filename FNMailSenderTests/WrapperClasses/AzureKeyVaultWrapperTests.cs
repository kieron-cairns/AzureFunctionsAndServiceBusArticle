using Azure.Security.KeyVault.Secrets;
using FNMailSender.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using Moq;
using Azure;

namespace FNMailSenderTests.WrapperClasses
{
    public class AzureKeyVaultWrapperTests
    {
        [Fact]
        public async Task GetSecretAsync_ReturnsExpectedSecret()
        {
            // Arrange
            var mockSecretClient = new Mock<SecretClient>(MockBehavior.Strict);
            var testSecretName = "TestSecret";
            var testSecretValue = "TestValue";
            var mockResponse = new Mock<Response<KeyVaultSecret>>();
            mockResponse.SetupGet(r => r.Value).Returns(new KeyVaultSecret(testSecretName, testSecretValue));

            mockSecretClient.Setup(client => client.GetSecretAsync(testSecretName, null, default))
                .ReturnsAsync(mockResponse.Object);

            var azureKeyVaultWrapper = new AzureKeyVaultWrapper(mockSecretClient.Object);

            // Act
            var returnedSecret = await azureKeyVaultWrapper.GetSecretAsync(testSecretName);

            // Assert
            Assert.Equal(testSecretValue, returnedSecret);
        }

    }
}
