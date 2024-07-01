using System.Linq;
using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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
        [TestCase("Python", "All Cities in Poland")]
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

            // Locate the autocomplete dropdown element and close it
            IWebElement autocompleteDropdown = driver.FindElement(By.ClassName("autocomplete-suggestions"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].style.display='none';", autocompleteDropdown);

            //wait for some time to see the result
            Thread.Sleep(3000);

            ///click on the dropdown input, click left control to move the dropdown up
            IWebElement dropdown = searchPanel.FindElement(By.CssSelector("span.select2-selection__rendered[title='All Cities in Poland']"));
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
            IWebElement findButton = searchPanel.FindElement(By.XPath("//*[@id = 'jobSearchFilterForm']/button"));
            findButton.Click();

            // Waiting for the page to load
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class='search-result__sorting-label list' and text()='Date']")));

            // Get the title of the first item before sorting
            string initialFirstItemTitle = driver.FindElement(By.XPath("//*[@id='main']//ul/li[1]//h5/a")).Text;
            Console.WriteLine(initialFirstItemTitle);
            
            var sortByDate = driver.FindElement(By.XPath("//*[@class='search-result__sorting-label list' and text()='Date']"));
            sortByDate.Click();

            //Waiting until sorting is done
            Thread.Sleep(8000);
            WebDriverWait waitUntilSorted = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

            //checking if sorting was done - (if 1st position was changed.
            //if it wasnt changed after 3 seconds, assuming that sorting was done
            //but the same position is 1st on the list again)
            bool isSorted = waitUntilSorted.Until(driver =>
            {
                string newFirstItemTitle = driver.FindElement(By.XPath("//*[@id='main']//ul/li[1]//h5/a")).Text;
                Console.WriteLine(newFirstItemTitle);
                return newFirstItemTitle != initialFirstItemTitle;
            });

            if (!isSorted)
            {
                Console.WriteLine("The list was not sorted within 3 seconds. Proceeding with the initial order.");
            }

            string newFirstItemTitle = driver.FindElement(By.XPath("//*[@id='main']//ul/li[1]//h5/a")).Text;
            Console.WriteLine(newFirstItemTitle);

            //Clicking on the 1st search result's "View and apply" button
            IWebElement resulItem1 = driver.FindElement(By.XPath("//*[@id='main']//ul/li[1]"));
            IWebElement viewAndApplyButton = resulItem1.FindElement(By.XPath("//a[contains(text(), 'View and apply')]"));
            viewAndApplyButton.Click();

            //waiting for the page to load
            WebDriverWait wait3 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));


          //checking if searched language is on the page
            bool isTextPresent = driver.FindElements(By.XPath($"//*[contains(text(), {language})]")).Count > 0;
            Assert.IsTrue(isTextPresent, $"Text {language} not found on the page.");
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

            Thread.Sleep(3000);

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
            var findButton = searchPanel.FindElement(By.XPath("//button[contains(span/text(), 'Find')]"));
            findButton.Click();

            //Waiting for results
            var searchResultsWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Assuming search results are displayed as a list of anchor elements (<a>)
            var searchResultLinks = searchResultsWait.Until(driver =>
                driver.FindElements(By.ClassName("search-results__title-link")));

            // Validating links using LINQ
            var linksContainQuery = searchResultLinks.All(link =>
                link.Text.Contains(query) || link.GetAttribute("href").Contains(query));

            // Assertion for NUnit test case
            Assert.IsTrue(linksContainQuery, $"Not all links contain the query term '{query}'");
        }                                  
    }
}
