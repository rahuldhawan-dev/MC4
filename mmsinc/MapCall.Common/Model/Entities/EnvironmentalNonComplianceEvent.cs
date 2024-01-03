using MMSINC.Data;
using MMSINC.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalNonComplianceEvent : IEntityWithCreationTracking<User>, IEntity, IThingWithNotes, IThingWithDocuments,
        IThingWithOperatingCenter
    {
        public struct StringLengths
        {
            public const int RESPONSIBILITY_NAME = 50,
                             FAILURE_TYPE_DESCRIPTION = 255,
                             ISSUING_ENTITY_NAME = 50;
        }

        public struct DisplayNames
        {
            public const string RESPONSIBILITY = "Work Group Classification",
                                ROOT_CAUSE = "Root Cause",
                                DATE_FINALIZED = "Date NOV Issued",
                                CREATED_BY = "Created By";
        }

        public virtual string TableName => nameof(EnvironmentalNonComplianceEvent) + "s";

        public virtual int Id { get; set; }
        public virtual State State { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        [View(DisplayName = WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public virtual WasteWaterSystem WasteWaterSystem { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual EnvironmentalNonComplianceEventType IssueType { get; set; }
        public virtual EnvironmentalNonComplianceEventSubType IssueSubType { get; set; }

        [View(DisplayName = DisplayNames.RESPONSIBILITY)]
        public virtual EnvironmentalNonComplianceEventResponsibility Responsibility { get; set; }
        [View(DisplayNames.ROOT_CAUSE)]
        public virtual IList<EnvironmentalNonComplianceEventRootCause> RootCauses { get; set; }
        public virtual EnvironmentalNonComplianceEventFailureType FailureType { get; set; }
        public virtual EnvironmentalNonComplianceEventStatus IssueStatus { get; set; }
        public virtual EnvironmentalNonComplianceEventEntityLevel IssuingEntity { get; set; }
        public virtual DateTime AwarenessDate { get; set; }
        public virtual DateTime EventDate { get; set; }

        [View(DisplayNames.DATE_FINALIZED)]
        public virtual DateTime? DateFinalized { get; set; }
        public virtual DateTime? DateOfEnvironmentalLeadershipTeamReview { get; set; }
        public virtual DateTime? EnforcementDate { get; set; }

        [View("Name Of Third Party")]
        public virtual string NameOfThirdParty { get; set; }

        public virtual string SummaryOfEvent { get; set; }

        [View(Description = "Character limit of 255. This description will be shared within the notification email.")]
        public virtual string FailureTypeDescription { get; set; }
        public virtual decimal? FineAmount { get; set; }

        [View("Name Of Entity")]
        public virtual string NameOfEntity { get; set; }

        public virtual int? IssueYear { get; set; }
        public virtual EnvironmentalNonComplianceEventCountsAgainstTarget CountsAgainstTarget { get; set; }
        public virtual DateTime? NOVWorkGroupReviewDate { get; set; }
        public virtual DateTime? ChiefEnvOfficerApprovalDate { get; set; }

        public virtual IList<Note<EnvironmentalNonComplianceEvent>> Notes { get; set; }
        public virtual IList<Document<EnvironmentalNonComplianceEvent>> Documents { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();
        public virtual IList<EnvironmentalNonComplianceEventActionItem> ActionItems { get; set; }

        [View(DisplayNames.ROOT_CAUSE)]
        public virtual string DisplayRootCause => string.Join(", ", RootCauses.Select(x => x.Description).Distinct());

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS)]
        public virtual DateTime CreatedAt { get; set; }

        [DoesNotExport]
        public virtual User CreatedBy { get; set; }

        [View(DisplayNames.CREATED_BY)]
        public virtual string CreatedByFullName => CreatedBy?.FullName;

        public EnvironmentalNonComplianceEvent()
        {
            Notes = new List<Note<EnvironmentalNonComplianceEvent>>();
            Documents = new List<Document<EnvironmentalNonComplianceEvent>>();
            ActionItems = new List<EnvironmentalNonComplianceEventActionItem>();
            RootCauses = new List<EnvironmentalNonComplianceEventRootCause>();
        }
    }
}
