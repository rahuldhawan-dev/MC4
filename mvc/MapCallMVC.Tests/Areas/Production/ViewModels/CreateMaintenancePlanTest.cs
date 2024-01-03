using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using System;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class CreateMaintenancePlanTest : MapCallMvcInMemoryDatabaseTestBase<MaintenancePlan>
    {
        #region Private Members

        private ViewModelTester<CreateMaintenancePlan, MaintenancePlan> _vmTester;
        private CreateMaintenancePlan _viewModel;
        private MaintenancePlan _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _user = GetEntityFactory<User>().Create();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);

            _entity = GetEntityFactory<MaintenancePlan>().Create();
            _viewModel = _viewModelFactory.Build<CreateMaintenancePlan, MaintenancePlan>(_entity);
            _vmTester = new ViewModelTester<CreateMaintenancePlan, MaintenancePlan>(_viewModel, _entity);
        }

        [TestMethod]
        public void TestMapToEntitySetsWorkDescriptionToUniqueMaintenancePlanProductionWorkDescription()
        {
            _entity.WorkDescription = null;

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.WorkDescription?.Description == ProductionWorkDescription.StaticDescriptions.MAINTENANCE_PLAN && _entity.WorkDescription.OrderType.Id == OrderType.Indices.ROUTINE_13);
        }
        
        [TestMethod]
        public void TestMapToEntitySetsIsPlanPausedToFalse()
        {
            _entity.IsPlanPaused = true;

            _vmTester.MapToEntity();

            Assert.AreEqual(false, _entity.IsPlanPaused);
        }

        [TestMethod]
        public void TestMapToEntitySetsForecastPeriodMultiplierTo1()
        {
            _entity.ForecastPeriodMultiplier = 0.0m;

            _vmTester.MapToEntity();

            Assert.AreEqual(1.0m, _entity.ForecastPeriodMultiplier);
        }

        [TestMethod]
        public void TestMapToEntitySetsDeactivationDateAndEmployeeIfInactive()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            _entity.IsActive = false;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedDate, _entity.DeactivationDate);
            Assert.AreEqual(_user.Employee, _entity.DeactivationEmployee);
        }

        [TestMethod]
        public void TestMapToEntitySetsDeactivationDateIfInactiveAndEmployeeToNullIfUserEmployeeIsNull()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            _entity.IsActive = false;
            _user.Employee = null;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedDate, _entity.DeactivationDate);
            Assert.AreEqual(null, _entity.DeactivationEmployee);
        }

        #endregion
    }
}
