using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.WasteWaterSystems
{
    public class SearchWasteWaterSystem : SearchSet<WasteWaterSystem>
    {
        #region Properties

        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(State))]
        [SearchAlias("OperatingCenter", "State.Id")]
        public int? State { get; set; }

        [DropDown("", nameof(OperatingCenter), "ByStateId", DependsOn = "State")]
        [EntityMap]
        [EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", nameof(BusinessUnit), "FindByOperatingCenterIdForWasteWaterOrCFS", DependsOn = "OperatingCenter")]
        [EntityMap]
        [EntityMustExist(typeof(BusinessUnit))]
        public int? BusinessUnit { get; set; }

        [View(DisplayName = WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public string WasteWaterSystemName { get; set; }
        public string PermitNumber { get; set; }

        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(WasteWaterSystemStatus))]
        public int? Status { get; set; }

        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(WasteWaterSystemOwnership))]
        public int? Ownership { get; set; }

        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(LicensedOperatorCategory))]
        public int? LicensedOperatorStatus { get; set; }

        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(WasteWaterSystemType))]
        public int? Type { get; set; }

        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(WasteWaterSystemSubType))]
        public int? SubType { get; set; }

        public DateRange DateOfOwnership { get; set; }

        public DateRange DateOfResponsibility { get; set; }

        public NumericRange GravityLength { get; set; }
        public NumericRange ForceLength { get; set; }
        public NumericRange NumberOfLiftStations { get; set; }
        public string TreatmentDescription { get; set; }
        public NumericRange NumberOfCustomers { get; set; }
        public NumericRange PeakFlowMGD { get; set; }
        public bool? IsCombinedSewerSystem { get; set; }
        public bool? HasConsentOrder { get; set; }

        public DateRange ConsentOrderStartDate { get; set; }

        public DateRange ConsentOrderEndDate { get; set; }

        public DateRange NewSystemInitialSafetyAssessmentCompleted { get; set; }

        [View(DisplayName = WasteWaterSystem.DisplayNames.NEW_SYSTEM_INITIAL_WATER_QUALITY_ENVIRONMENTAL_ASSESSMENT_COMPLETED)]
        public DateRange NewSystemInitialWQEnvAssessmentCompleted { get; set; }

        #endregion
    }
}
