using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;
using log4net;

namespace PageObject.Pages
{
    public class About : TestBase
    {
        public About(IWebDriver driver)
        {
           this.driver = driver ?? throw new ArgumentException(nameof(driver));
            this.actions = new Actions(driver);
            this.scripts = new Scripts(driver);
        }

        public void DownloadOverviewFile()
        {
            Log.Info("Downloading Overview File...");
            var downloadButton = driver.FindElement(Locators.AboutLocators.downloadButtonLocator);
            scripts.ScrollToElement(downloadButton);
            downloadButton.Click();
            Thread.Sleep(5000);
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
