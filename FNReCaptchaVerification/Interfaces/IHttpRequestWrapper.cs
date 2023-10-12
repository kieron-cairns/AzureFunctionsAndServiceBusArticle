using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Interfaces
{
    public interface IHttpRequestWrapper
    {
        Stream Body { get; }
        HttpResponse HttpResponse { get; }

        void AddResponseHeader(string key, string value);
    }
}
