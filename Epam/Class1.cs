using System.Linq;
using System;
using System.Reflection.PortableExecutable;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Threading;


namespace EpamTests
{
    [TestFixture]
    public class GlobalSearchTests
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {

            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }


        [TestCase("Java", "All Locations")]
        public void PositionSearch(string language, string location)
        {
            driver.Navigate().GoToUrl(@"https://www.epam.com/");

            // Accept cookies if present
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement cookies = wait.Until(driver =>
            {
                var element = driver.FindElement(By.Id("onetrust-accept-btn-handler"));
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            });
            cookies.Click();

            // Click on Careers tab
            var careersTab = driver.FindElement(By.LinkText("Careers"));
            careersTab.Click();

            //Searching and actions on Keyword field
            IWebElement searchPanel = driver.FindElement(By.Id("jobSearchFilterForm"));
            IWebElement keyword = searchPanel.FindElement(By.Id("new_form_job_search-keyword"));

            keyword.Click();
            keyword.SendKeys(language);

            ///click on the dropdown input, clicking left control to move the dropdown up
            IWebElement dropdown = searchPanel.FindElement(By.ClassName("select2-selection__rendered"));
            dropdown.Click();

            new Actions(driver)
                .SendKeys(Keys.LeftControl)
                .Perform();

            //searching for chosen location and clicking on it
            var dropdownOption = driver.FindElement(By.XPath($"//li[contains(text(), '{location}')]"));
            dropdownOption.Click();

            //searching for "Remote" checkbox and clicking on it
            IWebElement remoteCheckbox = searchPanel.FindElement(By.ClassName("recruiting-search__filter-label-23"));
            remoteCheckbox.Click();

            //Clicking on "Find" button
            IWebElement findButton = searchPanel.FindElement(By.XPath("//*[@type = 'submit']"));
            findButton.Click();

            //Waiting for the page loading
            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sorting results by Date
            IWebElement searchHeader = driver.FindElement(By.ClassName("search-result__header"));
            IWebElement sortByDate = searchHeader.FindElement(By.XPath("//li[@title='Date']"));
            sortByDate.Click();

            //Waiting until sorting is done
            Thread.Sleep(8000);
            WebDriverWait wait5 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Clicking on the 1st search result's "View and apply" button
            IWebElement resulItem1 = driver.FindElement(By.XPath("//*[@id='main']//ul/li[1]"));
            IWebElement viewAndApplyButton = resulItem1.FindElement(By.XPath("//a[contains(text(), 'View and apply')]"));
            viewAndApplyButton.Click();

            //waiting for the page to load
            WebDriverWait wait3 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            //checking if searched language is on the page
            bool isTextPresent = driver.FindElements(By.XPath($"//*[contains(text(), {language})]")).Count > 0;
            Assert.IsTrue(isTextPresent, $"Text {language} not found on the page.");

            //time to verify that correct page was opened
            Thread.Sleep(8000);
        }

        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void GlobalSearch(string query)
        {
            driver.Navigate().GoToUrl(@"https://www.epam.com/");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Accepting cookies
            IWebElement cookies = wait.Until(driver =>
            {
                var element = driver.FindElement(By.Id("onetrust-accept-btn-handler"));
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            });
            cookies.Click();

            //Clicking Search icon
            var searchIcon = driver.FindElement(By.ClassName("dark-iconheader-search__search-icon"));
            searchIcon.Click();

            //Waiting for Search Panel
            var searchPanelWait = new WebDriverWait(driver, TimeSpan.FromSeconds(2))
            {
                PollingInterval = TimeSpan.FromSeconds(0.25),
                Message = "Search panel was not found"
            };

            var searchPanel = searchPanelWait.Until(driver => driver.FindElement(By.ClassName("header-search__panel")));
            var searchInput = searchPanel.FindElement(By.Name("q"));

            //Seach Input
            var clickAndSendKeysActions = new Actions(driver);
            clickAndSendKeysActions.Click(searchInput)
                .Pause(TimeSpan.FromSeconds(1))
                .SendKeys(query)
                .Perform();

            //Clicking on Find button
            var findButton = searchPanel.FindElement(By.XPath(".//*[@class='search-results__input-holder']/following-sibling::button"));
            findButton.Click();

            //Waiting for results
            var searchResultsWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var searchResults = searchResultsWait.Until(driver => driver.FindElements(By.XPath("//a[contains(@class, 'search-results__item')]")));

            // Validate that all links contain the query word
            bool allLinksContainQuery = searchResults.All(link => link.Text.Contains(query, StringComparison.OrdinalIgnoreCase));

            Assert.IsTrue(allLinksContainQuery, $"Not all links contain the word '{query}'.");
        }
    }
}