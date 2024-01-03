using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class VerifyNewService
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
        public void TheVerifyNewServiceTest()
        {
            selenium.Click("link=Edit / Print New Service Record");
            selenium.WaitForPageToLoad("30000");
            Assert.AreEqual(streetNumber, selenium.GetValue("StNum"));
            Assert.AreEqual(street, selenium.GetSelectedLabel("StName"));
            Assert.AreEqual(town, selenium.GetSelectedLabel("Town"));
            Assert.AreEqual(lot, selenium.GetValue("Lot"));
            Assert.AreEqual(block, selenium.GetValue("Block"));
            Assert.AreEqual(categoryOfService, selenium.GetSelectedLabel("CatOfService"));
        }
    }
}