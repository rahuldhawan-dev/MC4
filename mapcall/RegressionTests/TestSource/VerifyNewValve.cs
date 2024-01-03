using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class VerifyNewValve
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
        public void TheVerifyNewValveTest()
        {
            selenium.Click("link=Edit Valve Record");
            selenium.WaitForPageToLoad("30000");
            try
            {
                Assert.IsTrue(selenium.IsTextPresent(""));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
            try
            {
                Assert.IsTrue(selenium.IsTextPresent(""));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
            Assert.AreEqual(street, selenium.GetSelectedLabel("StName"));
            Assert.AreEqual(valveSize, selenium.GetSelectedLabel("ValveSize"));
            Assert.AreEqual(valveZone, selenium.GetSelectedLabel("ValveZone"));
        }
    }
}
