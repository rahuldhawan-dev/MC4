using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace SeleniumTests
{
    [TestFixture]
    public class VerifyNewStormwaterAsset
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
        public void TheVerifyNewStormwaterAssetTest()
        {
            Assert.AreEqual(operatingCenterCode, selenium.GetText("content_cphMain_cphMain_dvStormWaterAsset_lblOpCntr"));
            try
            {
                Assert.IsTrue(selenium.IsTextPresent(town));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
            try
            {
                Assert.IsTrue(selenium.IsTextPresent(stormwaterAssetType));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
            try
            {
                Assert.IsTrue(selenium.IsTextPresent(street));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
            try
            {
                Assert.IsTrue(selenium.IsTextPresent(user));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
            try
            {
                Assert.IsTrue(selenium.IsTextPresent(assetNumber));
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
        }
    }
}
