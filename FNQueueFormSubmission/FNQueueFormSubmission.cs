using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FNQueueFormSubmission.Interfaces.UtilityInterfaces;
using Microsoft.Extensions.Configuration;

namespace FNQueueFormSubmission
{
    public class FNQueueFormSubmission
    {

        private readonly IConfiguration _config;
        private readonly IAzureSecretClientWrapper _azureSecretClientWrapper;

        public  FNQueueFormSubmission(IConfiguration config, IAzureSecretClientWrapper azureSecretClientWrapper)
        {
            _config = config;
            _azureSecretClientWrapper = azureSecretClientWrapper;
        }

        [FunctionName("FNQueueFormSubmission")]
        public async Task<IActionResult> Run(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
         [ServiceBus("mail-queue", Connection = "ServiceBusConnectionString")] IAsyncCollector<string> collector,
         ILogger log)
        {

            req.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            req.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST");
            req.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                await collector.AddAsync(requestBody); // Queue the data to Service Bus
                return new OkObjectResult("Form submission received and is being processed.");
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logging mechanism
                log.LogError(ex, "An error occurred while adding the message to the queue.");

                // Return a user-friendly error message
                return new ObjectResult(new
                {
                    status = 500,
                    error = ex
                })
                {
                    StatusCode = 500
                };
            }
        }

        private async Task<string> GetKeyVaultSecret(string keyVaultSecretName)
        {
            var secret = await _azureSecretClientWrapper.GetSecretAsync(keyVaultSecretName);

            return secret;
        }
    }
}
