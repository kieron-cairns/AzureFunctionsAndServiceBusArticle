using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Interfaces
{
    public interface IJsonConvertWrapper
    {
        T DeserializeObject<T>(string value);
        string SerializeObject(object obj);
    }
}
