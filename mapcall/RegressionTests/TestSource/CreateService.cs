using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class CreateService
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
        public void TheCreateServiceTest()
        {
            selenium.SelectFrame("main");
            selenium.Click("view");
            selenium.WaitForPageToLoad("30000");
            selenium.Select("Town", "label=" + town);
            selenium.Click("view");
            selenium.WaitForPageToLoad("30000");
            selenium.Type("StNum", streetNumber);
            selenium.Select("StName", "label=" + street);
            selenium.Type("Lot", lot);
            selenium.Type("Block", block);
            selenium.Click("view");
            selenium.WaitForPageToLoad("30000");
            selenium.Select("CatOfService", "label=" + categoryOfService);
            selenium.Click("view");
            selenium.WaitForPageToLoad("30000");
        }
    }
}
