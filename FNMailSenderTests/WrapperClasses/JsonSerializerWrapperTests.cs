using FNMailSender.Utilities;
using FNMailSenderTests.Model;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNMailSenderTests.WrapperClasses
{
    using Moq;
    using Xunit;
    using Newtonsoft.Json;
    using FNMailSender.Interfaces;

    public class JsonSerializerWrapperTests
    {
        [Fact]
        public void SerializeObject_GivenObject_ReturnsSerializedString()
        {
            // Arrange
            var mockSerializer = new Mock<IJsonSerializerWrapper>();
            var testObject = new { Name = "John", Age = 25 };
            var expectedSerialization = JsonConvert.SerializeObject(testObject);

            mockSerializer.Setup(s => s.SerializeObject(It.IsAny<object>())).Returns(expectedSerialization);

            // Act
            var serializedResult = mockSerializer.Object.SerializeObject(testObject);

            // Assert
            Assert.Equal(expectedSerialization, serializedResult);
        }

        [Fact]
        public void DeserializeObject_GivenSerializedString_ReturnsDeserializedObject()
        {
            // Arrange
            var mockSerializer = new Mock<IJsonSerializerWrapper>();
            var serializedString = "{\"Name\":\"John\",\"Age\":25}";
            var expectedDeserialization = JsonConvert.DeserializeObject<TestModel>(serializedString);

            mockSerializer.Setup(s => s.DeserializeObject<TestModel>(It.IsAny<string>())).Returns(expectedDeserialization);

            // Act
            var deserializedResult = mockSerializer.Object.DeserializeObject<TestModel>(serializedString);

            // Assert
            Assert.Equal("John", deserializedResult.Name);
            Assert.Equal(25, deserializedResult.Age);
        }
    }

}
