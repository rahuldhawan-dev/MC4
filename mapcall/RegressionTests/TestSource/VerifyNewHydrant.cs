using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class VerifyNewHydrant
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
        public void TheVerifyNewHydrantTest()
        {
            selenium.Click("link=Edit Hydrant Record");
            selenium.WaitForPageToLoad("30000");
            selenium.SelectFrame("main");
            Assert.AreEqual(premiseNumber, selenium.GetValue("premiseNumber"));
            Assert.AreEqual(street, selenium.GetSelectedLabel("StName"));
            Assert.AreEqual(town, selenium.GetText("//form[@id='form1']/table/tbody/tr[3]/td/table/tbody/tr[5]/td[1]/font[2]"));
            Assert.AreEqual(billingDate, selenium.GetValue("BillingDate"));
            try
            {
                Assert.AreEqual(opCenterVerify, selenium.GetText("//form[@id='form1']/table/tbody/tr[3]/td/table/tbody/tr[1]/td[5]/font[2]"));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
        }
    }
}
