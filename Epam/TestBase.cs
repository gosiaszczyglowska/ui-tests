using System;
using System.IO;
using NUnit.Framework;
using log4net;
using log4net.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PageObject.Pages;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Configuration;


namespace PageObject
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
        //public string downloadDirectory;


        public ILog Log
        {
            get { return LogManager.GetLogger(this.GetType()); }
        }

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
        XmlConfigurator.Configure(new FileInfo("Log.config"));
        Console.WriteLine($"Logs will be stored in: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")}");
        }

        [SetUp]
        public void SetUp()
        {
            string browserType = Environment.GetEnvironmentVariable("BROWSER_TYPE") ?? "chrome";
            bool headless = Environment.GetEnvironmentVariable("HEADLESS_MODE") == "true";
            string downloadDirectory = SetUpFixture.AppSettings.DownloadDirectory;

            driver = BrowserFactory.GetDriver(browserType, downloadDirectory, headless);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();

            indexPage = new IndexPage(driver);
            waits = new Waits(driver);
            navigation = new Navigation(driver);
            careers = new Careers(driver);
            insights = new Insights(driver);
            about = new About(driver);
            scripts = new Scripts(driver);
            actions = new Actions(driver);

            indexPage.Open();
            indexPage.AcceptCookies();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Screenshot.TakeScreenshot(driver, TestContext.CurrentContext.Test.Name);
            }
            driver.Quit();
        }
    }
}