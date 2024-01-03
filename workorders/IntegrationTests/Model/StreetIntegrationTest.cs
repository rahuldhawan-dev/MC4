using System;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for StreetIntegrationTest
    /// </summary>
    [TestClass]
    public class StreetIntegrationTest : WorkOrdersTestClass<Street>
    {
        #region Constants

        public const int MIN_SAMPLE_COUNT = 5635;
        public const string REFERENCE_STREET_NAME = "BRIGHTON";
        public const string REFERENCE_CROSS_STREET_NAME = "WOODMERE";
        // Brighton Ave, Shark River Hills, Neptune, NJ
        private const short REFERENCE_STREET_ID = 7067;

        #endregion

        #region Private Static Members

        private static Street _referenceStreet, _referenceCrossStreet;

        #endregion

        #region Static Properties

        public static Street ReferenceStreet
        {
            get
            {
                if (_referenceStreet == null)
                    _referenceStreet = StreetRepository.GetStreetByNameAndTownID(
                        REFERENCE_STREET_NAME, TownIntegrationTest.REFERENCE_TOWN_ID);
                return _referenceStreet;
            }
        }

        public static Street ReferenceCrossStreet
        {
            get
            {
                if (_referenceCrossStreet == null)
                    _referenceCrossStreet = StreetRepository.GetStreetByNameAndTownID(
                        REFERENCE_CROSS_STREET_NAME, TownIntegrationTest.REFERENCE_TOWN_ID);
                return _referenceCrossStreet;
            }
        }

        #endregion

        #region Private Methods

        protected override Street GetValidObject()
        {
            return StreetRepository.GetEntity(REFERENCE_STREET_ID);
        }

        protected override Street GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(Street entity)
        {
            StreetRepository.Delete(entity);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void StreetIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void StreetIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestSampleData()
        {
            const string disclaimer = "This may have undesirable consequences on some automated tests.  Please refer to the MMSINC Wiki for " +
                                      "proper setup instructions for this project.";

            using (_simulator.SimulateRequest())
            {
                MyAssert.IsGreaterThanOrEqualTo(new StreetRepository().Count(),
                                                MIN_SAMPLE_COUNT, "Your test database has less than the expected number of records in Streets.  " +
                                                                  disclaimer);
                Assert.IsNotNull(ReferenceStreet, "Reference Street object does not seem to exist in the test database.  " +
                                                  disclaimer);
                Assert.IsNotNull(ReferenceCrossStreet, "Reference Cross Street object does not seem to exist in the test database.  " +
                                                       disclaimer);
            }
        }

        [TestMethod]
        public void TestRepositorySelectByTownID()
        {
            using (_simulator.SimulateRequest())
            {
                var town = TownIntegrationTest.ReferenceTown;
                var streets = StreetRepository.SelectByTownID(town.TownID);

                foreach (var target in streets)
                    Assert.AreEqual(town.TownID, target.TownID);
                
            }
        }

        [TestMethod]
        public void TestRepositorySelectByTownIDReturnsStreetsAlphabetically()
        {
            string last = null;
            using (_simulator.SimulateRequest())
            {
                var town = TownIntegrationTest.ReferenceTown;
                foreach (var target in StreetRepository.SelectByTownID(town.TownID))
                {
                    if (!String.IsNullOrEmpty(last))
                        Assert.IsTrue(target.StreetName.CompareTo(last) >= 0);
                    last = target.StreetName;
                }
            }
        }

        [TestMethod]
        public void TestAttachedValves()
        {
            using (_simulator.SimulateRequest())
            {
                Assert.IsTrue(ReferenceStreet.Valves.Count >= 44,
                                "There should be 44 valves attached to the reference street, Brighton Avenue in Neptune, NJ.");

                Assert.AreEqual(13,
                                ReferenceCrossStreet.Valves.Count,
                                "There should be 13 valves attached to the reference cross street, Woodmere Avenue in Neptune, NJ.");
            }
        }

        [TestMethod]
        public void TestRepositorySelectAllAsListLimitedToACount()
        {
            var count = 20;

            using (_simulator.SimulateRequest())
                Assert.AreEqual(count,
                                StreetRepository.SelectAllAsList(count).Count);
        }

        [TestMethod]
        public void TestRepositorySelectAllAsListReturnsStreetsAlphabetically()
        {
            var count = 50;
            string last = null;

            using (_simulator.SimulateRequest())
            {
                foreach (var target in StreetRepository.SelectAllAsList(count))
                {
                    if (!String.IsNullOrEmpty(last))
                        Assert.IsTrue(target.StreetName?.ToLower().CompareTo(last) >= 0,
                            String.Format(
                                "alphabetization seems to have failed. {0} should come before {1}",
                                last, target.StreetName));
                    last = target.StreetName?.ToLower();
                }
            }
        }

        [TestMethod]
        public void TestCannotCreateNewStreet()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new Street();

                MyAssert.Throws(() => StreetRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotAlterStreet()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.StreetName = "Elm";

                MyAssert.Throws(() => StreetRepository.Update(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteStreet()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(() => StreetRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }
    }
}