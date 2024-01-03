using System;
using MMSINC.Testing;
using MMSINC.Utilities;
using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyFirmCapacities;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels.PublicWaterSupplyFirmCapacities
{
    [TestClass]
    public abstract class PublicWaterSupplyFirmCapacityViewModelTestBase<TViewModel> : ViewModelTestBase<PublicWaterSupplyFirmCapacity, TViewModel> where TViewModel : PublicWaterSupplyFirmCapacityViewModel
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now = DateTime.Now);

            _container.Inject(_dateTimeProvider.Object);

            _viewModel.SetDefaults();
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
            _vmTester.CanMapBothWays(x => x.CurrentSystemPeakDailyDemandMGD);
            _vmTester.CanMapBothWays(x => x.CurrentSystemPeakDailyDemandYearMonth);
            _vmTester.CanMapBothWays(x => x.TotalSystemSourceCapacityMGD);
            _vmTester.CanMapBothWays(x => x.FirmCapacityMultiplier);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // There is not any string length validation for this view model / entity. 
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PublicWaterSupply);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CurrentSystemPeakDailyDemandMGD);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CurrentSystemPeakDailyDemandYearMonth);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FirmCapacityMultiplier);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
        }

        [TestMethod]
        public void TestRequiredRangeValidation()
        {
            ValidationAssert.PropertyHasRequiredRange(_viewModel,
                x => x.FirmCapacityMultiplier,
                (decimal)PublicWaterSupplyFirmCapacity.CapacityMultiplierRange.MIN_VALUE,
                (decimal)PublicWaterSupplyFirmCapacity.CapacityMultiplierRange.MAX_VALUE);
        }

        #endregion

        #endregion
    }
}