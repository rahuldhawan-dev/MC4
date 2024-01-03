using System;
using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;
using MapCall.Common.Metadata;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.RiskRegisterAssets
{
    public abstract class RiskRegisterAssetViewModel : ViewModel<RiskRegisterAsset>
    {
        #region Properties

        [Required,
         DropDown, 
         EntityMap,
         EntityMustExist(typeof(RiskRegisterAssetGroup))]
        public int? RiskRegisterAssetGroup { get; set; } 

        [Required]
        public int? CofMax { get; set; }

        [Required,
         DropDown, 
         EntityMap,
         EntityMustExist(typeof(RiskRegisterAssetCategory))]
        public int? RiskRegisterAssetCategory { get; set; } 

        [Required]
        public int? LofMax { get; set; } 

        [Required,
         DropDown, 
         EntityMap,
         EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [Required,
         EntityMap,
         EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [EntityMap,
         EntityMustExist(typeof(PublicWaterSupply)),
         DropDown("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center")]
        public int? PublicWaterSupply { get; set; }

        [EntityMap,
         EntityMustExist(typeof(WasteWaterSystem)),
         DropDown("Environmental", "WasteWaterSystem", "ByOperatingCenters", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center.")]
        [View(MapCall.Common.Model.Entities.WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public int? WasteWaterSystem { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Facility)),
         DropDown("", "Facility", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center."),
         RequiredWhen(nameof(RiskRegisterAssetGroup), ComparisonType.EqualToAny, nameof(GetFacilityRequiredGroupingIds), typeof(RiskRegisterAssetViewModel), ErrorMessage = "Facility is required for facility / equipment groupings.")]
        public int? Facility { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Equipment)),
         DropDown("", "Equipment", "ByFacilityId", DependsOn = nameof(Facility), PromptText = "Please select a Facility.")]
        public int? Equipment { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Coordinate)),
         Coordinate(IconSet = IconSets.Pins),
         RequiredWhen(nameof(RiskRegisterAssetGroup), MapCall.Common.Model.Entities.RiskRegisterAssetGroup.Indices.TRANSMISSION_AND_DISTRIBUTION)]
        public virtual int? Coordinate { get; set; }

        [Required,
         EntityMap, 
         EntityMustExist(typeof(Employee)),
         DropDown("", "Employee", "RiskRegisterEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center.")]
        public int? Employee { get; set; }

        [Required,
         DataType(DataType.MultilineText),
         StringLength(RiskRegisterAsset.StringLengths.IMPACT_DESCRIPTION)]
        public virtual string ImpactDescription { get; set; }

        [Required,
         DataType(DataType.MultilineText),
         StringLength(RiskRegisterAsset.StringLengths.RISK_DESCRIPTION)]
        public virtual string RiskDescription { get; set; }

        [Required, Range(RiskRegisterAsset.Ranges.RISK_QUADRANT_LOW, RiskRegisterAsset.Ranges.RISK_QUADRANT_HIGH)]
        public virtual int? RiskQuadrant { get; set; }

        [Required]
        public virtual DateTime? IdentifiedAt { get; set; }

        [DataType(DataType.MultilineText),
         StringLength(RiskRegisterAsset.StringLengths.INTERIM_MITIGATION_MEASURES_TAKEN),
         RequiredWhen(nameof(InterimMitigationMeasuresTakenAt), ComparisonType.NotEqualTo, null)]
        public virtual string InterimMitigationMeasuresTaken { get; set; }

        [RequiredWhen(nameof(InterimMitigationMeasuresTaken), ComparisonType.NotEqualTo, null)]
        public virtual DateTime? InterimMitigationMeasuresTakenAt { get; set; }
        
        public virtual decimal? InterimMitigationMeasuresTakenEstimatedCosts { get; set; }

        [DataType(DataType.MultilineText),
         StringLength(RiskRegisterAsset.StringLengths.FINAL_MITIGATION_MEASURES_TAKEN),
         RequiredWhen(nameof(FinalMitigationMeasuresTakenAt), ComparisonType.NotEqualTo, null)]
        public virtual string FinalMitigationMeasuresTaken { get; set; }

        [RequiredWhen(nameof(FinalMitigationMeasuresTaken), ComparisonType.NotEqualTo, null)]
        public virtual DateTime? FinalMitigationMeasuresTakenAt { get; set; }

        public virtual decimal? FinalMitigationMeasuresTakenEstimatedCosts { get; set; }

        public virtual DateTime? CompletionTargetDate { get; set; }

        public virtual DateTime? CompletionActualDate { get; set; }

        public virtual bool IsProjectInComprehensivePlanningStudy { get; set; }

        public virtual bool IsProjectInCapitalPlan { get; set; }

        [StringLength(RiskRegisterAsset.StringLengths.RELATED_WORK_BREAKDOWN_STRUCTURE)]
        public virtual string RelatedWorkBreakdownStructure { get; set; }

        [Required]
        public int? TotalRiskWeighted { get; set; }

        [StringLength(RiskRegisterAsset.StringLengths.RISK_REGISTER_ID)]
        public virtual string RiskRegisterId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RiskRegisterAssetZone))]
        public int? RiskRegisterAssetZone { get; set; }

        #endregion

        #region Constructors

        public RiskRegisterAssetViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            IdentifiedAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
        }

        public static int[] GetFacilityRequiredGroupingIds() => new[] {
            MapCall.Common.Model.Entities.RiskRegisterAssetGroup.Indices.FACILITY,
            MapCall.Common.Model.Entities.RiskRegisterAssetGroup.Indices.EQUIPMENT
        };

        #endregion
    }
}