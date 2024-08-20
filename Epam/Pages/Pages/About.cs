using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;
using log4net;
using PageObject.Tests;
using PageObject.Utilities;
using PageObject.Core;
using PageObject.Pages.Scripts;

namespace PageObject.Pages.Pages
{
    public class About
    //do not inherit TestBase here, instead inherit BrowserFactory (refer to the comment on [SetUp] in TestBase class)
    {
        private SeleniumScripts scripts;
        private Actions actions;
        public IWebDriver driver { get; set; }

        public About(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            scripts = new SeleniumScripts(driver);
            actions = new Actions(driver);
        }



        public void DownloadOverviewFile()
        {
            Log.LogInfo("Downloading Overview File...");
            var downloadButton = driver.FindElement(Locators.AboutLocators.downloadButtonLocator);
            scripts.ScrollToElement(downloadButton);
            downloadButton.Click();
        }

        public void DeleteFile(string downloadedFilePath)
        {

            if (File.Exists(downloadedFilePath))
            {
                try
                {
                    Log.LogInfo("Deleting file...");
                    File.Delete(downloadedFilePath);
                    Log.LogInfo("File deleted successfully");
                }
                catch (Exception ex)
                {
                    Log.LogError("Failed to delete file", ex);
                    Log.LogInfo($"{ex.Message}");
                }
            }
            else
            {
                Log.LogWarn("File does not exist");
            }
        }
    }
}
