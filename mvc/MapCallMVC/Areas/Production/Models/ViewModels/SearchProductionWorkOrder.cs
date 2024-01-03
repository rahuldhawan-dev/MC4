using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.ViewModels;
using MMSINC.Utilities.ObjectMapping;


namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public enum PerformanceSearchType
    {
        Created,
        Unscheduled,
        Scheduled,
        Incomplete,
        Canceled,
        Completed,
        NotApproved,
    }

    public class SearchProductionWorkOrderBase : SearchSet<ProductionWorkOrder>, ISearchProductionWorkOrder, ISearchProductionWorkOrderHistory
    {
        #region Properties

        [EntityMap, EntityMustExist(typeof(MaintenancePlan))]
        public virtual int? MaintenancePlan { get; set; }

        [View(DisplayName = "Id")]
        public int? EntityId { get; set; }
        [DropDown("", "Employee", "EmployeesWithCurrentProductionAssignmentsByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMustExist(typeof(Employee))]
        [DisplayName("Currently Assigned Employee"), SearchAlias("CurrentAssignments", "AssignedTo.Id")]
        public virtual int? Employee { get; set; }
        [DropDown(Area = "", Controller = "PlanningPlant", Action = "ByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]        
        [EntityMap, EntityMustExist(typeof(PlanningPlant))]
        public virtual int? PlanningPlant { get; set; }
        [UIHint("FunctionalLocation")]
        public string FunctionalLocation { get; set; }
        [DropDown, EntityMustExist(typeof(ProductionSkillSet)), SearchAlias("ProductionWorkDescription", "pwd", "ProductionSkillSet.Id")]
        public int? ProductionSkillSet { get; set; }

        public DateRange BasicStart { get; set; }
        public DateRange BasicFinish { get; set; }

        [DropDown(Area = "", Controller = "Facility", Action = "GetByPublicWaterSupplyId", DependsOn = "PublicWaterSupply", PromptText = "Please select PWSID above", DependentsRequired = DependentRequirement.One)]
        [EntityMustExist(typeof(Facility))]
        [EntityMap]
        public virtual int? Facility { get; set; }

        [SearchAlias("FacilityFacilityArea", "ffa", "Id", Required = true)]
        [DropDown(Area = "", Controller = "FacilityFacilityArea", Action = "ByFacilityId", DependsOn = "Facility", PromptText = "Please select Facility above")]
        [View("Facility Area")]
        public virtual int? FacilityFacilityArea { get; set; }

        [SearchAlias("ffa.FacilityArea", "area", "Id", Required = true)]
        public virtual int? FacilityArea { get; set; }

        public bool? IsOpen { get; set; }

        [View("Lockout Form Created"), Search(ChecksExistenceOfChildCollection = true)]
        public bool? LockoutForms { get; set; }

        [View("Has Well Test"), Search(ChecksExistenceOfChildCollection = true)]
        public bool? WellTests { get; set; }

        [View("Lockout Forms Still Open?")]
        public bool? IsLockoutFormStillOpen { get; set; }
        [View("Is the Work Order Assigned ?")]
        public bool? HasAssignmentsOnNonCancelledWorkOrder { get; set; }

        [CheckBox,
         View("Red Tag Permits Still Open?")]
        public bool? IsRedTagPermitStillOpen { get; set; }

        [View("Confined Space Form Created"), Search(ChecksExistenceOfChildCollection = true)]
        public bool? ConfinedSpaceForms { get; set; }
        
        [View("Has Tank Inspection Form"), Search(ChecksExistenceOfChildCollection = true)]
        
        public bool? TankInspections { get; set; }
        
        public string LocalTaskDescription { get; set; }

        [DropDown]
        [EntityMap, EntityMustExist(typeof(EquipmentType))]
        public int? EquipmentType { get; set; }

        [DropDown(Area = "", Controller = "Equipment", Action = "ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId", DependsOn = "Facility,FacilityFacilityArea,EquipmentType", PromptText = "Select a facility above.", DependentsRequired = DependentRequirement.One)]
        [EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }
        
        [View(DisplayName = "SAP Equipment #")]
        public virtual int? SAPEquipmentId { get; set; }

        [StringLength(ProductionWorkOrder.StringLengths.SAP_WORK_ORDER)]
        public string SAPWorkOrder { get; set; }

        [DropDown, EntityMustExist(typeof(ProductionWorkOrderPriority))]
        public int? Priority { get; set; }

        [MultiSelect, EntityMustExist(typeof(OrderType))]
        [SearchAlias("ProductionWorkDescription", "pwd", "OrderType.Id")]
        public int[] OrderType { get; set; }

        [DropDown("SAP", "PlantMaintenanceActivityType", "ByOrderTypeIds", DependsOn = "OrderType", PromptText = "Please select an Order Type."), EntityMap, EntityMustExist(typeof(PlantMaintenanceActivityType))]
        [SearchAlias("ProductionWorkDescription", "pwd", "PlantMaintenanceActivityType.Id")]
        public int? PlantMaintenanceActivityType { get; set; }

        // Unless you want duplicates, this needs to be a DropDown and not a Mulitselect. This is a one to many list
        [DropDown, EntityMap, EntityMustExist(typeof(ProductionPrerequisite))]
        [SearchAlias("ProductionWorkOrderProductionPrerequisites", "ppre", "ProductionPrerequisite.Id")]
        public int? Prerequisites { get; set; }

        public DateRange DateReceived { get; set; }

        public string WBSElement { get; set; }
        public virtual long? SAPMaintenancePlanId { get; set; }
        public bool? HasProcessSafetyManagement { get; set; }
        public bool? HasCompanyRequirement { get; set; }
        public bool? HasRegulatoryRequirement { get; set; }
        public bool? HasOshaRequirement { get; set; }
        public bool? OtherCompliance { get; set; }
        [View("Pre Job Safety Brief created")]
        [Search(ChecksExistenceOfChildCollection = true)]
        public bool? ProductionPreJobSafetyBriefs { get; set; }
        
        [DropDown("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an operating center")]
        [EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        [SearchAlias("Facility", "PublicWaterSupply.Id")]
        [View(ProductionWorkOrder.DisplayNames.PUBLIC_WATER_SUPPLY)]
        public int? PublicWaterSupply { get; set; }

        [DropDown("Environmental", "PublicWaterSupplyPressureZone", "ByPublicWaterSupply", DependsOn = nameof(PublicWaterSupply), PromptText = "Please select a public water supply")]
        [EntityMap, EntityMustExist(typeof(PublicWaterSupplyPressureZone))]
        [SearchAlias("Facility", "PublicWaterSupplyPressureZone.Id")]
        [View(ProductionWorkOrder.DisplayNames.PUBLIC_WATER_SUPPLY_PRESSURE_ZONE)]
        public int? PublicWaterSupplyPressureZone { get; set; }

        [SearchAlias("MaintenancePlan", "PlanNumber")]
        public virtual string PlanNumber { get; set; }

        #region Properties used by the Production Work Order Performance report

        [Search(CanMap = false)]
        public PerformanceSearchType? PerformanceSearchType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("OperatingCenter", "oc", "State.Id")]
        public int? State { get; set; }
        [Search(ChecksExistenceOfChildCollection = true)]
        public bool? EmployeeAssignments { get; set; }
        public DateRange DateCancelled { get; set; }
        public DateRange DateCompleted { get; set; }
        public DateRange ApprovedOn { get; set; }

        [SearchAlias("EmployeeAssignments", "DateStarted")]
        public DateRange EmployeeAssignmentsDateStarted { get; set; }

        [View(ProductionWorkOrder.DisplayNames.AUTO_CREATED_CORRECTIVE_WORK_ORDER)]
        public bool? AutoCreatedCorrectiveWorkOrder { get; set; }

        // This is needed because the links on the production work order performance
        // report need to have links to search results where the PlanningPlant is specifically
        // null. However, we can't assume that anytime the PlanningPlant property is null that
        // we need that explicit null check because other variations of the report do not
        // send the PlanningPlant value ever.
        [Search(CanMap = false)]
        public bool? PlanningPlantIsNull { get; set; }

        [SearchAlias("ProductionWorkOrderRequiresSupervisorApproval", "RequiresSupervisorApproval")]
        public bool? RequiresSupervisorApproval { get; set; }

        [Search(CanMap = false)]
        public bool? RegulatoryComplianceSearch { get; set; }

        [SearchAlias("Equipments", "e", "Equipment.Id")]
        public int? Equipments { get; set; }

        [DoesNotAutoMap]
        public int? OperatingCenterId { get; set; }

        #endregion

        #endregion

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            if (PerformanceSearchType.HasValue)
            {
                // NOTE: All of this is duplicating logic done in ProductionWorkOrderRepository.CreatePerformanceResultModel for
                // the ProductionWorkOrderPerformance report.
                // This is necessary due to that report linking to the regular search results so they can
                // drill down on the counts from the report.
                var mp = mapper.MappedProperties;
                switch (PerformanceSearchType.Value)
                {
                    case ViewModels.PerformanceSearchType.Unscheduled:
                        mp[nameof(EmployeeAssignments)].Value = false;
                        mp[nameof(DateCancelled)].Value = SearchMapperSpecialValues.IsNull;
                        mp[nameof(DateCompleted)].Value = SearchMapperSpecialValues.IsNull;
                        break;

                    case ViewModels.PerformanceSearchType.Scheduled:
                        mp[nameof(DateCancelled)].Value = SearchMapperSpecialValues.IsNull;
                        mp[nameof(DateCompleted)].Value = SearchMapperSpecialValues.IsNull;
                        mp[nameof(EmployeeAssignments)].Value = true;
                        mp[nameof(EmployeeAssignmentsDateStarted)].Value = SearchMapperSpecialValues.IsNull;
                        break;

                    case ViewModels.PerformanceSearchType.Incomplete:
                        mp[nameof(DateCancelled)].Value = SearchMapperSpecialValues.IsNull;
                        mp[nameof(DateCompleted)].Value = SearchMapperSpecialValues.IsNull;
                        mp[nameof(EmployeeAssignmentsDateStarted)].Value = SearchMapperSpecialValues.IsNotNull;
                        break;

                    case ViewModels.PerformanceSearchType.Canceled:
                        mp[nameof(DateCancelled)].Value = SearchMapperSpecialValues.IsNotNull;
                        break;

                    case ViewModels.PerformanceSearchType.Completed:
                        if (!(RegulatoryComplianceSearch.GetValueOrDefault() && DateCompleted != null))
                        {
                            mp[nameof(DateCompleted)].Value = SearchMapperSpecialValues.IsNotNull;
                        }
                        break;

                    case ViewModels.PerformanceSearchType.NotApproved:
                        mp[nameof(DateCompleted)].Value = SearchMapperSpecialValues.IsNotNull;
                        mp[nameof(ApprovedOn)].Value = SearchMapperSpecialValues.IsNull;
                        break;
                }

                if (PlanningPlantIsNull.GetValueOrDefault())
                {
                    mp[nameof(PlanningPlant)].Value = SearchMapperSpecialValues.IsNull;
                }

                if (RegulatoryComplianceSearch.GetValueOrDefault() && PerformanceSearchType.Value == ViewModels.PerformanceSearchType.Incomplete)
                {
                    mp[nameof(EmployeeAssignmentsDateStarted)].Value = SearchMapperSpecialValues.IsNull;
                }
            }
        }

        public override string DefaultSortBy => nameof(DateReceived);
        public override bool DefaultSortAscending => true;
    }

    public class SearchProductionWorkOrder : SearchProductionWorkOrderBase
    {
        #region Properties

        [MultiSelect("", "OperatingCenter", "ByStateIdForProductionWorkManagement", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int[] OperatingCenter { get; set; }
        
        [DropDown("", "Employee", "EmployeesWithCurrentProductionAssignmentsByOperatingCenterIds", DependsOn = "OperatingCenter"), EntityMustExist(typeof(Employee))]
        //Must add an alias in SearchAlias(2nd param) if used in Join Query. See ProductionWorkOrderRepository CurrentAssignment join in SearchForExcel method
        [DisplayName("Currently Assigned Employee"), SearchAlias("CurrentAssignments", "currentAssignment", "AssignedTo.Id")]
        public override int? Employee { get; set; }
        
        [DropDown(Area = "", Controller = "PlanningPlant", Action = "ByOperatingCenters", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMap, EntityMustExist(typeof(PlanningPlant))]
        public override int? PlanningPlant { get; set; }
        
        [DropDown(Area = "", Controller = "Facility", Action = "ActiveByOperatingCentersOrPlanningPlant", DependsOn = "OperatingCenter,PlanningPlant", PromptText = "Please select an Operating Center above", DependentsRequired = DependentRequirement.One)]
        [EntityMustExist(typeof(Facility))]
        [EntityMap]
        public override int? Facility { get; set; }

        [DropDown(Area = "", Controller = "FacilityFacilityArea", Action = "ByFacilityId", DependsOn = "Facility", PromptText = "Please select Facility above")]
        [View("Facility Area")]
        public override int? FacilityFacilityArea { get; set; }

        [Search(CanMap = false)]
        public MaintenancePlan MaintenancePlanEntity { get; set; }

        [EntityMap, MultiSelect, EntityMustExist(typeof(MaintenancePlanTaskType))]
        [SearchAlias("MaintenancePlan.TaskGroup", "MaintenancePlanTaskType.Id")]
        public int[] TaskType { get; set; }

        [EntityMap, EntityMustExist(typeof(TaskGroup))]
        [View(DisplayName = ProductionWorkOrder.DisplayNames.TASK_GROUP_NAME), SearchAlias("MaintenancePlan", "TaskGroup.Id", Required = true)]
        [MultiSelect(Area = "Production", Controller = "TaskGroup", Action = "ByMaintenancePlanTaskTypeIds", DependsOn = "TaskType", PromptText = "Please select a Task Type above.")]
        public int[] TaskGroup { get; set; }

        [EntityMap, EntityMustExist(typeof(ProductionWorkDescription))]
        [View(DisplayName = ProductionWorkOrder.DisplayNames.WORK_DESCRIPTION), SearchAlias("ProductionWorkDescription", "pwd", "Id", Required = true)]
        [MultiSelect(Area = "Production", Controller = "ProductionWorkDescription", Action = "ByEquipmentTypeIdForCreate", DependsOn = "EquipmentType")]
        public int[] ProductionWorkDescription { get; set; }

        #endregion
    }

    public class SearchProductionWorkOrderFromMaintenancePlan : SearchSet<ProductionWorkOrder>
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(MaintenancePlan))]
        public int? MaintenancePlan { get; set; }

        [Search(CanMap = false)]
        public MaintenancePlan MaintenancePlanEntity { get; set; }

        public DateRange DateReceived { get; set; }

        public override string DefaultSortBy => nameof(DateReceived);
        public override bool DefaultSortAscending => false;

        #endregion
    }
}
