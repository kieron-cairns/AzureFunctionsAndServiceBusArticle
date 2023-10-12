using FNReCaptchaVerification.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Utilities
{
    public class StreamReaderWrapper : IStreamReaderWrapper
    {
        private readonly StreamReader _streamReader;

        public StreamReaderWrapper(Stream stream)
        {
            _streamReader = new StreamReader(stream);
        }

        public async Task<string> ReadToEndAsync()
        {
            return await _streamReader.ReadToEndAsync();
        }
    }
}
