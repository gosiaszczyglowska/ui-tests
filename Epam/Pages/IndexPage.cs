using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V124.Network;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;


namespace PageObject.Pages
{
    internal class IndexPage
    {
        private static string Url { get; } = "https://www.epam.com";

        private readonly IWebDriver driver;
        private Actions actions;

        private readonly By acceptCookiesButtonLocator = By.Id("onetrust-accept-btn-handler");
        private readonly By searchIconLocator = By.ClassName("dark-iconheader-search__search-icon");
        private readonly By searchPanelLocator = By.ClassName("header-search__panel");
        private readonly By searchInputLocator = By.Name("q");
        private readonly By findButtonLocator = By.XPath("//button[contains(span/text(), 'Find')]");
        private readonly By resultsLinksLocator = By.ClassName("search-results__title-link");

        public IndexPage(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            this.actions = new Actions(driver);
        }

        public IndexPage Open() 
        {
            driver.Url = Url;
            return this;
        }

        public void AcceptCookies() 
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement AcceptCookiesButton = wait.Until(driver =>
            {
                var element = driver.FindElement(acceptCookiesButtonLocator);
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            });
            AcceptCookiesButton.Click();

        }

        public void Search(string query)
        {
            //Clicking Search icon
            var searchIcon = driver.FindElement(searchIconLocator);
            searchIcon.Click();

            //Waiting for Search Panel
            var searchPanelWait = new WebDriverWait(driver, TimeSpan.FromSeconds(2))
            {
                PollingInterval = TimeSpan.FromSeconds(0.25),
                Message = "Search panel was not found"
            };

            var searchPanel = searchPanelWait.Until(driver => driver.FindElement(searchPanelLocator));
            var searchInput = searchPanel.FindElement(searchInputLocator);

            actions.ClickAndSendKeys(searchInput,1,query);
            
            var findButton = searchPanel.FindElement(findButtonLocator);
            findButton.Click();
        }

        public void ResultsLinksContainQuery(string query)
        {
            //Waiting for results
            var searchResultsWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Assuming search results are displayed as a list of anchor elements (<a>)
            var searchResultLinks = searchResultsWait.Until(driver =>
                driver.FindElements(resultsLinksLocator));

            // Validating links using LINQ
            var linksContainQuery = searchResultLinks.All(link =>
                link.Text.Contains(query) || link.GetAttribute("href").Contains(query));

            // Assertion for NUnit test case
            Assert.IsTrue(linksContainQuery, $"Not all links contain the query term '{query}'");
        }
    }
}
