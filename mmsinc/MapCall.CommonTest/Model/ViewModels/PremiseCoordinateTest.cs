using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.ViewModels
{
    [TestClass]
    public class PremiseCoordinateTest
    {
        [TestMethod]
        public void Test_Coordinate_IncludesLatitudeAndLongitude()
        {
            var target = new PremiseCoordinate {
                Latitude = 1.1m,
                Longitude = 2.2m
            };
            
            Assert.AreEqual(target.Latitude, target.Coordinate.Latitude);
            Assert.AreEqual(target.Longitude, target.Coordinate.Longitude);
        }

        [TestMethod]
        public void Test_CoordinateIcon_IncludesMapIconId()
        {
            var target = new PremiseCoordinate {
                MapIconId = 123
            };
            
            Assert.AreEqual(target.MapIconId, target.Coordinate.Icon.Id);
        }
    }
}
