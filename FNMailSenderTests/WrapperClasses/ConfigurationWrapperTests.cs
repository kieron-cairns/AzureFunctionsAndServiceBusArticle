using FNMailSender.Utilities;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNMailSenderTests.WrapperClasses
{
    public class ConfigurationWrapperTests
    {
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
