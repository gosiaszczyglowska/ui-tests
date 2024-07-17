using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V124.Network;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace PageObject
{
    public class Waits
    {
        private readonly IWebDriver driver;

        public Waits(IWebDriver driver) => this.driver = driver ?? throw new ArgumentException(nameof(driver));

        public ILog Log
        {
            get { return LogManager.GetLogger(this.GetType()); }
        }
        

        public void Wait(int seconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        public IWebElement WaitUntilVisible(By locator, int time)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public IWebElement WaitUntilClickable(By locator, int time)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public IReadOnlyCollection<IWebElement> WaitUntilElementsArePresent(By locator, int time)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
            return wait.Until(driver => driver.FindElements(locator));
        }

        public bool WaitForFileToDownload(string filePath, int timeoutInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => File.Exists(filePath));
        }

        public IWebElement WaitForButtonOnToastNotification(By buttonLocator)
        {
            Log.Info("Waiting for the Accept Cookies button on the toast notification...");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            Func<IWebDriver, IWebElement> waitForButton = (driver) =>
            {
                try
                {
                    IWebElement button = driver.FindElement(buttonLocator);
                    if (button.Displayed && button.Enabled)
                    {
                        Log.Info("Accept Cookies button is visible and clickable.");
                        return button;
                    }
                    else
                    {
                        Log.Debug("Accept Cookies button is not clickable yet.");
                    }
                }
                catch (NoSuchElementException)
                {
                    Log.Debug("Accept Cookies button not found, waiting...");
                }
                return null;
            };

            return wait.Until(waitForButton);
        }


    }
}
