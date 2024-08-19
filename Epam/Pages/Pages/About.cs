using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;
using log4net;
using PageObject.Tests;

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
            Log.Info("Downloading Overview File...");
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
                    Log.Info("Deleting file...");
                    File.Delete(downloadedFilePath);
                    Log.Info("File deleted successfully");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Failed to delete file: {ex.Message}");
                    Log.Error(ex);
                    Log.Info($"{ex.Message}");
                }
            }
            else
            {
                Log.Warn("File does not exist");
                //Console.WriteLine("File does not exist.");
            }
        }
    }
}
