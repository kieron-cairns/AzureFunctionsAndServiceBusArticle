using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNMailSender.Interfaces
{
    public interface IJsonSerializerWrapper
    {
        string SerializeObject(object value);
        T DeserializeObject<T>(string value);
    }

}
