using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
        Task<string> GetStringAsync(string requestUri);
    }
}
