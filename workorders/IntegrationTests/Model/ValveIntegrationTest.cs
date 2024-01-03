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
    /// Summary description for ValveIntegrationTest
    /// </summary>
    [TestClass]
    public class ValveIntegrationTest : WorkOrdersTestClass<Valve>
    {
        #region Constants

        public const int MIN_SAMPLE_COUNT = 16352;
        // valve on Brighton Ave, Shark River Hills, Neptune, NJ
        private const int REFERENCE_VALVE_ID = 53827, REFERENCE_VALVE_ID_WITH_ARC_GENERATOR_PROPERTIES = 39355;
        private const int REFERENCE_VALVE_STREET_ID = 7067;
        private const int REFERENCE_VALVE_TOWN_ID = 62;
        private const string REFERENCE_FULL_VALVE_SUFFIX = "1752";
        private const double REFERENCE_VALVE_LATITUDE = 40.193586; // update Coordinates set Latitude = '40.193586', Longitude = '-74.056421' where CoordinateID = 987105
        private const double REFERENCE_VALVE_LONGITUDE = -74.056421;
        private const string REFERENCE_VALVE_NEAREST_CROSS_STREET_NAME =
            "WOODMERE AVE";
        private const string REFERENCE_VALVE_OP_CODE = "NJ7";
        private const string REFERENCE_VALVE_STATUS = "ACTIVE";
        private const string REFERENCE_VALVE_NUMBER = "VNT-1752";
        private const int REFERENCE_VALVE_SUFFIX = 1752;

        #endregion

        #region Private Methods

        protected override Valve GetValidObject()
        {
            return GetValidObjectFromDatabase();
        }

        protected override Valve GetValidObjectFromDatabase()
        {
            return GetValidValve();
        }

        protected override void DeleteObject(Valve entity)
        {
            DeleteValve(entity);
        }

        #endregion

        #region Exposed Static Methods

        public static Valve GetValidValve()
        {
            return ValveRepository.GetEntity(REFERENCE_VALVE_ID);
        }

        public static void DeleteValve(Valve entity)
        {
            throw new DomainLogicException("Cannot delete Valve objects in this context.");
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void ValveIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void ValveIntegrationTestCleanup()
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
        /// referenced Valve
        /// </summary>
        [TestMethod]
        public void TestPropertiesUsedForArcCollectorLinkGeneratorMapCorrectly()
        {
            using (_simulator.SimulateRequest())
            {
                var valve = ValveRepository.GetEntity(REFERENCE_VALVE_ID_WITH_ARC_GENERATOR_PROPERTIES);

                Assert.AreEqual(valve.ValveControlsId, valve.ValveControls.Id);
                Assert.AreEqual(valve.ValveSizeId, valve.ValveSize.Id);
                Assert.AreEqual(valve.LastUpdatedById, valve.LastUpdatedBy.EmployeeID);
                Assert.AreEqual(valve.AssetStatusID, valve.AssetStatus.AssetStatusID);
                Assert.AreEqual(valve.ValveMakeId, valve.ValveMake.Id);
                Assert.AreEqual(valve.NormalPositionId, valve.NormalPosition.Id);
                Assert.AreEqual(valve.OpensId, valve.OpenDirection.Id);
                Assert.AreEqual(valve.ValveTypeId, valve.ValveType.Id);
            }
        }

        [TestMethod]
        public void TestSampleData()
        {
            const string disclaimer = "This may have undesirable consequences on some automated tests.  Please refer to the MMSINC Wiki for " +
                                      "proper setup instructions for this project.";

            using (_simulator.SimulateRequest())
            {
                MyAssert.IsGreaterThanOrEqualTo(new ValveRepository().Count(),
                                                MIN_SAMPLE_COUNT, "Your test database has less than the expected number of records in tblNJAWValves.  " +
                                                                  disclaimer);

                var valve = ValveRepository.GetEntity(REFERENCE_VALVE_ID);

                Assert.AreEqual(REFERENCE_VALVE_TOWN_ID, valve.TownID);
                Assert.AreEqual(REFERENCE_VALVE_STREET_ID, valve.StreetID);
                Assert.AreEqual(REFERENCE_VALVE_NEAREST_CROSS_STREET_NAME,
                                valve.NearestCrossStreet.FullStName);
                Assert.AreEqual(REFERENCE_VALVE_OP_CODE, valve.OperatingCenter.OpCntr);
                Assert.AreEqual(REFERENCE_VALVE_STATUS, valve.AssetStatus.Description);
                Assert.AreEqual(REFERENCE_FULL_VALVE_SUFFIX, valve.FullValveSuffix);
                Assert.AreEqual(REFERENCE_VALVE_LATITUDE, valve.Latitude);
                Assert.AreEqual(REFERENCE_VALVE_LONGITUDE, valve.Longitude);
                Assert.AreEqual(REFERENCE_VALVE_NUMBER, valve.ValveNumber);
                Assert.AreEqual(REFERENCE_VALVE_SUFFIX, valve.ValveSuffix);
            }
        }

        [TestMethod]
        public void TestRepositorySelectAllAsListLimitedToACount()
        {
            var count = 20;

            using (_simulator.SimulateRequest())
                Assert.AreEqual(count,
                                ValveRepository.SelectAllAsList(count).Count);
        }

        [TestMethod]
        public void TestCanAlterValve()
        {
            using (_simulator.SimulateRequest())
            {
                var target = (from v in ValveRepository.SelectAllAsList()
                              where v.Latitude != null && v.Longitude != null
                              select v).First();
                double? oldLongitude = target.Longitude,
                        oldLatitude = target.Latitude,
                        newLongitude = 1.1,
                        newLatitude = 1.1;

                target.Longitude = newLongitude;
                target.Latitude = newLatitude;

                MyAssert.DoesNotThrow(() => ValveRepository.Update(target));

                target.Longitude = oldLongitude;
                target.Latitude = oldLatitude;

                MyAssert.DoesNotThrow(() => ValveRepository.Update(target));
            }
        }

        [TestMethod]
        public void TestCannotCreateNewValve()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new Valve();

                MyAssert.Throws(() => ValveRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteValve()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(() => ValveRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestStreetAndStreetIDProperites()
        {
            using (_simulator.SimulateRequest())
            {
                var targets = ValveRepository.SelectAllAsList(50);

                foreach (var target in targets)
                    if (target.StreetID != null)
                        Assert.AreEqual(target.Street.StreetID,
                            target.StreetID.Value);
            }
        }

        [TestMethod]
        public void TestTownAndTownIDProperties()
        {
            using (_simulator.SimulateRequest())
            {
                var targets = ValveRepository.SelectAllAsList(50);

                foreach (var target in targets)
                    if (target.TownID != null)
                        Assert.AreEqual(target.Town.TownID,
                            target.TownID.Value);
            }
        }
    }
}