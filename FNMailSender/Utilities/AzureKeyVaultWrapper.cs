using Azure.Security.KeyVault.Secrets;
using FNMailSender.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNMailSender.Utilities
{
    public class AzureKeyVaultWrapper : IAzureKeyVaultWrapper
    {
        private readonly SecretClient _secretClient;

        public AzureKeyVaultWrapper(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);

            return secret.Value;
        }
    }
}
