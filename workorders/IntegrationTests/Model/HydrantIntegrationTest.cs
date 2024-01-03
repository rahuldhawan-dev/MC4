using System;
using System.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for HydrantIntegrationTest
    /// </summary>
    [TestClass]
    public class HydrantIntegrationTest : WorkOrdersTestClass<Hydrant>
    {
        #region Constants

        private const int MIN_SAMPLE_COUNT = 3975;
        // hydrant on Rivderside Dr, Shark River Hills, Neptune, NJ
        private const int REFERENCE_HYDRANT_ID = 3878;
        private const string REFERENCE_HYDRANT_NUMBER = "HNT-227";
        private const int REFERENCE_HYDRANT_SUFFIX = 227;
        private const double REFERENCE_LATITUDE = 40.191092;
        private const double REFERENCE_LONGITUDE = -74.053444; // update Coordinates set Latitude = '40.191092', Longitude = '-74.053444' where CoordinateID = 925438
        private const string REFERENCE_NEAREST_CROSS_STREET_NAME = "OAKDALE DR";
        private const string REFERENCE_OP_CODE = "NJ7";
        private const string REFERENCE_STATUS = "ACTIVE";
        private const int REFERENCE_STREET_ID = 1727;
        private const int REFERENCE_TOWN_ID = 62;

        #endregion

        #region Private Members

        protected override Hydrant GetValidObject()
        {
            return HydrantRepository.GetEntity(REFERENCE_HYDRANT_ID);
        }

        protected override Hydrant GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(Hydrant entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void HydrantIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void HydrantIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        /// <summary>
        /// These properties are used in the <see cref="MapCall.Common.Utility.ArcCollectorLinkGenerator"/>.
        /// If the mappings are changed in MapCall.Common.Model.Entities, they can be missed in WorkOrders.Model.
        /// This will catch the properties used in the generator if they aren't updated in WorkOrders.Model
        ///
        /// This hits real data from the db. If it's failing oddly, the data may have changed for the
        /// referenced Hydrant
        /// </summary>
        [TestMethod]
        public void TestPropertiesUsedForArcCollectorLinkGeneratorMapCorrectly()
        {
            using (_simulator.SimulateRequest())
            {
                var hydrant = HydrantRepository.GetEntity(REFERENCE_HYDRANT_ID);

                Assert.AreEqual(hydrant.FireDistrictId, hydrant.FireDistrict.FireDistrictID);
                Assert.AreEqual(hydrant.HydrantBillingId, hydrant.HydrantBilling.Id);
                Assert.AreEqual(hydrant.LastUpdatedById, hydrant.LastUpdatedBy.EmployeeID);
                Assert.AreEqual(hydrant.AssetStatusID, hydrant.AssetStatus.AssetStatusID);
                Assert.AreEqual(hydrant.ManufacturerID, hydrant.HydrantManufacturer.Id);
            }
        }
        
        [TestMethod]
        public void TestSampleData()
        {
            using (_simulator.SimulateRequest())
            {
                const string disclaimer = "This may have undesirable consequences on some automated tests.  Please refer to the MMSINC Wiki for " +
                                          "proper setup instructions for this project.";

                MyAssert.IsGreaterThanOrEqualTo(
                    new HydrantRepository().Count(),
                    MIN_SAMPLE_COUNT, 
                    "Your test database has less than the expected number of records in tblNJAWHydrant.  " + disclaimer);

                var hydrant = HydrantRepository.GetEntity(REFERENCE_HYDRANT_ID);
                Assert.AreEqual(REFERENCE_HYDRANT_NUMBER, hydrant.HydrantNumber);
                Assert.AreEqual(REFERENCE_HYDRANT_SUFFIX, hydrant.HydrantSuffix);
                Assert.AreEqual(REFERENCE_LATITUDE, hydrant.Latitude);
                Assert.AreEqual(REFERENCE_LONGITUDE, hydrant.Longitude);
                Assert.AreEqual(REFERENCE_NEAREST_CROSS_STREET_NAME, hydrant.CrossStreet.FullStName);
                Assert.AreEqual(REFERENCE_OP_CODE, hydrant.OperatingCenter.OpCntr);
                Assert.AreEqual(REFERENCE_STATUS, hydrant.AssetStatus.Description);
                Assert.AreEqual(REFERENCE_STREET_ID, hydrant.StreetID);
                Assert.AreEqual(REFERENCE_TOWN_ID, hydrant.TownID);
            }
        }

        [TestMethod]
        public void TestCanAlterHydrant()
        {
            using (_simulator.SimulateRequest())
            {
                var target = (from h in HydrantRepository.SelectAllAsList()
                              where h.Latitude != null && h.Longitude != null
                              select h).First();
                double? oldLongitude = target.Longitude,
                        oldLatitude = target.Latitude,
                        newLongitude = 1.1,
                        newLatitude = 1.1;

                target.Longitude = newLongitude;
                target.Latitude = newLatitude;

                MyAssert.DoesNotThrow(() => HydrantRepository.Update(target));

                target.Longitude = oldLongitude;
                target.Latitude = oldLatitude;

                MyAssert.DoesNotThrow(() => HydrantRepository.Update(target));
            }
        }

        [TestMethod]
        public void TestCannotCreateNewHydrant()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new Hydrant();

                MyAssert.Throws(() => HydrantRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteHydrant()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(() => HydrantRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }
    }
}