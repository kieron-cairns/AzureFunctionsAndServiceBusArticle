using FNReCaptchaVerification.Interfaces;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Utilities
{
    public class AsyncCollectorWrapper<T> : IAsyncCollectorWrapper<T>
    {
        private readonly IAsyncCollector<T> _collector;

        public AsyncCollectorWrapper(IAsyncCollector<T> collector)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
        }

        public async Task AddAsync(T item)
        {
            await _collector.AddAsync(item);
        }

        public async Task FlushAsync(CancellationToken cancellationToken = default)
        {
            await _collector.FlushAsync(cancellationToken);
        }
    }
}
