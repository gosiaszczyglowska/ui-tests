using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;


namespace PageObject.Pages
{
    internal class Careers
    {
        private readonly IWebDriver driver;
        private readonly Waits waits;
        private readonly Scripts scripts;
        private readonly Actions actions;

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
            Console.WriteLine($"Initial first item title is: {initialFirstItemTitle}");
            sortByDate.Click();

            VerifyIfSortingWasDone(initialFirstItemTitle);

            string newFirstItemTitle = driver.FindElement(newFirstItemTitleLocator).Text;
            Console.WriteLine($"First item title after sorting is: {newFirstItemTitle}");
        }
        public void ApplyForFirstPosition()
        {
            IWebElement resulItem1 = driver.FindElement(resulItem1Locator);
            IWebElement viewAndApplyButton = resulItem1.FindElement(viewAndApplyButtonLocator);
            viewAndApplyButton.Click();
        }

        public void VerifyIfSortingWasDone(string initialFirstItemTitle)
        {
            Thread.Sleep(8000);

            WebDriverWait waitUntilSorted = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            try
            {
                bool isSorted = waitUntilSorted.Until(driver =>
                {
                    string newFirstItemTitle = driver.FindElement(newFirstItemTitleLocator).Text;
                    
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
        }
    }
}
