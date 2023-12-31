﻿using FNMailSender.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNMailSender.Utilities
{
    public class ConfigurationWrapper : IConfigurationWrapper
    {
        private readonly IConfiguration _configuration;

        public ConfigurationWrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetValue(string key)
        {
            return _configuration[key];
        }

        // Implement other necessary methods from IConfiguration as required
    }
}
