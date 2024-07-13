using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.DevTools.V124.Network;


namespace PageObject.Pages
{
    internal class Insights
    {
        private readonly IWebDriver driver;
        private  Waits waits;
        private Actions actions;

        private readonly By sliderRightArrowLocator = By.XPath("//button[@type='button' and @class='slider__right-arrow slider-navigation-arrow']");
        private readonly By firstSliderLocator = By.XPath("(//div[@class='slider section'])[1]");
        private readonly By activeSlideLocator = By.XPath(".//*[@class='owl-item active' and @aria-hidden='false']");
        private readonly By slideTextElementLocator = By.XPath(".//p[@class='scaling-of-text-wrapper']");
        private readonly By readMoreButtonLocator = By.TagName("a");
        private readonly By articleFirstSectionLocator = By.XPath("//main[@id='main']//div[@class='section'][1]");
        private readonly By articleTitleElementLocator = By.XPath("//div[contains(@class, 'column-control')]//div[contains(@class, 'text')]");

        public Insights(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            this.waits = new Waits(driver);
            this.actions = new Actions(driver);
        }

        public void SwipeToTheThirdSlide()
        {
            var sliderRightArrow = driver.FindElement(sliderRightArrowLocator);
            actions.Click2Times(sliderRightArrow);
        }

        public void ReadArticleAndCompareTitles()
        {
            // Wait for the carousel element (first slider) to be visible
            IWebElement firstSlider = waits.WaitUntilVisible(firstSliderLocator, 10);

            IWebElement activeSlide = firstSlider.FindElement(activeSlideLocator);
            IWebElement slideTextElement = activeSlide.FindElement(slideTextElementLocator);
            string slideText = slideTextElement.Text.Trim();


            Console.WriteLine("Text from the active slide:");
            Console.WriteLine(slideText);

            IWebElement readMoreButton = activeSlide.FindElement(readMoreButtonLocator);
            readMoreButton.Click();

            IWebElement articlefirstSection = driver.FindElement(articleFirstSectionLocator);
            IWebElement articleTitleElement = articlefirstSection.FindElement(articleTitleElementLocator);


            string titleElement = articleTitleElement.Text.Trim();
            Console.WriteLine("Text from the title:");
            Console.WriteLine(titleElement);

            Assert.True(slideText.Equals(titleElement));
        }

    }
}
