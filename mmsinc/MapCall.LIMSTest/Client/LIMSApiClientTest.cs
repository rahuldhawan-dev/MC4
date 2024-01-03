using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;
using log4net;
using MapCall.LIMS.Client;
using MapCall.LIMS.Configuration;
using MapCall.LIMS.Model.Entities;
using MMSINC.Testing.Utilities;

namespace MapCall.LIMSTest.Client
{
    [TestClass]
    public class LIMSApiClientTest
    {
        #region Private Members

        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject(new Mock<ILog>().Object);
        }

        #endregion

        [TestMethod]
        public void TestGetProfilesDeserializesProperly()
        {
            var httpClientMock = new APIMClientMockFactory<Profile[]>(new[] {
                new Profile { Number = 205, PublicWaterSupplyIdentifier = "M02010519", AnalysisType = "CHEM" },
                new Profile { Number = 206, PublicWaterSupplyIdentifier = "M05010413", AnalysisType = "BACT" }
            });

            var limsApiClient = new LIMSApiClient(
                _container.GetInstance<ILog>(), 
                _container.GetInstance<LIMSClientConfiguration>(),
                httpClientMock);

            var apiResponse = limsApiClient.GetProfiles();

            Assert.IsNotNull(apiResponse);

            var profile205 = apiResponse[0];
            var profile206 = apiResponse[1];

            Assert.AreEqual(2, apiResponse.Length);
            Assert.AreEqual(205, profile205.Number);
            Assert.AreEqual("M02010519", profile205.PublicWaterSupplyIdentifier);
            Assert.AreEqual("CHEM", profile205.AnalysisType);

            Assert.AreEqual(206, profile206.Number);
            Assert.AreEqual("M05010413", profile206.PublicWaterSupplyIdentifier);
            Assert.AreEqual("BACT", profile206.AnalysisType);
        }

        [TestMethod]
        public void TestCreateLocationDeserializesProperly()
        {
            var expectedLocation = new Location {
                Active = "active",
                Address = "address",
                City = "city",
                State = "state",
                ClientId = "client-id",
                SiteId = "site-id", 
                PrimaryStationCode = "primary-station-code",
                FacilityId = "facility-id",
                Latitude = "13.000",
                Longitude = "-12.000",
                LocationDescription = "location-description", 
                LocationName = "location-name",
                ProfileNumber = "200", 
                PublicWaterSupplyIdentifier = "pwsid-01",
                SampleSiteId = "1",
                Zip = "zip",
                LocationSequenceNumber = 1
            };

            var httpClientMock = new APIMClientMockFactory<Location[]>(new Location[] {
                expectedLocation
            });

            var limsApiClient = new LIMSApiClient(
                _container.GetInstance<ILog>(),
                _container.GetInstance<LIMSClientConfiguration>(),
                httpClientMock);

            var actualLocation = limsApiClient.CreateLocation(new Location {
                Active = "active",
                Address = "address",
                City = "city",
                State = "state",
                ClientId = "client-id",
                SiteId = "site-id",
                PrimaryStationCode = "primary-station-code",
                FacilityId = "facility-id",
                Latitude = "13.000",
                Longitude = "-12.000",
                LocationDescription = "location-description",
                LocationName = "location-name",
                ProfileNumber = "200",
                PublicWaterSupplyIdentifier = "pwsid-01",
                SampleSiteId = "1",
                Zip = "zip"
            });

            Assert.AreEqual(expectedLocation.Active, actualLocation.Active);
            Assert.AreEqual(expectedLocation.Address, actualLocation.Address);
            Assert.AreEqual(expectedLocation.City, actualLocation.City);
            Assert.AreEqual(expectedLocation.State, actualLocation.State);
            Assert.AreEqual(expectedLocation.ClientId, actualLocation.ClientId);
            Assert.AreEqual(expectedLocation.PrimaryStationCode, actualLocation.PrimaryStationCode);
            Assert.AreEqual(expectedLocation.FacilityId, actualLocation.FacilityId);
            Assert.AreEqual(expectedLocation.Latitude, actualLocation.Latitude);
            Assert.AreEqual(expectedLocation.Longitude, actualLocation.Longitude);
            Assert.AreEqual(expectedLocation.LocationDescription, actualLocation.LocationDescription);
            Assert.AreEqual(expectedLocation.LocationName, actualLocation.LocationName);
            Assert.AreEqual(expectedLocation.ProfileNumber, actualLocation.ProfileNumber);
            Assert.AreEqual(expectedLocation.PublicWaterSupplyIdentifier, actualLocation.PublicWaterSupplyIdentifier);
            Assert.AreEqual(expectedLocation.SampleSiteId, actualLocation.SampleSiteId);
            Assert.AreEqual(expectedLocation.Zip, actualLocation.Zip);
            Assert.AreEqual(expectedLocation.LocationSequenceNumber, actualLocation.LocationSequenceNumber);
        }

