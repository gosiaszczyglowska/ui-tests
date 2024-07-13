using OpenQA.Selenium;

using System;

namespace PageObject.Pages
{
    internal class Scripts
    {
        private readonly IWebDriver driver;
        public Scripts(IWebDriver driver) => this.driver = driver ?? throw new ArgumentException(nameof(driver));

        public readonly By autocompleteDropdownLocator = By.ClassName("autocomplete-suggestions");
        public void closeAutocompleteDropdown()
        {
            IWebElement autocompleteDropdown = driver.FindElement(autocompleteDropdownLocator);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].style.display='none';", autocompleteDropdown);
        }
    }
}
