using FNReCaptchaVerification.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Utilities
{
    public class HttpRequestWrapper : IHttpRequestWrapper
    {
        private readonly HttpRequest _request;

        public HttpRequestWrapper(HttpRequest request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public Stream Body => _request.Body;

        public HttpResponse HttpResponse => _request.HttpContext.Response;

        public void AddResponseHeader(string key, string value)
        {
            _request.HttpContext.Response.Headers.Add(key, value);
        }
    }
}
