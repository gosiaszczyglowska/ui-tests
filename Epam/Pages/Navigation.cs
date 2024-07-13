using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V124.Network;
using System;

namespace PageObject.Pages
{
    internal class Navigation
    {
        private readonly IWebDriver driver;

        private readonly By careersTabLocator = By.LinkText("Careers");
        private readonly By aboutTabLocator = By.LinkText("About");
        private readonly By insightsTabLocator = By.LinkText("Insights");

        public Navigation(IWebDriver driver) => this.driver = driver ?? throw new ArgumentException(nameof(driver));
      
        // Click on Careers tab
        public void CareersTab()
        {
            var careersTab = driver.FindElement(careersTabLocator);
            careersTab.Click();
        }
        public void AboutTab()
        {
            var careersTab = driver.FindElement(aboutTabLocator);
            careersTab.Click();
        }
        public void InsightsTab()
        {
            var careersTab = driver.FindElement(insightsTabLocator);
            careersTab.Click();
        }
    }
}
