using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class LocalTest
    {
        [TestMethod]
        public void TestLatitudeLongitudeReturnCorrectly()
        {
            var local = new Local();

            Assert.IsNull(local.Latitude);
            Assert.IsNull(local.Longitude);

            var coordinate = new Coordinate {Latitude = 41, Longitude = -73};
            local.Coordinate = coordinate;

            Assert.AreEqual(41, local.Latitude);
            Assert.AreEqual(-73, local.Longitude);
        }
    }
}
