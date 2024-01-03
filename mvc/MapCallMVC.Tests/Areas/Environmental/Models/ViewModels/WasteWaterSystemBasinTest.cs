using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class WasteWaterSystemBasinTest : MapCallMvcInMemoryDatabaseTestBase<WasteWaterSystemBasin>
    {
        #region Fields

        private ViewModelTester<WasteWaterSystemBasinViewModel, WasteWaterSystemBasin> _vmTester;
        private WasteWaterSystemBasinViewModel _viewModel;
        private WasteWaterSystemBasin _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<WasteWaterSystemBasinViewModel>();
            _entity = new WasteWaterSystemBasin();
            _vmTester = new ViewModelTester<WasteWaterSystemBasinViewModel, WasteWaterSystemBasin>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.BasinName);
            _vmTester.CanMapBothWays(x => x.FirmCapacity);
            _vmTester.CanMapBothWays(x => x.FirmCapacityUnderStandbyPower);
            _vmTester.CanMapBothWays(x => x.FirmCapacityDateUpdated);
        }

        [TestMethod]
        public void TestRequiredProperties()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BasinName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WasteWaterSystem);
        }

         [TestMethod]
        public void TestRequiredRange()
        {
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.FirmCapacity, 0m, 999.999m);
            ValidationAssert.PropertyHasRequiredRange(_viewModel, x => x.FirmCapacityUnderStandbyPower, 0m, 999.999m);
        }

        #endregion
    }
}
