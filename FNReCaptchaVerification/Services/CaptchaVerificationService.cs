using FNReCaptchaVerification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Services
{
    public class CaptchaVerificationService : ICaptchaVerificationService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly string _recaptchaSecretKey;

        public CaptchaVerificationService(IHttpClientWrapper httpClientWrapper, string recaptchaSecretKey)
        {
            _httpClientWrapper = httpClientWrapper;
            _recaptchaSecretKey = recaptchaSecretKey;
        }

        public async Task<bool> VerifyCaptchaAsync(string captchaValue)
        {
            var verificationURL = $"https://www.google.com/recaptcha/api/siteverify?secret={_recaptchaSecretKey}&response={captchaValue}";
            var verificationResponse = await _httpClientWrapper.PostAsync(verificationURL, null);
            var verificationContent = await verificationResponse.Content.ReadAsStringAsync();

            return verificationContent.Contains("\"success\": true");
        }
    }
}
