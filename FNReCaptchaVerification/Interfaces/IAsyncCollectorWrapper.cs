using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FNReCaptchaVerification.Interfaces
{
    public interface IAsyncCollectorWrapper<T>
    {
        Task AddAsync(T item);
        Task FlushAsync(CancellationToken cancellationToken = default);
    }
}
