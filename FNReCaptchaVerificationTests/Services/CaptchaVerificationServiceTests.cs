using FNReCaptchaVerification.Interfaces;
using FNReCaptchaVerification.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerificationTests.Services
{
    public class CaptchaVerificationServiceTests
    {
        [Fact]
        public async Task VerifyCaptchaAsync_SuccessfulVerification_ReturnsTrue()
        {
            // Arrange
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var successfulVerificationResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"success\": true}")
            };

            mockHttpClientWrapper.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(successfulVerificationResponse);

            var captchaVerificationService = new CaptchaVerificationService(mockHttpClientWrapper.Object, "TestSecretKey");

            // Act
            var isVerified = await captchaVerificationService.VerifyCaptchaAsync("testCaptchaValue");

            // Assert
            Assert.True(isVerified);
        }

        [Fact]
        public async Task VerifyCaptchaAsync_FailedVerification_ReturnsFalse()
        {
            // Arrange
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var failedVerificationResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"success\": false}")
            };

            mockHttpClientWrapper.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(failedVerificationResponse);

            var captchaVerificationService = new CaptchaVerificationService(mockHttpClientWrapper.Object, "TestSecretKey");

            // Act
            var isVerified = await captchaVerificationService.VerifyCaptchaAsync("testCaptchaValue");

            // Assert
            Assert.False(isVerified);
        }

        // More tests can be added to handle other scenarios if necessary.
    }

}
