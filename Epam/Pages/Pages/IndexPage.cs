using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PageObject.Tests;
using System;
using System.Linq;
using PageObject.Utilities;
using PageObject.Pages.Scripts;


namespace PageObject.Pages.Pages
{
    public class IndexPage
    {
        private SeleniumScripts scripts;

        private Actions actions;

        public IWebDriver driver { get; set; }

        private Waits waits;

        private static string Url { get; } = "https://www.epam.com";

        public IndexPage(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            scripts = new SeleniumScripts(driver);
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
            IWebElement acceptCookiesButton = waits.WaitForButtonOnToastNotification(Locators.IndexPageLocators.acceptCookiesButtonLocator);
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
            var searchIcon = driver.FindElement(Locators.IndexPageLocators.searchIconLocator);
            searchIcon.Click();
        }
        public void InputSearchQuery(string query)
        {
            IWebElement searchPanel = waits.WaitUntilVisible(Locators.IndexPageLocators.searchPanelLocator, 5);
            var searchInput = searchPanel.FindElement(Locators.IndexPageLocators.searchInputLocator);

            actions.ClickAndSendKeys(searchInput, 1, query);
        }
        public void ClickFindButton()
        {
            IWebElement searchPanel = waits.WaitUntilVisible(Locators.IndexPageLocators.searchPanelLocator, 5);
            var findButton = searchPanel.FindElement(Locators.IndexPageLocators.findButtonLocator);
            findButton.Click();
        }

        public void StepValidateSearchResults(string query) //TODO: use word "Step", to indicate that this method is in fact a step, in the end of the method
                                                            //example ValidateSearchResultsStep
        {
            Log.LogInfo("Checking if all search results links contain query...");
            var searchResultLinks = waits.WaitUntilElementsArePresent(Locators.IndexPageLocators.resultsLinksLocator, 10);

            var linksContainQuery = searchResultLinks.All(link =>
                link.Text.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                link.GetAttribute("href").Contains(query, StringComparison.OrdinalIgnoreCase));

            Assert.IsTrue(linksContainQuery, $"Not all links contain the query term '{query}'"); //TODO: do not use Assert in PageObject
                                                                                                 //instead return false or null  if the element is not as expected to the test method
                                                                                                 //after that use Assert in the test method
        }
        public bool CheckSearchResultsContainQuery(string query)
        {
            Log.LogInfo("Checking if all search results links contain query...");
            var searchResultLinks = waits.WaitUntilElementsArePresent(Locators.IndexPageLocators.resultsLinksLocator, 10);

            return searchResultLinks.All(link =>
                link.Text.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                link.GetAttribute("href").Contains(query, StringComparison.OrdinalIgnoreCase));
        }


    }
}
