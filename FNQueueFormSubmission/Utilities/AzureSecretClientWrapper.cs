using FNQueueFormSubmission.Interfaces.UtilityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNQueueFormSubmission.Utilities
{
    public class AzureSecretClientWrapper : IAzureSecretClientWrapper
    {
        private readonly IAzureKeyVaultWrapper _azureKeyVaultWrapper;

        public AzureSecretClientWrapper(IAzureKeyVaultWrapper azureKeyVaultWrapper)
        {
            _azureKeyVaultWrapper = azureKeyVaultWrapper;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            string secret = await _azureKeyVaultWrapper.GetSecretAsync(secretName);
            return secret;
        }
    }
}
