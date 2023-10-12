using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FNReCaptchaVerification.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using FNReCaptchaVerification.Utilities;

namespace FNReCaptchaVerification
{
    public class FNReCaptchaVerify
    {

        private readonly IConfiguration _config;
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IJsonConvertWrapper _jsonConvertWrapper;
        private readonly IStreamReaderWrapper _streamReaderWrapper;
        private readonly IHttpRequestWrapper _httpRequestWrapper;

        public FNReCaptchaVerify(IConfiguration config, IHttpClientWrapper httpClientWrapper, IJsonConvertWrapper jsonConvertWrapper, IStreamReaderWrapper streamReaderWrapper, IHttpRequestWrapper httpRequestWrapper)
        {
            _config = config;
            _httpClientWrapper = httpClientWrapper;
            _jsonConvertWrapper = jsonConvertWrapper;
            _streamReaderWrapper = streamReaderWrapper;
            _httpRequestWrapper = httpRequestWrapper;
        }

        [FunctionName("FNReCaptchaVerify")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] IHttpRequestWrapper req, [ServiceBus("sb-brightbyte-queue", Connection = "ServiceBusConnectionString")] string formData, IAsyncCollectorWrapper<string> collector,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var streamReaderWrapper = new StreamReaderWrapper(req.Body);
            string requestBody = await streamReaderWrapper.ReadToEndAsync(); dynamic data = _jsonConvertWrapper.DeserializeObject<dynamic>(requestBody);
            string captchaValue = data?.captchaValue;

            if (string.IsNullOrEmpty(captchaValue))
            {
                return new BadRequestObjectResult("Please pass a captchaValue in the request body");
            }

            var keyVaultSecretName = _config["AzureKeyVaultConfig:KVSecretName"];
            log.LogInformation($"KVSecretName: {keyVaultSecretName}");

            string recaptchaSecretKey = "ConfigHere";

            var verificationURL = $"https://www.google.com/recaptcha/api/siteverify?secret={recaptchaSecretKey}&response={captchaValue}";

            var verificationResponse = await _httpClientWrapper.PostAsync(verificationURL, null);
            var verificationContent = await verificationResponse.Content.ReadAsStringAsync();

            _httpRequestWrapper.AddResponseHeader("Access-Control-Allow-Origin", "*");
            _httpRequestWrapper.AddResponseHeader("Access-Control-Allow-Methods", "GET, POST");
            _httpRequestWrapper.AddResponseHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");

            log.LogInformation($"Verification Response: {verificationContent}");

            if (verificationContent.Contains("\"success\": true"))
            {
                try
                {
                    await collector.AddAsync(formData);
                    return new OkObjectResult(new { success = true, msg = "Captcha verification passed & added to service bus verification queue." });
                }
                catch (Exception ex)
                {
                    log.LogError($"Failed to add formData to the service bus. Error: {ex.Message}");
                    return new ObjectResult(new { success = false, msg = "Captcha verification passed, but an error occurred while adding to the service bus verification queue." }) { StatusCode = (int)HttpStatusCode.InternalServerError };
                }
            }
            else
            {
                return new UnauthorizedObjectResult(new { success = false, msg = "Captcha verification failed." });
            }
        }
    }
}
