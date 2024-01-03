using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    public abstract class AssetReliabilityViewModelTest<TViewModel> : ViewModelTestBase<AssetReliability, TViewModel> where TViewModel : AssetReliabilityViewModel
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public User _user;
        public Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void AssetReliabilityViewModelTestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = GetEntityFactory<User>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);
            _entity.Employee = _user.Employee;
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.AssetReliabilityTechnologyUsedType);
            _vmTester.CanMapBothWays(x => x.TechnologyUsedNote);
            _vmTester.CanMapBothWays(x => x.RepairCostAllowedToFail);
            _vmTester.CanMapBothWays(x => x.RepairCostNotAllowedToFail);
            _vmTester.CanMapBothWays(x => x.CostAvoidanceNote);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AssetReliabilityTechnologyUsedType);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.TechnologyUsedNote, "Maximum Derek", x => x.AssetReliabilityTechnologyUsedType, AssetReliabilityTechnologyUsedType.Indices.OTHER);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RepairCostAllowedToFail);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RepairCostNotAllowedToFail);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Equipment, GetEntityFactory<Equipment>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Employee, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.AssetReliabilityTechnologyUsedType, GetEntityFactory<AssetReliabilityTechnologyUsedType>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CostAvoidanceNote, AssetReliability.StringLengths.NOTE_LENGTH);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.TechnologyUsedNote, AssetReliability.StringLengths.NOTE_LENGTH);
        }

        [TestMethod]
        public void TestMapToEntitySetsCostAvoidanceAndEmployeeAndDateTimeEntered()
        {
            var date = _dateTimeProvider.Object.GetCurrentDate();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);
            _viewModel.RepairCostAllowedToFail = 200;
            _viewModel.RepairCostNotAllowedToFail = 100;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(date, _entity.DateTimeEntered);
            Assert.AreEqual(_user.Employee, _entity.Employee);
            Assert.AreEqual(100, _entity.CostAvoidance);
        }

        #endregion
    }

    [TestClass]
    public class CreateAssetReliabilityTest : AssetReliabilityViewModelTest<CreateAssetReliability> { }

    [TestClass]
    public class EditAssetReliabilityTest : AssetReliabilityViewModelTest<EditAssetReliability>
    {
        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IProductionWorkOrderRepository>().Mock();
            e.For<IEquipmentRepository>().Mock();
        }
    }
}
