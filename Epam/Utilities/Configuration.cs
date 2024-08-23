using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.DevTools.V124.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObject.Utilities
{
    public class Configuration
    {
        private static Configuration instance;
        private static readonly object instanceLock = new object();
        private readonly AppSettings appSettings;

        private Configuration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            appSettings = config.GetSection("AppSettings").Get<AppSettings>();
        }

        public static Configuration Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        instance = new Configuration();
                    }
                }
                return instance;
            }
        }

        public AppSettings GetAppSettings()
        { 
        return appSettings;
        }

    }
}
