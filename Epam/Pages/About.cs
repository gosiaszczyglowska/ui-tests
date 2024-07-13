using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;

namespace PageObject.Pages
{
    internal class About
    {
        private readonly IWebDriver driver;
        private readonly Actions actions;

        private readonly By downloadButtonLocator = By.XPath("//span[contains(@class, 'button__content') and contains(text(), 'DOWNLOAD')]");
       
        public About(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            this.actions = new Actions(driver);
        }

        public void DownloadOverviewFile(string downloadedFileName, string downloadDirectory)
        {
            var downloadButton = driver.FindElement(downloadButtonLocator);
            actions.ScrollToElement(downloadButton);
         
            downloadButton.Click();
            Thread.Sleep(5000);
        }
        
        public void CheckIfFileExistsAndDelete(string downloadedFilePath)
        {
            if (File.Exists(downloadedFilePath))
            {
                try
                {
                    File.Delete(downloadedFilePath);
                    Console.WriteLine("File deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }
    }
}
