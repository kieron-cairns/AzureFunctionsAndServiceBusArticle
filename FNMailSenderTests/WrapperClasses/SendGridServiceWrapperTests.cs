using FNMailSender.Utilities;
using Moq;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FNMailSenderTests.WrapperClasses
{
    public class SendGridServiceWrapperTests
    {
        [Fact]
        public async void SendEmailAsync_ValidData_ShouldSendEmailSuccessfully()
        {
            // Arrange
            var mockResponse = new Mock<Response>(HttpStatusCode.Accepted, null, null);

            var mockSendGridClient = new Mock<ISendGridClient>();
            mockSendGridClient.Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(mockResponse.Object);

            var apiKey = "test_api_key";
            var wrapper = new SendGridServiceWrapper(apiKey, mockSendGridClient.Object);

            var from = new EmailAddress("from@example.com", "Sender");
            var to = new EmailAddress("to@example.com", "Recipient");
            string subject = "Test Subject";
            string plainTextContent = "Plain Text";
            string htmlContent = "<strong>HTML Content</strong>";

            // Act
            var result = await wrapper.SendEmailAsync(from, to, subject, plainTextContent, htmlContent);

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
            mockSendGridClient.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
