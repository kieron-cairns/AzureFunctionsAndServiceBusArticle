using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FNQueueFormSubmission
{
    public class FNQueueFormSubmission
    {
        [FunctionName("FNQueueFormSubmission")]
        public async Task<IActionResult> Run(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
         [ServiceBus("form-submissions", Connection = "ServiceBusConnectionString")] IAsyncCollector<string> collector,
         ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            await collector.AddAsync(requestBody); // Queue the data to Service Bus
            return new OkObjectResult("Form submission received and is being processed.");
        }

    }
}
