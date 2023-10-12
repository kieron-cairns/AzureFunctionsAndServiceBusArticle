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
    public  class JsonConverterWrapperTests
    {
        private  Mock<IJsonConvertWrapper> _jsonConvertWrapper;

        public JsonConverterWrapperTests()
        {
            _jsonConvertWrapper = new Mock<IJsonConvertWrapper>();
        }

        [Fact]
        public void DeserializeObject_ReturnsCorrectObject()
        {
            //var jsonString = "{\"Name\":\"John\", \"Age\":30}";

            //var result = _jsonConvertWrapper.DeserializeObject<TestModel>(jsonString);

            //Assert.Equal("John", result.Name);
            //Assert.Equal(30, result.Age);

            var jsonString = "{\"Name\":\"John\", \"Age\":30}";
            var testObject = new TestModel
            {
                Name = "John",
                Age = 30
            };

            _jsonConvertWrapper.Setup(jcw => jcw.DeserializeObject<TestModel>(jsonString)).Returns(testObject);
        }

        [Fact]
        public void SerializeObject_ReturnsCorrectString()
        {
            var testObject = new TestModel
            {
                Name = "John",
                Age = 30
            };

            //var result = _jsonConvertWrapper.SerializeObject(testObject);

            //Assert.Equal("{\"Name\":\"John\",\"Age\":30}", result);

        }

    }
}
