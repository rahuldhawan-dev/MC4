using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class NavigateToContractors_New
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
        public void TheNavigateToContractors_NewTest()
        {
            selenium.Click("link=Contractors");
            selenium.Click("//div[@id='mainMenu']/div[6]/div[2]/ul/li[1]/span/a");
            selenium.WaitForPageToLoad("30000");
            selenium.Click("link=Add New Contractor");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
