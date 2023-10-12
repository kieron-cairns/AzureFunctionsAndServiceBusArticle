using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Interfaces
{
    public interface ICaptchaVerificationService
    {
        Task<bool> VerifyCaptchaAsync(string captchaValue);
    }
}
