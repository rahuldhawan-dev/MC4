using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class NavigateToSewerManhole_New
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
        public void TheNavigateToSewerManhole_NewTest()
        {
            selenium.Click("link=MapCall™");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("contents");
            selenium.Click("McMenu1_hlSewer");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("relative=up");
            selenium.SelectFrame("main");
            selenium.Click("cphMain_hlManholes");
            selenium.WaitForPageToLoad("30000");
            selenium.Click("cphMain_btnAdd");
            selenium.WaitForPageToLoad("30000");
        }
    }
}