using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PageObject.Utilities
{
    internal class Screenshot
    {
        public static void TakeScreenshot(IWebDriver driver, string testName)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string sanitizedTestName = SanitizeFileName(testName);
            var screenshotPath = Path.Combine(Environment.CurrentDirectory, $"{sanitizedTestName}_{DateTime.Now:yyyyMMdd_HHmmss}.png"); //TODO: create a folder for screenshots, now it falls under
                                                                                                                                        //C:\Users\[User]\source\repos\mentoring1\Epam\bin\Debug\net8.0
            screenshot.SaveAsFile(screenshotPath);
            Console.WriteLine($"Screenshot saved to: {screenshotPath}");
        }

        private static string SanitizeFileName(string fileName)
        {
            return Regex.Replace(fileName, @"[<>:""/\\|?*\[\]]", "_");
        }
    }
}
