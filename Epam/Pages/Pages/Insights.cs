using NUnit.Framework;
using OpenQA.Selenium;
using PageObject.Tests;
using System;
using System.Security.Cryptography.X509Certificates;

namespace PageObject.Pages.Pages
{
    public class Insights : TestBase
    {
        public Insights(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentException(nameof(driver));
            waits = new Utilities.Waits(driver);
            actions = new Scripts.Actions(driver);
        }

        public void SwipeToTheThirdSlide()
        {
            Log.Info("Swipping to the 3rd slide");
            var sliderRightArrow = driver.FindElement(Locators.InsightsLocators.sliderRightArrowLocator);
            actions.Click2Times(sliderRightArrow);
        }

        public void StepReadArticleAndCompareTitles()
        {
            Log.Info("Comparing Title of the slide with the Title of the Article");

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
            Log.Info("Saving Slide Title as SLideTextElement...");
            IWebElement firstSlider = waits.WaitUntilVisible(Locators.InsightsLocators.firstSliderLocator, 10);
            IWebElement activeSlide = firstSlider.FindElement(Locators.InsightsLocators.activeSlideLocator);
            IWebElement slideTextElement = activeSlide.FindElement(Locators.InsightsLocators.slideTextElementLocator);
            return slideTextElement.Text.Trim();
        }

        public void CLickReadMoreOnActiveSlide()
        {
            Log.Info("Clicking Read More button...");
            IWebElement firstSlider = waits.WaitUntilVisible(Locators.InsightsLocators.firstSliderLocator, 10);
            IWebElement activeSlide = firstSlider.FindElement(Locators.InsightsLocators.activeSlideLocator);
            IWebElement readMoreButton = activeSlide.FindElement(Locators.InsightsLocators.readMoreButtonLocator);
            readMoreButton.Click();
        }

        public string GetTitleFromArticle()
        {
            Log.Info("Saving Article Title as ArticleTitleElement...");
            IWebElement articleFirstSection = driver.FindElement(Locators.InsightsLocators.articleFirstSectionLocator);
            IWebElement articleTitleElement = articleFirstSection.FindElement(Locators.InsightsLocators.articleTitleElementLocator);
            return articleTitleElement.Text.Trim();
        }
    }
}
