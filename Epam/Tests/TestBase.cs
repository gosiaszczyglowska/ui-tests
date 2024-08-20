using log4net.Config;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using PageObject.Core;
using PageObject.Pages.Pages;
using PageObject.Pages.Scripts;
using PageObject.Utilities;
using System.IO;
using System;
using PageObject.Core;

namespace PageObject.Tests
{
    public class TestBase
    {
        protected BrowserFactory browserFactory;
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
            browserFactory.ConfigureLogging();
            browserFactory.LoadConfiguration();
        }

        [SetUp]
        public void SetUp()
        {
            browserFactory = new BrowserFactory(AppSettings.DownloadDirectory);
            driver = browserFactory.InitializeWebDriver(AppSettings.DownloadDirectory);
            InitializePageObjects();
            PrepareTestEnvironment();
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

            browserFactory.CloseAndQuit();
        }
    }
}
