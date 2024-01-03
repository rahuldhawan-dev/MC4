using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class CompleteProductionWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrder>
    {
        #region Fields

        private ViewModelTester<CompleteProductionWorkOrder, ProductionWorkOrder> _vmTester;
        private CompleteProductionWorkOrder _viewModel;
        private ProductionWorkOrder _entity;
        private ProductionWorkDescription _productionWorkDescription;
        private OrderType _orderTypeCorrective;
        private Equipment _equipment;
        private ISet<ProductionWorkOrderEquipment> _productionWorkOrderEquipments;
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
            _orderTypeCorrective = GetFactory<CorrectiveActionOrderTypeFactory>().Create();
            _equipment = GetEntityFactory<Equipment>().Create();
            _productionWorkOrderEquipments = new HashSet<ProductionWorkOrderEquipment> {
                GetEntityFactory<ProductionWorkOrderEquipment>().Create()
            };
            
            _viewModel = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(_entity);
            _vmTester = new ViewModelTester<CompleteProductionWorkOrder, ProductionWorkOrder>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsDateCompletedAndCompletedBy()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            _vmTester.MapToEntity();

            MyAssert.AreClose(expectedDate, _entity.DateCompleted.Value);
            Assert.AreEqual(_user, _entity.CompletedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsEquipmentFieldsIfCorrectiveActionRehabRenew()
        {
            var _orderNotes = "Mr. Sulu, engage";
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _productionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new
            {
                OrderType = _orderTypeCorrective,
                Description = "REHAB/RENEW"
            });
            
            _entity = GetEntityFactory<ProductionWorkOrder>().Create( new
            {
                ProductionWorkDescription = _productionWorkDescription,
                OrderNotes = _orderNotes,
                Equipments = _productionWorkOrderEquipments
            });

            _viewModel = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(_entity);

            _vmTester = new ViewModelTester<CompleteProductionWorkOrder, ProductionWorkOrder>(_viewModel, _entity);

            _vmTester.MapToEntity();

            MyAssert.AreClose(expectedDate, _entity.Equipments.First().Equipment.LifeExtendedOnDate.Value);
            Assert.AreEqual(_orderNotes, _entity.Equipments.First().Equipment.ExtendedUsefulLifeComment);
            Assert.AreEqual(_entity.Id, _entity.Equipments.First().Equipment.ExtendedUsefulLifeWorkOrderId);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotSetEquipmentFieldsIfNotCorrectiveActionRehabRenew()
        {
            var _orderNotes = "Mr. Chekov, engage";
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _productionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new
            {
                OrderType = _orderTypeCorrective,
                Description = "Anything but REHAB/RENEW"
            });

            _entity = GetEntityFactory<ProductionWorkOrder>().Create(new
            {
                ProductionWorkDescription = _productionWorkDescription,
                OrderNotes = _orderNotes,
                Equipments = _productionWorkOrderEquipments
            });

            _viewModel = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(_entity);

            _vmTester = new ViewModelTester<CompleteProductionWorkOrder, ProductionWorkOrder>(_viewModel, _entity);

            _vmTester.MapToEntity();

            Assert.AreNotEqual(_orderNotes, _entity.Equipments.First().Equipment.ExtendedUsefulLifeComment);
        }

        #endregion
    }
}