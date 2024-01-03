using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class SetupTapOrderImageVariables
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
        public void TheSetupTapOrderImageVariablesTest()
        {
            String town = "BAY HEAD";
            String dated = "04/27/10";
            String serviceType = "WATER";
            String premiseNumber = "123456789";
            String street = "EGBERT ST";
            String streetNumber = "123";
            String filename = "C:\\Solutions\\mapcall\\lib\\images\\NJ\\00-Tap\\Uploads\\1814\\00114008.tif";
            String serviceNumber = "12345678";
        }
    }
}
