using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class StormWaterAssetTest
    {
        [TestMethod]
        public void TestToStringReturnsAbbreviation()
        {
            var expected = "626-1";
            var target = new StormWaterAsset {AssetNumber = expected};

            Assert.AreEqual(expected, target.ToString());
        }
    }
}
