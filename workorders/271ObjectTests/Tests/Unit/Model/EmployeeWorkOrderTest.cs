using System;
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
    /// Summary description for EmployeeWorkOrderTestTest
    /// </summary>
    [TestClass]
    public class EmployeeWorkOrderTest
    {
        #region Private Members

        private IRepository<EmployeeWorkOrder> _repository;
        private EmployeeWorkOrder _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void EmployeeWorkOrderTestInitialize()
        {
            _repository = new MockEmployeeWorkOrderRepository();
            _target = new TestEmployeeWorkOrderBuilder();
        }

        #endregion

        [TestMethod]
        public void TestAssigningAutomaticallyUpdatesDateAssigned()
        {
            _target = new TestEmployeeWorkOrderBuilder()
               .WithAssignedEmployee(new Employee { UserName = "1234" });

            Assert.IsNotNull(_target.DateAssigned,
                "DateAssigned should be set when AssignedTo is set.");
            Assert.AreNotEqual(DateTime.MinValue,
                (DateTime)_target.DateAssigned,
                "DateAssigned should be set when AssignedTo is set.");
        }

        [TestMethod]
        public void TestCannotSetTimeJobLeftBeforeSettingTimeArrivedOnJob()
        {
            MyAssert.Throws<DomainLogicException>(
                () => _target.TimeLeftJob = DateTime.Now,
                "Cannot set TimeLeftJob without first setting TimeArrivedOnJob");
        }

        [TestMethod]
        public void TestCannotCompleteUntilAssigned()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithAssignedEmployee(null)
                .WithApprovingEmployee(null)
                .WithDateAssigned(DateTime.Now)
                .WithTimeArrivedOnJob(DateTime.Now)
                .WithTimeLeftJob(DateTime.Now.AddHours(5));

            MyAssert.Throws(() => _target.WorkCompleted = true,
                "Cannot set WorkCompleted on an EmployeeWorkOrder until the AssignedTo field has been set.");
        }

        [TestMethod]
        public void TestCannotCompleteWithoutDateAssigned()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithAssignedEmployee(null)
                .WithApprovingEmployee(null)
                .WithTimeArrivedOnJob(DateTime.Now)
                .WithTimeLeftJob(DateTime.Now.AddHours(5))
                .WithDateAssigned(null);

            MyAssert.Throws<DomainLogicException>(
                () => _target.WorkCompleted = true,
                "EmployeeWorkOrder should now allow completion until DateAssigned has been set");
        }

        //[TestMethod]
        public void TestCannotCompleteUntilApproved()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithAssignedEmployee(new Employee())
                .WithApprovingEmployee(null)
                .WithTimeArrivedOnJob(DateTime.Now)
                .WithTimeLeftJob(DateTime.Now.AddHours(5));

            MyAssert.Throws<DomainLogicException>(
                () => _target.WorkCompleted = true,
                "EmployeeWorkOrder should not allow completion until the order has been approved");
        }

        [TestMethod]
        public void TestCannotSetTimeJobCompletedToAValueBeforeTimeArrivedOnJob()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithApprovingEmployee(new Employee())
                .WithAssignedEmployee(new Employee())
                .WithDateAssigned(DateTime.Now)
                .WithTimeArrivedOnJob(DateTime.Now.AddHours(5));

            MyAssert.Throws<DomainLogicException>(
                () => _target.TimeLeftJob = _target.DateAssigned,
                "Should not be allowed to set a TimeJobCompleted that lies chronologically before TimeArrivedOnJob");
        }

        [TestMethod]
        public void TestSettingCompletedDateUpdatesTotalManHours()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithApprovingEmployee(new Employee())
                .WithAssignedEmployee(new Employee())
                .WithTimeArrivedOnJob(DateTime.Now)
                .WithTimeLeftJob(DateTime.Now.AddHours(5));

            Assert.AreEqual(5, (int)_target.TotalManHours,
                "Setting TimeJobCompleted should automatically update the TotalManHours property");
        }

        [TestMethod]
        public void TestCreateNewEmployeeWorkOrder()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            _target = new TestEmployeeWorkOrderBuilder().WithWorkOrder(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target),
                "Attempting to save an EmployeeWorkOrder that's not linked to a WorkOrder should throw an exception");
        }

        /*
        [TestMethod]
        public void TestEmployeeWorkOrderCanBeSetCompleteWhenApprovedByAssignedToDateAssignedTimeArrivedOnJobAndTimeLeftJobAreSet()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithApprovingEmployee(null)
                .WithAssignedEmployee(null)
                .WithDateAssigned(null)
                .WithTimeArrivedOnJob(null)
                .WithTimeLeftJob(null);

            MyAssert.Throws<DomainLogicException>(
                () => _target.WorkCompleted = true,
                "EmployeeWorkOrder cannot be complete without ApprovedBy, AssignedTo, DateAssigned, TimeArrivedOnJob, and TimeLeftJob");

            _target.ApprovedBy = _target.AssignedTo = new Employee();
            _target.TimeArrivedOnJob = _target.DateAssigned;
            _target.TimeLeftJob = _target.TimeArrivedOnJob.Value.AddHours(5);

            MyAssert.DoesNotThrow(() => _target.WorkCompleted = true,
                "Setting ApprovedBy, AssignedTo, DateAssigned, TimeArrivedOnJob, and TimeLeftJob should allow the WorkCompleted property to be set to true.");
        }
        */

        [TestMethod]
        public void TestAssigningEmployeeWorkOrderToEmployeeAddsOneToNumberOfEmployees()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithAssignedEmployee(null);

            Assert.AreEqual<short>(0, _target.NumberOfEmployees,
                "A new EmployeeWorkOrder object with no assigned Employee should have 0 in NumberOfEmployees.");

            _target.AssignedTo = new Employee();

            Assert.AreEqual<short>(1, _target.NumberOfEmployees,
                "Assigning an EmployeeWorkOrder to an Employee should increment NumberOfEmployees by 1.");

            _target.AssignedTo = null;

            Assert.AreEqual<short>(0, _target.NumberOfEmployees,
                "Un-Assigning an EmployeeWorkOrder should decrement NumberOfEmployees by 1.");
        }

        [TestMethod]
        public void TestSettingWorkCompletedSetsTheValueOfDateCompletedOnParentWorkOrderToTimeLeftJob()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithApprovingEmployee(new Employee())
                .WithAssignedEmployee(new Employee())
                .WithDateAssigned(DateTime.Now)
                .WithTimeArrivedOnJob(DateTime.Now)
                .WithTimeLeftJob(DateTime.Now.AddHours(5))
                .WithWorkCompleted(true);

            Assert.AreEqual(_target.TimeLeftJob,
                _target.WorkOrder.DateCompleted);
        }

        [TestMethod]
        public void TestSettingTimeArrivedOnJobSetsDateTimeArrivedOnJobSetToCurrentDate()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithTimeArrivedOnJob(DateTime.Now);

            Assert.IsNotNull(_target.DateTimeArrivedOnJobSet,
                "Setting TimeArrivedOnJob should automatically update the DateTimeArrivedOnJobSet field");
            MyAssert.AreClose(DateTime.Now, _target.DateTimeArrivedOnJobSet.Value);
        }

        [TestMethod]
        public void TestSettingTimeLeftJobSetsDateTimeLeftJobSetToCurrentDate()
        {
            var expected = DateTime.Now;
            _target = new TestEmployeeWorkOrderBuilder()
                .WithTimeArrivedOnJob(expected)
                .WithTimeLeftJob(DateTime.Now.AddHours(5));

            Assert.IsNotNull(_target.DateTimeLeftJobSet,
                "Setting TimeLeftJob should automatically update the DateTimeLeftJobSet field");
            MyAssert.AreClose(expected, _target.DateTimeLeftJobSet.Value);
        }

        [TestMethod]
        public void TestTotalManHoursIsProductOfDifferenceBetweenTimeArrivedAndTimeLeftMultipliedByNumberOfEmployees()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithTimeArrivedOnJob(DateTime.Now)
                .WithTimeLeftJob(DateTime.Now.AddHours(5));

            _target.NumberOfEmployees = 2;
            Assert.AreEqual(10, (int)_target.TotalManHours);

            _target.NumberOfEmployees = 4;
            Assert.AreEqual(20, (int)_target.TotalManHours);
        }

        [TestMethod]
        public void TestChangingNumberOfEmployeesRecalculatesTotalManHours()
        {
            _target = new TestEmployeeWorkOrderBuilder()
                .WithAssignedEmployee(null)
                .WithTimeArrivedOnJob(DateTime.Now)
                .WithTimeLeftJob(DateTime.Now.AddHours(5));

            Assert.AreEqual(0, (int)_target.TotalManHours,
                "TotalManHours should not have been set yet.");

            _target.NumberOfEmployees = 1;

            Assert.AreEqual(5, (int)_target.TotalManHours);
        }
    }

    internal class TestEmployeeWorkOrderBuilder : TestDataBuilder<EmployeeWorkOrder>
    {
        #region Private Members

        private Employee _assignedEmployee = new Employee(),
                         _approvedByEmployee = new Employee();

        private DateTime? _dateAssigned, _timeArrivedOnJob, _timeLeftJob;
        private WorkOrder _workOrder = new WorkOrder();
        private bool? _workCompleted;

        #endregion

        #region Exposed Methods

        public override EmployeeWorkOrder Build()
        {
            var obj = new EmployeeWorkOrder();
            if (_assignedEmployee != null)
                obj.AssignedTo = _assignedEmployee;
            //if (_approvedByEmployee != null)
            //    obj.ApprovedBy = _approvedByEmployee;
            if (_dateAssigned != null)
                obj.DateAssigned = _dateAssigned;
            if (_timeArrivedOnJob != null)
                obj.TimeArrivedOnJob = _timeArrivedOnJob;
            if (_timeLeftJob != null)
                obj.TimeLeftJob = _timeLeftJob;
            if (_workOrder != null)
                obj.WorkOrder = _workOrder;
            if (_workCompleted != null)
                obj.WorkCompleted = _workCompleted;
            return obj;
        }

        public TestEmployeeWorkOrderBuilder WithAssignedEmployee(Employee employee)
        {
            _assignedEmployee = employee;
            return this;
        }

        public TestEmployeeWorkOrderBuilder WithApprovingEmployee(Employee employee)
        {
            _approvedByEmployee = employee;
            return this;
        }

        public TestEmployeeWorkOrderBuilder WithDateAssigned(DateTime? dateTime)
        {
            _dateAssigned = dateTime;
            return this;
        }

        public TestEmployeeWorkOrderBuilder WithTimeArrivedOnJob(DateTime? dateTime)
        {
            _timeArrivedOnJob = dateTime;
            return this;
        }

        public TestEmployeeWorkOrderBuilder WithTimeLeftJob(DateTime? dateTime)
        {
            _timeLeftJob = dateTime;
            return this;
        }

        public TestEmployeeWorkOrderBuilder WithWorkOrder(WorkOrder order)
        {
            _workOrder = order;
            return this;
        }

        public TestEmployeeWorkOrderBuilder WithWorkCompleted(bool? workCompleted)
        {
            _workCompleted = workCompleted;
            return this;
        }

        #endregion
    }

    internal class MockEmployeeWorkOrderRepository : MockRepository<EmployeeWorkOrder>
    {
    }
}
