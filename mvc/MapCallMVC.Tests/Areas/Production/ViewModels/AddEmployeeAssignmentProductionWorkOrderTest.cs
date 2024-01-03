using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Linq;
using User = MapCall.Common.Model.Entities.Users.User;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class AddEmployeeAssignmentProductionWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrder>
    {
        #region Fields

        private ViewModelTester<AddEmployeeAssignmentProductionWorkOrder, ProductionWorkOrder> _vmTester;
        private AddEmployeeAssignmentProductionWorkOrder _viewModel;
        private ProductionWorkOrder _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<ProductionWorkOrder>().Create();
            _viewModel = _viewModelFactory.Build<AddEmployeeAssignmentProductionWorkOrder, ProductionWorkOrder>(_entity);
            _vmTester = new ViewModelTester<AddEmployeeAssignmentProductionWorkOrder, ProductionWorkOrder>(_viewModel, _entity);
            _viewModel.AssignedTo =  GetEntityFactory<Employee>().Create().Id;
            _viewModel.AssignedFor = DateTime.Now;
        }

        #endregion

        #region Tests
        [TestMethod]
        public void TestMapToEntitySetsProgressWorkOrder()
        {
            var assignedTo = _viewModel.AssignedTo;
            var assignedFor = (DateTime)_viewModel.AssignedFor;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(assignedFor);

            var progressWorkOrder = true;
            _entity.ApprovedOn = null;
            _entity.DateCancelled = null;
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            _entity.OperatingCenter = operatingCenter;

            _vmTester.MapToEntity();
            Assert.AreEqual(progressWorkOrder, _viewModel.ProgressWorkOrder);

        }
        [TestMethod]
        public void TestMapToEntitySetsAssignedFor()
        {
            var assignedTo = _viewModel.AssignedTo;
            var assignedFor = (DateTime)_viewModel.AssignedFor;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(assignedFor);

            _entity.ApprovedOn = null;
            _entity.DateCancelled = null;
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            _entity.OperatingCenter = operatingCenter;

            _vmTester.MapToEntity();
            Assert.AreEqual(assignedFor, _entity.EmployeeAssignments.Single().AssignedFor);
        }
        [TestMethod]
        public void TestMapToEntitySetsAssignedTo()
        {
            var assignedTo = _viewModel.AssignedTo;
            var assignedFor = (DateTime)_viewModel.AssignedFor;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(assignedFor);

            _entity.ApprovedOn = null;
            _entity.DateCancelled = null;
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            _entity.OperatingCenter = operatingCenter;

            _vmTester.MapToEntity();
            Assert.AreEqual(assignedTo, _entity.EmployeeAssignments.Single().AssignedTo.Id);
        }

        [TestMethod]
        public void TestMapToEntitySetsAssignedBy()
        {
            var assignedTo = _viewModel.AssignedTo;
            var assignedFor = (DateTime)_viewModel.AssignedFor;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(assignedFor);

            _entity.ApprovedOn = null;
            _entity.DateCancelled = null;
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            _entity.OperatingCenter = operatingCenter;

            _vmTester.MapToEntity();
            Assert.AreEqual(_user.Employee.Id, _entity.EmployeeAssignments.Single().AssignedBy.Id);
        }
        #endregion
    }
}
