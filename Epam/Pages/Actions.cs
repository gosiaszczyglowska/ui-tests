using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //Seach Input
            new OpenQA.Selenium.Interactions.Actions(driver)
            .Click(element)
            .Pause(TimeSpan.FromSeconds(pauseTime))
            .SendKeys(query)
            .Perform();
        }

        public void MoveToElement(IWebElement element)
        {
            new OpenQA.Selenium.Interactions.Actions(driver)
            .MoveToElement(element)
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
