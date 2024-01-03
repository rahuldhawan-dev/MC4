using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CommunityRightToKnow : IEntity, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public struct StringLengths
        {
            public const int COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID = 50,
                             NORTH_AMERICAN_INDUSTRY_CLASSIFICATION_SYSTEM = 50,
                             FACILITY_EMERGENCY_CONTACT_EMERGENCY_PHONE_NUMBER = 20,
                             FACILITY_EMERGENCY_CONTACT_NAME = 255,
                             FACILITY_EMERGENCY_CONTACT_PHONE_NUMBER = 20,
                             FACILITY_EMERGENCY_CONTACT_TITLE = 255,
                             RD_EXEMPTION_APPROVAL_NUMBER = 50,
                             RISK_MANAGEMENT_PLAN_FACILITY_ID = 50,
                             TOXINS_RELEASE_INVENTORY_FACILITY_ID = 50;
        }

        public struct DisplayNames
        {
            public const string SUBMISSION_EXPIRED = "Submission Expired";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual Facility Facility { get; set; }
        /// <summary>
        /// This is a numerical value but might have padded zeros.
        /// </summary>
        [StringLength(StringLengths.COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID)]
        public virtual string CommunityRightToKnowFacilityId { get; set; }

        /// <summary>
        /// This is a numerical value but might have padded zeros. This
        /// value is supplied to the company from the government. No one
        /// could tell me what "CoMu" is short for at the time of writing this.
        /// </summary>
        [StringLength(StringLengths.COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID)]
        public virtual string CoMu { get; set; }

        /// <summary>
        /// This is a numerical value but might have padded zeros. This
        /// value is supplied to the company from the government. "NAICS" is pronounced as "NAKES"
        /// if you were wondering.
        /// </summary>
        [View("NAICS")]
        [StringLength(StringLengths.NORTH_AMERICAN_INDUSTRY_CLASSIFICATION_SYSTEM)]
        public virtual string NorthAmericanIndustryClassificationSystem { get; set; }

        [DoesNotExport]
        public virtual int? NumberOfEmployees { get; set; }

        #region Facility Emergency Contact

        [View("Name"), ExcelExportColumn(UsePropertyName = true)]
        [StringLength(StringLengths.FACILITY_EMERGENCY_CONTACT_NAME)]
        public virtual string FacilityEmergencyContactName { get; set; }

        [View("Title"), ExcelExportColumn(UsePropertyName = true)]
        [StringLength(StringLengths.FACILITY_EMERGENCY_CONTACT_TITLE)]
        public virtual string FacilityEmergencyContactTitle { get; set; }

        [View("Phone Number"), ExcelExportColumn(UsePropertyName = true)]
        [StringLength(StringLengths.FACILITY_EMERGENCY_CONTACT_PHONE_NUMBER)]
        public virtual string FacilityEmergencyContactPhoneNumber { get; set; }

        [View("Emergency Phone Number"), ExcelExportColumn(UsePropertyName = true)]
        [StringLength(StringLengths.FACILITY_EMERGENCY_CONTACT_EMERGENCY_PHONE_NUMBER)]
        public virtual string FacilityEmergencyContactEmergencyPhoneNumber { get; set; }

        #endregion

        [View("In any quantity?")]
        public virtual bool? FacilityHasNJCRTKHazardousSubstancesInAnyQuantity { get; set; }

        [View("Above thresholds?")]
        public virtual bool? FacilityHasNJCRTKHazardousSubstancesAboveThresholds { get; set; }

        [StringLength(StringLengths.RD_EXEMPTION_APPROVAL_NUMBER)]
        [View("R&D Exemption Approval Number")]
        public virtual string RDExemptionApprovalNumber { get; set; }

        [StringLength(StringLengths.TOXINS_RELEASE_INVENTORY_FACILITY_ID)]
        [View("TRI Facility ID")]
        public virtual string ToxinsReleaseInventoryFacilityId { get; set; }

        [StringLength(StringLengths.RISK_MANAGEMENT_PLAN_FACILITY_ID)]
        [View("RMP Facility ID")]
        public virtual string RiskManagementPlanFacilityId { get; set; }

        public virtual int? MaximumNumberOfOccupants { get; set; }
        public virtual bool? LocationIsManned { get; set; }

        [View("Is this facility subject to Chemical Accident Prevention under section 112R of CAA (40 CFR, Part 68, Risk Management Program?")]
        public virtual bool? IsSubjectToChemicalAccidentPrevention { get; set; }

        [View("Is this facility subject to Emergency Planning under Section 302 of EPCRA (40CFR Part 355)?")]
        public virtual bool? IsSubjectToEmergencyPlanning { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? SubmissionDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? ExpirationDate { get; set; }

        public virtual IList<Document<CommunityRightToKnow>> CommunityRightToKnowDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments 
        {
            get { return CommunityRightToKnowDocuments.Map(fd => (IDocumentLink)fd); }
        }

        public virtual IList<Note<CommunityRightToKnow>> CommunityRightToKnowNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return CommunityRightToKnowNotes.Map(fd => (INoteLink)fd); }
        }

        [DoesNotExport]
        public virtual string TableName => CommunityRightToKnowMap.TABLE_NAME;

        #region Logical Properties

        [View(DisplayNames.SUBMISSION_EXPIRED)]
        public virtual bool Expired { get; set; }

        #endregion
        #endregion
    }
}
