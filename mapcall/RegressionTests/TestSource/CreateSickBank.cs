using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class CreateSickBank
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
        public void TheCreateSickBankTest()
        {
            selenium.Select("content_cphMain_cphMain_DataElement1_DetailsView1_dv_ddlCalendar_Year", "label=" + calendarYear);
            selenium.Click("content_cphMain_cphMain_DataElement1_DetailsView1_LinkButton1");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
