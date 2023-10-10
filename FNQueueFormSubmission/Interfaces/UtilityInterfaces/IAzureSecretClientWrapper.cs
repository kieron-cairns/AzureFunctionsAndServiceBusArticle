using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNQueueFormSubmission.Interfaces.UtilityInterfaces
{
    public interface IAzureSecretClientWrapper
    {
        Task<string> GetSecretAsync(string secretName);
    }
}
