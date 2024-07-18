using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V124.Network;
using OpenQA.Selenium.Support.UI;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;


namespace PageObject.Pages
{
    public class Careers : TestBase
    {
        public Careers(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            this.waits = new Waits(driver);
            this.scripts = new Scripts(driver);
            this.actions = new Actions(driver);
        }


        public void StepSearchPostition(string language, string location)
        {
            SearchKeyword(language);
            scripts.closeAutocompleteDropdown();
            SearchLocation(location);
            ClickRemoteCheckbox();
            ClickFindButton();
        }
        public void SearchKeyword(string language)
        {
            IWebElement searchPanel = driver.FindElement(Locators.CareersLocators.searchPanelLocator);
            IWebElement keywordField = searchPanel.FindElement(Locators.CareersLocators.keywordFieldLocator);
            keywordField.Click();
            keywordField.SendKeys(language);
        }
        public void SearchLocation(string location)
        {
            IWebElement searchPanel = driver.FindElement(Locators.CareersLocators.searchPanelLocator);
            IWebElement locationsDropdown = searchPanel.FindElement(Locators.CareersLocators.locationsDropdownLocator);
            locationsDropdown.Click();

            actions.SendKey(Keys.LeftControl);
            var dropdownOption = driver.FindElement(By.XPath($"//li[contains(text(), '{location}')]"));
            dropdownOption.Click();
        }
        public void ClickRemoteCheckbox()
        {
            IWebElement searchPanel = driver.FindElement(Locators.CareersLocators.searchPanelLocator);
            IWebElement remoteCheckbox = searchPanel.FindElement(Locators.CareersLocators.remoteCheckboxLocator);
            remoteCheckbox.Click();
        }

        public void ClickFindButton()
        {
            IWebElement searchPanel = driver.FindElement(Locators.CareersLocators.searchPanelLocator);
            IWebElement findButton = searchPanel.FindElement(Locators.CareersLocators.findButtonLocator);
            findButton.Click();
        }

        public void StepSortByDateAndVerify()
        {
            Log.Info("Sorting Postitions by date...");
            string initialFirstItemTitle = driver.FindElement(Locators.CareersLocators.initialFirstItemTitleLocator).Text;
            Console.WriteLine($"Initial first item title is: {initialFirstItemTitle}");
            SortByDate();
            VerifyIfSortingWasDone(initialFirstItemTitle);
            string newFirstItemTitle = driver.FindElement(Locators.CareersLocators.newFirstItemTitleLocator).Text;
            Console.WriteLine($"First item title after sorting is: {newFirstItemTitle}");
        }

        public void SortByDate()
        {
            IWebElement sortByDate = waits.WaitUntilVisible(Locators.CareersLocators.sortByDateLocator, 5);
            sortByDate.Click();
        }

        public void ApplyForFirstPosition()
        {
/*            Log.Info("Downloading Overview File...");
            var downloadButton = driver.FindElement(Locators.AboutLocators.downloadButtonLocator);
            scripts.ScrollToElement(downloadButton);
            downloadButton.Click();

*/
            Log.Info("Opening details of the first position on the list...");
            IWebElement resulItem1 = driver.FindElement(Locators.CareersLocators.resulItem1Locator);
            scripts.ScrollToElement(resulItem1);
            IWebElement viewAndApplyButton = resulItem1.FindElement(Locators.CareersLocators.viewAndApplyButtonLocator);
            viewAndApplyButton.Click();
        }

        public void VerifyIfSortingWasDone(string initialFirstItemTitle)
        {
            Log.Info("Verifying if sorting was done...");
            WebDriverWait waitUntilSorted = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                bool isSorted = waitUntilSorted.Until(driver =>
                {
                    string newFirstItemTitle = driver.FindElement(Locators.CareersLocators.newFirstItemTitleLocator).Text;
                  
                    return newFirstItemTitle != initialFirstItemTitle;
                });

                if (!isSorted)
                {
                    Log.Info("Sorting was not done.");
                    Console.WriteLine("The list was not sorted within 3 seconds. Proceeding with the initial order.");
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Error(ex);
                Log.Info("It's the same position after sorting");
                //Console.WriteLine("Its the same position after sorting");
            }

        }
        
        public bool IsLanguagePresent(string language)
        {
            return driver.FindElements(By.XPath($"//*[contains(text(), '{language}')]")).Count > 0;
        }
    }
}
