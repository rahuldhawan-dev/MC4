using System;
using System.Data.Linq;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Library;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for MarkoutTestTest
    /// </summary>
    [TestClass]
    public class MarkoutTest : WorkOrdersTestClass<Markout>
    {
        #region Private Members

        private TestMarkout _target;
        private IRepository<Markout> _repository;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void MarkoutTestInitialize()
        {
            _target = new TestMarkoutBuilder();
            _repository = new MockMarkoutRepository();
        }

        protected override Markout GetValidObjectFromDatabase()
        {
            throw new NotImplementedException();
        }

        protected override void DeleteObject(Markout entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestExpiration()
        {
            _target = new TestMarkoutBuilder()
               .WithExpirationDate(DateTime.Now.GetNextDay());

            Assert.IsFalse(_target.IsExpired,
                "Markout with a DueDate that has not yet passed should not be considered expired");

            _target = new TestMarkoutBuilder()
                .WithExpirationDate(DateTime.Now.GetPreviousDay());
            Assert.IsTrue(_target.IsExpired,
                "Markout with a Expiration that has passed should be considered expired");
        }

        #endregion

        #region Validation Tests

        [TestMethod]
        public void TestCreateNewMarkout()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutMarkoutNumber()
        {
            _target = new TestMarkoutBuilder().WithMarkoutNumber(null);

            MyAssert.Throws(() => _repository.InsertNewEntity(_target),
                typeof(DomainLogicException),
                "Attempting to save a Markout object without a value for MarkoutNumber should throw an exception");
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            _target = new TestMarkoutBuilder().WithWorkOrder(null);

            MyAssert.Throws(() => _repository.InsertNewEntity(_target),
                typeof(DomainLogicException),
                "Trying to save a Markout without a linked WorkOrder should throw an exception");
        }

        [TestMethod]
        public void TestCannotChangeWorkOrderAfterSave()
        {
            _repository.InsertNewEntity(_target);

            MyAssert.Throws<DomainLogicException>(
                () => _target.WorkOrder = new WorkOrder());
        }

        [TestMethod]
        public void TestReadyDatePropertyPopulatesItselfBasedOnDateOfRequestOnSave()
        {
            var today = DateTime.Today.Date;
            var expected = WorkOrdersWorkDayEngine.GetReadyDate(today,
                MarkoutRequirementEnum.Routine);

            _repository.InsertNewEntity(_target);

            Assert.AreEqual(expected, _target.ReadyDate.Date);
        }

        [TestMethod]
        public void TestExpirationDatePropertyPopulatesItselfBasedOnDateOfRequestOnSave()
        {
            var today = DateTime.Today.Date;
            var expected = WorkOrdersWorkDayEngine.GetExpirationDate(today,
                MarkoutRequirementEnum.Routine);

            _repository.InsertNewEntity(_target);

            Assert.AreEqual(expected, _target.ExpirationDate.Date);

            // TODO: change these around if necessary.

            // simulate work being assigned, should not extend expiration date
            _target = new TestMarkoutBuilder();
            _target.WorkOrder.CrewAssignments.Add(new CrewAssignment {
                Crew = new Crew(),
                AssignedFor = expected
            });

            _repository.InsertNewEntity(_target);
            Assert.AreEqual(expected, _target.ExpirationDate.Date);

            // simulate work being done, should extend expiration date
            _target = new TestMarkoutBuilder();
            _target.WorkOrder.CrewAssignments.Add(new CrewAssignment {
                Crew = new Crew(),
                DateStarted = expected.EndOfDay()
            });
            expected = WorkOrdersWorkDayEngine.GetExpirationDate(today,
                MarkoutRequirementEnum.Routine, true);

            _repository.InsertNewEntity(_target);

            Assert.AreEqual(expected, _target.ExpirationDate.Date);
        }

        [TestMethod]
        public void TestChangingDateCalledUpdatesReadyAndExpirationDates()
        {
            var requirement = MarkoutRequirementEnum.Routine;
            var today = DateTime.Today.Date;
            var yesterday = today.AddDays(-1);

            _target = new TestMarkoutBuilder()
                .WithDateOfRequest(today);

            _repository.InsertNewEntity(_target);

            _target.DateOfRequest = yesterday;

            _repository.UpdateCurrentEntity(_target);

            var newReadyDate =
                WorkOrdersWorkDayEngine.GetReadyDate(yesterday, requirement);
            var newExpirationDate =
                WorkOrdersWorkDayEngine.GetExpirationDate(yesterday, requirement);

            Assert.AreEqual(newReadyDate, _target.ReadyDate);
            Assert.AreEqual(newExpirationDate, _target.ExpirationDate);
        }

        [TestMethod]
        public void TestNewMarkoutDefaultsDateOfRequestToCurrentDateOnSave()
        {
            _target = new TestMarkoutBuilder()
                .WithDateOfRequest(null);

            _repository.InsertNewEntity(_target);

            Assert.AreEqual(DateTime.Today.Date, _target.DateOfRequest.Date);
        }

        [TestMethod]
        public void TestMarkoutEntitSetGetCurrentExtension()
        {
            var target = new EntitySet<Markout>();
            Assert.IsNull(target.GetCurrent(),
                "GetCurrent() on an empty EntitySet<Markouts> should return null.");

            var markout = new Markout();
            target.Add(markout);
            Assert.AreSame(markout, target.GetCurrent(),
                "EntitySet<Markout>.GetCurrent() failed.");

            var markout2 = new Markout();
            target.Add(markout2);
            Assert.AreSame(markout2, target.GetCurrent());
        }
        
        #endregion
    }

    internal class TestMarkoutBuilder : TestDataBuilder<TestMarkout>
    {
        #region Constants

        private const string MARKOUT_NUMBER = "123456789";

        #endregion

        #region Private Members

        private DateTime? _expirationDate, _dateOfRequest, _readyDate;

        private WorkOrder _order = new TestWorkOrderBuilder();
        private string _markoutNumber = MARKOUT_NUMBER;

        #endregion

        #region Exposed Methods

        public override TestMarkout Build()
        {
            var mo = new TestMarkout();
            if (_dateOfRequest != null)
                mo.DateOfRequest = _dateOfRequest.Value;
            if (_order != null)
                mo.WorkOrder = _order;
            if (_markoutNumber != null)
                mo.MarkoutNumber = _markoutNumber;
            // need to set these after the work order is set
            if (_readyDate != null)
                mo.SetReadyDate(_readyDate.Value);
            if (_expirationDate != null)
                mo.SetExpirationDate(_expirationDate.Value);
            return mo;
        }

        public TestMarkoutBuilder WithExpirationDate(DateTime? expirationDate)
        {
            _expirationDate = expirationDate;
            return this;
        }

        public TestMarkoutBuilder WithDateOfRequest(DateTime? dateOfRequest)
        {
            _dateOfRequest = dateOfRequest;
            return this;
        }

        public TestMarkoutBuilder WithWorkOrder(WorkOrder order)
        {
            _order = order;
            return this;
        }
        
        public TestMarkoutBuilder WithMarkoutNumber(string markoutNumber)
        {
            _markoutNumber = markoutNumber;
            return this;
        }

        public TestMarkoutBuilder WithReadyDate(DateTime date)
        {
            _readyDate = date;
            return this;
        }

        #endregion
    }

    internal class TestMarkout : Markout
    {
        #region Exposed Methods

        public void SetReadyDate(DateTime readyDate)
        {
            _readyDate = readyDate;
        }

        public void SetExpirationDate(DateTime expirationDate)
        {
            _expirationDate = expirationDate;
        }

        #endregion
    }

    internal class MockMarkoutRepository : MockRepository<Markout> {}
}
