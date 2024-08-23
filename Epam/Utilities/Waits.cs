using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V124.Network;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using PageObject.Utilities;

namespace PageObject.Utilities
{
    public class Waits  
    {
        private readonly IWebDriver driver;

        public Waits(IWebDriver driver) => this.driver = driver ?? throw new ArgumentException(nameof(driver));


        public WebDriverWait Wait(int time)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(time));
        }

        public IWebElement WaitUntilVisible(By locator, int time)
        {
            var wait = Wait(time);                                                                                                                                       //https://www.lambdatest.com/blog/explicit-fluent-wait-in-selenium-c/
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public IReadOnlyCollection<IWebElement> WaitUntilElementsArePresent(By locator, int time)
        {
            var wait = Wait(time);
            return wait.Until(driver => driver.FindElements(locator));
        }

        public bool WaitForFileToDownload(string filePath, int time)
        {
            var wait = Wait(time);
            return wait.Until(d => File.Exists(filePath));
        }

        public IWebElement WaitForButtonOnToastNotification(By buttonLocator)
        {
            Log.LogInfo("Waiting for the Accept Cookies button on the toast notification...");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            Func<IWebDriver, IWebElement> waitForButton = (driver) =>
            {
                try
                {
                    IWebElement button = driver.FindElement(buttonLocator);
                    if (button.Displayed && button.Enabled)
                    {
                        Log.LogInfo("Accept Cookies button is visible and clickable.");
                        Wait(3);
                        return button;
                    }
                    else
                    {
                        Log.LogDebug("Accept Cookies button is not clickable yet.");
                    }
                }
                catch (NoSuchElementException)
                {
                    Log.LogDebug("Accept Cookies button not found, waiting...");
                }
                return null;
            };

            return wait.Until(waitForButton);
        }


    }
}
