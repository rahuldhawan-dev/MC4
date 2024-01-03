using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class LocalityTest
    {
        [TestMethod]
        public void TestToStringReturnsCode()
        {
            var code = "ABBA";
            var target = new Locality {Code = code};

            Assert.AreEqual(code, target.ToString());
        }
    }
}
