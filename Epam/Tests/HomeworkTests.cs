using System.IO;
using log4net;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Configuration;

namespace PageObject.Tests
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
            bool isLanguagePresent = careers.IsLanguagePresent(language); //TODO: lines 25 abd 28 do the same.
                                                                          //instead create/reuse WaitUntilElementsArePresent method in Assert

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


        [TestCase("EPAM_Corporate_Overview_Q4_EOY.pdf")]
        public void DownloadCheck(string downloadedFileName)
        {
            string downloadDirectory = SetUpFixture.AppSettings.DownloadDirectory;

            navigation.AboutTab();
            about.DownloadOverviewFile();

            string downloadedFilePath = Path.Combine(downloadDirectory, downloadedFileName);

            waits.WaitForFileToDownload(downloadedFilePath, 30);

            Assert.IsTrue(File.Exists(downloadedFilePath));

            about.DeleteFile(downloadedFilePath); //TODO: if Assert.IsTrue fails, this part of code will never be reached
                                                  //instead use try catch finally or call DeleteFile in the teardown method
                                                  //you can delete all files in the folder if you create your own folder
        }


        [Test]
        public void TitleCheck()
        {
            Log.Info("Running TitleCheck test");
            navigation.InsightsTab();
            insights.SwipeToTheThirdSlide();
            string slideText = insights.GetTitleFromSlide();
            insights.CLickReadMoreOnActiveSlide();
            string titleElement = insights.GetTitleFromArticle();

            Log.Info("Verifying that Slide Title is the same as Article Title"); //TODO: you can write the fail message inside Assert.True
                                                                                 //Assert.True(slideText.Equals(titleElement), "Slide Title is not the same as Article Title");
            Assert.True(slideText.Equals(titleElement));
        }

        [Test]
        public void ExampleTest_Failure()
        {
            Assert.Fail("This test is expected to fail, triggering the screenshot capture.");
        }

        [Test]
        public void ExampleTest_Success()
        {
            Assert.Pass("This test will pass, and no screenshot will be taken.");
        }
    }
}
