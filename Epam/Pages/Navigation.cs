using OpenQA.Selenium;
using System;

namespace PageObject.Pages
{
    public class Navigation
    {
        private readonly IWebDriver driver;



        public Navigation(IWebDriver driver) => this.driver = driver ?? throw new ArgumentException(nameof(driver));
      
        public void CareersTab()
        {
            var careersTab = driver.FindElement(Locators.NavigationLocators.careersTabLocator);
            careersTab.Click();
        }
        public void AboutTab()
        {
            var careersTab = driver.FindElement(Locators.NavigationLocators.aboutTabLocator);
            careersTab.Click();
        }
        public void InsightsTab()
        {
            var careersTab = driver.FindElement(Locators.NavigationLocators.insightsTabLocator);
            careersTab.Click();
        }
    }
}
