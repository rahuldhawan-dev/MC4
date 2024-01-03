using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class CreateTailgateTalk
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
        public void TheCreateTailgateTalkTest()
        {
            selenium.Type("content_cphMain_cphMain_DataElement1_DetailsView1_dvTxtHeldOn_-1", heldOn);
            selenium.Select("content_cphMain_cphMain_DataElement1_DetailsView1_dv_ddlTailgateTopicID", "label=" + topic);
            selenium.Type("content_cphMain_cphMain_DataElement1_DetailsView1_dvTxtTopicDescription_-1", description);
            selenium.Select("content_cphMain_cphMain_DataElement1_DetailsView1_dv_ddlPresentedBy", "label=" + presentedBy);
            selenium.Type("content_cphMain_cphMain_DataElement1_DetailsView1_dvTxtTrainingTimeHours_-1", trainingTimeHours);
            selenium.Click("content_cphMain_cphMain_DataElement1_DetailsView1_LinkButton1");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
