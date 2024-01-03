using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class FacilityTest
    {
        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var expected = "Some Facility - NJSB-13";
            var facility = new Facility {
                Id = 13,
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJSB"},
                FacilityName = "Some Facility"
            };

            Assert.AreEqual(expected, facility.ToString());
        }

        [TestMethod]
        public void TestDescriptionReturnsFacilityNameAndFacilityId()
        {
            var expected = "Some Facility - NJSB-13";
            var facility = new Facility {
                Id = 13,
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJSB"},
                FacilityName = "Some Facility"
            };

            Assert.AreEqual(expected, facility.Description);
        }

        [TestMethod]
        public void TestLatitudeReturnsCoordinateLatitudeLongitudeIfNotNull()
        {
            var coordinate = new Coordinate {Latitude = 43, Longitude = -74};
            var facility = new Facility {Coordinate = coordinate};

            Assert.AreEqual(coordinate.Latitude, facility.Latitude);
            Assert.AreEqual(coordinate.Longitude, facility.Longitude);
        }

        [TestMethod]
        public void TestLatitudeReturnsNullIfCoordinateNull()
        {
            var facility = new Facility();

            Assert.IsNull(facility.Latitude);
            Assert.IsNull(facility.Longitude);
        }

        [TestMethod]
        public void TestAddressReturnsNicelyFormattedAddress()
        {
            var facility = new Facility {
                StreetNumber = "666",
                Street = new Street {
                    Prefix = new StreetPrefix {Description = "N"},
                    Name = "Main",
                    Suffix = new StreetSuffix {Description = "St"},
                },
                Town = new Town {FullName = "City", ShortName = "City", State = new State {Abbreviation = "NJ "}},
                ZipCode = "07711"
            };

            Assert.AreEqual(
                String.Format(
                    Facility.ADDRESS_FORMAT,
                    facility.StreetNumber,
                    facility.Street.Prefix,
                    facility.Street.Name,
                    facility.Street.Suffix,
                    facility.Town,
                    facility.Town.State,
                    facility.ZipCode
                ),
                facility.Address);
        }

        [TestMethod]
        public void Test_FacilityToJson_ReturnsCorrectProps()
        {
            var facility = new Facility {
                Id = 1,
                FacilityName = "lolz",
                StreetNumber = "16",
                WasteWaterSystem = new WasteWaterSystem{Id = 1, WasteWaterSystemName = "WW", OperatingCenter = new OperatingCenter{ OperatingCenterCode = "NJ7" } },
                OperatingCenter = new OperatingCenter{OperatingCenterCode = "NJ7"}
            };

            var jsonObject = facility.FacilityToJson().AsDynamic();

            Assert.AreEqual(facility.FacilityName, jsonObject.FacilityName);
            Assert.AreEqual(facility.StreetNumber, jsonObject.StreetNumber);
            Assert.AreEqual(facility.WasteWaterSystem.Description, jsonObject.WasteWaterSystem);
        }
    }
}
