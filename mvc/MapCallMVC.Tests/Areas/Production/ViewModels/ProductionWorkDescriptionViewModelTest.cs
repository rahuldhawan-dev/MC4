using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    public abstract class ProductionWorkDescriptionViewModelTestBase<TViewModel> : ViewModelTestBase<ProductionWorkDescription, TViewModel> where TViewModel : ProductionWorkDescriptionViewModel
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.BreakdownIndicator);
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.EquipmentType, GetEntityFactory<EquipmentType>().Create());
            _vmTester.CanMapBothWays(x => x.OrderType, GetEntityFactory<OrderType>().Create());
            _vmTester.CanMapBothWays(x => x.PlantMaintenanceActivityType, GetEntityFactory<PlantMaintenanceActivityType>().Create());
            _vmTester.CanMapBothWays(x => x.ProductionSkillSet, GetEntityFactory<ProductionSkillSet>().Create());
            _vmTester.CanMapBothWays(x => x.MaintenancePlanTaskType, GetEntityFactory<MaintenancePlanTaskType>().Create());
            _vmTester.CanMapBothWays(x => x.TaskGroup, GetEntityFactory<TaskGroup>().Create());
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.EquipmentType, GetEntityFactory<EquipmentType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OrderType, GetEntityFactory<OrderType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PlantMaintenanceActivityType, GetEntityFactory<PlantMaintenanceActivityType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ProductionSkillSet, GetEntityFactory<ProductionSkillSet>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.MaintenancePlanTaskType, GetEntityFactory<MaintenancePlanTaskType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.TaskGroup, GetEntityFactory<TaskGroup>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BreakdownIndicator);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OrderType);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Description, ProductionWorkDescription.StringLengths.DESCRIPTION);
        }

        #endregion

        #endregion
    }

    [TestClass]
    public class CreateProductionWorkDescriptionTest : ProductionWorkDescriptionViewModelTestBase<CreateProductionWorkDescription> { }

    [TestClass]
    public class EditProductionWorkDescriptionTest : ProductionWorkDescriptionViewModelTestBase<EditProductionWorkDescription> { }

}
