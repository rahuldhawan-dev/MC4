using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class SetupNewPermitVariables
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
        public void TheSetupNewPermitVariablesTest()
        {
            String municipality = "Some Municipality";
            String county = "Dauphin County";
            String feePermit = "1";
            String feeInspec = "2";
            String streetMaterial = "Asphalt";
            String bondAmt = "3";
            String feeRestoration = "4";
            String feeTotal = "5";
            String dateBegin = "04/14/2011";
            String dateEnd = "04/21/2011";
            String engineerAmt = "6";
            String statePermitID = "7";
            String successText = "Thank you. Your application has been submitted.";
        }
    }
}
