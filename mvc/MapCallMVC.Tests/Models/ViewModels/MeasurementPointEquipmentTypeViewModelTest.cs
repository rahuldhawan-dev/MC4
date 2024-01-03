using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class MeasurementPointEquipmentTypeViewModelTestBase<TViewModel> : ViewModelTestBase<MeasurementPointEquipmentType, TViewModel> 
        where TViewModel: MeasurementPointEquipmentTypeViewModel
    {
        #region Validations

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EquipmentType);
            _vmTester.CanMapBothWays(x => x.Category);
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.Min);
            _vmTester.CanMapBothWays(x => x.Max);
            _vmTester.CanMapBothWays(x => x.Position);
            _vmTester.CanMapBothWays(x => x.UnitOfMeasure);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.EquipmentType, GetEntityFactory<EquipmentType>().Create());
            ValidationAssert.EntityMustExist(x => x.UnitOfMeasure, GetEntityFactory<UnitOfMeasure>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.IsActive);
            ValidationAssert.PropertyIsRequired(x => x.Description);
            ValidationAssert.PropertyIsRequired(x => x.Category);
            ValidationAssert.PropertyIsRequired(x => x.Position);
            ValidationAssert.PropertyIsRequired(x => x.UnitOfMeasure);
            ValidationAssert.PropertyIsRequired(x => x.Min);
            ValidationAssert.PropertyIsRequired(x => x.Max);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.Category, MeasurementPointEquipmentType.StringLengths.CATEGORY);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Description, MeasurementPointEquipmentType.StringLengths.DESCRIPTION);
        }

        [TestMethod]
        public void TestValidationFailsIfMinIsGreaterThanMax()
        {
            _viewModel.Min = 50m;
            _viewModel.Max = 10m;

            _vmTester.MapToEntity();

            ValidationAssert.ModelStateHasError(x => x.Min, "Min must be less than or equal to Max.");
        }
        
        #endregion
    }

    [TestClass]
    public class MeasurementPointEquipmentTypeViewModelTest : MeasurementPointEquipmentTypeViewModelTestBase<
        MeasurementPointEquipmentTypeViewModel> { }
}