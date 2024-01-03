using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class SetupStormwaterAssetVariables
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
        public void TheSetupStormwaterAssetVariablesTest()
        {
            String assetNumber = "123";
            String user = "mcadmin";
            String operatingCenterCode = "NJ4";
            String stormwaterAssetType = "Catch Basin";
            String street = "EGBERT ST";
            String operatingCenter = "NJ4 - Lakewood";
            String town = "BAY HEAD";
        }
    }
}
