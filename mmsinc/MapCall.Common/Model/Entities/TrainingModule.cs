using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities.Excel;
using MapCall.Common.Model.Mappings;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TrainingModule : IEntityLookup, IThingWithDocuments, IThingWithNotes, IThingWithVideos
    {
        #region Constants

        public struct StringLengths
        {
            public const int TITLE = 300,
                             COURSE_APPROVAL_NUMBER = 50;
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        public virtual TrainingModuleCategory TrainingModuleCategory { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual Facility Facility { get; set; }

        // public virtual ProcessStage ProcessStage { get; set; }
        public virtual TrainingRequirement TrainingRequirement { get; set; }
        public virtual TrainingModuleRecurrantType TrainingModuleRecurrantType { get; set; }

        public virtual string Title { get; set; }

        //NJDEP_TCH_COURSE_APPROVAL_NUMBER
        [DisplayName("NJDEP TCH COURSE APPROVAL NUMBER")]
        public virtual string CourseApprovalNumber { get; set; }

        public virtual bool? TCHCertified { get; set; }
        public virtual float? TCHCreditValue { get; set; }
        public virtual float? TotalHours { get; set; }
        public virtual string Description { get; set; }
        public virtual string TrainingObjectives { get; set; }
        public virtual string PracticalTestStandards { get; set; }

        [Required]
        public virtual bool SafetyRelated { get; set; }

        public virtual bool? Production { get; set; }

        public virtual int? EquipmentID { get; set; }

        //  public virtual int? ProcessStageSequence { get; set; }
        public virtual bool? IsActive { get; set; }

        public virtual IList<TrainingRecord> TrainingRecords { get; set; }

        [DisplayName("LEARN ID")]
        public virtual string AmericanWaterCourseNumber { get; set; }

        public virtual LEARNItemType LEARNItemType { get; set; }

        #endregion

        #region Notes/Documents/Log

        public virtual IList<TrainingModuleDocument> TrainingModuleDocuments { get; set; }
        public virtual IList<TrainingModuleNote> TrainingModuleNotes { get; set; }
        public virtual IList<TrainingModuleVideo> Videos { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return TrainingModuleDocuments.Map(d => (IDocumentLink)d); }
        }

        public virtual IList<Document> Documents
        {
            get { return TrainingModuleDocuments.Map(d => d.Document); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return TrainingModuleNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return TrainingModuleNotes.Map(n => n.Note); }
        }

        public virtual IEnumerable<IVideoLink> LinkedVideos => Videos;

        [DoesNotExport]
        public virtual string TableName => TrainingModuleMap.TABLE_NAME;

        #endregion

        #region Logical Properties

        public virtual bool HasAttachedDocuments { get; set; }

        public virtual string TCHClassification
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(CourseApprovalNumber) && CourseApprovalNumber.Length > 2)
                {
                    switch (CourseApprovalNumber.Substring(CourseApprovalNumber.Length - 2, 2))
                    {
                        case "10":
                            return "Water Non-Safety";
                        case "11":
                            return "Water Safety";
                        case "20":
                            return "Wastewater Non-Safety";
                        case "21":
                            return "Wastewater Safety";
                        case "30":
                            return "Water/Wastewater Non-Safety";
                        case "31":
                            return "Water/Wastewater Safety";
                        default:
                            return string.Empty;
                    }
                }

                return string.Empty;
            }
        }

        public virtual string Display => new TrainingModuleDisplayItem {
            Id = Id,
            Title = Title
        }.Display;

        /// <summary>
        /// Gets all the employees attended from all the training records for this module.
        /// </summary>
        public virtual IEnumerable<TrainingRecordAttendedEmployee> EmployeesAttended
        {
            get { return TrainingRecords.SelectMany(x => x.EmployeesAttended); }
        }

        public virtual bool IsActiveInitialTrainingModule =>
            (TrainingRequirement != null && TrainingRequirement.ActiveInitialTrainingModule == this);

        public virtual bool IsActiveRecurringTrainingModule =>
            (TrainingRequirement != null && TrainingRequirement.ActiveRecurringTrainingModule == this);

        public virtual bool IsActiveInitialAndRecurringTrainingModule =>
            (TrainingRequirement != null && TrainingRequirement.ActiveInitialAndRecurringTrainingModule == this);

        #endregion

        #endregion

        #region Constructors

        public TrainingModule()
        {
            TrainingModuleDocuments = new List<TrainingModuleDocument>();
            TrainingModuleNotes = new List<TrainingModuleNote>();
            TrainingRecords = new List<TrainingRecord>();
            Videos = new List<TrainingModuleVideo>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Title;
        }

        #endregion
    }

    public class TrainingModuleDisplayItem : DisplayItem<TrainingModule>
    {
        public string Title { get; set; }

        public override string Display => $"{Id} - {Title}";
    }

    [Serializable]
    public class JobSiteSafetyAnalysis : TrainingModule { }
}
