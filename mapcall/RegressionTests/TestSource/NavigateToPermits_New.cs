using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class NavigateToPermits_New
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
        public void TheNavigateToPermits_NewTest()
        {
            selenium.Click("link=MapCall™");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("contents");
            selenium.Click("McMenu1_Hyperlink1");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("relative=up");
            selenium.SelectFrame("main");
            selenium.SelectFrame("waterLeft");
            selenium.Click("//tr[2]/td/a[contains(@href, '3.asp')]");
            selenium.WaitForPageToLoad("30000");
            selenium.Click("link=State Permits");
            selenium.WaitForPageToLoad("30000");
            selenium.Click("link=here");
            selenium.WaitForPageToLoad("30000");
            selenium.Click("link=here");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
