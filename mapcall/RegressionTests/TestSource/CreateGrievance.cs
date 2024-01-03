using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class CreateGrievance
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
        public void TheCreateGrievanceTest()
        {
            selenium.Click("content_cphMain_cphMain_Grievance1_DetailsView1_ImgdvTxtDate_Grievance_Received_-1");
            selenium.Click("content_cphMain_cphMain_Grievance1_DetailsView1_ctl03_day_4_4");
            selenium.Type("content_cphMain_cphMain_Grievance1_DetailsView1_dvTxtDate_Grievance_Received_-1", dateReceived);
            selenium.Click("content_cphMain_cphMain_Grievance1_DetailsView1_LinkButton1");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
