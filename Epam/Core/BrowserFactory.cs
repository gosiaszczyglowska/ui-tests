using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System;
using System.Configuration;

namespace PageObject.Core
{
    public static class BrowserFactory 
    {
        public static IWebDriver GetDriver(string browserType, string downloadDirectory, bool headless) 
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

        private static ChromeDriver ReturnChrome(string downloadDirectory, bool headless)
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
        private static FirefoxDriver ReturnFirefox(string downloadDirectory, bool headless) 
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
        private static EdgeDriver ReturnEdge(string downloadDirectory, bool headless) 
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
    }
}