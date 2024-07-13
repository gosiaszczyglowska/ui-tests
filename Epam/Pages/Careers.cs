using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Runtime.CompilerServices;
using System.Threading;


namespace PageObject.Pages
{
    internal class Careers
    {
        private readonly IWebDriver driver;
        private Waits waits;
        private Scripts scripts;
        private Actions actions;

        private readonly By searchPanelLocator = By.Id("jobSearchFilterForm");
        private readonly By keywordFieldLocator = By.Id("new_form_job_search-keyword");
        private readonly By locationsDropdownLocator = By.CssSelector("span.select2-selection__rendered[title='All Cities in Poland']");
        private readonly By remoteCheckboxLocator = By.ClassName("recruiting-search__filter-label-23");
        private readonly By findButtonLocator = By.XPath("//*[@id = 'jobSearchFilterForm']/button");
        private readonly By sortByDateLocator = By.XPath("//*[@class='search-result__sorting-label list' and text()='Date']");
        private readonly By initialFirstItemTitleLocator = By.XPath("//*[@id='main']//ul/li[1]//h5/a");
        private readonly By newFirstItemTitleLocator = By.XPath("//*[@id='main']//ul/li[1]//h5/a");
        private readonly By resulItem1Locator = By.XPath("//*[@id='main']//ul/li[1]");
        private readonly By viewAndApplyButtonLocator = By.XPath("//a[contains(text(), 'View and apply')]");


        public Careers(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            this.waits = new Waits(driver);
            this.scripts = new Scripts(driver);
            this.actions = new Actions(driver);
        }

        public void SearchPosition(string language, string location)
        {
            IWebElement searchPanel = driver.FindElement(searchPanelLocator);
            IWebElement keywordField = searchPanel.FindElement(keywordFieldLocator);        
            keywordField.Click();
            keywordField.SendKeys(language);

            scripts.closeAutocompleteDropdown();

            Thread.Sleep(3000);

            //click on the dropdown input, click left control to move the dropdown up
            IWebElement locationsDropdown = searchPanel.FindElement(locationsDropdownLocator);
            locationsDropdown.Click();

            actions.SendKey(Keys.LeftControl);

            var dropdownOption = driver.FindElement(By.XPath($"//li[contains(text(), '{location}')]"));
            dropdownOption.Click();

            IWebElement remoteCheckbox = searchPanel.FindElement(remoteCheckboxLocator);
            remoteCheckbox.Click();

            IWebElement findButton = searchPanel.FindElement(findButtonLocator);
            findButton.Click();
        }
        public void SortPositionsByDate()
        {

            IWebElement sortByDate = waits.WaitUntilVisible(sortByDateLocator, 5);

            string initialFirstItemTitle = driver.FindElement(initialFirstItemTitleLocator).Text;
            Console.WriteLine(initialFirstItemTitle);

            sortByDate.Click();

            Thread.Sleep(8000);

            waits.Wait(3);
            WebDriverWait waitUntilSorted = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

            //checking if sorting was done - (if 1st position was changed.
            //if it wasnt changed after 3 seconds, assuming that sorting was done
            //but the same position is 1st on the list again)
            try
            {
                bool isSorted = waitUntilSorted.Until(driver =>
                {
                    string newFirstItemTitle = driver.FindElement(newFirstItemTitleLocator).Text;
                    Console.WriteLine(newFirstItemTitle);
                    return newFirstItemTitle != initialFirstItemTitle;
                });

                if (!isSorted)
                {
                    Console.WriteLine("The list was not sorted within 3 seconds. Proceeding with the initial order.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Its the same position after sorting");
            }

            string newFirstItemTitle = driver.FindElement(newFirstItemTitleLocator).Text;
            Console.WriteLine(newFirstItemTitle);
        }
        public void ApplyForFirstPosition()
        {
            //Clicking on the 1st search result's "View and apply" button
            IWebElement resulItem1 = driver.FindElement(resulItem1Locator);
            IWebElement viewAndApplyButton = resulItem1.FindElement(viewAndApplyButtonLocator);
            viewAndApplyButton.Click();
        }


    }
}
