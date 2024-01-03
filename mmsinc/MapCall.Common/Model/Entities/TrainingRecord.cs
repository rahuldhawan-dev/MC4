using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TrainingRecord : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes, IThingWithEmployees
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int OUTSIDE_INSTRUCTOR = 255;

            #endregion
        }

        public struct DataTypeNames
        {
            #region Constants

            public const string EMPLOYEES_SCHEDULED = "Employees Scheduled",
                                EMPLOYEES_ATTENDED = "Employees Attended";

            #endregion
        }

        #endregion

        #region Private Members

        [NonSerialized] private IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual TrainingModule TrainingModule { get; set; }
        public virtual Employee Instructor { get; set; }
        public virtual Employee SecondInstructor { get; set; }
        public virtual ClassLocation ClassLocation { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? HeldOn { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ScheduledDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? NextDueDate { get; set; }

        public virtual int? MaximumClassSize { get; set; }

        // display Class Location Details
        [DisplayName("Class Location Details")]
        public virtual string CourseLocation { get; set; }

        public virtual string OutsideInstructor { get; set; }
        public virtual string OutsideInstructorTitle { get; set; }

        public virtual bool IsOpen { get; protected set; }
        public virtual bool? Canceled { get; set; }
        public virtual DateTime? MinSessionDate { get; protected set; }
        public virtual DateTime? MaxSessionDate { get; protected set; }
        public virtual DateTime? AttendeesExportedDate { get; set; }
        public virtual bool? Exported { get; set; }

        public virtual TrainingContactHoursProgramCoordinator ProgramCoordinator { get; set; }

        public virtual IList<TrainingRecordNote> TrainingRecordNotes { get; set; }
        public virtual IList<TrainingRecordDocument> TrainingRecordDocuments { get; set; }
        public virtual IList<TrainingRecordScheduledEmployee> EmployeesScheduled { get; set; }
        public virtual IList<TrainingRecordAttendedEmployee> EmployeesAttended { get; set; }
        public virtual IList<ReadOnlyEmployeeLink> EmployeesScheduledOrAttended { get; set; }
        public virtual IList<TrainingSession> TrainingSessions { get; set; }

        #endregion

        #region Logical Properties

        public virtual bool HasAttachedDocuments { get; set; }
        public virtual string CreatedBy { get; set; }

        [DoesNotExport]
        public virtual bool AllowNotifications => true;

        [DoesNotExport]
        public virtual string RoleModule => RoleModules.OperationsTrainingRecords.ToString();

        [DoesNotExport]
        public virtual string NotificationPurpose => "Training Record";

        [DoesNotExport]
        public virtual string EntityType => typeof(TrainingRecord).AssemblyQualifiedName;

        [DoesNotExport]
        public virtual int OperatingCenterId => 0;

        [DoesNotExport]
        public virtual string DataTypeName { get; set; }

        [DoesNotExport]
        public virtual string TableName => TrainingRecordMap.TABLE_NAME;

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return TrainingRecordNotes.Map(x => (INoteLink)x); }
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return TrainingRecordDocuments.Map(x => (IDocumentLink)x); }
        }

        public virtual IList<IEmployeeLink> LinkedEmployees
        {
            get
            {
                switch (DataTypeName)
                {
                    case DataTypeNames.EMPLOYEES_SCHEDULED:
                        return LinkedEmployeesScheduled;
                    case DataTypeNames.EMPLOYEES_ATTENDED:
                        return LinkedEmployeesAttended;
                    default:
                        return null;
                }
            }
        }

        public virtual IList<IEmployeeLink> LinkedEmployeesScheduled
        {
            get { return EmployeesScheduled.Map(x => (IEmployeeLink)x); }
        }

        public virtual IList<IEmployeeLink> LinkedEmployeesAttended
        {
            get { return EmployeesAttended.Map(x => (IEmployeeLink)x); }
        }

        public virtual IList<IEmployeeLink> LinkedEmployeesEligibleForCertificates
        {
            get { return LinkedEmployeesAttended.Where(x => x.Employee.IsLicensed).ToList(); }
        }

        public virtual string AmericanWaterCourseNumber =>
            (TrainingModule != null) ? TrainingModule.AmericanWaterCourseNumber : string.Empty;

        public virtual double TotalSessionHours
        {
            get { return TrainingSessions.Sum(x => x.Duration); }
        }

        public virtual bool HasEnoughTrainingSessionsHoursForTrainingModule { get; protected set; }

        public virtual bool Past
        {
            get
            {
                var lastSession = TrainingSessions.Any() ? TrainingSessions.Max(x => x.EndDateTime) : (DateTime?)null;

                return lastSession != null &&
                       lastSession < _dateTimeProvider.GetCurrentDate();
            }
        }

        /// <summary>
        /// A terrible terrible hack property for passing the url
        /// for this record to a notification template.
        /// </summary>
        public virtual string RecordUrl { get; set; }

        public virtual int ScheduledCount => EmployeesScheduled.Count;
        public virtual int AttendedCount => EmployeesAttended.Count;

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IDateTimeProvider DateTimeProvider
        {
            set => _dateTimeProvider = value;
        }

        #endregion

        #endregion

        #region Constructors

        public TrainingRecord()
        {
            EmployeesScheduledOrAttended = new List<ReadOnlyEmployeeLink>();
            EmployeesScheduled = new List<TrainingRecordScheduledEmployee>();
            EmployeesAttended = new List<TrainingRecordAttendedEmployee>();
            TrainingRecordDocuments = new List<TrainingRecordDocument>();
            TrainingRecordNotes = new List<TrainingRecordNote>();
            TrainingSessions = new List<TrainingSession>();
        }

        #endregion

        #region Exposed Methods

        public virtual bool AllowMoreEmployeesFor(string dataTypeName)
        {
            switch (dataTypeName)
            {
                case DataTypeNames.EMPLOYEES_SCHEDULED:
                    return IsOpen;
                case DataTypeNames.EMPLOYEES_ATTENDED:
                    return true;
            }

            return false;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return String.Format("{0} : {1}, Scheduled For: {2:d}, Location: {3}", Id, TrainingModule, ScheduledDate,
                ClassLocation);
        }

        #endregion
    }
}
