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
    public class JsonSerializerWrapperTests
    {
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
    }
}
