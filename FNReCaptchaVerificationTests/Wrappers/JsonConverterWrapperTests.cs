using FNReCaptchaVerification.Interfaces;
using FNReCaptchaVerification.Utilities;
using FNReCaptchaVerificationTests.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerificationTests.Wrappers
{
    using Moq;
    using Xunit;

    public class JsonConvertWrapperTests
    {
        [Fact]
        public void SerializeObject_GivenObject_ReturnsExpectedSerializedString()
        {
            // Arrange
            var mockConverter = new Mock<IJsonConverter>();
            var testObject = new { Name = "John", Age = 25 };
            var expectedSerialization = "{\"Name\":\"John\",\"Age\":25}";

            mockConverter.Setup(s => s.SerializeObject(It.IsAny<object>())).Returns(expectedSerialization);

            var wrapper = new JsonConvertWrapper(mockConverter.Object);

            // Act
            var serializedResult = wrapper.SerializeObject(testObject);

            // Assert
            Assert.Equal(expectedSerialization, serializedResult);
        }

        [Fact]
        public void DeserializeObject_GivenSerializedString_ReturnsExpectedDeserializedObject()
        {
            // Arrange
            var mockConverter = new Mock<IJsonConverter>();
            var serializedString = "{\"Name\":\"John\",\"Age\":25}";
            var expectedDeserialization = new TestModel { Name = "John", Age = 25 };

            mockConverter.Setup(s => s.DeserializeObject<TestModel>(It.IsAny<string>())).Returns(expectedDeserialization);

            var wrapper = new JsonConvertWrapper(mockConverter.Object);

            // Act
            var deserializedResult = wrapper.DeserializeObject<TestModel>(serializedString);

            // Assert
            Assert.Equal(expectedDeserialization.Name, deserializedResult.Name);
            Assert.Equal(expectedDeserialization.Age, deserializedResult.Age);
        }
    }

}
