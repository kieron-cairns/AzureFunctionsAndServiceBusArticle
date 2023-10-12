﻿using System;
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
        public async Task TestSendGridSuccessfullEmailSend()
        {
            // Arrange

            // Mock SendGrid client
            var mockSendGridClient = new Mock<ISendGridClient>();

            // Mock SendGrid service wrapper
            var mockSendGridServiceWrapper = new Mock<ISendGridServiceWrapper>();
            mockSendGridServiceWrapper.Setup(wrapper => wrapper.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<EmailAddress>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Response(HttpStatusCode.Accepted, null, null));

            // Mock Logger
            var mockLogger = new Mock<ILogger>();

            // Mock Configuration wrapper
            var mockConfigurationWrapper = new Mock<IConfigurationWrapper>();
            mockConfigurationWrapper.Setup(x => x["AzureKeyVaultConfig:KVSecretName"]).Returns("YourKeyValue");

            // Mock Azure Secret client wrapper
            var mockAzureSecretClientWrapper = new Mock<IAzureSecretClientWrapper>();
            mockAzureSecretClientWrapper.Setup(x => x.GetSecretAsync(It.IsAny<string>())).ReturnsAsync("YourSecretAPIKey");

            // Create the function instance
            var function = new MailSenderFunction(mockConfigurationWrapper.Object, mockAzureSecretClientWrapper.Object, mockSendGridServiceWrapper.Object);

            // Setup form data
            var formData = formDataMock;

            // Act
            await function.Run(formData, mockLogger.Object);

            // Assert
            mockSendGridServiceWrapper.Verify(wrapper => wrapper.SendEmailAsync(It.IsAny<EmailAddress>(), It.IsAny<EmailAddress>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task TestEmailSendFails()
        {
            // Arrange
            Response capturedResponse = null;


            // Mock SendGrid client
            // Mock SendGrid client
            var mockSendGridClient = new Mock<ISendGridClient>();
            mockSendGridClient.Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response(HttpStatusCode.BadRequest, null, null)) // Simulate an error scenario
                .Callback<SendGridMessage, CancellationToken>((message, token) => capturedResponse = new Response(HttpStatusCode.BadRequest, null, null));

            // Setup SendGrid service wrapper
            var mockSendGridServiceWrapper = new SendGridServiceWrapper("YourSecretAPIKey", mockSendGridClient.Object);

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
            await function.Run(formData, Mock.Of<ILogger>());  // Using Mock.Of<ILogger>() to bypass logger interactions

            // Assert
            mockSendGridClient.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Once());

            // Assert that the captured response has a BadRequest status
            Assert.NotNull(capturedResponse);
            Assert.Equal(HttpStatusCode.BadRequest, capturedResponse.StatusCode);
        }
    }

}