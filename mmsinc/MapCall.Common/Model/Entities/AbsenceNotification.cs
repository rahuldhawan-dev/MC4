using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AbsenceNotification
        : IValidatableObject,
            IThingWithDocuments,
            IThingWithNotes,
            IThingWithEmployee,
            IEntityWithCreationTimeTracking
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual User SubmittedBy { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? LastDayOfWork { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        [DisplayName("Absence Start Date")]
        public virtual DateTime? StartDate { get; set; }

        [DisplayName("Date RTW")]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? EndDate { get; set; }

        public virtual decimal? TotalHoursOfAbsence { get; set; }
        public virtual EmployeeAbsenceClaim EmployeeAbsenceClaim { get; set; }
        public virtual string SupervisorNotes { get; set; }

        [DisplayName("FMLA Case")]
        public virtual FamilyMedicalLeaveActCase FamilyMedicalLeaveActCase { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        [BoolFormat("Yes", "No")]
        public virtual bool ReturnToWorkNote { get; set; }

        [DisplayName("Local Management Reviewed"), BoolFormat("Yes", "No")]
        public virtual bool HumanResourcesReviewed { get; set; }

        [DisplayName("Notes")]
        public virtual string HumanResourcesNotes { get; set; }

        [DisplayName("FMLA Package Date Sent")]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? PackageDateSent { get; set; }

        [DisplayName("FMLA Package Date Due")]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? PackageDateDue { get; set; }

        [DisplayName("Employee notified FMLA process")]
        public virtual EmployeeFMLANotification EmployeeFMLANotification { get; set; }

        [DisplayName("Progressive Discipline Step")]
        public virtual ProgressiveDiscipline ProgressiveDiscipline { get; set; }

        public virtual AbsenceStatus AbsenceStatus { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? ProgressiveDisciplineAdministered { get; set; }

        #region Logical Properties

        #region Documents

        public virtual IList<AbsenceNotificationDocument> AbsenceNotificationDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return AbsenceNotificationDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return AbsenceNotificationDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<AbsenceNotificationNote> AbsenceNotificationNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return AbsenceNotificationNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return AbsenceNotificationNotes.Map(n => n.Note); }
        }

        #endregion

        [DoesNotExport]
        public virtual string TableName => "AbsenceNotifications";

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class EmployeeAbsenceClaim : EntityLookup { }

    [Serializable]
    public class EmployeeFMLANotification : EntityLookup { }

    [Serializable]
    public class ProgressiveDiscipline : EntityLookup { }

    [Serializable]
    public class AbsenceStatus : EntityLookup
    {
        public struct Indices
        {
            public const int FMLA_APPROVED = 1, OCCURRENCE = 2;
        }
    }

    #region Occurrence Report

    public class OccurrenceReportItem
    {
        #region Properties

        public Employee Employee { get; set; }
        public OperatingCenter OperatingCenter { get; set; }
        public int AbsenceNotificationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalHoursOfAbsence { get; set; }
        public ProgressiveDiscipline ProgressiveDiscipline { get; set; }
        public string SupervisorNotes { get; set; }
        public string Notes { get; set; }

        #endregion
    }

    public interface ISearchOccurrence : ISearchSet<OccurrenceReportItem>
    {
        int? OperatingCenter { get; set; }
    }

    #endregion
}
