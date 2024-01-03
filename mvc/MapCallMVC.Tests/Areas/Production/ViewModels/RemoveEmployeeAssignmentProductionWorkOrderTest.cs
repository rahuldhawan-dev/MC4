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
using StructureMap;
using System;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class RemoveEmployeeAssignmentProductionWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrder>
    {
        #region Fields

        private ViewModelTester<RemoveEmployeeAssignmentProductionWorkOrder, ProductionWorkOrder> _vmTester;
        private RemoveEmployeeAssignmentProductionWorkOrder _viewModel;
        private ProductionWorkOrder _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);
            _entity = GetEntityFactory<ProductionWorkOrder>().Create();
            _viewModel = _viewModelFactory.Build<RemoveEmployeeAssignmentProductionWorkOrder, ProductionWorkOrder>(_entity);
            _vmTester = new ViewModelTester<RemoveEmployeeAssignmentProductionWorkOrder, ProductionWorkOrder>(_viewModel, _entity);
        }

        #endregion

        #region Tests
        [TestMethod]
        public void TestMapToEntitySetsProgressWorkOrder()
        {
            var assignedTo = GetEntityFactory<Employee>().Create();
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();

            var employeeAssignment = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = assignedTo, ProductionWorkOrder = productionWorkOrder });
            _viewModel.EmployeeAssignment = employeeAssignment.Id;

            var progressWorkOrder = true;
            _entity.ApprovedOn = null;
            _entity.DateCancelled = null;
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            _entity.OperatingCenter = operatingCenter;

            _vmTester.MapToEntity();
            Assert.AreEqual(progressWorkOrder, _viewModel.ProgressWorkOrder);
        }

        [TestMethod]
        public void TestMapToEntitySetsEmployeeAssignments()
        {
            var assignedTo = GetEntityFactory<Employee>().Create();
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();

            var employeeAssignment = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = assignedTo, ProductionWorkOrder = productionWorkOrder });
            _viewModel.EmployeeAssignment = employeeAssignment.Id;

            _vmTester.MapToEntity();
            Assert.IsFalse(_entity.EmployeeAssignments.Contains(employeeAssignment));
        }
        #endregion
    }
}
