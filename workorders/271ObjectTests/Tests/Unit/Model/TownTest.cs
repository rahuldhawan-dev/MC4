using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for TownTestTest
    /// </summary>
    [TestClass]
    public class TownTest : WorkOrdersTestClass<Town>
    {
        #region Constants

        // NEPTUNE
        public const int REFERENCE_TOWN_ID = 64;

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

        [TestMethod]
        public void TestToStringMethodReflectsNameProperty()
        {
            const string townName = "Township of Springwood";
            var target = new Town {
                Name = townName
            };

            Assert.AreEqual(townName, target.ToString(),
                "Town#ToString() should reflect the Name property.");
        }

        [TestMethod]
        public void TestTownID()
        {
            // may seem like a dumb test, but this was set at RecID
            // (and still is in the db table).  this test won't even
            // compile if that fix gets broken.
            
            const int expected = 1;
            var target = new Town {
                TownID = expected
            };

            Assert.AreEqual(expected, target.TownID);
        }

        [TestMethod]
        public void TestCountyReturnsCountyName()
        {
            var county = new County() {
                Name = "Monmouth"
            };
            var target = new Town() {
                County = county
            };

            Assert.AreEqual(county.Name, target.County.ToString());
        }

        [TestMethod]
        public void TestStateReturnsCountysStateName()
        {
            var state = new State() { Name = "New Jersey" };
            var county = new County() { Name = "Monmouth", State = state };
            var target = new Town() { County = county };

            Assert.AreEqual(state.Name, target.State.ToString());
        }
    }

    internal class TestTownBuilder : TestDataBuilder<Town>
    {
        #region Private Members

        private string _name;
        private State _state;
        private County _county;

        #endregion

        #region Exposed Methods

        public override Town Build()
        {
            var town = new Town();
            if (_name != null)
                town.Name = _name;
            if (_county != null)
                town.County = _county;
            if (_state != null && _county != null)
                _county.State = _state;
            return town;
        }

        public TestTownBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public TestTownBuilder WithCounty(County county)
        {
            _county = county;
            return this;
        }

        public TestTownBuilder WithState(State state)
        {
            _state = state;
            return this;
        }

        #endregion
    }
}
