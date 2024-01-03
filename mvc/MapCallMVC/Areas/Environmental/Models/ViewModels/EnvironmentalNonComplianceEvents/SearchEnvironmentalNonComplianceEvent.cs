using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System.ComponentModel;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents
{
    public class SearchEnvironmentalNonComplianceEvent : SearchSet<EnvironmentalNonComplianceEvent>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public virtual int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [DropDown("", "PublicWaterSupply", "ActiveByStateIdOrOperatingCenterId", DependsOn = "State,OperatingCenter", DependentsRequired = DependentRequirement.One), EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public virtual int? PublicWaterSupply { get; set; }

        [DropDown("Environmental", "WasteWaterSystem", "ActiveByStateOrOperatingCenter", DependsOn = "State,OperatingCenter", DependentsRequired = DependentRequirement.One), EntityMap, EntityMustExist(typeof(WasteWaterSystem))]
        [View(MapCall.Common.Model.Entities.WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public virtual int? WasteWaterSystem { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Facility))]
        public virtual int? Facility { get; set; }

        public DateRange EventDate { get; set; }

        public DateRange AwarenessDate { get; set; }

        [View(EnvironmentalNonComplianceEvent.DisplayNames.DATE_FINALIZED)]
        public DateRange DateFinalized { get; set; }

        public DateRange DateOfEnvironmentalLeadershipTeamReview { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventCountsAgainstTarget))]
        public int? CountsAgainstTarget { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventStatus))]
        public int? IssueStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventType))]
        public int? IssueType { get; set; }

        [DropDown("Environmental", "EnvironmentalNonComplianceEventSubType", "ByTypeId", DependsOn = "IssueType"), EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventSubType))]
        public virtual int? IssueSubType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventResponsibility))]
        [DisplayName(EnvironmentalNonComplianceEvent.DisplayNames.RESPONSIBILITY)]
        public int? Responsibility { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventEntityLevel))]
        public virtual int? IssuingEntity { get; set; }

        public SearchString NameOfEntity { get; set; }

        public SearchString NameOfThirdParty { get; set; }

        [View(EnvironmentalNonComplianceEvent.DisplayNames.ROOT_CAUSE), 
         MultiSelect, EntityMap, 
         EntityMustExist(typeof(EnvironmentalNonComplianceEventRootCause)), 
         SearchAlias("RootCauses", "Id")]
        public int[] RootCauses { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventFailureType))]
        public virtual int? FailureType { get; set; }

        public virtual int? IssueYear { get; set; }

        public virtual DateRange NOVWorkGroupReviewDate { get; set; }

        public virtual DateRange ChiefEnvOfficerApprovalDate { get; set; }

        #endregion
    }
}
