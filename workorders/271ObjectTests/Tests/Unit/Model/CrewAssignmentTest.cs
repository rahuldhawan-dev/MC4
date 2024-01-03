using System;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for CrewAssignmentTestTest
    /// </summary>
    [TestClass]
    public class CrewAssignmentTest
    {
        #region Private Members

        private IRepository<CrewAssignment> _repository;
        private TestCrewAssignment _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void CrewAssignmentTestInitialize()
        {
            _repository = new MockCrewAssignmentRepository();
            _target = new TestCrewAssignmentBuilder();
        }

        #endregion

        [TestMethod]
        public void TestAssignedOnPropertySetsSelfWithCurrentDateWhenNotSpecified()
        {
            var expected = DateTime.Today;
            _target = new TestCrewAssignment();

            Assert.AreEqual(expected.Date, _target.AssignedOn.Date);
        }

        [TestMethod]
        public void TestCreateNewCrewAssignmentWithOperatingCenterCrew()
        {
            _target.Crew.OperatingCenterID = 0;
            MyAssert.DoesNotThrow(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCreateNewCrewAssignmentWithContractorCrew()
        {
            _target.Crew.ContractorID = 0;
            MyAssert.DoesNotThrow(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutCrew()
        {
            _target = new TestCrewAssignmentBuilder().WithCrew(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            _target = new TestCrewAssignmentBuilder().WithWorkOrder(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutAssignmentDate()
        {
            _target =
                new TestCrewAssignmentBuilder().WithAssignmentDate(
                    default(DateTime));

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutPriority()
        {
            _target = new TestCrewAssignmentBuilder().WithPriority(default(int));

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithWorkOrderWithMarkoutNotReadyByAssignedDate()
        {
            var assignmentDate = DateTime.Today;
            var markoutReadyDate = assignmentDate.AddDays(1);
            var markoutExpirationDate = assignmentDate.AddDays(2);
            var operatingCenter = new OperatingCenter { OperatingCenterID = 1 };
            var order = new TestWorkOrderBuilder()
                       .WithMarkoutRequirement(TestMarkoutRequirementBuilder.Routine)
                       .WithOperatingCenter(operatingCenter)
                       .Build();
            order.Markouts.Add(new TestMarkoutBuilder()
                              .WithReadyDate(markoutReadyDate)
                              .WithExpirationDate(markoutExpirationDate)
                              .WithWorkOrder(order));
            var crew = new TestCrewBuilder()
               .WithOperatingCenter(operatingCenter);

            _target = new TestCrewAssignmentBuilder()
                     .WithCrew(crew)
                     .WithAssignmentDate(assignmentDate)
                     .WithWorkOrder(order);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCanSaveWithWorkOrderWithMarkoutReadyOnAssignedDate()
        {
            var assignmentDate = DateTime.Today;
            var markoutReadyDate = assignmentDate.Date;
            var markoutExpirationDate = assignmentDate.AddDays(2);
            var operatingCenter = new OperatingCenter { OperatingCenterID = 1 };
            var order = new TestWorkOrderBuilder()
                       .WithMarkoutRequirement(TestMarkoutRequirementBuilder.Routine)
                       .WithOperatingCenter(operatingCenter)
                       .Build();
            order.Markouts.Add(new TestMarkoutBuilder()
                              .WithReadyDate(markoutReadyDate)
                              .WithExpirationDate(markoutExpirationDate)
                              .WithWorkOrder(order));
            var crew = new TestCrewBuilder()
               .WithOperatingCenter(operatingCenter);

            _target = new TestCrewAssignmentBuilder()
                     .WithCrew(crew)
                     .WithAssignmentDate(assignmentDate)
                     .WithWorkOrder(order);

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCanSaveWithWorkOrderWithMultipleMarkoutsWhereOneIsReadyByAssignedDate()
        {
            var assignmentDate = DateTime.Today;
            var order = new TestWorkOrderBuilder()
                       .WithMarkoutRequirement(TestMarkoutRequirementBuilder.Routine).Build();
            order.Markouts.Add(new TestMarkoutBuilder()
                              .WithReadyDate(assignmentDate.AddDays(-1))
                              .WithExpirationDate(assignmentDate.AddDays(1))
                              .WithWorkOrder(order));
            order.Markouts.Add(new TestMarkoutBuilder()
                              .WithReadyDate(assignmentDate.AddDays(1))
                              .WithExpirationDate(assignmentDate.AddDays(1))
                              .WithWorkOrder(order));
            var crew = new TestCrewBuilder()
               .WithOperatingCenter(order.OperatingCenter);

            _target = new TestCrewAssignmentBuilder()
                     .WithCrew(crew)
                     .WithAssignmentDate(assignmentDate)
                     .WithWorkOrder(order);

            MyAssert.DoesNotThrow(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithWorkOrderWithMarkoutExpiredBeforeAssignedDate()
        {
            var markoutExpirationDate = DateTime.Today;
            var assignmentDate = markoutExpirationDate.AddDays(1);
            var order = new TestWorkOrderBuilder()
                .WithMarkoutRequirement(TestMarkoutRequirementBuilder.Routine).Build();
            order.Markouts.Add(new TestMarkoutBuilder()
                .WithExpirationDate(markoutExpirationDate)
                .WithWorkOrder(order));

            _target = new TestCrewAssignmentBuilder()
                .WithAssignmentDate(assignmentDate)
                .WithWorkOrder(order);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCanSaveWithWorkOrderWithMultipleMarkoutsWhereOneIsNotExpiredBeforeAssignedDate()
        {
            var assignmentDate = DateTime.Today;
            var order = new TestWorkOrderBuilder()
                       .WithMarkoutRequirement(TestMarkoutRequirementBuilder.Routine).Build();
            order.Markouts.Add(new TestMarkoutBuilder()
                              .WithReadyDate(assignmentDate.AddDays(-2))
                              .WithExpirationDate(assignmentDate.AddDays(1))
                              .WithWorkOrder(order));
            order.Markouts.Add(new TestMarkoutBuilder()
                              .WithReadyDate(assignmentDate.AddDays(-2))
                              .WithExpirationDate(assignmentDate.AddDays(-1))
                              .WithWorkOrder(order));
            var crew = new TestCrewBuilder()
               .WithOperatingCenter(order.OperatingCenter);

            _target = new TestCrewAssignmentBuilder()
                     .WithCrew(crew)
                     .WithAssignmentDate(assignmentDate)
                     .WithWorkOrder(order);

            MyAssert.DoesNotThrow(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestTimeToCompleteReturnsDifferenceBetweenStartAndEndTimesIfSet()
        {
            var start = DateTime.Now;
            var end = start.AddHours(5).AddMinutes(30);
            var expected = end - start;
            _target = new TestCrewAssignmentBuilder()
                .WithDateStarted(start)
                .WithDateEnded(end);

            Assert.AreEqual(expected, _target.TimeToComplete);
        }

        [TestMethod]
        public void TestTimeToCompleteReturnsNullIfStartOrEndTimeNotSet()
        {
            _target = new TestCrewAssignmentBuilder();

            Assert.IsNull(_target.TimeToComplete);
        }

        [TestMethod]
        public void TestCannotSetEndTimeWithoutAValueForStartTime()
        {
            _target = new TestCrewAssignmentBuilder();

            MyAssert.Throws<DomainLogicException>(
                () => _target.DateEnded = DateTime.Now);
        }

        [TestMethod]
        public void TestCannotSaveWithEndTimeAndNoValueForEmployeesOnJob()
        {
            _target = new TestCrewAssignmentBuilder()
                .WithDateStarted(DateTime.Now.AddHours(-1))
                .WithDateEnded(DateTime.Now);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target));
        }

        [TestMethod]
        public void TestCannotSetEmployeesOnJobWithoutAValueForEndTime()
        {
            _target = new TestCrewAssignmentBuilder();

            MyAssert.Throws<DomainLogicException>(
                () => _target.EmployeesOnJob = 1);
        }

        [TestMethod]
        public void TestCannotSetEmployeesOnJobToAValueLessThanOne()
        {
            _target = new TestCrewAssignmentBuilder()
                .WithDateStarted(DateTime.Now.AddHours(-1))
                .WithDateEnded(DateTime.Now);

            MyAssert.Throws<DomainLogicException>(
                () => _target.EmployeesOnJob = 0);

            MyAssert.Throws<DomainLogicException>(
                () => _target.EmployeesOnJob = -1);

            MyAssert.Throws<DomainLogicException>(
                () => _target.EmployeesOnJob = 0.5f);
        }

        [TestMethod]
        public void TestCannotSetEmployeesOnJobWithADecimalValueOtherThanHalf()
        {
            _target = new TestCrewAssignmentBuilder()
                .WithDateStarted(DateTime.Now.AddHours(-1))
                .WithDateEnded(DateTime.Now);

            MyAssert.Throws<DomainLogicException>(
                () => _target.EmployeesOnJob = 1.25f);

            MyAssert.Throws<DomainLogicException>(
                () => _target.EmployeesOnJob = 1.75f);
        }

        [TestMethod]
        public void TestCannotSetEmployeesOnJobToNull()
        {
            _target = new TestCrewAssignmentBuilder()
                .WithDateStarted(DateTime.Now.AddHours(-1))
                .WithDateEnded(DateTime.Now)
                .WithEmployeesOnJob(1);

            MyAssert.Throws<DomainLogicException>(
                () => _target.EmployeesOnJob = null);
        }

        [TestMethod]
        public void TestSaveWithEmployeesOnJob()
        {
            _target = new TestCrewAssignmentBuilder()
                .WithDateStarted(DateTime.Now.AddHours(-1))
                .WithDateEnded(DateTime.Now)
                .WithEmployeesOnJob(1.5f);

            MyAssert.DoesNotThrow(() => _repository.UpdateCurrentEntity(_target));
        }

        [TestMethod]
        public void TestTotalManHoursReturnsNullWhenAssignmentNotStarted()
        {
            _target = new TestCrewAssignmentBuilder();

            Assert.IsNull(_target.TotalManHours);
        }

        [TestMethod]
        public void TestTotalManHoursReturnsNullWhenAssignmentNotEnded()
        {
            _target = new TestCrewAssignmentBuilder()
                .WithDateStarted(DateTime.Now);

            Assert.IsNull(_target.TotalManHours);
        }

        [TestMethod]
        public void TestTotalManHoursReturnsHoursToCompleteMultipliedByEmployeesOnJob()
        {
            _target = new TestCrewAssignmentBuilder()
                .WithDateStarted(DateTime.Now)
                .WithDateEnded(DateTime.Now.AddHours(2))
                .WithEmployeesOnJob(1);

            Assert.AreEqual(2, _target.TotalManHours);

            _target = new TestCrewAssignmentBuilder()
                .WithDateStarted(DateTime.Now)
                .WithDateEnded(DateTime.Now.AddHours(3))
                .WithEmployeesOnJob(2);

            Assert.AreEqual(6, _target.TotalManHours);
        }

        [TestMethod]
        public void TestIsOpenPropertyReturnsTrueIfTheAssignmentHasBeenStartedButNotFinished()
        {
            _target = new TestCrewAssignmentBuilder();

            Assert.IsFalse(_target.IsOpen);

            _target.DateStarted = DateTime.Now;

            Assert.IsTrue(_target.IsOpen);

            _target.DateEnded = DateTime.Now;

            Assert.IsFalse(_target.IsOpen);
        }

        [TestMethod]
        public void TestHasStartedPropertyReturnsTrueIfTheAssignmentHasBeenStarted()
        {
            _target = new TestCrewAssignmentBuilder();

            Assert.IsFalse(_target.HasStarted);

            _target.DateStarted = DateTime.Now;

            Assert.IsTrue(_target.HasStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsTrueIfMarkoutRequirementNone()
        {
            var order = new TestWorkOrderBuilder().WithMarkoutRequirement(TestMarkoutRequirementBuilder.None).Build();
            _target = new TestCrewAssignmentBuilder().WithWorkOrder(order).Build();
            
            Assert.IsTrue(_target.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsTrueIfMarkoutRequirementEmergencyAndNoMarkoutExists()
        {
            var order = new TestWorkOrderBuilder().WithMarkoutRequirement(TestMarkoutRequirementBuilder.Emergency).Build();
            _target = new TestCrewAssignmentBuilder().WithWorkOrder(order).Build();
            
            Assert.IsTrue(_target.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsTrueIfMarkoutRequirementEmergencyAndValidMarkoutExists()
        {
            var order = new TestWorkOrderBuilder().WithMarkoutRequirement(TestMarkoutRequirementBuilder.Emergency).Build();
            var markout = new TestMarkoutBuilder().WithWorkOrder(order)
                                                  .WithDateOfRequest(DateTime.Now.BeginningOfDay())
                                                  .WithReadyDate(DateTime.Now.BeginningOfDay())
                                                  .WithExpirationDate(DateTime.Now.EndOfDay())
                                                  .Build();
            _target = new TestCrewAssignmentBuilder().WithWorkOrder(order).Build();

            Assert.IsTrue(_target.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsTrueIfMarkoutRequirementEmergencyAndInvalidMarkoutExistInThePast()
        {
            var order = new TestWorkOrderBuilder().WithMarkoutRequirement(TestMarkoutRequirementBuilder.Emergency).Build();
            var markout = new TestMarkoutBuilder().WithWorkOrder(order)
                                                  .WithDateOfRequest(DateTime.Today.AddDays(-2))
                                                  .WithReadyDate(DateTime.Today.AddDays(-2))
                                                  .WithExpirationDate(DateTime.Today.AddDays(-1))
                                                  .Build();
            _target = new TestCrewAssignmentBuilder().WithWorkOrder(order).Build();

            Assert.IsTrue(_target.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsFalseIfMarkoutRequirementRoutineAndNoMarkoutExists()
        {
            var order = new TestWorkOrderBuilder().WithMarkoutRequirement(TestMarkoutRequirementBuilder.Routine).Build();
            _target = new TestCrewAssignmentBuilder().WithWorkOrder(order).Build();
            
            Assert.IsFalse(_target.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsFalseIfMarkoutRequirementRoutineAndMarkoutExistsButInThePast()
        {
            var order = new TestWorkOrderBuilder().WithMarkoutRequirement(TestMarkoutRequirementBuilder.Routine).Build();
            var markout = new TestMarkoutBuilder().WithWorkOrder(order)
                                                  .WithDateOfRequest(DateTime.Now.AddDays(-3))
                                                  .WithReadyDate(DateTime.Now.AddDays(-2))
                                                  .WithExpirationDate(DateTime.Now.AddDays(-1))
                                                  .Build();
            _target = new TestCrewAssignmentBuilder().WithWorkOrder(order).Build();

            Assert.IsFalse(_target.CanBeStarted);
        }
        
        [TestMethod]
        public void TestCanBeStartedReturnsTrueIfMarkoutRequirementRoutineAndMarkoutExistsAndIsValid()
        {
            var order = new TestWorkOrderBuilder().WithMarkoutRequirement(TestMarkoutRequirementBuilder.Routine).Build();
            var markout = new TestMarkoutBuilder().WithWorkOrder(order)
                                                  .WithDateOfRequest(DateTime.Now.AddDays(-2))
                                                  .WithReadyDate(DateTime.Now.AddDays(-1))
                                                  .WithExpirationDate(DateTime.Now.AddDays(1)).Build();
            _target = new TestCrewAssignmentBuilder().WithWorkOrder(order).Build();

            Assert.IsTrue(_target.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsFalseIfMarkoutRequirementRoutineAndMarkoutExistsButInTheFuture()
        {
            var order = new TestWorkOrderBuilder().WithMarkoutRequirement(TestMarkoutRequirementBuilder.Routine).Build();
            var markout = new TestMarkoutBuilder().WithWorkOrder(order)
                                                  .WithDateOfRequest(DateTime.Now.AddDays(-3))
                                                  .WithReadyDate(DateTime.Now.AddDays(2))
                                                  .WithExpirationDate(DateTime.Now.AddDays(4))
                                                  .Build();
            _target = new TestCrewAssignmentBuilder().WithWorkOrder(order).Build();

            Assert.IsFalse(_target.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedThrowsInvalidOperationExceptionWhenMarkoutRequirementNotSet()
        {
            var order = new TestWorkOrderBuilder().WithMarkoutRequirement(null);
            _target = new TestCrewAssignmentBuilder().WithWorkOrder(order).Build();

            MyAssert.Throws<InvalidOperationException>(() => _target.CanBeStarted);
        }
    }

    internal class TestCrewAssignmentBuilder : TestDataBuilder<TestCrewAssignment>
    {
        #region Private Members

        private int? _priority = 1;

        private float? _employeesOnJob;

        private DateTime? _assignedFor = DateTime.Today,
                          _dateStarted,
                          _dateEnded;
        private WorkOrder _workOrder =
            new TestWorkOrderBuilder().WithMarkoutRequirement(
                TestMarkoutRequirementBuilder.None);
        private Crew _crew = new Crew();

        #endregion

        #region Exposed Methods

        public override TestCrewAssignment Build()
        {
            var obj = new TestCrewAssignment();
            if (_workOrder != null)
                obj.WorkOrder = _workOrder;
            if (_crew != null)
                obj.Crew = _crew;
            if (_priority != null)
                obj.Priority = _priority.Value;
            if (_assignedFor != null)
                obj.AssignedFor = _assignedFor.Value;
            if (_dateStarted != null)
                obj.DateStarted = _dateStarted.Value;
            if (_dateEnded != null)
                obj.DateEnded = _dateEnded.Value;
            if (_employeesOnJob != null)
                obj.EmployeesOnJob = _employeesOnJob.Value;
            return obj;
        }

        public TestCrewAssignmentBuilder WithWorkOrder(WorkOrder order)
        {
            _workOrder = order;
            return this;
        }

        public TestCrewAssignmentBuilder WithCrew(Crew crew)
        {
            _crew = crew;
            return this;
        }

        public TestCrewAssignmentBuilder WithAssignmentDate(DateTime time)
        {
            _assignedFor = time;
            return this;
        }

        public TestCrewAssignmentBuilder WithPriority(int? priority)
        {
            _priority = priority;
            return this;
        }

        public TestCrewAssignmentBuilder WithDateStarted(DateTime? started)
        {
            _dateStarted = started;
            return this;
        }

        public TestCrewAssignmentBuilder WithDateEnded(DateTime? ended)
        {
            _dateEnded = ended;
            return this;
        }

        public TestCrewAssignmentBuilder WithEmployeesOnJob(float? f)
        {
            _employeesOnJob = f;
            return this;
        }

        #endregion
    }

    internal class TestCrewAssignment : CrewAssignment
    {
    }

    internal class MockCrewAssignmentRepository : MockRepository<CrewAssignment>
    {
    }
}