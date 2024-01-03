using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits
{
    [TestClass]
    public class CreateRedTagPermitViewModelTest : RedTagPermitViewModelTest<CreateRedTagPermitViewModel>
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EquipmentImpairedOn);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EquipmentImpairedOn);
        }

        [TestMethod]
        public override void TestNonSpecificPropertyErrors()
        {
            _viewModel.EquipmentImpairedOn = _now.AddDays(-1);
            _viewModel.HazardousOperationsStopped = true;

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, CreateRedTagPermitViewModel.EQUIPMENT_IMPAIRED_ON_DATE_CANNOT_BE_BEFORE_TODAY);

            _viewModel.FireHoseLaidOut = true;
            _viewModel.EquipmentImpairedOn = _now;

            ValidationAssert.ModelStateIsValid(_viewModel);
        }

        #endregion

        #endregion
    }
}