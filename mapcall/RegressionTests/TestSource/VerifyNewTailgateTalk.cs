using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class VerifyNewTailgateTalk
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
        public void TheVerifyNewTailgateTalkTest()
        {
            Assert.AreEqual(heldOn, selenium.GetText("//table[@id='content_cphMain_cphMain_DataElement1_DetailsView1']/tbody/tr[2]/td[2]"));
            Assert.AreEqual(topic, selenium.GetText("//table[@id='content_cphMain_cphMain_DataElement1_DetailsView1']/tbody/tr[3]/td[2]"));
            Assert.AreEqual(description, selenium.GetText("//table[@id='content_cphMain_cphMain_DataElement1_DetailsView1']/tbody/tr[4]/td[2]"));
            Assert.AreEqual(presentedBy, selenium.GetText("//table[@id='content_cphMain_cphMain_DataElement1_DetailsView1']/tbody/tr[5]/td[2]"));
            Assert.AreEqual(trainingTimeHours, selenium.GetText("//table[@id='content_cphMain_cphMain_DataElement1_DetailsView1']/tbody/tr[6]/td[2]"));
        }
    }
}
