using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Import
{
    [TestClass]
    public class MaintenancePlanExcelRecordTest : ExcelRecordTestBase<MaintenancePlan, MyCreateMaintenancePlan,
        MaintenancePlanExcelRecord>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateStuffForMaintenancePlansInAberdeenNJ(_container);
        }

        #endregion

        protected override MaintenancePlanExcelRecord CreateTarget()
        {
            return new MaintenancePlanExcelRecord {
                State = NJState.ABBREVIATION,
                OperatingCenter = OperatingCenters.NJ7.CODE + " - blah blah this doesn't matter",
                District = P218PlanningPlant.CODE + " - " + OperatingCenters.NJ7.CODE + " - " + P218PlanningPlant.DESCRIPTION,
                FacilityId = Facilities.NJSB11.ID,
                FacilityAreas = "Filter, Filter - Filter Gallery, Generator",
                Equipment = "1",
                EquipmentTypes = "ADJSPD",
                TaskGroupCategory = "CHEMICAL",
                TaskGroupName = TaskGroups.NAME,
                RequiredSkillSet = "Maintenance Services - Specialist",
                Frequency = "Daily",
                EquipmentPurposes = "ASDG",
                LocalTaskDescription = "Local Task Desc test",
                IsActive = true
            };
        }

        protected override void InnerTestMappings(
            ExcelRecordMappingTester<MaintenancePlan, MyCreateMaintenancePlan, MaintenancePlanExcelRecord> test)
        {
            test.DateTime(x => x.Start, x => x.Start);
            test.TestedElsewhere(x => x.IsActive); //Following three deactivation fields are tested in conditional test cases below.
            test.TestedElsewhere(x => x.DeactivationDate);
            test.TestedElsewhere(x => x.DeactivationReason);
            test.TestedElsewhere(x => x.DeactivationEmployeeId);
            test.String(x => x.AdditionalTaskDetails, x => x.AdditionalTaskDetails);
            test.Boolean(x => x.HasCompanyRequirement, x => x.HasCompanyRequirement);
            test.Boolean(x => x.HasOshaRequirement, x => x.HasOshaRequirement);
            test.Boolean(x => x.HasPsmRequirement, x => x.HasPsmRequirement);
            test.Boolean(x => x.HasRegulatoryRequirement, x => x.HasRegulatoryRequirement);
            test.String(x => x.LocalTaskDescription, x => x.LocalTaskDescription);
            test.Decimal(x => x.EstimatedCompletionTime, x => x.EstimatedHours);
            test.Decimal(x => x.NumberOfResources, x => x.Resources);
            test.Decimal(x => x.EstimatedContractorCost, x => x.ContractorCost);
            
            test.TestedElsewhere(x => x.State);
            test.TestedElsewhere(x => x.OperatingCenter);
            test.TestedElsewhere(x => x.District);

            test.RequiredEntityRef(x => x.FacilityId, x => x.Facility);
            test.NotMapped(x => x.FacilityName);
            
            test.TestedElsewhere(x => x.TaskGroupName);
            test.TestedElsewhere(x => x.TaskGroupCategory);
            test.TestedElsewhere(x => x.RequiredSkillSet);
            test.TestedElsewhere(x => x.Frequency);
            test.TestedElsewhere(x => x.EquipmentPurposes);
            test.TestedElsewhere(x => x.EquipmentTypes);
            test.TestedElsewhere(x => x.Equipment);
            test.TestedElsewhere(x => x.FacilityAreas);
            test.TestedElsewhere(x => x.HasOtherCompliance);
            test.TestedElsewhere(x => x.OtherComplianceReason);
            test.TestedElsewhere(x => x.AutoCancel);
        }

        #region State

        [TestMethod]
        public void TestStateIsMappedFromState()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.State, _target.MapToEntity(uow, 1, MappingHelper).State.Abbreviation);
            });
        }

        [TestMethod]
        public void TestThrowsWhenStateNotFound()
        {
            _target.State = "this is not a valid state";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenStateNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.State = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region OperatingCenter

        [TestMethod]
        public void TestOperatingCenterIsMappedFromOperatingCenter()
        {
            WithUnitOfWork(uow => {
                Assert.IsTrue(
                    _target.OperatingCenter.StartsWith(_target.MapToEntity(uow, 1, MappingHelper).OperatingCenter.OperatingCenterCode));
            });
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterNotFound()
        {
            _target.OperatingCenter = "this is not a valid Operating Center";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterCannotBeParsed()
        {
            foreach (var value in new[] { "blah", "blah - blah" })
            {
                _target.OperatingCenter = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        [TestMethod]
        public void TestThrowsWhenOperatingCenterNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.OperatingCenter = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        [TestMethod]
        public void TestDoesNotChokeOnOperatingCenterCodeWithoutNumbers()
        {
            WithUnitOfWork(uow => {
                var frequencyUnit = new RecurringFrequencyUnit {
                    Id = RecurringFrequencyUnit.Indices.YEAR
                };
                var oc = uow.Insert(new OperatingCenter {
                    OperatingCenterCode = "ILIU",
                    OperatingCenterName = "Belleville/East St Louis",
                    HydrantInspectionFrequencyUnit = frequencyUnit,
                    LargeValveInspectionFrequencyUnit = frequencyUnit,
                    SmallValveInspectionFrequencyUnit = frequencyUnit,
                    SewerOpeningInspectionFrequencyUnit = frequencyUnit,
                    State = new State {Id = NJState.ID}
                });

                _target.OperatingCenter = $"{oc.OperatingCenterCode} - {oc.OperatingCenterName}";

                Assert.AreEqual(oc.Id, _target.MapToEntity(uow, 1, MappingHelper).OperatingCenter.Id);
            });
        }

        #endregion

        #region PlanningPlant

        [TestMethod]
        public void TestPlanningPlantIsMappedFromDistrict()
        {
            _target.District =
                $"{P218PlanningPlant.CODE} - {OperatingCenters.NJ7.CODE} - {P218PlanningPlant.DESCRIPTION}";

            WithUnitOfWork(uow => {
                Assert.IsTrue(
                    _target.District.StartsWith(_target.MapToEntity(uow, 1, MappingHelper).PlanningPlant.Code));
            });
        }

        [TestMethod]
        public void TestThrowsWhenDistrictNotFound()
        {
            _target.District = "P666 - MI666 - this is not a real planning plant";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenDistrictCannotBeParsed()
        {
            foreach (var value in new[] { "blah", "blah - blah", "blah - blah - blah" })
            {
                _target.District = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        [TestMethod]
        public void TestThrowsWhenDistrictNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.District = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region SkillSet

        [TestMethod]
        public void TestSkillSetIsMappedFromRequiredSkillSet()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.RequiredSkillSet, _target.MapToEntity(uow, 1, MappingHelper).SkillSet.Name);
            });
        }

        [TestMethod]
        public void TestThrowsWhenSkillSetNotFound()
        {
            _target.RequiredSkillSet = "this is not a valid skill set";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region TaskGroupName

        [TestMethod]
        public void TestTaskGroupNameIsMappedFromTaskGroupName()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.TaskGroupName, _target.MapToEntity(uow, 1, MappingHelper).TaskGroup.TaskGroupName);
            });
        }

        [TestMethod]
        public void TestThrowsWhenTaskGroupNameNotFound()
        {
            _target.TaskGroupName = "this is not a valid task group";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenTaskGroupNameNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.TaskGroupName = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region TaskGroupCategory

        [TestMethod]
        public void TestTaskGroupCategoryIsMappedFromTaskGroupCategory()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.TaskGroupCategory, _target.MapToEntity(uow, 1, MappingHelper).TaskGroupCategory.Type);
            });
        }

        [TestMethod]
        public void TestThrowsWhenTaskGroupCategoryNotFound()
        {
            _target.TaskGroupCategory = "this is not a valid TaskGroupCategory";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenTaskGroupCategoryNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.TaskGroupCategory = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region Frequency

        [TestMethod]
        public void TestFrequencyIsMappedFromFrequency()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.Frequency, _target.MapToEntity(uow, 1, MappingHelper).ProductionWorkOrderFrequency.Name);
            });
        }

        [TestMethod]
        public void TestThrowsWhenFrequencyNotFound()
        {
            _target.Frequency = "this is not a valid Frequency";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenFrequencyNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.Frequency = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region EquipmentPurposes

        [TestMethod]
        public void TestEquipmentPurposesAreMappedFromEquipmentPurposes()
        {
            var items = _target.EquipmentPurposes.SplitCommaSeparatedValues();
            WithUnitOfWork(uow => {
                Assert.IsTrue(_target.MapToEntity(uow, 1, MappingHelper)
                                     .EquipmentPurposes.All(x => items.Contains(x.Abbreviation)));
            });
        }

        [TestMethod]
        public void TestThrowsWhenEquipmentPurposesNotFound()
        {
            _target.EquipmentPurposes = "NOT, VALID, EQUIPMENT, TYPES";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region EquipmentTypes

        [TestMethod]
        public void TestEquipmentTypesAreMappedFromEquipmentTypes()
        {
            var items = _target.EquipmentTypes.SplitCommaSeparatedValues();
            WithUnitOfWork(uow => {
                Assert.IsTrue(_target.MapToEntity(uow, 1, MappingHelper)
                                     .EquipmentTypes.All(x => items.Contains(x.Abbreviation)));
            });
        }

        [TestMethod]
        public void TestThrowsWhenEquipmentTypesNotFound()
        {
            _target.EquipmentTypes = "NOT, VALID";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenEquipmentTypesNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.EquipmentTypes = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region Equipment

        [TestMethod]
        public void TestEquipmentAreMappedFromEquipment()
        {
            var items = _target.Equipment.SplitCommaSeparatedValues().Select(x => int.Parse(x));

            WithUnitOfWork(uow => {
                Assert.IsTrue(_target.MapToEntity(uow, 1, MappingHelper)
                                     .Equipment.All(x => items.Contains(x.Id)));
            });
        }

        [TestMethod]
        public void TestThrowsWhenEquipmentNotFound()
        {
            _target.Equipment = "NOT, VALID";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenEquipmentNotProvided()
        {
            foreach (var value in new[] { null, " ", string.Empty })
            {
                _target.Equipment = value;

                WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
            }
        }

        #endregion

        #region FacilityAreas

        [TestMethod]
        public void TestFacilityAreasAreMappedFromFacilityAreas()
        {
            var items = _target.FacilityAreas.SplitCommaSeparatedValues();
            WithUnitOfWork(uow => {
                Assert.IsTrue(_target.MapToEntity(uow, 1, MappingHelper)
                                     .FacilityAreas.All(x => items.Contains(x.FacilityArea.Description)));
            });
        }

        [TestMethod]
        public void TestThrowsWhenFacilityAreasNotFound()
        {
            _target.FacilityAreas = "NOT, VALID";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region Compliance Requirements

        [TestMethod]
        public void TestHasOtherComplianceIsMappedFromHasOtherCompliance()
        {
            _target.HasOtherCompliance = true;
            _target.OtherComplianceReason = "Test"; // This needs to be here otherwise validation will fail

            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.HasOtherCompliance, _target.MapToEntity(uow, 1, MappingHelper).HasOtherCompliance);
            });
        }

        [TestMethod]
        public void TestThrowsWhenHasOtherComplianceIsTrueButOtherComplianceReasonNotProvided()
        {
            _target.HasOtherCompliance = true;
            _target.OtherComplianceReason = null;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenOtherComplianceReasonProvidedAndHasOtherComplianceIsFalse()
        {
            _target.HasOtherCompliance = false;
            _target.OtherComplianceReason = "Test";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenCompanyRequirementTrueAndAutoCancelIsAlsoTrue()
        {
            _target.HasCompanyRequirement = true;
            _target.AutoCancel = true;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenOshaRequirementTrueAndAutoCancelIsAlsoTrue()
        {
            _target.HasOshaRequirement = true;
            _target.AutoCancel = true;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenPsmRequirementTrueAndAutoCancelIsAlsoTrue()
        {
            _target.HasPsmRequirement = true;
            _target.AutoCancel = true;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenRegulatoryRequirementTrueAndAutoCancelIsAlsoTrue()
        {
            _target.HasRegulatoryRequirement = true;
            _target.AutoCancel = true;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenOtherComplianceRequirementTrueAndAutoCancelIsAlsoTrue()
        {
            _target.HasOtherCompliance = true;
            _target.OtherComplianceReason = "Test"; // This needs to be here otherwise validation will fail
            _target.AutoCancel = true;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion

        #region IsActive

        [TestMethod]
        public void TestIsActiveIsMappedFromIsActiveWhenIsActiveIsTrueAndNoDeactivationFieldsAreProvided()
        {
            _target.IsActive = true;
            _target.DeactivationEmployeeId = null;
            _target.DeactivationReason = null;
            _target.DeactivationDate = null;

            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.IsActive, _target.MapToEntity(uow, 1, MappingHelper).IsActive);
            });
        }

        [TestMethod]
        public void TestIsActiveIsMappedFromIsActiveWhenIsActiveIsFalseAndDeactivationFieldsAreProvided()
        {
            _target.IsActive = false;
            _target.DeactivationEmployeeId = "99999999";
            _target.DeactivationReason = "Some Reason";
            _target.DeactivationDate = DateTime.Today;

            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.IsActive, _target.MapToEntity(uow, 1, MappingHelper).IsActive);
            });
        }

        [TestMethod]
        public void TestThrowsWhenIsActiveIsTrueAndDeactivationDateIsProvided()
        {
            _target.IsActive = false;
            _target.DeactivationEmployeeId = null;
            _target.DeactivationReason = null;
            _target.DeactivationDate = DateTime.Today;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenIsActiveIsTrueAndDeactivationReasonIsProvided()
        {
            _target.IsActive = false;
            _target.DeactivationEmployeeId = null;
            _target.DeactivationDate = null;
            _target.DeactivationReason = "Some Reason";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenIsActiveIsTrueAndDeactivationEmployeeIstProvided()
        {
            _target.IsActive = false;
            _target.DeactivationReason = null;
            _target.DeactivationDate = null;
            _target.DeactivationEmployeeId = "99999999";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenIsActiveIsFalseAndDeactivationDateIsNotProvided()
        {
            _target.IsActive = false;
            _target.DeactivationEmployeeId = "99999999";
            _target.DeactivationReason = "Some reason";

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenIsActiveIsFalseAndDeactivationReasonIsNotProvided()
        {
            _target.IsActive = false;
            _target.DeactivationEmployeeId = "99999999";
            _target.DeactivationDate = DateTime.Today;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        [TestMethod]
        public void TestThrowsWhenIsActiveIsFalseAndDeactivationEmployeeIsNotProvided()
        {
            _target.IsActive = false;
            _target.DeactivationReason = "Some Reason";
            _target.DeactivationDate = DateTime.Today;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion
    }
}