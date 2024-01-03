using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels
{
    public abstract class GasMonitorViewModelTest<TViewModel> : ViewModelTestBase<GasMonitor, TViewModel> where TViewModel : GasMonitorViewModel
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            var testDataFactoryService = _container.GetInstance<ITestDataFactoryService>();
            _vmTester.CanMapBothWays(x => x.AssignedEmployee);
            _vmTester.CanMapBothWays(x => x.CalibrationFrequencyDays);
            _vmTester.CanMapBothWays(x => x.Equipment);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CalibrationFrequencyDays);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Equipment);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop: No properties with string length validation.
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.AssignedEmployee, GetFactory<EmployeeFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Equipment, GetFactory<GasMonitorEquipmentFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetFactory<OperatingCenterFactory>().Create());
        }

        [TestMethod]
        public void TestValidationFailsIfCalibrationFrequencyDaysIsLessThanOne()
        {
            _viewModel.CalibrationFrequencyDays = 0;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.CalibrationFrequencyDays, "The field CalibrationFrequencyDays must be greater than or equal to 1");
        }

        [TestMethod]
        public void TestValidationFailsIfEquipmentIsNotGasDetector()
        {
            var gasDetector = GetFactory<GasMonitorEquipmentFactory>().Create();
            var otherEquipment = GetEntityFactory<Equipment>().Create(new
            {
                EquipmentType = typeof(EquipmentTypeAeratorFactory)
            });

            _viewModel.Equipment = gasDetector.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Equipment);

            _viewModel.Equipment = otherEquipment.Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.Equipment, "Equipment must be a Gas Detector.");
        }

        #endregion

        #endregion
    }

    [TestClass]
    public class CreateGasMonitorTest : GasMonitorViewModelTest<CreateGasMonitor> { }

    [TestClass]
    public class EditGasMonitorTest : GasMonitorViewModelTest<EditGasMonitor>
    {
        #region Mapping

        [TestMethod]
        public void TestMapSetsOperatingCenterFromEquipment()
        {
            // Equipment.OperatingCenter comes through Facility.OperatingCenter.
            var facility = GetEntityFactory<Facility>().Create();
            var equipment = GetEntityFactory<Equipment>().Create(new { Facility = facility });
            // Need to refresh because Equipment.OperatingCenter is a formula field.
            Session.Refresh(equipment);
            _entity.Equipment = equipment;

            _vmTester.MapToViewModel();

            Assert.AreEqual(equipment.OperatingCenter.Id, _viewModel.OperatingCenter);
        }

        #endregion
    }
}
