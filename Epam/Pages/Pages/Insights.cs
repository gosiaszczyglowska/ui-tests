using NUnit.Framework;
using OpenQA.Selenium;
using PageObject.Tests;
using System;
using System.Security.Cryptography.X509Certificates;
using PageObject.Utilities;
using PageObject.Pages.Scripts;

namespace PageObject.Pages.Pages
{
    public class Insights
    {
        private Actions actions;
        private Waits waits;
        public IWebDriver driver { get; set; }
        public Insights(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            waits = new Waits(driver);
            actions = new Actions(driver);
        }

        public void SwipeToTheThirdSlide()
        {
            Log.LogInfo("Swipping to the 3rd slide");
            var sliderRightArrow = driver.FindElement(Locators.InsightsLocators.sliderRightArrowLocator);
            actions.Click2Times(sliderRightArrow);
        }

        public void StepReadArticleAndCompareTitles()
        {
            Log.LogInfo("Comparing Title of the slide with the Title of the Article");

            string slideText = GetTitleFromSlide();
            CLickReadMoreOnActiveSlide();
            string titleElement = GetTitleFromArticle();

            Console.WriteLine($"Text from the active slide: {slideText}");
            Console.WriteLine($"Text from the title: {titleElement}");

            Assert.True(slideText.Equals(titleElement));//TODO: do not use Assert in PageObject
                                                        //instead return titleElement to the test method
                                                        //after that use Assert in the test method
        }

        public string GetTitleFromSlide()
        {
            Log.LogInfo("Saving Slide Title as SLideTextElement...");
            IWebElement firstSlider = waits.WaitUntilVisible(Locators.InsightsLocators.firstSliderLocator, 10);
            IWebElement activeSlide = firstSlider.FindElement(Locators.InsightsLocators.activeSlideLocator);
            IWebElement slideTextElement = activeSlide.FindElement(Locators.InsightsLocators.slideTextElementLocator);
            return slideTextElement.Text.Trim();
        }

        public void CLickReadMoreOnActiveSlide()
        {
            Log.LogInfo("Clicking Read More button...");
            IWebElement firstSlider = waits.WaitUntilVisible(Locators.InsightsLocators.firstSliderLocator, 10);
            IWebElement activeSlide = firstSlider.FindElement(Locators.InsightsLocators.activeSlideLocator);
            IWebElement readMoreButton = activeSlide.FindElement(Locators.InsightsLocators.readMoreButtonLocator);
            readMoreButton.Click();
        }

        public string GetTitleFromArticle()
        {
            Log.LogInfo("Saving Article Title as ArticleTitleElement...");
            IWebElement articleFirstSection = driver.FindElement(Locators.InsightsLocators.articleFirstSectionLocator);
            IWebElement articleTitleElement = articleFirstSection.FindElement(Locators.InsightsLocators.articleTitleElementLocator);
            return articleTitleElement.Text.Trim();
        }
    }
}
