using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObject.Pages
{
    internal class Waits
    {
        private readonly IWebDriver driver;
        public Waits(IWebDriver driver) => this.driver = driver ?? throw new ArgumentException(nameof(driver));

        public void Wait(int seconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        public IWebElement WaitUntilVisible(By locator, int time)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }
    }
}
