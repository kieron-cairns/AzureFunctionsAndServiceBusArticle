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
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IHttpClientWrapper> _mockHttpClientWrapper;
        private readonly Mock<IJsonConvertWrapper> _mockJsonConvertWrapper;
        private readonly Mock<IStreamReaderWrapper> _mockStreamReaderWrapper;
        private readonly Mock<IHttpRequestWrapper> _mockHttpRequestWrapper;
        private readonly Mock<ICaptchaVerificationService> _mockCaptchaVerificationService;
        private readonly Mock<IAsyncCollectorWrapper<string>> _mockCollector;
        private readonly Mock<ILogger> _mockLogger;

        public FNReCaptchaVerifyTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            _mockJsonConvertWrapper = new Mock<IJsonConvertWrapper>();
            _mockStreamReaderWrapper = new Mock<IStreamReaderWrapper>();
            _mockHttpRequestWrapper = new Mock<IHttpRequestWrapper>();
            _mockCaptchaVerificationService = new Mock<ICaptchaVerificationService>();
            _mockCollector = new Mock<IAsyncCollectorWrapper<string>>();
            _mockLogger = new Mock<ILogger>();
        }

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
           

            // Mock the captcha verification to return true
            _mockCaptchaVerificationService.Setup(service => service.VerifyCaptchaAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Setup for AddAsync method
            _mockCollector.Setup(c => c.AddAsync(It.IsAny<string>()))
                         .Returns(Task.CompletedTask);

            // Setup for FlushAsync method
            _mockCollector.Setup(c => c.FlushAsync(It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

            // Mock the JsonConvertWrapper to return the captchaValue from formDataMock
            dynamic mockData = new ExpandoObject();
            mockData.captchaValue = "validCaptcha";
            _mockJsonConvertWrapper.Setup(json => json.DeserializeObject<dynamic>(It.IsAny<string>()))
                                  .Returns(mockData);

            // Mocking the StreamReaderWrapper's ReadToEndAsync method
            _mockStreamReaderWrapper.Setup(sr => sr.ReadToEndAsync())
                                   .ReturnsAsync("{\"captchaValue\":\"validCaptcha\"}");

            // Mocking the HttpRequestWrapper's Body property
            _mockHttpRequestWrapper.Setup(req => req.Body)
                                  .Returns(new MemoryStream());

            var fnReCaptchaVerify = new FNReCaptchaVerify(
                _mockConfig.Object,
                _mockHttpClientWrapper.Object,
                _mockJsonConvertWrapper.Object,
                _mockStreamReaderWrapper.Object,
                _mockHttpRequestWrapper.Object,
                _mockCaptchaVerificationService.Object);

            // Act
            var result = await fnReCaptchaVerify.Run(_mockHttpRequestWrapper.Object, formDataMock, _mockCollector.Object, _mockLogger.Object);

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