using FNMailSender.Utilities;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net;
using Moq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using FNMailSenderTests.Model;
using Microsoft.Extensions.Configuration;

namespace FNMailSenderTests
{
    public class FunctionUnitTests
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

        [Fact]
        public void SerializeObject_GivenObject_ReturnsSerializedString()
        {
            // Arrange
            var serializer = new JsonSerializerWrapper();
            var testObject = new { Name = "John", Age = 25 };

            // Act
            var serializedResult = serializer.SerializeObject(testObject);

            // Assert
            var expectedSerialization = JsonConvert.SerializeObject(testObject);
            Assert.Equal(expectedSerialization, serializedResult);
        }

        [Fact]
        public void DeserializeObject_GivenSerializedString_ReturnsDeserializedObject()
        {
            // Arrange
            var serializer = new JsonSerializerWrapper();
            var serializedString = "{\"Name\":\"John\",\"Age\":25}";

            // Act
            var deserializedResult = serializer.DeserializeObject<TestModel>(serializedString);

            // Assert
            Assert.Equal("John", deserializedResult.Name);
            Assert.Equal(25, deserializedResult.Age);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var testKey = "TestKey";
            var testValue = "TestValue";

            mockConfiguration.Setup(c => c[It.Is<string>(s => s == testKey)]).Returns(testValue);
            var configWrapper = new ConfigurationWrapper(mockConfiguration.Object);

            // Act
            var result = configWrapper.GetValue(testKey);

            // Assert
            Assert.Equal(testValue, result);
        }

    }
}