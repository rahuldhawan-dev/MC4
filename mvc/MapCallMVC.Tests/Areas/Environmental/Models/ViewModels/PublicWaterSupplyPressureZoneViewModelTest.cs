using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyPressureZones;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class PublicWaterSupplyPressureZoneViewModelTest : ViewModelTestBase<PublicWaterSupplyPressureZone, PublicWaterSupplyPressureZoneViewModel>
    {
        #region Constants

        public const int MIN_RANGE_LOWER = PublicWaterSupplyPressureZone.PressureRangeValues.Min.LOWER;
        public const int MIN_RANGE_UPPER = PublicWaterSupplyPressureZone.PressureRangeValues.Min.UPPER;
        public const int MAX_RANGE_LOWER = PublicWaterSupplyPressureZone.PressureRangeValues.Max.LOWER;
        public const int MAX_RANGE_UPPER = PublicWaterSupplyPressureZone.PressureRangeValues.Max.UPPER;
        
        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
            _vmTester.CanMapBothWays(x => x.HydraulicModelName);
            _vmTester.CanMapBothWays(x => x.CommonName);
            _vmTester.CanMapBothWays(x => x.HydraulicGradientMin);
            _vmTester.CanMapBothWays(x => x.HydraulicGradientMax);
            _vmTester.CanMapBothWays(x => x.PressureMin);
            _vmTester.CanMapBothWays(x => x.PressureMax);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.HydraulicModelName, PublicWaterSupplyPressureZone.StringLengths.HYDRAULIC_MODEL_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CommonName, PublicWaterSupplyPressureZone.StringLengths.COMMON_NAME);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PublicWaterSupply);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HydraulicModelName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HydraulicGradientMin);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HydraulicGradientMax);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.PressureMin);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.PressureMax);
        }

        [TestMethod]
        public void TestRequiredRangeValidation()
        {
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.HydraulicGradientMin, MIN_RANGE_LOWER, MIN_RANGE_UPPER);
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.HydraulicGradientMax, MAX_RANGE_LOWER, MAX_RANGE_UPPER);
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.PressureMin, MIN_RANGE_LOWER, MIN_RANGE_UPPER);
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.PressureMax, MAX_RANGE_LOWER, MAX_RANGE_UPPER);
        }

        [TestMethod]
        public void TestCompareToFieldValidation()
        {
            _viewModel.HydraulicGradientMin = 10;
            _viewModel.HydraulicGradientMax = 1;
            _viewModel.PressureMin = 10;
            _viewModel.PressureMax = 1;

            ValidationAssert.ModelStateHasError(
                _viewModel, 
                x => x.HydraulicGradientMin, 
                $"{nameof(_viewModel.HydraulicGradientMin)} must be less than {nameof(_viewModel.HydraulicGradientMax)}.");

            ValidationAssert.ModelStateHasError(
                _viewModel, 
                x => x.HydraulicGradientMax, 
                $"{nameof(_viewModel.HydraulicGradientMax)} must be greater than {nameof(_viewModel.HydraulicGradientMin)}.");

            ValidationAssert.ModelStateHasError(
                _viewModel, 
                x => x.PressureMin, 
                $"{nameof(_viewModel.PressureMin)} must be less than {nameof(_viewModel.PressureMax)}.");

            ValidationAssert.ModelStateHasError(
                _viewModel, 
                x => x.PressureMax, 
                $"{nameof(_viewModel.PressureMax)} must be greater than {nameof(_viewModel.PressureMin)}.");

            _viewModel.PressureMin = null;
            _viewModel.PressureMax = null;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PressureMin);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PressureMax);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
        }

        #endregion

        #endregion
    }
}
