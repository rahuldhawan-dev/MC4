using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Permits.Data.Client;

namespace Permits.Data.ClientTest
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void TestBaseAddressReadsTheBaseAddressFromTheConfigFileWithTheProperKey()
        {
            Assert.AreEqual(
                new Uri(ConfigurationManager.AppSettings[Utilities.BASE_ADDRESS_CONFIG_KEY]),
                Utilities.BaseAddress);
        }
    }
}
