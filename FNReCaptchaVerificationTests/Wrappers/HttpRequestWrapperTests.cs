using FNReCaptchaVerification.Utilities;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerificationTests.Wrappers
{
    public class HttpRequestWrapperTests
    {
        [Fact]
        public void Body_ReturnsUnderlyingBodyStream()
        {
            // Arrange
            var mockRequest = new Mock<HttpRequest>();
            var sampleStream = new MemoryStream(Encoding.UTF8.GetBytes("Sample Stream Content"));
            mockRequest.Setup(r => r.Body).Returns(sampleStream);

            var requestWrapper = new HttpRequestWrapper(mockRequest.Object);

            // Act
            var resultStream = requestWrapper.Body;

            // Assert
            Assert.Equal(sampleStream, resultStream);
        }

        [Fact]
        public void HttpResponse_ReturnsUnderlyingResponse()
        {
            // Arrange
            var mockRequest = new Mock<HttpRequest>();
            var mockResponse = new Mock<HttpResponse>();
            mockRequest.Setup(r => r.HttpContext.Response).Returns(mockResponse.Object);

            var requestWrapper = new HttpRequestWrapper(mockRequest.Object);

            // Act
            var resultResponse = requestWrapper.HttpResponse;

            // Assert
            Assert.Equal(mockResponse.Object, resultResponse);
        }
    }
}
