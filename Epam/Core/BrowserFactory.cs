﻿using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System;
using PageObject.Utilities;
using System.IO;
using log4net.Config;


namespace PageObject.Core
{
    public class BrowserFactory
    {
        public IWebDriver Driver { get; set; }

        public BrowserFactory()
        {
            Driver = InitializeWebDriver(Configuration.LoadConfiguration().DownloadDirectory);
        }

        public IWebDriver InitializeWebDriver(string downloadDirectory)
        {
            string browserType = Environment.GetEnvironmentVariable("BROWSER_TYPE") ?? "chrome";
            bool headless = Environment.GetEnvironmentVariable("HEADLESS_MODE") == "true";

            var driver = GetDriver(browserType, downloadDirectory, headless);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();

            return driver;
        }

        public IWebDriver GetDriver(string browserType, string downloadDirectory, bool headless)
        {
            switch (browserType.ToLower())
            {
                case "chrome":
                    return ReturnChrome(downloadDirectory, headless);

                case "firefox":
                    return ReturnFirefox(downloadDirectory, headless);

                case "edge":
                    return ReturnEdge(downloadDirectory, headless);

                default:
                    throw new ArgumentException("Unsupported browser type: " + browserType);
            }
        }

        private ChromeDriver ReturnChrome(string downloadDirectory, bool headless)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddArgument("--disable-search-engine-choice-screen");
            if (headless)
            {
                chromeOptions.AddArgument("--headless");
                chromeOptions.AddArgument("--no-sandbox");
                chromeOptions.AddArgument("--disable-gpu");
                chromeOptions.AddArgument("--window-size=1920,1080");
            }
            return new ChromeDriver(chromeOptions);
        }

        private FirefoxDriver ReturnFirefox(string downloadDirectory, bool headless)
        {
            var firefoxProfile = new FirefoxProfile();
            firefoxProfile.SetPreference("browser.download.folderList", 2);
            firefoxProfile.SetPreference("browser.download.dir", downloadDirectory);
            firefoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf,application/octet-stream");

            var firefoxOptions = new FirefoxOptions { Profile = firefoxProfile };
            if (headless)
            {
                firefoxOptions.AddArgument("--headless");
            }
            return new FirefoxDriver(firefoxOptions);
        }

        private EdgeDriver ReturnEdge(string downloadDirectory, bool headless)
        {
            var edgeOptions = new EdgeOptions();
            edgeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            edgeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            if (headless)
            {
                edgeOptions.AddArgument("--headless");
                edgeOptions.AddArgument("--no-sandbox");
                edgeOptions.AddArgument("--disable-gpu");
                edgeOptions.AddArgument("--window-size=1920,1080");
            }
            return new EdgeDriver(edgeOptions);
        }

        public void ConfigureLogging()
        {
            XmlConfigurator.Configure(new FileInfo("Log.config"));
            Console.WriteLine($"Logs will be stored in: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")}");
        }

        public void DeleteAllFilesInDownloadDirectory()
        {
            string downloadDirectory = Configuration.LoadConfiguration().DownloadDirectory;

            var files = Directory.GetFiles(downloadDirectory);

            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                    Console.WriteLine($"Deleted file: {file}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete file: {file}. Exception: {ex.Message}");
                }
            }
        }

        public void CloseAndQuit()
        {
            if (Driver != null)
            {
                Driver.Close();
                Driver.Quit();
            }
        }
    }
}