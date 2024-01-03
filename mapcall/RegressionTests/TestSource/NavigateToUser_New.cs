using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class NavigateToUser_New
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
        public void TheNavigateToUser_NewTest()
        {
            selenium.Click("link=MapCall™");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("contents");
            selenium.Click("McMenu1_hlAdmin");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("relative=up");
            selenium.SelectFrame("main");
            selenium.Click("link=Users");
            selenium.WaitForPageToLoad("30000");
            selenium.Click("ContentPlaceHolder1_btnAddUser");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
