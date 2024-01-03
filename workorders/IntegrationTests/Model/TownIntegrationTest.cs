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
    /// Summary description for TownIntegrationTest
    /// </summary>
    [TestClass]
    public class TownIntegrationTest : WorkOrdersTestClass<Town>
    {
        #region Constants

        // NEPTUNE
        public const int REFERENCE_TOWN_ID = 62;
        public const int MIN_SAMPLE_COUNT = 11;
        public const string REFERENCE_TOWN_NAME = "NEPTUNE";

        #endregion

        #region Private Static Members

        private static Town _referenceTown;

        #endregion

        #region Static Properties

        public static Town ReferenceTown
        {
            get
            {
                if (_referenceTown == null)
                    _referenceTown = TownRepository.GetEntity(REFERENCE_TOWN_ID);
                return _referenceTown;
            }
        }

        public static Town GetTown(int townId)
        {
            return TownRepository.GetEntity(townId);
        }

        #endregion

        #region Exposed Static Methods

        public static Town GetValidTown()
        {
            return TownRepository.GetEntity(REFERENCE_TOWN_ID);
        }

        #endregion

        #region Private Methods

        protected override Town GetValidObject()
        {
            return GetValidObjectFromDatabase();
        }

        protected override Town GetValidObjectFromDatabase()
        {
            return GetValidTown();
        }

        protected override void DeleteObject(Town entity)
        {
            throw new DomainLogicException("Cannot delete Town objects in this context.");
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void TownIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void TownIntegrationTestCleanup()
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
                MyAssert.IsGreaterThanOrEqualTo((new TownRepository()).Count(),
                                                MIN_SAMPLE_COUNT, "Your test database has less than the expected number of records in tblNJAWTownNames.  " +
                                                                  disclaimer);
                Assert.IsNotNull(ReferenceTown, "Reference Town object does not seem to exist in the test database.  " +
                                                disclaimer);
            }
        }

        [TestMethod]
        public void TestAttachedValves()
        {
            using (_simulator.SimulateRequest())
            {
                MyAssert.IsGreaterThanOrEqualTo(ReferenceTown.Valves.Count, 2847,
                    "There should be 2818 valves attached to the reference town, Neptune NJ.");
            }
        }

        [TestMethod]
        public void TestRepositorySelectAllSortedReturnsTownsAlphabeticallyByTownName()
        {
            string last = null;
            using (_simulator.SimulateRequest())
            {
                foreach (var town in TownRepository.SelectAllSorted())
                {
                    if (!String.IsNullOrEmpty(last) && !last.Contains("-") && !last.Contains("'"))
                        Assert.IsTrue(town.Name.CompareTo(last) >= 0);
                    last = town.Name;
                }
            }
        }

        [TestMethod]
        public void TestCannotCreateNewTown()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new Town();

                MyAssert.Throws(() => TownRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotAlterTown()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObjectFromDatabase();
                target.Name = "FooTown";

                MyAssert.Throws(() => TownRepository.Update(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteTown()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObjectFromDatabase();

                MyAssert.Throws(() => TownRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }


    }
}