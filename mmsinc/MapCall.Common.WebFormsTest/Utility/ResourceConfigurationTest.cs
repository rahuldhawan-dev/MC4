using System;
using System.Linq;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.Common.WebFormsTest.Utility
{
    /// <summary>
    /// Summary description for ResourceConfigurationTest
    /// </summary>
    [TestClass]
    public class ResourceConfigurationTest
    {
        [TestMethod]
        public void TestIsDevMachinePropertyReturnsSetValue()
        {
            var target = new ResourceConfiguration();

            target.IsDevMachine = true;
            Assert.IsTrue(target.IsDevMachine);

            target.IsDevMachine = false;
            Assert.IsFalse(target.IsDevMachine);
        }

        [TestMethod]
        public void TestSitePropertyReturnsSetValue()
        {
            var target = new ResourceConfiguration();

            foreach (var expectedSite in Enum.GetValues(typeof(Site)).OfType<Site>())
            {
                target.Site = expectedSite;
                Assert.AreEqual(target.Site, expectedSite);
            }
        }

        [TestMethod]
        public void TestConfigurationResourceNamePropertyReturnsSetValue()
        {
            var expected = "Someconfig.xml";
            var target = new ResourceConfiguration();
            target.ConfigurationResourceName = expected;
            Assert.AreEqual(expected, target.ConfigurationResourceName);
        }
    }
}
