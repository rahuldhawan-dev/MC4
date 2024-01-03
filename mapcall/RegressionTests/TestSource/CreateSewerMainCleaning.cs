using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class CreateSewerMainCleaning
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
        public void TheCreateSewerMainCleaningTest()
        {
            selenium.Select("content_cphMain_cphMain_dvSewerMainCleaning_ddlOpCntr_ddlOpCntr", "label=" + opCenterCreate);
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (selenium.IsEditable("content_cphMain_cphMain_dvSewerMainCleaning_ddlTown")) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            selenium.Select("content_cphMain_cphMain_dvSewerMainCleaning_ddlTown", "label=" + town);
            selenium.Type("content_cphMain_cphMain_dvSewerMainCleaning_dvTxtDate_-1", date);
            selenium.Type("content_cphMain_cphMain_dvSewerMainCleaning_dvTxtCompletedDate_-1", completedDate);
            selenium.Click("content_cphMain_cphMain_dvSewerMainCleaning_btnInsert");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
