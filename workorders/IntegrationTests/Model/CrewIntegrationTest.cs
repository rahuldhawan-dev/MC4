using System;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for CrewIntegrationTest
    /// </summary>
    [TestClass]
    public class CrewIntegrationTest : WorkOrdersTestClass<Crew>
    {
        #region Constants

        // Test Crew 1
        public const short DEFAULT_CREW_ID = 1;

        #endregion

        #region Private Members

        private Crew _target;
        private IDisposable _simulatedRequest;

        #endregion

        #region Exposed Static Methods

        public static Crew GetValidCrew()
        {
            return CrewRepository.GetEntity(DEFAULT_CREW_ID);
        }

        public static void DeleteCrew(Crew entity)
        {
            CrewRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        [Obsolete]
        protected override Crew GetValidObject()
        {
            throw new NotImplementedException("Do not use this obsolete method.");
        }

        [Obsolete]
        protected override Crew GetValidObjectFromDatabase()
        {
            throw new NotImplementedException("Do not use this obsolete method.");
        }

        protected override void DeleteObject(Crew entity)
        {
            DeleteCrew(entity);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void CrewIntegrationTestInitailize()
        {
            _simulatedRequest = _simulator.SimulateRequest();
            _target = new TestCrewBuilder();
        }

        [TestCleanup]
        public void CrewIntegrationTestCleanup()
        {
            _simulatedRequest.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestRepositoryDoesNotDoubleInsert()
        {
            var instance = new CrewRepository();
            var count = instance.Count();

            CrewRepository.Insert(_target);
            CrewRepository.Delete(_target);

            Assert.AreEqual(count, instance.Count());
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }
    }

    internal class TestCrewBuilder : TestDataBuilder<Crew>
    {
        #region Constants

        private const string DEFAULT_TEST_CREW_DESCRIPTION = "Test Crew";

        #endregion

        #region Private Members

        private string _description = DEFAULT_TEST_CREW_DESCRIPTION;

        private OperatingCenter _operatingCenter =
            OperatingCenterIntegrationTest.GetValidOperatingCenter();

        private decimal? _availability = 6;

        #endregion

        #region Exposed Methods

        public override Crew Build()
        {
            var crew = new Crew();
            if (_description != null)
                crew.Description = _description;
            if (_operatingCenter != null)
                crew.OperatingCenter = _operatingCenter;
            if (_availability != null)
                crew.Availability = _availability.Value;
            return crew;
        }

        public TestCrewBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public TestCrewBuilder WithOperatingCenter(OperatingCenter operatingCenter)
        {
            _operatingCenter = operatingCenter;
            return this;
        }

        public TestCrewBuilder WithAvailability(decimal? availability)
        {
            _availability = availability;
            return this;
        }

        #endregion
    }
}
