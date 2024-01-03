using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class EchoshoreSiteTest
    {
        [TestMethod]
        public void TestToStringReturnsOperatingCenterCodeTownDescription()
        {
            var target = new EchoshoreSite {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJ7"},
                Town = new Town {ShortName = "Winden"},
                Description = "Bus stop"
            };

            Assert.AreEqual("NJ7 - Winden - Bus stop", target.ToString());
        }
    }
}
