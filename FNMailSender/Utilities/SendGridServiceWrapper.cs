using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNMailSender.Interfaces;

namespace FNMailSender.Utilities
{
    public class SendGridServiceWrapper : ISendGridServiceWrapper
    {
        private readonly ISendGridClient _client;

        public SendGridServiceWrapper(string apiKey, ISendGridClient client = null)
        {
            _client = client ?? new SendGridClient(apiKey);
        }

        public async Task<Response> SendEmailAsync(EmailAddress from, EmailAddress to, string subject, string plainTextContent, string htmlContent)
        {
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return await _client.SendEmailAsync(msg);
        }
    }

}
