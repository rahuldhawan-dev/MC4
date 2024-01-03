using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class CreatePermit
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
        public void TheCreatePermitTest()
        {
            selenium.Click("ContentPlaceHolder1_btnSubmit");
            selenium.WaitForPageToLoad("30000");
            selenium.Type("locTown", municipality);
            selenium.Select("locCounty1", county);
            selenium.Select("streetMaterial", "label=" + streetMaterial);
            selenium.Type("feePermit", feePermit);
            selenium.Type("feeInspec", feeInspec);
            selenium.Type("bondAmt", bondAmt);
            selenium.Type("feeRestoration", feeRestoration);
            selenium.Type("feeTotal", feeTotal);
            selenium.Type("dateBegin", dateBegin);
            selenium.Type("dateEnd", dateEnd);
            selenium.Type("engineerAmt", engineerAmt);
            selenium.Type("statePermitID", statePermitID);
            selenium.Click("submitForm");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
