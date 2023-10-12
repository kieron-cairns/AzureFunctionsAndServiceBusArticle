using FNReCaptchaVerification.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Utilities
{
    public class JsonConvertWrapper : IJsonConvertWrapper
    {
        private readonly IJsonConverter _converter;

        public JsonConvertWrapper(IJsonConverter converter)
        {
            _converter = converter;
        }

        public T DeserializeObject<T>(string value)
        {
            return _converter.DeserializeObject<T>(value);
        }

        public string SerializeObject(object obj)
        {
            return _converter.SerializeObject(obj);
        }
    }

}
