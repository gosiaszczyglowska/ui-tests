using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.IO;
using System.Threading;


namespace PageObject.Pages
{
    internal class About
    {
        private readonly IWebDriver driver;
        private Actions actions;

        private readonly By downloadButtonLocator = By.XPath("//span[contains(@class, 'button__content') and contains(text(), 'DOWNLOAD')]");
       
        public About(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            this.actions = new Actions(driver);
        }

        public void DownloadOverviewFile(string downloadedFileName)
        {
            var downloadButton = driver.FindElement(downloadButtonLocator);
            actions.MoveToElement(downloadButton);
         
            downloadButton.Click();
            Thread.Sleep(5000);

            // Path to the downloaded file
            string downloadDirectory = @"C:\TestDownload";

            // Construct the full path to the downloaded file
            string downloadedFilePath = Path.Combine(downloadDirectory, downloadedFileName);

            //Validation
            Assert.IsTrue(File.Exists(downloadedFilePath));

            // Check if the file exists and delete it
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
