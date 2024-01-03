using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for TownSectionIntegrationTest
    /// </summary>
    [TestClass]
    public class TownSectionIntegrationTest : WorkOrdersTestClass<TownSection>
    {
        #region Constants

        private const short REFERENCE_TOWN_SECTION_ID = 33;
        public const int MIN_SAMPLE_COUNT = 23;
        public const string REFERENCE_TOWN_SECTION_NAME = "SHARK RIVER HILLS";

        #endregion

        #region Private Static Members

        private static TownSection _referenceTownSection;

        #endregion

        #region Static Properties

        public static TownSection ReferenceTownSection
        {
            get
            {
                if (_referenceTownSection == null)
                    _referenceTownSection = TownSectionRepository.GetTownSectionByNameAndTownID(
                        REFERENCE_TOWN_SECTION_NAME, TownIntegrationTest.REFERENCE_TOWN_ID);
                return _referenceTownSection;
            }
        }

        #endregion

        #region Private Methods

        protected override TownSection GetValidObject()
        {
            return TownSectionRepository.GetEntity(REFERENCE_TOWN_SECTION_ID);
        }

        protected override TownSection GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(TownSection entity)
        {
            TownSectionRepository.Delete(entity);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void TownSectionIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void TownSectionIntegrationTestCleanup()
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
                MyAssert.IsGreaterThanOrEqualTo(
                    new TownSectionRepository().Count(),
                    MIN_SAMPLE_COUNT,
                    "Your test database has less than the expected number of records in tblNJAWTwnSection.  " +
                    disclaimer);

                Assert.IsNotNull(ReferenceTownSection,
                                 "Reference TownSection object does not seem to exist in the test database.  " +
                                 disclaimer);
            }
        }

        [TestMethod]
        public void TestRepositorySelectByTownName()
        {
            using (_simulator.SimulateRequest())
            {
                var town = TownIntegrationTest.ReferenceTown;
                var townSections = TownSectionRepository.SelectByTownID(town.TownID);

                foreach (var target in townSections)
                    Assert.AreEqual(town.TownID, target.TownID);
            }
        }

        [TestMethod]
        public void TestCannotCreateNewTownSection()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new TownSection();

                MyAssert.Throws(() => TownSectionRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotAlterTownSection()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.Name = "TestName";

                MyAssert.Throws(() => TownSectionRepository.Update(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteTownSection()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(() => TownSectionRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }
    }
}