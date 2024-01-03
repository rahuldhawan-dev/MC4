using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;
using StructureMap;
using UserType = MMSINC.Testing.UserType;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class CreateScheduledAssignmentsTest : MapCallMvcInMemoryDatabaseTestBase<MaintenancePlan>
    {
        private ViewModelTester<CreateScheduledAssignments, MaintenancePlan> _vmTester;
        private CreateScheduledAssignments _viewModel;
        private MaintenancePlan _entity;
        private User _currentUser;
        private Mock<IAuthenticationService<User>> _authServ;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<MaintenancePlan>().Create();
            _viewModel = _viewModelFactory.Build<CreateScheduledAssignments, MaintenancePlan>(_entity);
            _vmTester = new ViewModelTester<CreateScheduledAssignments, MaintenancePlan>(_viewModel, _entity);
            _currentUser = GetEntityFactory<User>().Create(new { IsUserAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_currentUser);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = new Mock<IAuthenticationService<User>>();
            e.For<IAuthenticationService<User>>().Use(_authServ.Object);
        }

        #endregion

        [TestMethod]
        public void TestMapToEntityRemovesScheduledAssignmentsFromMaintenancePlan()
        { 
            _viewModel.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            _viewModel.AssignedTo = new[] { GetEntityFactory<Employee>().Create().Id };
            _viewModel.AssignedFor = DateTime.Today;
            _viewModel.ScheduledDates = new[] { DateTime.Today };

            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.ScheduledAssignments.Count);
        }
    }
}
