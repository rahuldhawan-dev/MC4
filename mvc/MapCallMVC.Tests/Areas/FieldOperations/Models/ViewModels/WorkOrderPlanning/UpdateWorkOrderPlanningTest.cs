using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderPlanning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.WorkOrderPlanning
{
    [TestClass]
    public class UpdateWorkOrderPlanningTest : ViewModelTestBase<WorkOrder, UpdateWorkOrderPlanning>
    {
        #region Private Members

        protected Mock<IAuthenticationService<User>> _authServ;
        protected User _user;

        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion
        
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester
               .CanMapBothWays(x => x.NumberOfOfficersRequired)
               .CanMapBothWays(x => x.PlannedCompletionDate)
               .CanMapBothWays(x => x.TrafficControlRequired);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop
        }

        [TestMethod]
        public void TestMapToEntityAppendsNotes()
        {
            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(new DateTime(2023, 6, 14, 10, 12, 42));
            _viewModel.AdditionalNotes = "Foo";
            
            _vmTester.MapToEntity();

            Assert.AreEqual($"hey this is a note{Environment.NewLine}{_user.FullName} 6/14/2023 10:12:42 AM (EST) Foo", _entity.Notes);
        }

        [TestMethod]
        public void TestPlannedCompletionDateErrorsForEmergencyPriorityWorkOrdersForYesterday()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var emergencyPriority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create();
            _entity = GetEntityFactory<WorkOrder>().Create(new { Priority = emergencyPriority });
            _viewModel.PlannedCompletionDate = now.AddDays(-1);

            ValidationAssert.ModelStateHasError(x => x.PlannedCompletionDate,
                CreateWorkOrder.PLANNED_COMPLETION_DATE_ERROR_MESSAGE);
        }

        [TestMethod]
        public void TestPlannedCompletionDateDoesNotErrorForEmergencyPriorityWorkOrdersForToday()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var emergencyPriority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create();
            _entity.Priority = emergencyPriority;
            _viewModel.PlannedCompletionDate = now;

            ValidationAssert.ModelStateIsValid(x => x.PlannedCompletionDate);
        }

        [TestMethod]
        public void TestPlannedCompletionDateErrorsForNonEmergencyWorkOrdersForTomorrow()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            _viewModel.PlannedCompletionDate = now.AddDays(1);

            ValidationAssert.ModelStateHasError(x => x.PlannedCompletionDate,
                CreateWorkOrder.PLANNED_COMPLETION_DATE_ERROR_MESSAGE);
        }

        [TestMethod]
        public void TestPlannedCompletionDateHasNoErrorsForNonEmergencyWorkOrdersForTwoDaysFromNow()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            _viewModel.PlannedCompletionDate = now.AddDays(2);

            ValidationAssert.ModelStateIsValid(x => x.PlannedCompletionDate);
        }
    }
}