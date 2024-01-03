using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class CopyMaintenancePlanTest : MapCallMvcInMemoryDatabaseTestBase<MaintenancePlan>
    {
        #region Private Members

        private CopyMaintenancePlan _viewModel;
        private MaintenancePlan _entity;
        private Mock<IAuthenticationService<User>> _authServeMock;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServeMock = e.For<IAuthenticationService<User>>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IDateTimeProvider>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServeMock.Setup(x => x.CurrentUser).Returns(_user);
            _authServeMock.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            _user.IsAdmin = true;

            _entity = GetEntityFactory<MaintenancePlan>().Create();
            _viewModel = _viewModelFactory.Build<CopyMaintenancePlan, MaintenancePlan>(_entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesAreSetToCorrectValueInCopy()
        {
            var entity = GetEntityFactory<MaintenancePlan>().Create(new {
                State = GetEntityFactory<State>().Create(),
                OperatingCenter = GetEntityFactory<OperatingCenter>().Create(),
                PlanningPlant = GetEntityFactory<PlanningPlant>().Create(),
                TaskGroup = GetEntityFactory<TaskGroup>().Create(),
                TaskGroupCategory = GetEntityFactory<TaskGroupCategory>().Create(),
                WorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(),
                Start = DateTime.Now,
                IsActive = false, 
                HasACompletionRequirement = false,
                IsPlanPaused = false,
                PausedPlanNotes = string.Empty,
                PausedPlanResumeDate = DateTime.Now,
                AdditionalTaskDetails = string.Empty,
            });
            _viewModel = _viewModelFactory.Build<CopyMaintenancePlan, MaintenancePlan>(entity);

            _viewModel.Map(entity);

            Assert.IsNull(_viewModel.Equipment);

            Assert.IsNotNull(_viewModel.State);
            Assert.IsNotNull(_viewModel.OperatingCenter);
            Assert.IsNotNull(_viewModel.PlanningPlant);
            Assert.IsNotNull(_viewModel.TaskGroup);
            Assert.IsNotNull(_viewModel.TaskGroupCategory);

            Assert.IsFalse(_viewModel.HasACompletionRequirement);
            Assert.IsFalse(_viewModel.IsActive);
        }

        [TestMethod]
        public void TestEquipmentIsNotCopiedWithCopy()
        {
            var entity = GetEntityFactory<MaintenancePlan>().Create(new {
                State = GetEntityFactory<State>().Create(),
                OperatingCenter = GetEntityFactory<OperatingCenter>().Create(),
                PlanningPlant = GetEntityFactory<PlanningPlant>().Create(),
                TaskGroup = GetEntityFactory<TaskGroup>().Create(),
                TaskGroupCategory = GetEntityFactory<TaskGroupCategory>().Create(),
                WorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(),
                Start = DateTime.Now,
                IsActive = false,
                HasACompletionRequirement = false,
                AdditionalTaskDetails = string.Empty,
            });
            _viewModel = _viewModelFactory.Build<CopyMaintenancePlan, MaintenancePlan>(entity);

            _viewModel.Map(entity);

            Assert.IsNull(_viewModel.Equipment);
        }

        #endregion
    }
}
