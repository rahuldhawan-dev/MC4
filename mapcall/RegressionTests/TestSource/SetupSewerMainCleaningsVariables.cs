using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class SetupSewerMainCleaningsVariables
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
        public void TheSetupSewerMainCleaningsVariablesTest()
        {
            String opCenterVerify = "NJ4";
            String opCenterCreate = "NJ4 - Lakewood";
            String date = "4/20/2011 12:00:00 AM";
            String completedDate = "5/20/2011 12:00:00 AM";
            String town = "BAY HEAD";
        }
    }
}