        [TestMethod]
        public void TestUpdateLocationDeserializesProperly()
        {
            var expectedLocation = new Location {
                Active = "active",
                Address = "address",
                City = "city",
                State = "state",
                ClientId = "client-id",
                SiteId = "site-id",
                PrimaryStationCode = "primary-station-code",
                FacilityId = "facility-id",
                Latitude = "13.000",
                Longitude = "-12.000",
                LocationDescription = "location-description",
                LocationName = "location-name",
                ProfileNumber = "200",
                PublicWaterSupplyIdentifier = "pwsid-01",
                SampleSiteId = "1",
                Zip = "zip",
                LocationSequenceNumber = 1
            };

            var httpClientMock = new APIMClientMockFactory<Location[]>(new Location[] {
                expectedLocation
            });

            var limsApiClient = new LIMSApiClient(
                _container.GetInstance<ILog>(),
                _container.GetInstance<LIMSClientConfiguration>(),
                httpClientMock);

            var actualLocation = limsApiClient.UpdateLocation(new Location {
                Active = "active",
                Address = "address",
                City = "city",
                State = "state",
                ClientId = "client-id",
                SiteId = "site-id",
                PrimaryStationCode = "primary-station-code",
                FacilityId = "facility-id",
                Latitude = "13.000",
                Longitude = "-12.000",
                LocationDescription = "location-description",
                LocationName = "location-name",
                ProfileNumber = "200",
                PublicWaterSupplyIdentifier = "pwsid-01",
                SampleSiteId = "1",
                Zip = "zip",
                LocationSequenceNumber = 1
            });

            Assert.AreEqual(expectedLocation.Active, actualLocation.Active);
            Assert.AreEqual(expectedLocation.Address, actualLocation.Address);
            Assert.AreEqual(expectedLocation.City, actualLocation.City);
            Assert.AreEqual(expectedLocation.State, actualLocation.State);
            Assert.AreEqual(expectedLocation.ClientId, actualLocation.ClientId);
            Assert.AreEqual(expectedLocation.PrimaryStationCode, actualLocation.PrimaryStationCode);
            Assert.AreEqual(expectedLocation.FacilityId, actualLocation.FacilityId);
            Assert.AreEqual(expectedLocation.Latitude, actualLocation.Latitude);
            Assert.AreEqual(expectedLocation.Longitude, actualLocation.Longitude);
            Assert.AreEqual(expectedLocation.LocationDescription, actualLocation.LocationDescription);
            Assert.AreEqual(expectedLocation.LocationName, actualLocation.LocationName);
            Assert.AreEqual(expectedLocation.ProfileNumber, actualLocation.ProfileNumber);
            Assert.AreEqual(expectedLocation.PublicWaterSupplyIdentifier, actualLocation.PublicWaterSupplyIdentifier);
            Assert.AreEqual(expectedLocation.SampleSiteId, actualLocation.SampleSiteId);
            Assert.AreEqual(expectedLocation.Zip, actualLocation.Zip);
            Assert.AreEqual(expectedLocation.LocationSequenceNumber, actualLocation.LocationSequenceNumber);
        }
    }
}
