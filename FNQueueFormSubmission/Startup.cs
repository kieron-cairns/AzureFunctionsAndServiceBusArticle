using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FNQueueFormSubmission.Interfaces.UtilityInterfaces;
using FNQueueFormSubmission.Utilities;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(FNQueueFormSubmission.Startup))]


namespace FNQueueFormSubmission
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);

            builder.Services.AddSingleton<IAzureKeyVaultWrapper>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                string keyVaultUrl = config[":"];
                string clientSecretId = config[":"];

                var clientSecretCredential = new DefaultAzureCredential();
                var secretClient = new SecretClient(new Uri(keyVaultUrl), clientSecretCredential);
                return new AzureKeyVaultWrapper(secretClient);
            });

            builder.Services.AddSingleton<IAzureSecretClientWrapper, AzureSecretClientWrapper>();
        }
    }
}
