using System;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.RiskRegisterAssets
{
    public class SearchRiskRegisterAssetViewModel : SearchSet<RiskRegisterAsset>
    {
        #region Properties

        public int? Id { get; set; }

        [DropDown, 
         EntityMap,
         EntityMustExist(typeof(RiskRegisterAssetGroup)),
         View(RiskRegisterAsset.ViewDisplayNames.GROUP)]
        public int? RiskRegisterAssetGroup { get; set; }

        [View(RiskRegisterAsset.ViewDisplayNames.IMPACT)]
        public IntRange CofMax { get; set; } 

        [DropDown, 
         EntityMap,
         EntityMustExist(typeof(RiskRegisterAssetCategory)),
         View(RiskRegisterAsset.ViewDisplayNames.CATEGORY)]
        public int? RiskRegisterAssetCategory { get; set; }

        [View(RiskRegisterAsset.ViewDisplayNames.PROBABILITY)]
        public IntRange LofMax { get; set; } 

        [DropDown, 
         EntityMap,
         EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [EntityMap,
         EntityMustExist(typeof(OperatingCenter)),
         DropDown("", "OperatingCenter", "ByStateIdForEngineeringRiskRegisterAssets", DependsOn = nameof(State), PromptText = "Please select a State.")]
        public int? OperatingCenter { get; set; }

        [EntityMap,
         EntityMustExist(typeof(PublicWaterSupply)),
         DropDown("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center.")]
        public int? PublicWaterSupply { get; set; }

        [EntityMap,
         EntityMustExist(typeof(WasteWaterSystem)),
         DropDown("Environmental", "WasteWaterSystem", "ByOperatingCenters", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center.")]
        [View(DisplayName = MapCall.Common.Model.Entities.WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public int? WasteWaterSystem { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Facility)),
         DropDown("", "Facility", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center.")]
        public int? Facility { get; set; }

        [EntityMap, 
         EntityMustExist(typeof(Employee)),
         DropDown("", "Employee", "RiskRegisterEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center."),
         View(RiskRegisterAsset.ViewDisplayNames.EMPLOYEE)]
        public int? Employee { get; set; }

        [View(RiskRegisterAsset.ViewDisplayNames.RISK_QUADRANT)]
        public virtual IntRange RiskQuadrant { get; set; }

        [View(RiskRegisterAsset.ViewDisplayNames.IDENTIFIED_AT)]
        public virtual DateTime? IdentifiedAt { get; set; }

        [View(RiskRegisterAsset.ViewDisplayNames.COMPLETION_TARGET_DATE)]
        public virtual DateTime? CompletionTargetDate { get; set; }

        [View(RiskRegisterAsset.ViewDisplayNames.COMPLETION_ACTUAL_DATE)]
        public virtual DateTime? CompletionActualDate { get; set; }

        [View(RiskRegisterAsset.ViewDisplayNames.IS_PROJECT_IN_COMPREHENSIVE_PLANNING_STUDY)]
        public virtual bool? IsProjectInComprehensivePlanningStudy { get; set; }

        [View(RiskRegisterAsset.ViewDisplayNames.IS_PROJECT_IN_CAPITAL_PLAN)]
        public virtual bool? IsProjectInCapitalPlan { get; set; }

        [View(RiskRegisterAsset.ViewDisplayNames.RISK_REGISTER_ID)]
        public SearchString RiskRegisterId { get; set; }

        public virtual IntRange TotalRiskWeighted { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RiskRegisterAssetZone)),
         View(RiskRegisterAsset.ViewDisplayNames.RISK_REGISTER_ZONE)]
        public int? RiskRegisterAssetZone { get; set; }

        #endregion
    }

    public class RiskRegisterAssetExcelDisclaimerViewModel
    {
        public readonly struct ViewDisplayNames
        {
            public const string
                DISCLAIMER = "The information contained in the Risk Register is Confidential – Security Sensitive Information and is to be used for internal business purposes only.";
        }
        public string Disclaimer { get; set; }

        public RiskRegisterAssetExcelDisclaimerViewModel()
        {
            Disclaimer = ViewDisplayNames.DISCLAIMER;
        }
    }
}