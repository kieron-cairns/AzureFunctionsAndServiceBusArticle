using FNReCaptchaVerification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Utilities
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        // Default constructor
        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }

        // Constructor for testing, allowing a mocked HttpClient to be passed in
        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _httpClient.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return await _httpClient.PostAsync(requestUri, content);
        }

        public async Task<string> GetStringAsync(string requestUri)
        {
            return await _httpClient.GetStringAsync(requestUri);
        }
    }
}
