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
        private readonly ICaptchaVerificationService _captchaVerificationService;

        public FNReCaptchaVerify(IConfiguration config, IHttpClientWrapper httpClientWrapper, IJsonConvertWrapper jsonConvertWrapper, IStreamReaderWrapper streamReaderWrapper, IHttpRequestWrapper httpRequestWrapper, ICaptchaVerificationService captchaVerificationService)
        {
            _config = config;
            _httpClientWrapper = httpClientWrapper;
            _jsonConvertWrapper = jsonConvertWrapper;
            _streamReaderWrapper = streamReaderWrapper;
            _httpRequestWrapper = httpRequestWrapper;
            _captchaVerificationService = captchaVerificationService;   
        }

        [FunctionName("FNReCaptchaVerify")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] IHttpRequestWrapper req,
          [ServiceBus("mail-queue", Connection = "ServiceBusConnectionString")] string formData,
          IAsyncCollectorWrapper<string> collector,
          ILogger log)
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                // Read the request body
                var streamReaderWrapper = new StreamReaderWrapper(req.Body);
                string requestBody = await streamReaderWrapper.ReadToEndAsync();
                dynamic data = _jsonConvertWrapper.DeserializeObject<dynamic>(requestBody);
                string captchaValue = data?.captchaValue;

                if (string.IsNullOrEmpty(captchaValue))
                {
                    return new BadRequestObjectResult("Please pass a captchaValue in the request body");
                }

                var keyVaultSecretName = _config["AzureKeyVaultConfig:KVSecretName"];
                log.LogInformation($"KVSecretName: {keyVaultSecretName}");

                string recaptchaSecretKey = "ConfigHere";  // This should probably be fetched securely, not hardcoded

                // Utilizing the ICaptchaVerificationService to verify captcha
                var isCaptchaVerified = await _captchaVerificationService.VerifyCaptchaAsync(captchaValue);

                _httpRequestWrapper.AddResponseHeader("Access-Control-Allow-Origin", "*");
                _httpRequestWrapper.AddResponseHeader("Access-Control-Allow-Methods", "GET, POST");
                _httpRequestWrapper.AddResponseHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");

                if (isCaptchaVerified)
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
