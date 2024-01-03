using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class VerifyNewTapOrderImage
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
        public void TheVerifyNewTapOrderImageTest()
        {
            selenium.Click("btnBack");
            selenium.WaitForPageToLoad("30000");
            selenium.Type("F1Label", serviceNumber);
            selenium.Type("F6LABEL", premiseNumber);
            selenium.Click("Submit");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("relative=up");
            selenium.SelectFrame("main");
            Assert.IsTrue(selenium.IsTextPresent("Number of Records: 1"));
            Assert.IsTrue(selenium.IsTextPresent(premiseNumber));
            Assert.IsTrue(selenium.IsTextPresent(serviceType));
            Assert.IsTrue(selenium.IsTextPresent(town));
            Assert.IsTrue(selenium.IsTextPresent(street));
        }
    }
}
