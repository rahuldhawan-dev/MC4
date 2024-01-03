using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Production.ViewModels 
{
    [TestClass]
    public class EditMaintenancePlanTest : MapCallMvcInMemoryDatabaseTestBase<MaintenancePlan>
    {
        #region Private Members

        private ViewModelTester<EditMaintenancePlan, MaintenancePlan> _vmTester;
        private EditMaintenancePlan _viewModel;
        private MaintenancePlan _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IMaintenancePlanRepository> _maintenancePlanRepo;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _user = GetFactory<UserFactory>().Create();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<MaintenancePlan>().Create();
            _viewModel = _viewModelFactory.Build<EditMaintenancePlan, MaintenancePlan>(_entity);

            _maintenancePlanRepo = new Mock<IMaintenancePlanRepository>();
            _maintenancePlanRepo.Setup(x => x.Find(_viewModel.Id)).Returns(new MaintenancePlan());
            _container.Inject(_maintenancePlanRepo.Object);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);

            _vmTester = new ViewModelTester<EditMaintenancePlan, MaintenancePlan>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.TaskGroup, GetEntityFactory<TaskGroup>().Create());
            _vmTester.CanMapBothWays(x => x.Start);
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.TaskGroupCategory, GetEntityFactory<TaskGroupCategory>().Create());
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TaskGroup);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Start);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsActive);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TaskGroupCategory);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotSetDeactivationDateIfAlreadyInactive()
        {
            var expectedOriginalIsActiveSetDate = DateTime.Now.AddDays(-1);
            var expectedCurrentIsActiveSetDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedCurrentIsActiveSetDate);

            _viewModel.DeactivationDate = expectedOriginalIsActiveSetDate;
            _viewModel.IsActive = false;
            _entity.IsActive = false;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedOriginalIsActiveSetDate, _entity.DeactivationDate);
        }

        [TestMethod]
        public void TestMapToEntitySetsDeactivationDateIfChangedToInactive()
        {
            // accounts for case where it was Inactive, changed to Active, and now attempting to change back to Inactive
            var expectedOriginalIsActiveSetDate = DateTime.Now.AddMonths(-1);
            var expectedCurrentIsActiveSetDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedCurrentIsActiveSetDate);

            _viewModel.DeactivationDate = expectedOriginalIsActiveSetDate;
            _viewModel.IsActive = false;
            _entity.IsActive = true;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedCurrentIsActiveSetDate, _entity.DeactivationDate);
        }

        [TestMethod]
        public void TestMapSetOtherComplianceReasonToNullIfHasOtherComplianceReasonIsNotTrue()
        {
            _entity.OtherComplianceReason = null;
            _viewModel.HasOtherCompliance = true;
            _viewModel.OtherComplianceReason = "testing";
            _vmTester.MapToEntity();
            Assert.AreEqual(_entity.OtherComplianceReason, _viewModel.OtherComplianceReason);

            _entity.OtherComplianceReason = "testing";
            _viewModel.HasOtherCompliance = false;
            _viewModel.OtherComplianceReason = "should set to null regardless";
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.OtherComplianceReason);
        }
    }
}
