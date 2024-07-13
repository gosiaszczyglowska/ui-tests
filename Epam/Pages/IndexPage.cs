using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;


namespace PageObject.Pages
{
    internal class IndexPage
    {
        private static string Url { get; } = "https://www.epam.com";

        private readonly IWebDriver driver;
        private readonly Actions actions;
        private readonly Waits waits;

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
            this.waits = new Waits(driver);
        }

        public IndexPage Open() 
        {
            driver.Url = Url;
            return this;
        }

        public void AcceptCookies() 
        {
            IWebElement AcceptCookiesButton = waits.WaitUntilClickable(acceptCookiesButtonLocator, 10);
            AcceptCookiesButton.Click();
            waits.Wait(5);
        }

        public void Search(string query)
        {
            var searchIcon = driver.FindElement(searchIconLocator);
            searchIcon.Click();

            IWebElement searchPanel = waits.WaitUntilVisible(searchPanelLocator, 5);
            var searchInput = searchPanel.FindElement(searchInputLocator);

            actions.ClickAndSendKeys(searchInput,1,query);
            
            var findButton = searchPanel.FindElement(findButtonLocator);
            findButton.Click();
        }

        public void ValidateSearchResults(string query)
        {
        var searchResultLinks = waits.WaitUntilElementsArePresent(resultsLinksLocator, 10);

        var linksContainQuery = searchResultLinks.All(link =>
            link.Text.Contains(query, StringComparison.OrdinalIgnoreCase) || 
            link.GetAttribute("href").Contains(query, StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(linksContainQuery, $"Not all links contain the query term '{query}'");
        }
    
    }
}
