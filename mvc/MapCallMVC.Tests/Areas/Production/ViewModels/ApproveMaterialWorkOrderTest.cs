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
    public class ApproveMaterialWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrder>
    {
        #region Fields

        private ViewModelTester<ApproveMaterialWorkOrder, ProductionWorkOrder> _vmTester;
        private ApproveMaterialWorkOrder _viewModel;
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
            _user = GetFactory<UserFactory>().Create();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _container.Inject(_dateTimeProvider.Object);
            _container.Inject(_authServ.Object);

            _entity = GetEntityFactory<ProductionWorkOrder>().Create();
            _viewModel = _viewModelFactory.Build<ApproveMaterialWorkOrder, ProductionWorkOrder>(_entity);
            _vmTester = new ViewModelTester<ApproveMaterialWorkOrder, ProductionWorkOrder>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsMaterialsApprovedOnAndBy()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            _vmTester.MapToEntity();

            MyAssert.AreClose(expectedDate, _entity.MaterialsApprovedOn.Value);
            Assert.AreEqual(_user, _entity.MaterialsApprovedBy);
        }

        #endregion
    }
}