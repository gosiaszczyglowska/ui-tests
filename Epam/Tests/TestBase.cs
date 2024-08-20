using System;
using System.IO;
using NUnit.Framework;
using log4net;
using log4net.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Configuration;
using OpenQA.Selenium.DevTools.V124.Memory;
using PageObject.Pages.Pages;
using PageObject.Pages.Scripts;
using PageObject.Core;
using PageObject.Utilities;
using Microsoft.Extensions.Configuration;


namespace PageObject.Tests
{
    public class TestBase  
    {
        protected IWebDriver driver;
        protected IndexPage indexPage;
        protected Waits waits;
        protected Navigation navigation;
        protected Careers careers;
        protected Insights insights;
        protected About about;
        protected Scripts scripts;
        protected Actions actions;


        public static AppSettings AppSettings { get; private set; }

        [OneTimeSetUp] 
        public void BeforeAllTests()
        {
            ConfigureLogging();
            LoadConfiguration();
        }
        
        private void ConfigureLogging()
        {
            XmlConfigurator.Configure(new FileInfo("Log.config"));
            Console.WriteLine($"Logs will be stored in: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")}");
        }

        private void LoadConfiguration() 
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            AppSettings = config.GetSection("AppSettings").Get<AppSettings>();
        }

        [SetUp]
        public void SetUp() //TODO: move driver creation to a BrowserFactory class
                            //and inherit Pages from BrowserFactory class instead of TestBase class
                            //example https://toolsqa.com/selenium-webdriver/c-sharp/browser-factory/
        {
            InitializeWebDriver();
            InitializePageObjects();
            PrepareTestEnvironment();
        }

        private void InitializeWebDriver() 
        { 
            string browserType = Environment.GetEnvironmentVariable("BROWSER_TYPE") ?? "chrome";
            bool headless = Environment.GetEnvironmentVariable("HEADLESS_MODE") == "true";
            string downloadDirectory = AppSettings.DownloadDirectory;

            driver = BrowserFactory.GetDriver(browserType, downloadDirectory, headless);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();
        }

        private void InitializePageObjects() 
        {
            indexPage = new IndexPage(driver);
            waits = new Waits(driver);
            navigation = new Navigation(driver);
            careers = new Careers(driver);
            insights = new Insights(driver);
            about = new About(driver);
            scripts = new Scripts(driver);
            actions = new Actions(driver);
        }

        private void PrepareTestEnvironment()
        {
            indexPage.Open();
            indexPage.AcceptCookies();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Utilities.Screenshot.TakeScreenshot(driver, TestContext.CurrentContext.Test.Name);
            }
            driver.Close();
            driver.Quit();

        }
    }
}