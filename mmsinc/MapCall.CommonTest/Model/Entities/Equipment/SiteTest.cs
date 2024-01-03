using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class SiteTest
    {
        [TestMethod]
        public void TestToStringReturnsSiteName()
        {
            var site = new Site();
            site.Name = "Name";
            Assert.AreEqual("Name", site.ToString());
        }

        [TestMethod]
        public void TestToStringTrimsWhiteSpaceFromSiteName()
        {
            var site = new Site();
            site.Name = "Name             ";
            Assert.AreEqual("Name", site.ToString());
        }
    }
}
