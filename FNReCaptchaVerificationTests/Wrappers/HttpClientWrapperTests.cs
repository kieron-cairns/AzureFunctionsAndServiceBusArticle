using FNReCaptchaVerification.Utilities;
using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerificationTests.Wrappers
{
    public class HttpClientWrapperTests
    {
        [Fact]
        public async Task PostAsync_ReturnsExpectedResponse()
        {
            var testResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"status\":\"success\"}")
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(testResponseMessage);

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientWrapper = new HttpClientWrapper(httpClient);

            var response = await httpClientWrapper.PostAsync("https://test.com", null);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("{\"status\":\"success\"}", content);
        }

    }
}
