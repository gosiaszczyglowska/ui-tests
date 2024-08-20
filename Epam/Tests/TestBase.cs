using NUnit.Framework;
using PageObject.Core;
using PageObject.Pages.Pages;
using PageObject.Utilities;

namespace PageObject.Tests
{
    public class TestBase
    {
        protected BrowserFactory browserFactory;
        protected IndexPage indexPage;
        protected Waits waits;
        protected Navigation navigation;
        protected Careers careers;
        protected Insights insights;
        protected About about;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
        }

        [SetUp]
        public void SetUp()
        {
            browserFactory = new BrowserFactory();
            InitializePageObjects();
            PrepareTestEnvironment();
        }

        private void InitializePageObjects()
        {
            indexPage = new IndexPage(browserFactory.Driver);
            waits = new Waits(browserFactory.Driver);
            navigation = new Navigation(browserFactory.Driver);
            careers = new Careers(browserFactory.Driver);
            insights = new Insights(browserFactory.Driver);
            about = new About(browserFactory.Driver);
        }

        private void PrepareTestEnvironment()
        {
            indexPage.Open();
            indexPage.AcceptCookies();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Utilities.Screenshot.TakeScreenshot(browserFactory.Driver, TestContext.CurrentContext.Test.Name);
            }

            browserFactory.CloseAndQuit();
        }
    }
}
