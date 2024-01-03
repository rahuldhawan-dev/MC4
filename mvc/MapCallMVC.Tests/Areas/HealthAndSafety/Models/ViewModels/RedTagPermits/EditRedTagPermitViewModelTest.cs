using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits;
using MapCallMVC.Models.ViewModels;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits
{
    [TestClass]
    public class EditRedTagPermitViewModelTest : RedTagPermitViewModelTest<EditRedTagPermitViewModel>
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EquipmentRestoredOn);
        }

        [TestMethod]
        public void TestMapToEntityUpdatesSatisfiedOnRequirementForProductionWorkOrderPreRequisite()
        {
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(new {
                NeedsRedTagPermitAuthorization = true
            });

            var productionWorkOrderPrerequisite = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new {
                ProductionWorkOrder = productionWorkOrder,
                ProductionPrerequisite = GetFactory<RedTagPermitProductionPrerequisiteFactory>().Create()
            });
            
            productionWorkOrder.ProductionWorkOrderProductionPrerequisites.Add(productionWorkOrderPrerequisite);

            var redTagPermit = GetEntityFactory<RedTagPermit>().Create(new {
                ProductionWorkOrder = productionWorkOrder,
                EquipmentRestoredOn = _now
            });

            var target = _viewModelFactory.Build<EditRedTagPermitViewModel, RedTagPermit>(redTagPermit);

            target.MapToEntity(redTagPermit);

            Assert.IsNotNull(productionWorkOrderPrerequisite.SatisfiedOn);
            Assert.AreEqual(_now, productionWorkOrderPrerequisite.SatisfiedOn);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EquipmentRestoredOn);
        }

        [TestMethod]
        public override void TestNonSpecificPropertyErrors()
        {
            _viewModel.EquipmentRestoredOn = _now.AddDays(-1);
            _viewModel.HazardousOperationsStopped = true;
            _viewModel.EquipmentRestoredOnChangeReason = "Because I said so!";

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, EditRedTagPermitViewModel.EQUIPMENT_RESTORED_ON_DATE_CANNOT_BE_BEFORE_TODAY);

            _viewModel.EquipmentRestoredOn = _now.AddDays(1);

            ValidationAssert.ModelStateIsValid(_viewModel);
        }

        [TestMethod]
        public void TestCompareToFieldValidation()
        {
            _viewModel.HazardousOperationsStopped = true;
            _viewModel.EquipmentImpairedOn = _now;
            _viewModel.EquipmentRestoredOn = _now.AddDays(-1);

            ValidationAssert.ModelStateHasError(
                _viewModel,
                x => x.EquipmentRestoredOn,
                $"{nameof(_viewModel.EquipmentRestoredOn)} must be greater than {nameof(_viewModel.EquipmentImpairedOn)}.");

            ValidationAssert.ModelStateHasError(
                _viewModel, 
                x => x.EquipmentRestoredOnChangeReason,
                "Change Reason is required.");

            _viewModel.EquipmentRestoredOn = _now;
            ValidationAssert.ModelStateHasError(
                _viewModel,
                x => x.EquipmentRestoredOn,
                $"{nameof(_viewModel.EquipmentRestoredOn)} must be greater than {nameof(_viewModel.EquipmentImpairedOn)}.");

            _viewModel.EquipmentRestoredOn = _now.AddMinutes(1);
            _viewModel.EquipmentRestoredOnChangeReason = "Because I said so!";
            ValidationAssert.ModelStateIsValid(_viewModel);
        }

        #endregion

        #endregion
    }
}
