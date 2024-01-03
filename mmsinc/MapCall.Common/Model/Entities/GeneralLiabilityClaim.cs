using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GeneralLiabilityClaim
        : IEntityWithCreationTimeTracking,
            IValidatableObject,
            IThingWithCoordinate,
            IThingWithNotes,
            IThingWithDocuments,
            IThingWithActionItems
    {
        #region Constants

        public struct StringLengths
        {
            public const int CLAIM_NUMBER = 20,
                             NAME = 50,
                             PHONE_NUMBER = 20,
                             EMAIL = 50,
                             DRIVER_NAME = 50,
                             DRIVER_PHONE = 20,
                             OTHER_DRIVER = 50,
                             OTHER_DRIVER_PHONE = 20,
                             LOCATION_OF_INCIDENT = 50,
                             VEHICLE_MAKE = 50,
                             VEHICLE_VIN = 50,
                             LICENSE_NUMBER = 20,
                             VEHICLE_PHONE_NUMBER = 50,
                             POLICE_DEPARTMENT = 50,
                             POLICE_CASE_NUMBER = 20,
                             WITNESS = 50,
                             WITNESS_PHONE = 20,
                             REPORTED_BY = 50,
                             REPORTED_BY_PHONE = 20,
                             SAP_WORK_ORDER_ID = 50,
                             FIVE_WHYS = 255,
                             OTHER_TYPE_OF_CRASH = 100;
        }

        public struct DisplayNames
        {
            public const string FIVE_WHYS_COMPLETED = "5 Whys Completed",
                                QUESTION_WHY1 = "Why Did the Incident Occur?",
                                QUESTION_WHY2345 = "Why?";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual LiabilityType LiabilityType { get; set; }
        public virtual GeneralLiabilityClaimType GeneralLiabilityClaimType { get; set; }
        public virtual CrashType CrashType { get; set; }
        [View("Describe Other")]
        public virtual string OtherTypeOfCrash { get; set; }
        public virtual Coordinate Coordinate { get; set; }

        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;

        [View("District")]
        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual Employee CompanyContact { get; set; }
        public virtual ClaimsRepresentative ClaimsRepresentative { get; set; }
        public virtual string ClaimNumber { get; set; }
        public virtual bool? MeterBox { get; set; }
        public virtual bool? CurbValveBox { get; set; }
        public virtual bool? Excavation { get; set; }
        public virtual bool? Barricades { get; set; }
        public virtual bool? Vehicle { get; set; }
        public virtual bool? WaterMeter { get; set; }
        public virtual bool? FireHydrant { get; set; }
        public virtual bool? Backhoe { get; set; }
        public virtual bool? WaterQuality { get; set; }
        public virtual bool? WaterPressure { get; set; }
        public virtual bool? WaterMain { get; set; }
        public virtual bool? ServiceLine { get; set; }
        public virtual string Description { get; set; }

        [View("Claimant Name")]
        public virtual string Name { get; set; }

        [View("Claimant Phone Number")]
        public virtual string PhoneNumber { get; set; }

        [View("Claimant Address")]
        public virtual string Address { get; set; }

        [View("Claimant Email")]
        public virtual string Email { get; set; }

        [View("Driver/Employee Name")]
        public virtual string DriverName { get; set; }

        [View("Driver/Employee Phone")]
        public virtual string DriverPhone { get; set; }

        public virtual bool? PhhContacted { get; set; }
        public virtual string OtherDriver { get; set; }
        public virtual string OtherDriverPhone { get; set; }
        public virtual string OtherDriverAddress { get; set; }
        public virtual string LocationOfIncident { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime IncidentDateTime { get; set; }

        [View("Other Vehicle Year")]
        public virtual int? VehicleYear { get; set; }

        [View("Other Vehicle Make")]
        public virtual string VehicleMake { get; set; }

        [View("Other Vehicle VIN")]
        public virtual string VehicleVin { get; set; }

        [View("Other Vehicle License Plate Number")]
        public virtual string LicenseNumber { get; set; }

        public virtual bool? PoliceCalled { get; set; }
        public virtual string PoliceDepartment { get; set; }
        public virtual string PoliceCaseNumber { get; set; }
        public virtual bool? WitnessStatement { get; set; }
        public virtual string Witness { get; set; }
        public virtual string WitnessPhone { get; set; }
        public virtual bool? AnyInjuries { get; set; }
        public virtual string ReportedBy { get; set; }
        public virtual string ReportedByPhone { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime IncidentNotificationDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime? IncidentReportedDate { get; set; }

        public virtual DateTime? CompletedDate { get; set; }

        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        [View("SAP Work Order")]
        public virtual string SAPWorkOrderId { get; set; }

        [View(DisplayNames.FIVE_WHYS_COMPLETED, Description = "Must be completed within 3 business days.")]
        [Required]
        public virtual bool FiveWhysCompleted { get; set; }

        [View(DisplayNames.QUESTION_WHY1)]
        public virtual string Why1 { get; set; }

        [ExcelExportColumn(UsePropertyName = true)]
        [View(DisplayNames.QUESTION_WHY2345)]
        public virtual string Why2 { get; set; }

        [ExcelExportColumn(UsePropertyName = true)]
        [View(DisplayNames.QUESTION_WHY2345)]
        public virtual string Why3 { get; set; }

        [ExcelExportColumn(UsePropertyName = true)]
        [View(DisplayNames.QUESTION_WHY2345)]
        public virtual string Why4 { get; set; }

        [ExcelExportColumn(UsePropertyName = true)]
        [View(DisplayNames.QUESTION_WHY2345)]
        public virtual string Why5 { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateSubmitted { get; set; }

        #region Logical Properties

        #region Documents

        public virtual IList<GeneralLiabilityClaimDocument> GeneralLiabilityClaimDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return GeneralLiabilityClaimDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return GeneralLiabilityClaimDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<GeneralLiabilityClaimNote> GeneralLiabilityClaimNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return GeneralLiabilityClaimNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return GeneralLiabilityClaimNotes.Map(n => n.Note); }
        }

        #endregion

        public virtual IList<ActionItem<GeneralLiabilityClaim>> ActionItems { get; set; }

        public virtual IList<IActionItemLink> LinkedActionItems => ActionItems.Cast<IActionItemLink>().ToList();

        public virtual string TableName => nameof(GeneralLiabilityClaim) + "s";

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public GeneralLiabilityClaim()
        {
            ActionItems = new List<ActionItem<GeneralLiabilityClaim>>();
        }

        #endregion
    }
}
