using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;
using log4net;
using PageObject.Tests;
using PageObject.Utilities;

namespace PageObject.Pages.Pages
{
    public class About : TestBase   
                                    //do not inherit TestBase here, instead inherit BrowserFactory (refer to the comment on [SetUp] in TestBase class)
    {
        public About(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            actions = new Scripts.Actions(driver);
            scripts = new Scripts.Scripts(driver);
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
