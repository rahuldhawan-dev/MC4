using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class RiskRegisterAssetTest
    {
        #region Fields

        private RiskRegisterAsset _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new RiskRegisterAsset();
        }

        #endregion

        #region Tests

        #region Coordinates

        [TestMethod]
        public void TestLatitudeLongitudeReturnsCoordinateLatitudeLongitudeIfNotNull()
        {
            var coordinate = new Coordinate { Latitude = 42, Longitude = -42 };
            _target.Coordinate = coordinate;

            Assert.AreEqual(coordinate.Latitude, _target.Latitude);
            Assert.AreEqual(coordinate.Longitude, _target.Longitude);
        }

        [TestMethod]
        public void TestLatitudeLongitudeReturnsNullIfCoordinateNull()
        {
            Assert.IsNull(_target.Latitude);
            Assert.IsNull(_target.Longitude);
        }

        #endregion

        #endregion
    }
}
