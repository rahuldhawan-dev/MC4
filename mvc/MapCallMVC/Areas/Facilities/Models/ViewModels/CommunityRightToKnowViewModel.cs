using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MMSINC.Utilities.ObjectMapping;
using MapCall.Common.Model.Entities.Users;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class CommunityRightToKnowViewModel : ViewModel<CommunityRightToKnow>
    {
        #region Properties

        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State", PromptText = "Select a state above"), DoesNotAutoMap]
        [EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required, EntityMustExist(typeof(Facility)), EntityMap]
        [DropDown("", "Facility", "GetByOperatingCenterIdAndCommunityRightToKnowIsTrue", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center.")]
        public int? Facility { get; set; }

        [StringLength(CommunityRightToKnow.StringLengths.COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID)]
        [View("CRTK Facility ID")]
        public string CommunityRightToKnowFacilityId { get; set; }

        [StringLength(CommunityRightToKnow.StringLengths.COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID)]
        public string CoMu { get; set; }

        [View("NAICS")]
        [StringLength(CommunityRightToKnow.StringLengths.NORTH_AMERICAN_INDUSTRY_CLASSIFICATION_SYSTEM)]
        public string NorthAmericanIndustryClassificationSystem { get; set; }

        public int? NumberOfEmployees { get; set; }

        [View("Name")]
        [StringLength(CommunityRightToKnow.StringLengths.FACILITY_EMERGENCY_CONTACT_NAME)]
        public string FacilityEmergencyContactName { get; set; }

        [View("Title")]
        [StringLength(CommunityRightToKnow.StringLengths.FACILITY_EMERGENCY_CONTACT_TITLE)]
        public string FacilityEmergencyContactTitle { get; set; }

        [View("Phone Number")]
        [StringLength(CommunityRightToKnow.StringLengths.FACILITY_EMERGENCY_CONTACT_PHONE_NUMBER)]
        public string FacilityEmergencyContactPhoneNumber { get; set; }

        [View("Emergency Phone Number")]
        [StringLength(CommunityRightToKnow.StringLengths.FACILITY_EMERGENCY_CONTACT_EMERGENCY_PHONE_NUMBER)]
        public string FacilityEmergencyContactEmergencyPhoneNumber { get; set; }

        [View("In any quantity?")]
        public bool? FacilityHasNJCRTKHazardousSubstancesInAnyQuantity { get; set; }

        [View("Above thresholds?")]
        public bool? FacilityHasNJCRTKHazardousSubstancesAboveThresholds { get; set; }

        [StringLength(CommunityRightToKnow.StringLengths.RD_EXEMPTION_APPROVAL_NUMBER)]
        [View("R&D Exemption Approval Number")]
        public string RDExemptionApprovalNumber { get; set; }

        [StringLength(CommunityRightToKnow.StringLengths.TOXINS_RELEASE_INVENTORY_FACILITY_ID)]
        [View("TRI Facility ID")]
        public string ToxinsReleaseInventoryFacilityId { get; set; }

        [StringLength(CommunityRightToKnow.StringLengths.RISK_MANAGEMENT_PLAN_FACILITY_ID)]
        [View("RMP Facility ID")]
        public string RiskManagementPlanFacilityId { get; set; }

        public int? MaximumNumberOfOccupants { get; set; }
        public bool? LocationIsManned { get; set; }

        [View("Is this facility subject to Chemical Accident Prevention under section 112R of CAA (40 CFR, Part 68, Risk Management Program?")]
        public bool? IsSubjectToChemicalAccidentPrevention { get; set; }

        [View("Is this facility subject to Emergency Planning under Section 302 of EPCRA (40CFR Part 355)?")]
        public bool? IsSubjectToEmergencyPlanning { get; set; }

        [Required]
        public DateTime? SubmissionDate { get; set; }

        [Required]
        public DateTime? ExpirationDate { get; set; }


        #endregion

        #region Constructors

        public CommunityRightToKnowViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchCommunityRightToKnow : SearchSet<CommunityRightToKnow>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("Facility.OperatingCenter", "State.Id")]
        public int? State { get; set; }
        
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        [SearchAlias("Facility", "OperatingCenter.Id", Required = true)]
        [EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        
        [EntityMustExist(typeof(Facility)), EntityMap]
        [DropDown("", "Facility", "GetByOperatingCenterIdAndCommunityRightToKnowIsTrue", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center.")]
        public int? Facility { get; set; }

        public int? NumberOfEmployees { get; set; }

        public DateRange SubmissionDate { get; set; }

        public DateRange ExpirationDate { get; set; }

        [View(CommunityRightToKnow.DisplayNames.SUBMISSION_EXPIRED)]
        public bool? Expired { get; set; }
    }
}