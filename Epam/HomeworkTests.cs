using System.IO;
using log4net;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Configuration;

namespace PageObject
{

    [TestFixture]

    public class HomeworkTests : TestBase
    {

        [TestCase("Java", "All Locations")]
        [TestCase("Python", "All Cities in Poland")]
        public void PositionSearch(string language, string location)
        {
            navigation.CareersTab();
            careers.StepSearchPostition(language, location);
            careers.StepSortByDateAndVerify();
            careers.ApplyForFirstPosition();

            waits.Wait(5);
            bool isLanguagePresent = careers.IsLanguagePresent(language);
            
            Log.Info("Verifying that searched programming language is present in the opened article");
            Assert.IsTrue(isLanguagePresent, $"Text {language} not found on the page.");
        }

        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void GlobalSearch(string query)
        {
            indexPage.StepSearchQuery(query);
            bool checkSearchResultsContainQuery = indexPage.CheckSearchResultsContainQuery(query);
            
            Assert.IsTrue(checkSearchResultsContainQuery, $"Not all links contain the query term '{query}'");
        }

        [TestCase("EPAM_Corporate_Overview_Q4_EOY.pdf", @"C:\TestDownload")]
        public void DownloadCheck(string downloadedFileName, string downloadDirectory)
        {
            navigation.AboutTab();
            about.DownloadOverviewFile();

            string downloadedFilePath = Path.Combine(downloadDirectory, downloadedFileName);

            Assert.IsTrue(File.Exists(downloadedFilePath));

            about.DeleteFile(downloadedFilePath);
        }

        /*        [TestCase("EPAM_Corporate_Overview_Q4_EOY.pdf")]
                public void DownloadCheck(string downloadedFileName)
                {
                    navigation.AboutTab();
                    about.DownloadOverviewFile();

                    string downloadedFilePath = Path.Combine(downloadDirectory, downloadedFileName);

                    Assert.IsTrue(File.Exists(downloadedFilePath));

                    about.DeleteFile(downloadedFilePath);
                }*/


        [Test]
        public void TitleCheck()
        {
            Log.Info("Running TitleCheck test");
            navigation.InsightsTab();
            insights.SwipeToTheThirdSlide();
            string slideText = insights.GetTitleFromSlide();
            insights.CLickReadMoreOnActiveSlide();
            string titleElement = insights.GetTitleFromArticle();

            Log.Info("Verifying that Slide Title is the same as Article Title");
            Assert.True(slideText.Equals(titleElement));
        }
    }
}
