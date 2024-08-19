using OpenQA.Selenium;
using System;

namespace PageObject.Pages.Scripts
{
    public class Actions //TODO: let's put Actions under PageObject.Pages.Scripts namespace/folder structure
    {
        private readonly IWebDriver driver;
        public Actions(IWebDriver driver) => this.driver = driver ?? throw new ArgumentException(nameof(driver));

        public void Click2Times(IWebElement element) //TODO: Rename to DoubleClick
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

        public void SendKey(string key)
        {
            new OpenQA.Selenium.Interactions.Actions(driver)
            .SendKeys(key)
            .Perform();
        }
    }
}
