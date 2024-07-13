using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PageObject.Pages;

namespace PageObject
{
    [TestFixture]
    public class HomeworkTests
    {
        private IWebDriver driver;
        private IndexPage indexPage;
        private Waits waits;
        private Navigation navigation;
        private Careers careers;
        private Insights insights;
        private About about;

        [SetUp]
        public void SetUp()
        {
            string downloadDirectory = @"C:\TestDownload";
            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", downloadDirectory);
            options.AddUserProfilePreference("download.prompt_for_download", false);

            bool headless = Environment.GetEnvironmentVariable("HEADLESS_MODE") == "true";
            if (headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");
            }

            driver = new ChromeDriver(options);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();

            indexPage = new IndexPage(driver);           
            waits = new Waits(driver);
            navigation = new Navigation(driver);
            careers = new Careers(driver);
            insights = new Insights(driver);
            about = new About(driver);

            indexPage.Open();
            indexPage.AcceptCookies();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [TestCase("Java", "All Locations")]
        [TestCase("Python", "All Cities in Poland")]
        public void PositionSearch(string language, string location)
        {
            navigation.CareersTab();
            careers.SearchPosition(language, location);
            careers.SortPositionsByDate();
            careers.ApplyForFirstPosition();

            waits.Wait(5);

            bool isTextPresent = driver.FindElements(By.XPath($"//*[contains(text(), {language})]")).Count > 0;
            Assert.IsTrue(isTextPresent, $"Text {language} not found on the page.");
        }

        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void GlobalSearch(string query)
        {
            indexPage.Search(query);
            indexPage.ValidateSearchResults(query);
        }

        [TestCase("EPAM_Corporate_Overview_Q4_EOY.pdf", @"C:\TestDownload")]
        public void DownloadCheck(string downloadedFileName, string downloadDirectory)
        {
            navigation.AboutTab();
            about.DownloadOverviewFile(downloadedFileName, downloadDirectory);
            
            string downloadedFilePath = Path.Combine(downloadDirectory, downloadedFileName);
            
            Assert.IsTrue(File.Exists(downloadedFilePath));
            
            about.CheckIfFileExistsAndDelete(downloadedFilePath);
        }

        [Test]
        public void TitleCheck()
        {
            navigation.InsightsTab();
            insights.SwipeToTheThirdSlide();
            insights.ReadArticleAndCompareTitles();
        }
    }
}