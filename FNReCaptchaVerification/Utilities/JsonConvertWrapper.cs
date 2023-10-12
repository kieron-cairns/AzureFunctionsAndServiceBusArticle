using FNReCaptchaVerification.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Utilities
{
    public class JsonConvertWrapper : IJsonConvertWrapper
    {
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
