using System;
using System.IO;
using MMSINC.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib;
using Selenium;
using Config = MMSINC.Testing.Utilities.RegressionTestConfiguration;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class PleaseDontCrashTest : BaseTest
    {

        #region Constants

        private const string REAL_TIME_OPS_URL = "/Modules/Maps/RealTimeOperations.aspx";

        #endregion

        [Test]
        public void TestRealTimeOperationsDoesntCrashOnLoad()
        {
            try
            {
                Selenium.Open(Config.GetDevSiteUrl() + REAL_TIME_OPS_URL);
            }
            catch (SeleniumException e) when (e.Message.StartsWith("Timed out"))
            {
                if (!Selenium.IsTextPresent("Layers"))
                {
                    throw;
                }
            }
            Selenium.AssertTextPresent("Layers");
        }
    }
}
