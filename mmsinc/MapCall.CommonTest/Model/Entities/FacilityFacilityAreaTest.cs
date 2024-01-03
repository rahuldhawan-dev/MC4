using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class FacilityFacilityAreaTest
    {
        #region Tests

        [TestMethod]
        public void TestDisplayReturnsFacilityAreaAndFacilitySubAreaDescriptionsConcatenated()
        {
            //Arrange
            var expectedDisplay = "FacilityAreaDesc - FacilitySubAreaDesc";
            var facility = new Facility();
            var facilityArea = new FacilityArea {Description = "FacilityAreaDesc"};
            var facilitySubArea = new FacilitySubArea {Description = "FacilitySubAreaDesc", Area = facilityArea};
            var ffa = new FacilityFacilityArea {
                Id = 1,
                Facility = facility,
                FacilityArea = facilityArea,
                FacilitySubArea = facilitySubArea
            };
            //Act
            var actualDisplay = ffa.Display;

            //Assert
            Assert.AreEqual(expectedDisplay, actualDisplay);
        }

        [TestMethod]
        public void TestLatitudeReturnsAreaCoordinateLatitudeLongitudeIfNotNull()
        {
            var coordinate = new Coordinate { Latitude = 43, Longitude = -74 };
            var facilityFacilityArea = new FacilityFacilityArea { Coordinate = coordinate };

            Assert.AreEqual(coordinate.Latitude, facilityFacilityArea.Latitude);
            Assert.AreEqual(coordinate.Longitude, facilityFacilityArea.Longitude);
        }

        [TestMethod]
        public void TestLatitudeReturnsNullIfCoordinateNull()
        {
            var facility = new Facility();

            Assert.IsNull(facility.Latitude);
            Assert.IsNull(facility.Longitude);
        }

        #endregion
    }
}
