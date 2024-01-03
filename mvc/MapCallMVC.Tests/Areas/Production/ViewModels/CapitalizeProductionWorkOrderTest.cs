using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.ViewModels {
    [TestClass]
    public class CapitalizeProductionWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrder>
    {
        #region Fields

        private ViewModelTester<CapitalizeProductionWorkOrder, ProductionWorkOrder> _vmTester;
        private CapitalizeProductionWorkOrder _viewModel;
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

            _container.Inject(_dateTimeProvider.Object);
            _container.Inject(_authServ.Object);

            _entity = GetEntityFactory<ProductionWorkOrder>().Create();
            _viewModel = _viewModelFactory.Build<CapitalizeProductionWorkOrder, ProductionWorkOrder>(_entity);
            _vmTester = new ViewModelTester<CapitalizeProductionWorkOrder, ProductionWorkOrder>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsDateCancelledWhenNoEmployeeAssignments()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            _vmTester.MapToEntity();

            MyAssert.AreClose(expectedDate, _entity.DateCancelled.Value);
        }

        [TestMethod]
        public void TestMapToEntitySetsDateCompletedAndByWhenEmployeeAssignmentsExist()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _entity.EmployeeAssignments.Add(new EmployeeAssignment {DateEnded = DateTime.Now});

            _vmTester.MapToEntity();

            MyAssert.AreClose(expectedDate, _entity.DateCompleted.Value);
            Assert.AreEqual(_user, _entity.CompletedBy);
        }

        #endregion
    }
}