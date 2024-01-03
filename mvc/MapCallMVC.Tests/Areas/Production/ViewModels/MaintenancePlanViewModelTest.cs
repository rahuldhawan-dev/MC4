using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class MaintenancePlanViewModelTest : ViewModelTestBase<MaintenancePlan, MaintenancePlanViewModel>
    {
        #region Validations

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.State, GetEntityFactory<State>().Create());
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(x => x.PlanningPlant, GetEntityFactory<PlanningPlant>().Create());
            ValidationAssert.EntityMustExist(x => x.Facility, GetEntityFactory<Facility>().Create());
            ValidationAssert.EntityMustExist(x => x.TaskGroup, GetEntityFactory<TaskGroup>().Create());
            ValidationAssert.EntityMustExist(x => x.TaskGroupCategory, GetEntityFactory<TaskGroupCategory>().Create());
            ValidationAssert.EntityMustExist(x => x.ProductionWorkOrderFrequency, GetEntityFactory<ProductionWorkOrderFrequency>().Create());
            ValidationAssert.EntityMustExist(x => x.Facility, GetEntityFactory<Facility>().Create());
            ValidationAssert.EntityMustExist(x => x.SkillSet, GetEntityFactory<SkillSet>().Create());
            ValidationAssert.EntityMustExist(x => x.DeactivationEmployee, GetEntityFactory<Employee>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.PlanningPlant);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.Facility);
            _vmTester.CanMapBothWays(x => x.TaskGroup);
            _vmTester.CanMapBothWays(x => x.TaskGroupCategory);
            _vmTester.CanMapBothWays(x => x.LocalTaskDescription);
            _vmTester.CanMapBothWays(x => x.ProductionWorkOrderFrequency);
            _vmTester.CanMapBothWays(x => x.Start);
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.Facility);
            _vmTester.CanMapBothWays(x => x.HasACompletionRequirement);
            _vmTester.CanMapBothWays(x => x.IsPlanPaused);
            _vmTester.CanMapBothWays(x => x.AdditionalTaskDetails);
            _vmTester.CanMapBothWays(x => x.HasCompanyRequirement);
            _vmTester.CanMapBothWays(x => x.HasOshaRequirement);
            _vmTester.CanMapBothWays(x => x.HasPsmRequirement);
            _vmTester.CanMapBothWays(x => x.HasRegulatoryRequirement);
            _vmTester.CanMapBothWays(x => x.HasOtherCompliance);
            _vmTester.CanMapBothWays(x => x.OtherComplianceReason);
            _vmTester.CanMapBothWays(x => x.LocalTaskDescription);
            _vmTester.CanMapBothWays(x => x.DeactivationReason);
            _vmTester.CanMapBothWays(x => x.DeactivationEmployee);
            _vmTester.CanMapBothWays(x => x.DeactivationDate);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.State);
            ValidationAssert.PropertyIsRequired(x => x.PlanningPlant);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(x => x.Facility);
            ValidationAssert.PropertyIsRequired(x => x.EquipmentTypes);
            ValidationAssert.PropertyIsRequired(x => x.TaskGroup);
            ValidationAssert.PropertyIsRequired(x => x.TaskGroupCategory);
            ValidationAssert.PropertyIsRequired(x => x.ProductionWorkOrderFrequency);
            ValidationAssert.PropertyIsRequired(x => x.Start);
            ValidationAssert.PropertyIsRequired(x => x.IsActive);
            ValidationAssert.PropertyIsRequired(x => x.LocalTaskDescription);

            ValidationAssert.PropertyIsRequiredWhen(x => x.OtherComplianceReason, "Something", x => x.HasOtherCompliance, true);
            ValidationAssert.PropertyIsRequiredWhen(x => x.DeactivationReason, "Something", x => x.IsActive, false);

            ValidationAssert.PropertyIsNotRequired(x => x.AdditionalTaskDetails);
            ValidationAssert.PropertyIsNotRequired(x => x.Resources);
            ValidationAssert.PropertyIsNotRequired(x => x.EstimatedHours);
            ValidationAssert.PropertyIsNotRequired(x => x.ContractorCost);
            ValidationAssert.PropertyIsNotRequired(x => x.SkillSet);
            ValidationAssert.PropertyIsNotRequired(x => x.DeactivationEmployee);
            ValidationAssert.PropertyIsNotRequired(x => x.DeactivationDate);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.LocalTaskDescription, MaintenancePlan.StringLengths.LOCAL_TASK_DESCRIPTION);
            ValidationAssert.PropertyHasMaxStringLength(x => x.AdditionalTaskDetails, MaintenancePlan.StringLengths.ADDITIONAL_TASK_DETAILS);
            ValidationAssert.PropertyHasMaxStringLength(x => x.DeactivationReason, MaintenancePlan.StringLengths.DEACTIVATION_REASON);
        }

        [TestMethod]
        public void MapToEntityMapsFacilityAreas()
        {
            var areas = GetEntityFactory<FacilityFacilityArea>().CreateArray(2);
            _viewModel.FacilityAreas = new[] { areas[0].Id, areas[1].Id };

            _vmTester.MapToEntity();

            Assert.AreEqual(2, _entity.FacilityAreas.Count);
            Assert.AreEqual(areas[0].Id, _entity.FacilityAreas[0].Id);
            Assert.AreEqual(areas[1].Id, _entity.FacilityAreas[1].Id);
        }

        [TestMethod]
        public void MapToEntityMapsEquipmentTypes()
        {
            var equipmentTypes = GetEntityFactory<EquipmentType>().CreateArray(2);
            _viewModel.EquipmentTypes = new[] { equipmentTypes[0].Id, equipmentTypes[1].Id };

            _vmTester.MapToEntity();

            Assert.AreEqual(2, _entity.EquipmentTypes.Count);
            Assert.AreEqual(equipmentTypes[0].Id, _entity.EquipmentTypes[0].Id);
            Assert.AreEqual(equipmentTypes[1].Id, _entity.EquipmentTypes[1].Id);
        }

        [TestMethod]
        public void MapToEntityMapsEquipmentPurpose()
        {
            var equipmentPurposes = GetEntityFactory<EquipmentPurpose>().CreateArray(2);
            _viewModel.EquipmentPurposes = new[] { equipmentPurposes[0].Id, equipmentPurposes[1].Id };

            _vmTester.MapToEntity();

            Assert.AreEqual(2, _entity.EquipmentPurposes.Count);
            Assert.AreEqual(equipmentPurposes[0].Id, _entity.EquipmentPurposes.First().Id);
            Assert.AreEqual(equipmentPurposes[1].Id, _entity.EquipmentPurposes.Skip(1).First().Id);
        }
        
        [TestMethod]
        public void MapToEntityMapsIsPlanPausedAndForecastPeriodMultiplier()
        {
            var maintenancePlan = GetEntityFactory<MaintenancePlan>().Create();
            var forecastPeriodMultiplier = 1.0m;

            _vmTester.MapToEntity();

            Assert.IsFalse(maintenancePlan.IsPlanPaused);
            Assert.AreEqual(maintenancePlan.ForecastPeriodMultiplier, forecastPeriodMultiplier);
        }

        #endregion
    }
}
