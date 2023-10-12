using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNMailSender.Interfaces
{
    public interface ISendGridServiceWrapper
    {
        Task<Response> SendEmailAsync(EmailAddress from, EmailAddress to, string subject, string plainTextContent, string htmlContent);
    }

}
