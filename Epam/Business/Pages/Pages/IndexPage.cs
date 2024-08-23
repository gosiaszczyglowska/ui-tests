using OpenQA.Selenium;
using System;
using System.Linq;
using PageObject.Core.Utilities;
using PageObject.Business.Pages.Locators;
using PageObject.Business.Pages.Scripts;


namespace PageObject.Business.Pages.Pages
{
    public class IndexPage
    {
        private readonly Actions actions;

        public IWebDriver driver { get; set; }

        private readonly Waits waits;

        private static string Url { get; } = "https://www.epam.com";

        public IndexPage(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            actions = new Actions(driver);
            waits = new Waits(driver);
        }

        public IndexPage Open()
        {
            driver.Url = Url;
            return this;
        }

        public void AcceptCookies()
        {
            Log.LogInfo("Accepting cookies...");
            IWebElement acceptCookiesButton = waits.WaitForButtonOnToastNotification(IndexPageLocators.acceptCookiesButtonLocator);
            acceptCookiesButton.Click();
        }


        public void StepSearchQuery(string query)
        {
            Log.LogInfo($"Searching for a query {query}");
            ClickSearchIcon();
            InputSearchQuery(query);
            ClickFindButton();
        }

        public void ClickSearchIcon()
        {
            var searchIcon = driver.FindElement(IndexPageLocators.searchIconLocator);
            searchIcon.Click();
        }
        public void InputSearchQuery(string query)
        {
            IWebElement searchPanel = waits.WaitUntilVisible(IndexPageLocators.searchPanelLocator, 5);
            var searchInput = searchPanel.FindElement(IndexPageLocators.searchInputLocator);

            actions.ClickAndSendKeys(searchInput, 1, query);
        }
        public void ClickFindButton()
        {
            IWebElement searchPanel = waits.WaitUntilVisible(IndexPageLocators.searchPanelLocator, 5);
            var findButton = searchPanel.FindElement(IndexPageLocators.findButtonLocator);
            findButton.Click();
        }

        public bool CheckSearchResultsContainQuery(string query)
        {
            Log.LogInfo("Checking if all search results links contain query...");
            var searchResultLinks = waits.WaitUntilElementsArePresent(IndexPageLocators.resultsLinksLocator, 10);

            return searchResultLinks.All(link =>
                link.Text.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                link.GetAttribute("href").Contains(query, StringComparison.OrdinalIgnoreCase));
        }


    }
}
