using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;
using System;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.Markouts
{
    [TestClass]
    public class CreateMarkoutTest : ViewModelTestBase<Markout, CreateMarkout>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IRepository<WorkOrder>>().Use<WorkOrderRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        #region Validation

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DateOfRequest);
            _vmTester.CanMapBothWays(x => x.Note);
            _vmTester.CanMapBothWays(x => x.MarkoutNumber);

            _vmTester.DoesNotMapToEntity(x => x.ReadyDate, DateTime.Now.AddDays(1));
            _vmTester.DoesNotMapToEntity(x => x.ExpirationDate, DateTime.Now.AddDays(5));
            _vmTester.DoesNotMapToViewModel(x => x.ReadyDate, DateTime.Now.AddDays(2));
            _vmTester.DoesNotMapToEntity(x => x.ExpirationDate, DateTime.Now.AddDays(3));
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.WorkOrder);
            ValidationAssert.PropertyIsRequired(x => x.MarkoutNumber);
            ValidationAssert.PropertyIsRequired(x => x.MarkoutType);
            ValidationAssert.PropertyIsRequired(x => x.DateOfRequest);

            ValidationAssert.PropertyIsRequiredWhen(x => x.Note, "Testing Notes", x => x.MarkoutType, 38, 1, "Required.");
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.MarkoutType, GetEntityFactory<MarkoutType>().Create());
            ValidationAssert.EntityMustExist(x => x.WorkOrder, GetEntityFactory<WorkOrder>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            var errorMessage = "The field MarkoutNumber must be a string with a minimum length of 9 and a maximum length of 20.";
            ValidationAssert.PropertyHasMaxStringLength(x => x.MarkoutNumber, Markout.StringLengths.MARKOUT_NUMBER_MAX_LENGTH, error: errorMessage);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMapToEntitySetsReadyAndExpirationDatesWhenMarkoutsEditable()
        {
            var expectedDate = DateTime.Now;
            var workOrder = GetEntityFactory<WorkOrder>().Create();
            workOrder.OperatingCenter.MarkoutsEditable = true;

            _viewModel.WorkOrder = workOrder.Id;
            _viewModel.ReadyDate = expectedDate;
            _viewModel.ExpirationDate = expectedDate;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedDate, _entity.ReadyDate);
            Assert.AreEqual(expectedDate, _entity.ExpirationDate);
        }

        [TestMethod]
        public void TestMapSetsWorkOrderOperatingCenterMarkoutEditable()
        {
            var workOrder = GetEntityFactory<WorkOrder>().Create();
            workOrder.OperatingCenter.MarkoutsEditable = true;
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, workOrder.OperatingCenter, _user, RoleActions.Read);

            _entity.WorkOrder = workOrder;

            _vmTester.MapToViewModel();
            
            Assert.IsTrue(_viewModel.WorkOrderOperatingCenterMarkoutEditable);
            Assert.AreEqual(workOrder.Id, _viewModel.WorkOrder);
        }

        [TestMethod]
        public void TestMapDoesNotExtendARoutineMarkoutExpirationDateWhenWorkStarted()
        {
            // we have a work order, started crew assignment
            // when we add a new routine markout, the new markout' expiration date should be standard,
            // and not extended due to the existing assignment

            // use a specific date, so we know the expected dates
            var now = new DateTime(2023, 8, 20, 8, 8, 8);

            var routineMarkoutRequirement = GetFactory<RoutineMarkoutRequirementFactory>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new { MarkoutRequirement = routineMarkoutRequirement });
            workOrder.OperatingCenter.MarkoutsEditable = false;
            _viewModel.WorkOrder = workOrder.Id;
            _viewModel.DateOfRequest = now;

            var crew = GetEntityFactory<Crew>().Create(new { OperatingCenter = workOrder.OperatingCenter });
            // danger, WorkStarted uses DateTime.Now
            var assignment = GetEntityFactory<CrewAssignment>().Create(new { WorkOrder = workOrder, Crew = crew, DateStarted = now.AddDays(5) });
            workOrder.CrewAssignments.Add(assignment);
            Session.Flush();

            _vmTester.MapToEntity();

            // these are testing a specific date, so we don't have to deal with testing the work day engine which is already tested
            Assert.AreEqual(now.AddDays(5), _entity.ReadyDate, "A markout called in on 8/20/2023 is ready 5 days after its called in");
            Assert.AreEqual(now.AddDays(16), _entity.ExpirationDate, "A markout call in on 8/20/2023 will have an expiration date of 9/5");
        }

        #endregion

        #endregion
    }
}
