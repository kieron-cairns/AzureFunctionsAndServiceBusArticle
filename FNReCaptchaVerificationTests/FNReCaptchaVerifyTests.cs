using FNReCaptchaVerification.Interfaces;
using FNReCaptchaVerification.Utilities;
using FNReCaptchaVerificationTests.Utilities;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using FNReCaptchaVerification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using Newtonsoft.Json;

namespace FNReCaptchaVerificationTests
{
    public class FNReCaptchaVerifyTests
    {

        private readonly string formDataMock = @"
        {
            ""name"": ""John Doe"",
            ""email"": ""johndoe@example.com"",
            ""phone"": ""123-456-7890"",
            ""message"": ""Hello, this is a test message!""
        }";

        [Fact]
        public async Task Run_ValidCaptcha_ReturnsOkResult()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var mockJsonConvertWrapper = new Mock<IJsonConvertWrapper>();
            var mockStreamReaderWrapper = new Mock<IStreamReaderWrapper>();
            var mockHttpRequestWrapper = new Mock<IHttpRequestWrapper>();
            var mockCaptchaVerificationService = new Mock<ICaptchaVerificationService>();
            var mockCollector = new Mock<IAsyncCollectorWrapper<string>>();
            var mockLogger = new Mock<ILogger>();

            // Mock the captcha verification to return true
            mockCaptchaVerificationService.Setup(service => service.VerifyCaptchaAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Setup for AddAsync method
            mockCollector.Setup(c => c.AddAsync(It.IsAny<string>()))
                         .Returns(Task.CompletedTask);

            // Setup for FlushAsync method
            mockCollector.Setup(c => c.FlushAsync(It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

            // Mock the JsonConvertWrapper to return the captchaValue from formDataMock
            dynamic mockData = new ExpandoObject();
            mockData.captchaValue = "validCaptcha";
            mockJsonConvertWrapper.Setup(json => json.DeserializeObject<dynamic>(It.IsAny<string>()))
                                  .Returns(mockData);

            // Mocking the StreamReaderWrapper's ReadToEndAsync method
            mockStreamReaderWrapper.Setup(sr => sr.ReadToEndAsync())
                                   .ReturnsAsync("{\"captchaValue\":\"validCaptcha\"}");

            // Mocking the HttpRequestWrapper's Body property
            mockHttpRequestWrapper.Setup(req => req.Body)
                                  .Returns(new MemoryStream());

            var fnReCaptchaVerify = new FNReCaptchaVerify(
                mockConfig.Object,
                mockHttpClientWrapper.Object,
                mockJsonConvertWrapper.Object,
                mockStreamReaderWrapper.Object,
                mockHttpRequestWrapper.Object,
                mockCaptchaVerificationService.Object);

            // Act
            var result = await fnReCaptchaVerify.Run(mockHttpRequestWrapper.Object, formDataMock, mockCollector.Object, mockLogger.Object);

            // ... [rest of the test]

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serializedValue = JsonConvert.SerializeObject(okResult.Value); // Convert object to string
            dynamic value = JsonConvert.DeserializeObject(serializedValue); // Deserialize string to dynamic

            //Assert.True(value.success);
            Assert.True((bool)value.success);
            Assert.Equal("Captcha verification passed & added to service bus verification queue.", (string)value.msg);
        }
    }

}