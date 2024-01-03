using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Library;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using System;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;
using System.Linq;

namespace MapCallImporter.Models.Import
{
    public class MaintenancePlanExcelRecord : ExcelRecordBase<MaintenancePlan, MyCreateMaintenancePlan, MaintenancePlanExcelRecord>
    {
        #region Properties

        #region Core Fields

        public string State { get; set; }
        public string OperatingCenter { get; set; }

        [AutoMap("PlanningPlant")]
        public string District { get; set; }

        [AutoMap("Facility")]
        public int FacilityId { get; set; }

        [DoesNotAutoMap("FacilityName is for the Business to include in their template, but isn't used for mapping.")]
        public string FacilityName { get; set; }

        [AutoMap("TaskGroup")]
        public string TaskGroupName { get; set; }

        public string TaskGroupCategory { get; set; }

        [AutoMap("EstimatedHours")]
        public decimal? EstimatedCompletionTime { get; set; }

        [AutoMap("Resources")]
        public decimal? NumberOfResources { get; set; }

        [AutoMap("SkillSet")]
        public string RequiredSkillSet { get; set; }

        [AutoMap("ContractorCost")]
        public decimal? EstimatedContractorCost { get; set; }

        [AutoMap("ProductionWorkOrderFrequency")]
        public string Frequency { get; set; }

        public DateTime Start { get; set; }
        
        public bool IsActive { get; set; }
        
        public string DeactivationReason { get; set; }
       
        public string DeactivationEmployeeId { get; set; }

        public DateTime? DeactivationDate { get; set; }
        
        [AutoMap("HasACompletionRequirement")]
        public bool AutoCancel { get; set; }

        public string AdditionalTaskDetails { get; set; }
        public bool HasCompanyRequirement { get; set; }
        public bool HasOshaRequirement { get; set; }
        public bool HasPsmRequirement { get; set; }
        public bool HasRegulatoryRequirement { get; set; }
        public bool HasOtherCompliance { get; set; }
        public string OtherComplianceReason { get; set; }
        public string LocalTaskDescription { get; set; }
       
        public string EquipmentPurposes { get; set; }
        public string EquipmentTypes { get; set; }
        public string Equipment { get; set; }
        public string FacilityAreas { get; set; }

        #endregion

        #endregion

        #region Private Methods

        protected override MyCreateMaintenancePlan MapExtra(MyCreateMaintenancePlan viewModel, IUnitOfWork uow, int index,
            ExcelRecordItemHelperBase<MaintenancePlan> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.State = StringToEntity<State>(uow, 
                index, 
                helper, 
                nameof(State),
                State, 
                s => s.Abbreviation == State);
            
            viewModel.OperatingCenter = CommonModelMethods.FindOperatingCenter(OperatingCenter, 
                nameof(OperatingCenter), 
                uow, 
                index,
                helper);

            viewModel.PlanningPlant = CommonModelMethods.LookupPlanningPlant(uow, 
                index, 
                helper,
                nameof(District), 
                District);

            viewModel.TaskGroup = StringToEntity<TaskGroup>(uow, 
                index, 
                helper, 
                nameof(TaskGroupName), 
                TaskGroupName, 
                x => x.TaskGroupName == TaskGroupName);
            
            viewModel.TaskGroupCategory = StringToEntity<TaskGroupCategory>(uow, 
                index, 
                helper, 
                nameof(TaskGroupCategory), 
                TaskGroupCategory, 
                x => x.Type == TaskGroupCategory);

            viewModel.SkillSet = StringToEntity<SkillSet>(uow, 
                index, 
                helper, 
                nameof(SkillSet), 
                RequiredSkillSet,
                x => x.Name == RequiredSkillSet);

            viewModel.ProductionWorkOrderFrequency = StringToEntity<ProductionWorkOrderFrequency>(uow, 
                index, 
                helper, 
                nameof(Frequency), 
                Frequency, 
                x => x.Name == Frequency);

            viewModel.Equipment = IdsToEntities<MapCall.Common.Model.Entities.Equipment>(uow, 
                index, 
                helper, 
                nameof(Equipment), 
                Equipment);

            viewModel.EquipmentPurposes = StringValuesToEntities<EquipmentPurpose>(uow, 
                index, 
                helper, 
                nameof(EquipmentPurposes), 
                EquipmentPurposes, 
                x => x.Abbreviation);

            viewModel.EquipmentTypes = StringValuesToEntities<EquipmentType>(uow,
                index,
                helper, 
                nameof(EquipmentTypes), 
                EquipmentTypes, 
                x => x.Abbreviation);

            viewModel.FacilityAreas = CommonModelMethods.LookupFacilityAreas(uow, 
                index,
                helper, 
                FacilityId, 
                nameof(FacilityAreas), 
                FacilityAreas);

            viewModel.DeactivationEmployee = CommonModelMethods.LookupEmployee(uow,
                index,
                helper,
                nameof(DeactivationEmployeeId),
                DeactivationEmployeeId);

            return viewModel;
        }

