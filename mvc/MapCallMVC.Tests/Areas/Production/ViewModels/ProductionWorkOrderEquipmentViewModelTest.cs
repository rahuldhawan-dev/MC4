using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MMSINC.Authentication;
using Moq;
using MMSINC.Testing.ClassExtensions;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    public abstract class ProductionWorkOrderEquipmentViewModelTestBase<TViewModel> : ViewModelTestBase<ProductionWorkOrderEquipment, TViewModel>
    where TViewModel : ProductionWorkOrderEquipmentViewModel
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private IEnumerable<AsLeftCondition> _asLeftConditions;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _asLeftConditions = GetEntityFactory<AsLeftCondition>().CreateList(6);
            var employee = GetFactory<EmployeeFactory>().Create();
            _user = GetFactory<UserFactory>().Create(new {
                IsAdmin = true,
                Employee = employee
            });
            employee.User = _user;

            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Equipment);
            _vmTester.CanMapBothWays(x => x.AsFoundCondition);
            _vmTester.CanMapBothWays(x => x.AsFoundConditionReason);
            _vmTester.CanMapBothWays(x => x.AsFoundConditionComment);
            _vmTester.CanMapBothWays(x => x.AsLeftCondition);
            _vmTester.CanMapBothWays(x => x.AsLeftConditionReason);
            _vmTester.CanMapBothWays(x => x.AsLeftConditionComment);
            _vmTester.CanMapBothWays(x => x.RepairComment);
            _vmTester.CanMapBothWays(x => x.Priority);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.AsFoundCondition)
               .PropertyIsRequired(x => x.AsLeftCondition);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<Equipment>(x => x.Equipment)
               .EntityMustExist<AsFoundCondition>(x => x.AsFoundCondition)
               .EntityMustExist<AssetConditionReason>(x => x.AsFoundConditionReason)
               .EntityMustExist<AsLeftCondition>(x => x.AsLeftCondition)
               .EntityMustExist<AssetConditionReason>(x => x.AsLeftConditionReason)
               .EntityMustExist<ProductionWorkOrderPriority>(x => x.Priority);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert
               .PropertyHasMaxStringLength(x => x.AsFoundConditionComment,
                    ProductionWorkOrderEquipment.StringLengths.COMMENT)
               .PropertyHasMaxStringLength(x => x.AsLeftConditionComment,
                    ProductionWorkOrderEquipment.StringLengths.COMMENT)
               .PropertyHasMaxStringLength(x => x.RepairComment, 
                    ProductionWorkOrderEquipment.StringLengths.COMMENT);
        }

        [TestMethod]
        public void Test_MapToEntity_SetPriority_High()
        {
            _viewModel.AsLeftCondition = _asLeftConditions.Single(x => x.Id == AsLeftCondition.Indices.NEEDS_REPAIR).Id;
            
            var entity = _viewModel.MapToEntity(_entity);

            Assert.AreEqual((int)ProductionWorkOrderPriority.Indices.HIGH, entity.Priority.Id);
        }

        [TestMethod]
        public void Test_MapToEntity_SetPriority_Emergency()
        {
            _viewModel.AsLeftCondition = _asLeftConditions.Single(x => x.Id == AsLeftCondition.Indices.NEEDS_EMERGENCY_REPAIR).Id;

            var entity = _viewModel.MapToEntity(_entity);

            Assert.AreEqual((int)ProductionWorkOrderPriority.Indices.EMERGENCY, entity.Priority.Id);
        }

        [TestMethod]
        public void Test_MapToEntity_SetsOrderNotesAndRequestedBy()
        {
            _viewModel.AsLeftCondition = _asLeftConditions.Single(x => x.Id == AsLeftCondition.Indices.NEEDS_EMERGENCY_REPAIR).Id;
            _viewModel.RepairComment = "test";
            var entity = _viewModel.MapToEntity(_entity);

            Assert.AreEqual("test", entity.ProductionWorkOrder.OrderNotes);
            Assert.AreEqual(_user.Employee.Id, entity.ProductionWorkOrder.RequestedBy.Id);
        }

        #endregion
    }

    [TestClass]
    public class ProductionWorkOrderEquipmentViewModelTest : ProductionWorkOrderEquipmentViewModelTestBase<ProductionWorkOrderEquipmentViewModel> { }
}