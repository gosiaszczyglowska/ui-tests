using OpenQA.Selenium;
using System;

namespace PageObject.Pages
{
    internal class Actions
    {
        private readonly IWebDriver driver;
        public Actions(IWebDriver driver) => this.driver = driver ?? throw new ArgumentException(nameof(driver));

        public void Click2Times(IWebElement element)
        {
          new OpenQA.Selenium.Interactions.Actions(driver)
            .Click(element)
            .Click(element)
            .Perform();
        }

        public void ClickAndSendKeys(IWebElement element, int pauseTime, string query)
        {
            new OpenQA.Selenium.Interactions.Actions(driver)
            .Click(element)
            .Pause(TimeSpan.FromSeconds(pauseTime))
            .SendKeys(query)
            .Perform();
        }

        public void ScrollToElement(IWebElement element)
        {
            new OpenQA.Selenium.Interactions.Actions(driver)
            .ScrollToElement(element)
            .Perform();
        }

        public void SendKey(string key)
        {
            new OpenQA.Selenium.Interactions.Actions(driver)
            .SendKeys(key)
            .Perform();
        }
    }
}
