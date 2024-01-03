using System;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for CrewTestTest
    /// </summary>
    [TestClass]
    public class CrewTest
    {
        #region Private Members

        private IRepository<Crew> _repository;
        private TestCrew _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void CrewTestInitialize()
        {
            _repository = new MockCrewRepository();
            _target = new TestCrewBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringMethodReflectsDescriptionProperty()
        {
            var expected = "Test Crew";
            _target = new TestCrewBuilder().WithDescription(expected);

            Assert.AreEqual(expected, _target.ToString());
        }

        [TestMethod]
        public void TestAssigningWorkPrioritizesAssignmentsForAGivenDay()
        {
            WorkOrder order1 = new WorkOrder(),
                      order2 = new WorkOrder(),
                      order3 = new WorkOrder();
            DateTime today = DateTime.Today,
                     tomorrow = DateTime.Today.GetNextDay();
            CrewAssignment assignment1 = new CrewAssignment {
                               WorkOrder = order1,
                               AssignedFor = today
                           },
                           assignment2 = new CrewAssignment {
                               WorkOrder = order2,
                               AssignedFor = today
                           },
                           assignment3 = new CrewAssignment {
                               WorkOrder = order3,
                               AssignedFor = tomorrow
                           };

            _target.CrewAssignments.Add(assignment1);
            _target.CrewAssignments.Add(assignment2);
            _target.CrewAssignments.Add(assignment3);

            Assert.AreEqual(1, assignment1.Priority);
            Assert.AreEqual(2, assignment2.Priority);
            Assert.AreEqual(1, assignment3.Priority);
        }

        [TestMethod]
        public void TestCreateNewCrew()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestAlterCrew()
        {
            MyAssert.DoesNotThrow(()=> _repository.UpdateCurrentEntity(_target));
        }

        [TestMethod]
        public void TestDeleteCrew()
        {
            MyAssert.DoesNotThrow(() => _repository.DeleteEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutDescription()
        {
            _target = new TestCrewBuilder().WithDescription(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutOperatingCenter()
        {
            _target = new TestCrewBuilder().WithOperatingCenter(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutAvailability()
        {
            _target = new TestCrewBuilder().WithAvailability(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestFullDescriptionPropertyReturnsDescriptionAndOpCodeAndOpCenterName()
        {
            _target = new TestCrew {
                Description = "CREW NAME",
                OperatingCenter = new OperatingCenter {
                    OpCntr = "FOO",
                    OpCntrName = "BAR"
                }
            };

            var expected = "FOO BAR - CREW NAME";

            Assert.AreEqual(expected, _target.FullDescription);
        }
    }

    internal class TestCrewBuilder : TestDataBuilder<TestCrew>
    {
        #region Constants

        private const string DEFAULT_TEST_CREW_DESCRIPTION = "Test Crew";

        #endregion

        #region Private Members

        private string _description = DEFAULT_TEST_CREW_DESCRIPTION;

        private OperatingCenter _operatingCenter = new OperatingCenter();

        private decimal? _availability = 6;

        #endregion

        #region Exposed Methods

        public override TestCrew Build()
        {
            var crew = new TestCrew();
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

    internal class TestCrew : Crew
    {
    }

    internal class MockCrewRepository : MockRepository<Crew>
    {
    }
}