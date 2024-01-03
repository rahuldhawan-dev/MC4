using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class NavigateToTapOrderImage_New
    {
        private ISelenium selenium;
        private StringBuilder verificationErrors;

        [SetUp]
        public void SetupTest()
        {
            selenium = new DefaultSelenium("localhost", 4444, "*chrome", SetupFixtureBase.DEFAULT_BASE_URL);
            selenium.Start();
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                selenium.Stop();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheNavigateToTapOrderImage_NewTest()
        {
            selenium.Click("link=Field Operations");
            selenium.Click("link=Images");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("contents");
            selenium.Click("//a[@id='McMenu1_Hyperlink1']/img");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("relative=up");
            selenium.SelectFrame("main");
            selenium.SelectFrame("waterLeft");
            selenium.Click("//area[8]");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("relative=up");
            selenium.WaitForFrameToLoad("_waterright", "");
            selenium.SelectFrame("_waterright");
            selenium.Select("DistrictID", "label=" + town);
            selenium.Click("Tap");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("main");
            selenium.Click("upload");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
