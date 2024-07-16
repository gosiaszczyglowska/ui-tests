using System;
using System.IO;
using NUnit.Framework;
using log4net.Config;
using System.Configuration;

namespace PageObject
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            XmlConfigurator.Configure(new FileInfo("Log.config"));
            Console.WriteLine($"Logs will be stored in: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")}");
        }
    }
}
