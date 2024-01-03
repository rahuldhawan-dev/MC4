using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class NearMiss
        : IValidatableObject,
            IThingWithActionItems,
            IThingWithNotes,
            IThingWithDocuments,
            IEntityWithCreationTimeTracking
    {
        #region Consts

        public struct StringLengths
        {
            public const int INCIDENT_NUMBER = 15,
                             REPORTED_BY = 100,
                             SAFETY_NEAR_MISS = 100,
                             SEVERITY = 100,
                             LOCATION_DETAILS = 255,
                             PHONE_CELLULAR = 255,
                             EMAIL_ADDRESS = 255,
                             WORK_ORDER_NUMBER = 50,
                             CONTRACTOR_COMPANY = 100,
                             ACTION_TAKEN = 255,
                             DESCRIBE_OTHER = 100;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        [View("Near Miss Type")]
        public virtual NearMissType Type { get; set; }

        public virtual NearMissCategory Category { get; set; }

        public virtual string DescribeOther { get; set; }
        public virtual NearMissSubCategory SubCategory { get; set; }

        // Removed Incident id, not required on add page, shouldn't be visible, made nullable in database
        // will be kept for historical purposes
        public virtual string IncidentNumber { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        [Required]
        public virtual DateTime OccurredAt { get; set; }

        public virtual string ReportedBy { get; set; }

        public virtual string Severity { get; set; }

        [View("Description of Near Miss")]
        public virtual string Description { get; set; }

        public virtual WorkOrderType WorkOrderType { get; set; }

        [View("Actions Taken")]
        public virtual ActionTakenType ActionTakenType { get; set; }

        [View("T&D Work Order Number")]
        public virtual WorkOrder WorkOrder { get; set; }

        [View("Production Work Order Number")]
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }

        public virtual int? ShortCycleWorkOrderNumber { get; set; }

        [View("Work Order Number")]
        public virtual string WorkOrderNumber { get; set; }

        public virtual Town Town { get; set; }

        public virtual Facility Facility { get; set; }

        [View("Location of Observation")]
        public virtual string LocationDetails { get; set; }

        [View("Coordinates")]
        public virtual Coordinate Coordinate { get; set; }

        public virtual MapIcon Icon => Coordinate?.Icon;

        [View("Related To Contractor Activity")]
        public virtual bool? RelatedToContractor { get; set; }

        public virtual string ContractorCompany { get; set; }

        [View("SIF Potential")]
        public virtual bool? SeriousInjuryOrFatality { get; set; }

        [View("Life Saving Rule")]
        public virtual LifeSavingRuleType LifeSavingRuleType { get; set; }

        [View("Was a Stop Work Authority Performed?")]
        public virtual bool? StopWorkAuthorityPerformed { get; set; }

        [View("Who was stopped?")]
        public virtual StopWorkUsageType StopWorkUsageType { get; set; }

        [View("Occurred at Company Facility")]
        public virtual bool? NotCompanyFacility { get; set; }

        public virtual bool? ReportAnonymously { get; set; }

        [View("Describe Corrective Action(s) Completed and In Progress")]
        public virtual string ActionTaken { get; set; }

        public virtual bool? ReportedToRegulator { get; set; }

        public virtual SystemType SystemType { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        [View(WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public virtual WasteWaterSystem WasteWaterSystem { get; set; }

        public virtual bool? IsContractedOperations { get; set; }

        [View("Near Miss Completely Resolved")]
        public virtual bool? CompletedCorrectiveActions { get; set; }

        public virtual User ReviewedBy { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? ReviewedDate { get; set; }

        [View(
            "I have reviewed this Near Miss Record to ensure any corrective actions entered have been " +
            "completed")]
        public virtual bool? HaveReviewedNearMiss { get; set; }

        public virtual bool? SubmittedOnBehalfOfAnotherEmployee { get; set; }

        public virtual Employee Employee { get; set; }

        [View(FormatStyle.Date)] 
        public virtual DateTime? DateCompleted { get; set; }

        public virtual IList<ActionItem<NearMiss>> ActionItems { get; set; } =
            new List<ActionItem<NearMiss>>();

        public virtual IList<IActionItemLink> LinkedActionItems =>
            ActionItems.Cast<IActionItemLink>().ToList();

        public virtual IList<Note<NearMiss>> Notes { get; set; } =
            new List<Note<NearMiss>>();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual IList<Document<NearMiss>> Documents { get; set; } = new List<Document<NearMiss>>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => NearMissMap.TABLE_NAME;
        public virtual string RecordUrl { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}