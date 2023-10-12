using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FNMailSender.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net;
using FNMailSender.Utilities;

namespace FNMailSender
{
    public class MailSenderFunction
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IAzureSecretClientWrapper _azureSecretClientWrapper;
        private readonly ISendGridServiceWrapper _sendGridServiceWrapper;

        public MailSenderFunction(IConfigurationWrapper configurationWrapper, IAzureSecretClientWrapper azureSecretClientWrapper, ISendGridServiceWrapper sendGridServiceWrapper)
        {
            _configurationWrapper = configurationWrapper;
            _azureSecretClientWrapper = azureSecretClientWrapper;
            _sendGridServiceWrapper = sendGridServiceWrapper;
        }

        [FunctionName("FNMailSender")]
        public async Task Run(
        [ServiceBusTrigger("mail-queue", Connection = "ServiceBusConnectionString")] string formData, ILogger log)
        {
            log.LogInformation("Starting SendEmail function...");

            log.LogInformation($"Received message: {formData}");

            log.LogInformation("Starting SendGird Email Send Process...");

            try
            {
                string accountEmail = "hello@test.co.uk";
                dynamic data = JsonConvert.DeserializeObject(formData);

                string senderName = data?.name;
                string senderEmail = data?.email;
                string senderPhone = data?.phone;
                string message = data?.message;

                // Logging deserialized data
                log.LogInformation($"Received data - Name: {senderName}, Email: {senderEmail}");

                var keyVaultSecretName = _configurationWrapper["AzureKeyVaultConfig:KVSecretName"];
                log.LogInformation($"Attempting to retrieve SendGrid API key using KVSecretName: {keyVaultSecretName}");

                string sendGridKey = await _azureSecretClientWrapper.GetSecretAsync(keyVaultSecretName);
                log.LogInformation("SendGrid API key retrieved successfully.");
                log.LogInformation(sendGridKey);
                log.LogInformation(keyVaultSecretName);


                var from = new EmailAddress(accountEmail, senderName);
                var subject = "Contact Form Submission";
                var to = new EmailAddress(accountEmail, "Test Name");
                var plainTextContent = message;

                var htmlContent = $"<p>{message}</p><h2>Contact Info: </h2><storng>Name: {senderName}</strong><storng>Email: {senderEmail}</strong><storng>Phone: {senderPhone}</strong>";

                log.LogInformation($"Preparing to send email from {senderEmail} to hello@test.co.uk...");

                try
                {
                    log.LogInformation("Before sending email.");

                    var response = await _sendGridServiceWrapper.SendEmailAsync(from, to, subject, plainTextContent, htmlContent);
                    
                    log.LogInformation($"Response is : {response.StatusCode}");
                    if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
                    {
                        log.LogInformation("Email sent successfully!");
                    }
                    else
                    {
                        log.LogError($"Failed to send email. StatusCode: {response.StatusCode}, Body: {response.Body.ReadAsStringAsync().Result}");
                    }
                }
                catch (Exception ex)
                {
                    log.LogInformation("Send Grid Send Email Async Method Error!", ex);
                }
            }
            catch (Exception ex)
            {
                log.LogError($"An unexpected error occurred: {ex.Message}");
            }

        }
    }
}
