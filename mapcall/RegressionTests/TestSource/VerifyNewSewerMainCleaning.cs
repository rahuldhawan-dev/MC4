using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class VerifyNewSewerMainCleaning
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
        public void TheVerifyNewSewerMainCleaningTest()
        {
            Assert.AreEqual(opCenterVerify, selenium.GetText("content_cphMain_cphMain_dvSewerMainCleaning_lblOpCntr"));
            Assert.AreEqual(town, selenium.GetText("//table[@id='content_cphMain_cphMain_dvSewerMainCleaning']/tbody/tr[4]/td[2]"));
            Assert.AreEqual(date, selenium.GetText("//table[@id='content_cphMain_cphMain_dvSewerMainCleaning']/tbody/tr[6]/td[2]"));
            Assert.AreEqual(completedDate, selenium.GetText("//table[@id='content_cphMain_cphMain_dvSewerMainCleaning']/tbody/tr[7]/td[2]"));
        }
    }
}
