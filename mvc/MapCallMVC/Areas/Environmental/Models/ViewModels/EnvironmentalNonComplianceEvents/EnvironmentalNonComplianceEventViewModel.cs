using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using MapCall.Common.Model.Entities.Users;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents
{
    public class EnvironmentalNonComplianceEventViewModel : ViewModel<EnvironmentalNonComplianceEvent>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [Required]
        public virtual int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(WaterType))]
        [Required]
        public virtual int? WaterType { get; set; }     

        [DropDown("", "PublicWaterSupply", "ActiveByStateIdOrOperatingCenterId", DependsOn = "State,OperatingCenter", DependentsRequired = DependentRequirement.One), EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        [RequiredWhen("WaterType", ComparisonType.NotEqualTo, MapCall.Common.Model.Entities.WaterType.Indices.WASTEWATER)]
        public virtual int? PublicWaterSupply { get; set; }

        [DropDown("Environmental", "WasteWaterSystem", "ActiveByStateOrOperatingCenter", DependsOn = "State,OperatingCenter",DependentsRequired = DependentRequirement.One), EntityMap, EntityMustExist(typeof(WasteWaterSystem))]
        [RequiredWhen("WaterType", ComparisonType.NotEqualTo, MapCall.Common.Model.Entities.WaterType.Indices.WATER)]
        [View(MapCall.Common.Model.Entities.WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public virtual int? WasteWaterSystem { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Facility))]
        public virtual int? Facility { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventType))]
        [Required]
        public virtual int? IssueType { get; set; }

        [DropDown("Environmental", "EnvironmentalNonComplianceEventSubType", "ByTypeId", DependsOn = "IssueType"), EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventSubType))]
        public virtual int? IssueSubType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventResponsibility))]
        [Required]
        public virtual int? Responsibility { get; set; }

        [View(EnvironmentalNonComplianceEvent.DisplayNames.ROOT_CAUSE),MultiSelect, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventRootCause))]
        [Required]
        public virtual int[] RootCauses { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventFailureType))]
        [RequiredWhen("DateFinalized", ComparisonType.NotEqualTo, null)]
        public virtual int? FailureType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventStatus))]
        [Required]
        public virtual int? IssueStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventEntityLevel))]
        [RequiredWhen("IssueStatus", ComparisonType.EqualTo, EnvironmentalNonComplianceEventStatus.Indices.CONFIRMED)]
        public virtual int? IssuingEntity { get; set; }

        [Required]
        public virtual DateTime? EventDate { get; set; }

        public virtual DateTime? DateFinalized { get; set; }

        public virtual DateTime? EnforcementDate { get; set; }

        [Required]
        public virtual DateTime? AwarenessDate { get; set; }

        [Required, Multiline]
        public virtual string SummaryOfEvent { get; set; }

        [StringLength(EnvironmentalNonComplianceEvent.StringLengths.RESPONSIBILITY_NAME)]
        [RequiredWhen("Responsibility", ComparisonType.EqualTo, EnvironmentalNonComplianceEventResponsibility.Indices.THIRD_PARTY, FieldOnlyVisibleWhenRequired = true)]
        public virtual string NameOfThirdParty { get; set; }

        [StringLength(EnvironmentalNonComplianceEvent.StringLengths.FAILURE_TYPE_DESCRIPTION)]
        [RequiredWhen("FailureType", ComparisonType.NotEqualTo, null)]
        public virtual string FailureTypeDescription { get; set; }

        [RegularExpression("^\\d{1,6}(\\.\\d{1,2})?$", ErrorMessage = "Decimal must be no larger than the hundredths place ex. 500.00")]
        public virtual decimal? FineAmount { get; set; }

        [RequiredWhen("IssuingEntity", MapCall.Common.Model.Entities.EnvironmentalNonComplianceEventEntityLevel.Indices.OTHER)]
        [StringLength(EnvironmentalNonComplianceEvent.StringLengths.ISSUING_ENTITY_NAME)]
        public virtual string NameOfEntity { get; set; }

        public virtual int? IssueYear { get; set; }
        public DateTime? NOVWorkGroupReviewDate { get; set; }
        public DateTime? ChiefEnvOfficerApprovalDate { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventCountsAgainstTarget))]
        public virtual int? CountsAgainstTarget { get; set; }

        #endregion

        #region Constructors

        public EnvironmentalNonComplianceEventViewModel(IContainer container) : base(container) { }

        #endregion
    }
}
