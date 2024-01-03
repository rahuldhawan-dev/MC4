using System;
using System.Diagnostics;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
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
    public class CancelProductionWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrder>
    {
        #region Fields

        private ViewModelTester<CancelProductionWorkOrder, ProductionWorkOrder> _vmTester;
        private CancelProductionWorkOrder _viewModel;
        private ProductionWorkOrder _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authServ = new Mock<IAuthenticationService<User>>();
            
            _container.Inject(_dateTimeProvider.Object);
            _container.Inject(_authServ.Object);

            _entity = GetEntityFactory<ProductionWorkOrder>().Create();
            _viewModel = _viewModelFactory.Build<CancelProductionWorkOrder, ProductionWorkOrder>(_entity);
            _vmTester = new ViewModelTester<CancelProductionWorkOrder, ProductionWorkOrder>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsDateCancelled()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            _vmTester.MapToEntity();

            MyAssert.AreClose(expectedDate, _entity.DateCancelled.Value);
        }

        [TestMethod]
        public void TestMapToEntitySetsCancelledBy()
        {
            var expectedUser = new User {UserName = "testUser"};
            _authServ.Setup(x => x.CurrentUser).Returns(expectedUser);

            _vmTester.MapToEntity();

            MyAssert.StringsAreEqual(expectedUser.UserName, _entity.CancelledBy.UserName);
        }

        #endregion
    }
}