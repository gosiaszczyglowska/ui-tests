using System;
using System.IO;
using NUnit.Framework;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace PageObject
{
    [SetUpFixture]
    public class SetUpFixture
    {
        public static AppSettings AppSettings { get; private set; }

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            // Configure log4net
            XmlConfigurator.Configure(new FileInfo("Log.config"));
            Console.WriteLine($"Logs will be stored in: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")}");

            // Load appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            AppSettings = config.GetSection("AppSettings").Get<AppSettings>();
        }
    }
}
