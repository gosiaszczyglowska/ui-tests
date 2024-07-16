using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PageObject
{
    internal class Locators
    {
        public static class IndexPageLocators
        {
            public static readonly By acceptCookiesButtonLocator = By.Id("onetrust-accept-btn-handler");
            public static readonly By searchIconLocator = By.ClassName("dark-iconheader-search__search-icon");
            public static readonly By searchPanelLocator = By.ClassName("header-search__panel");
            public static readonly By searchInputLocator = By.Name("q");
            public static readonly By findButtonLocator = By.XPath("//button[contains(span/text(), 'Find')]");
            public static readonly By resultsLinksLocator = By.ClassName("search-results__title-link");
        }
        public static class AboutLocators
        {
            public static readonly By downloadButtonLocator = By.XPath("//span[contains(@class, 'button__content') and contains(text(), 'DOWNLOAD')]");
        }
        public static class CareersLocators
        {
            public static readonly By searchPanelLocator = By.Id("jobSearchFilterForm");
            public static readonly By keywordFieldLocator = By.Id("new_form_job_search-keyword");
            public static readonly By locationsDropdownLocator = By.CssSelector("span.select2-selection__rendered[title='All Cities in Poland']");
            public static readonly By remoteCheckboxLocator = By.ClassName("recruiting-search__filter-label-23");
            public static readonly By findButtonLocator = By.XPath("//*[@id = 'jobSearchFilterForm']/button");
            public static readonly By sortByDateLocator = By.XPath("//*[@class='search-result__sorting-label list' and text()='Date']");
            public static readonly By initialFirstItemTitleLocator = By.XPath("//*[@id='main']//ul/li[1]//h5/a");
            public static readonly By newFirstItemTitleLocator = By.XPath("//*[@id='main']//ul/li[1]//h5/a");
            public static readonly By resulItem1Locator = By.XPath("//*[@id='main']//ul/li[1]");
            public static readonly By viewAndApplyButtonLocator = By.XPath("//a[contains(text(), 'View and apply')]");
        }
        public static class InsightsLocators
        {

            public static By sliderRightArrowLocator = By.XPath("//button[@type='button' and @class='slider__right-arrow slider-navigation-arrow']");
            public static By firstSliderLocator = By.XPath("(//div[@class='slider section'])[1]");
            public static By activeSlideLocator = By.XPath(".//*[@class='owl-item active' and @aria-hidden='false']");
            public static By slideTextElementLocator = By.XPath(".//p[@class='scaling-of-text-wrapper']");
            public static By readMoreButtonLocator = By.TagName("a");
            public static By articleFirstSectionLocator = By.XPath("//main[@id='main']//div[@class='section'][1]");
            public static By articleTitleElementLocator = By.XPath("//div[contains(@class, 'column-control')]//div[contains(@class, 'text')]");
        }
        public static class NavigationLocators
        {
            public static By careersTabLocator = By.LinkText("Careers");
            public static By aboutTabLocator = By.LinkText("About");
            public static By insightsTabLocator = By.LinkText("Insights");
        }
    }
}
