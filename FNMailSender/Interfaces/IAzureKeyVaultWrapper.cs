﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNMailSender.Interfaces
{
    public interface IAzureKeyVaultWrapper
    {
        Task<string> GetSecretAsync(string secretName);
    }
}