        private void EnsureOtherComplianceReasonIsGivenIfHasOtherComplianceIsTrue(int index, ExcelRecordItemHelperBase<MaintenancePlan> helper)
        {
            if (!HasOtherCompliance)
            {
                return;
            }

            if (string.IsNullOrEmpty(OtherComplianceReason))
            {
                helper.AddFailure($"Row {index}: The column '{nameof(OtherComplianceReason)}' must have a value if '{nameof(HasOtherCompliance)}' is set to TRUE.");
            }
        }

        private void EnsureOtherComplianceReasonIsOmittedIfHasOtherComplianceIsFalse(int index, ExcelRecordItemHelperBase<MaintenancePlan> helper)
        {
            if (HasOtherCompliance)
            {
                return;
            }

            if (!string.IsNullOrEmpty(OtherComplianceReason))
            {
                helper.AddFailure($"Row {index}: The column '{nameof(OtherComplianceReason)}' must be left blank if '{nameof(HasOtherCompliance)}' is set to FALSE.");
            }
        }

        private void EnsureAutoCancelIsFalseIfAnyComplianceRequirementIsTrue(int index, ExcelRecordItemHelperBase<MaintenancePlan> helper)
        {
            if (AutoCancel && (HasCompanyRequirement || HasOshaRequirement || HasPsmRequirement || HasRegulatoryRequirement || HasOtherCompliance))
            {
                helper.AddFailure($"Row {index}: The value in the column '{nameof(AutoCancel)}' must be FALSE if any Compliance Requirements are set to TRUE.");
            }
        }

        private void EnsureDeactivationFieldsArePopulatedForInActivePlans(int index, ExcelRecordItemHelperBase<MaintenancePlan> helper)
        {
            if (!IsActive)
            {
                if (string.IsNullOrEmpty(DeactivationReason))
                {
                    helper.AddFailure($"Row {index}: The column '{nameof(DeactivationReason)}' must have a value if '{nameof(IsActive)}' is set to FALSE.");
                }

                if (DeactivationDate == null)
                {
                    helper.AddFailure($"Row {index}: The column '{nameof(DeactivationDate)}' must have a value if '{nameof(IsActive)}' is set to FALSE.");
                }

                if (DeactivationEmployeeId == null)
                {
                    helper.AddFailure($"Row {index}: The column '{nameof(DeactivationEmployeeId)}' must have a value if '{nameof(IsActive)}' is set to FALSE.");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(DeactivationReason))
                {
                    helper.AddFailure($"Row {index}: The column '{nameof(DeactivationReason)}' must be left blank if '{nameof(IsActive)}' is set to TRUE.");
                }

                if (DeactivationDate != null)
                {
                    helper.AddFailure($"Row {index}: The column '{nameof(DeactivationDate)}' must be left blank if '{nameof(IsActive)}' is set to TRUE.");
                }

                if (DeactivationEmployeeId != null)
                {
                    helper.AddFailure($"Row {index}: The column '{nameof(DeactivationEmployeeId)}' must be left blank if '{nameof(IsActive)}' is set to TRUE.");
                }
            }
        }

        #endregion

        #region Exposed Methods

        public override MaintenancePlan MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<MaintenancePlan> helper)
        {
            EnsureOtherComplianceReasonIsGivenIfHasOtherComplianceIsTrue(index, helper);
            EnsureAutoCancelIsFalseIfAnyComplianceRequirementIsTrue(index, helper);
            EnsureOtherComplianceReasonIsOmittedIfHasOtherComplianceIsFalse(index, helper);
            EnsureDeactivationFieldsArePopulatedForInActivePlans(index, helper);
            
            //ViewModel Overwrites the Employee for deactivated Maintenance Plans, need to set the following two fields back
            var entity = base.MapToEntity(uow, index, helper);
            if (entity != null)
            {
                if (!(IsActive || DeactivationDate == null || DeactivationEmployeeId == null))
                {
                    entity.DeactivationDate = this.DeactivationDate;
                    entity.DeactivationEmployee = LookUpEmployee(uow);
                }
            }
            return entity;
        }

        private Employee LookUpEmployee(IUnitOfWork uow)
        {
            var result = uow.GetRepository<Employee>().Where(u => u.EmployeeId == DeactivationEmployeeId).SingleOrDefault();
            if (result == null)
            {
                return null;
            }
            
            return new Employee {
                Id = result.Id
            };
        }
        
        public override MaintenancePlan MapAndValidate(IUnitOfWork uow, int index,
            ExcelRecordItemValidationHelper<MaintenancePlan, MyCreateMaintenancePlan, MaintenancePlanExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            return entity;
        }

        #endregion
    }
}
