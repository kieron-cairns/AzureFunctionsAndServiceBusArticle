using System;
using System.Net;
using FNMailSender;
using FNMailSender.Interfaces;
using FNMailSender.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using Xunit;

namespace FNMailSenderTests
{


    public class FNMailSenderFunctionTests
    {
        private Mock<IAzureSecretClientWrapper> _mockAzureSecretClientWrapper;
        private Mock<ISendGridServiceWrapper> _mockSendGridServiceWrapper;
        private Mock<IConfigurationWrapper> _mockConfigurationWrapper;
        private Mock<ILogger> _mockLogger;

        private MailSenderFunction _function;

        private readonly string formDataMock = @"
        {
            ""name"": ""John Doe"",
            ""email"": ""johndoe@example.com"",
            ""phone"": ""123-456-7890"",
            ""message"": ""Hello, this is a test message!""
        }";

        public FNMailSenderFunctionTests()
        {
            _mockAzureSecretClientWrapper = new Mock<IAzureSecretClientWrapper>();
            _mockSendGridServiceWrapper = new Mock<ISendGridServiceWrapper>();
            _mockLogger = new Mock<ILogger>();
            _mockConfigurationWrapper = new Mock<IConfigurationWrapper>();

            _function = new MailSenderFunction(_mockConfigurationWrapper.Object, _mockAzureSecretClientWrapper.Object, _mockSendGridServiceWrapper.Object);
        }

        [Fact]
        public async Task YourTestName_HappyPath()
        {
            // Arrange

            // Mock IConfigurationWrapper
            var mockConfigurationWrapper = new Mock<IConfigurationWrapper>();
            mockConfigurationWrapper.Setup(x => x["AzureKeyVaultConfig:KVSecretName"]).Returns("DummyKeyVaultSecretName");

            // Mock AzureSecretClientWrapper (assuming you have an ISecretClientWrapper interface for this)
            var mockAzureSecretClientWrapper = new Mock<IAzureSecretClientWrapper>();
            mockAzureSecretClientWrapper.Setup(x => x.GetSecretAsync("DummyKeyVaultSecretName")).ReturnsAsync("DummySendGridKey");

            // Mock SendGridServiceWrapper
            var mockSendGridServiceWrapper = new Mock<ISendGridServiceWrapper>();
            mockSendGridServiceWrapper.Setup(x => x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<EmailAddress>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                      .ReturnsAsync(new Response(HttpStatusCode.Accepted, null, null));

            // Mock Logger
            var mockLogger = new Mock<ILogger>();

            // Set up form data (assuming you have some kind of FormData object or similar structure)
            //var formData = new FormData
            //{
            //    // Your form properties here...
            //};

            var formData = formDataMock;

            // Instantiate your function with the mocked dependencies
            var function = new MailSenderFunction(mockConfigurationWrapper.Object, mockAzureSecretClientWrapper.Object, mockSendGridServiceWrapper.Object);

            // Act
            function.Run(formData, mockLogger.Object);

            // Assert

            // Assuming you want to check that an email was logged as sent successfully
            mockLogger.Verify(log => log.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<object>(state => state.ToString().Contains("Email sent successfully!")), null, It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task TestMethod1()
        {
            var mockSendGridServiceWrapper = new Mock<ISendGridServiceWrapper>();
            //mockSendGridServiceWrapper.Setup(x => x.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<EmailAddress>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            //                          .ReturnsAsync(new Response(HttpStatusCode.Accepted, null, null));
            var response = await mockSendGridServiceWrapper.Object.SendEmailAsync(null, null, null, null, null);
        }




        [Fact]
        public async Task TestEmailSendsSuccessfully()
        {
            // Arrange

            // Mock SendGrid client
            var mockSendGridClient = new Mock<ISendGridClient>();
            mockSendGridClient.Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response(HttpStatusCode.Accepted, null, null));

            // Setup SendGrid service wrapper
            var mockSendGridServiceWrapper = new SendGridServiceWrapper("YourSecretAPIKey", mockSendGridClient.Object);

            // Mock Logger
            var mockLogger = new Mock<ILogger>();

            // Mock Configuration wrapper
            var mockConfigurationWrapper = new Mock<IConfigurationWrapper>();
            mockConfigurationWrapper.Setup(x => x["AzureKeyVaultConfig:KVSecretName"]).Returns("YourKeyValue");

            // Mock Azure Secret client wrapper
            var mockAzureSecretClientWrapper = new Mock<IAzureSecretClientWrapper>();
            mockAzureSecretClientWrapper.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync("YourSecretAPIKey");

            // Create the function instance
            var function = new MailSenderFunction(mockConfigurationWrapper.Object, mockAzureSecretClientWrapper.Object, mockSendGridServiceWrapper);

            // Setup form data
            var formData = formDataMock;

            // Act

            await function.Run(formData, mockLogger.Object);

            // Assert

            mockLogger.Verify(log => log.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<object>(state => state.ToString().Contains("Email sent successfully!")), null, It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }





    }

}
