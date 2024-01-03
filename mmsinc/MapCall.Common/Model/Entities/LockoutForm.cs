using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations._2016;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class LockoutForm : IEntity, IValidatableObject, IThingWithNotes, IThingWithDocuments, IThingWithCoordinate
    {
        #region Constants

        public struct Questions
        {
            public const string
                AFFECTED_EMPLOYEES_NOTIFIED = "Have affected employees been notified prior to lock out?",
                AFFECTED_EQUIPMENT_SHUTDOWN = "Has the affected equipment been shut down?",
                ISOLATES_ENERGY_SOURCES = "Does this lock out isolate all energy sources to the equipment?",
                CLEARLY_INDICATES_PROHIBITED =
                    "Apply lockout devices to hold the machine or equipment in a “safe” position. Attach tag to the lockout device that clearly indicates that the operation of the equipment is prohibited?",
                RENDERED_SAFE_UNTIL_COMPLETE =
                    "Relieve, disconnect, restrain, or otherwise render safe and stored residual energy.  If re-accumulation of any stored energy is possible, continue to verify that it has been rendered “safe” until the job is complete.",
                CANNOT_BE_OPERATED =
                    "Verify that the machine or equipment cannot be operated before proceeding with the repair or maintenance work.  This is accomplished by trying to start-up the equipment.",
                PARKED_IN_HOME_SAFE =
                    "When returning equipment to service verify that non-essential items (tools, extra parts, etc.) are removed, components are intact and employees are safely positioned. Ensure that equipment is parked in the “Home” or safe “Start” position.",
                REMOVED_DEVICE_AND_NOTIFIED =
                    "Remove the lockout/tag out devices, and then notify all affected employees that the devices have been removed and the equipment is operational.",
                SAME_AS_INSTALLER = "Is the same person who installed the lockout device removing it?",
                CONFIRMED_BY_MANAGEMENT = "Authorized employee lock has been identified and confirmed by management?",
                REASONABLE_EFFORT_MADE =
                    "Authorized management person has made reasonable effort to contact employee who installed lock?",
                AUTHORIZED_MANAGEMENT_APPROVED =
                    "Authorized management person checked equipment and approved removal of lock?",
                SUPERVISOR_ENSURES_KNOWLEDGE =
                    "Supervisor will ensure that the authorized employee has this knowledge before he/she resumes work at that facility?",
                AUTH_AFFIX_LOCKOUT =
                    "Please note that American Water must authorize and affix Lock Out/Tag Out equipment for all contractor Lock Out/Tag Out required work. Contractors will affix a lock at each one of the isolation points.";
        }

        public struct StringLengths
        {
            public const int CONTRACTOR_NAME = 255,
                             CONTRACTOR_FIRST_NAME = 255,
                             CONTRACTOR_LAST_NAME = 255,
                             CONTRACTOR_PHONE = 20,
                             EQUIPMENT_ID_OTHER = 50,
                             METHOD_OF_CONTACT = 25;
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Facility Facility { get; set; }

        public virtual EquipmentType EquipmentType { get; set; }

        [DoesNotExport]
        public virtual Equipment Equipment { get; set; }

        [View(DisplayName = "Equipment")]
        public virtual string EquipmentDescriptionForExcel => Equipment?.Description;

        public virtual Coordinate Coordinate
        {
            get => Equipment?.Coordinate ?? Facility.Coordinate;
            set
            {
                //noop-we don't need to set this, it's always read from the equipment or facility
            }
        }

        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;

        public virtual LockoutReason LockoutReason { get; set; }
        public virtual LockoutDeviceLocation IsolationPoint { get; set; }
        public virtual LockoutDevice LockoutDevice { get; set; }
        public virtual DateTime LockoutDateTime { get; set; }
        public virtual string ReasonForLockout { get; set; }
        public virtual bool? ContractorLockOutTagOut { get; set; }
        public virtual string LocationOfLockoutNotes { get; set; }

        //OUT OF SERVICE
        public virtual string AdditionalLockoutNotes { get; set; }

        public virtual Employee OutOfServiceAuthorizedEmployee { get; set; }

        public virtual DateTime OutOfServiceDateTime { get; set; }

        public virtual string ReturnedToServiceNotes { get; set; }
        public virtual Employee ReturnToServiceAuthorizedEmployee { get; set; }
        public virtual DateTime? ReturnedToServiceDateTime { get; set; }

        [DisplayName(Questions.SAME_AS_INSTALLER)]
        public virtual bool? SameAsInstaller { get; set; }

        public virtual Employee SupervisorInvolved { get; set; }
        public virtual DateTime? DateOfContact { get; set; }

        [StringLength(StringLengths.METHOD_OF_CONTACT)]
        public virtual string MethodOfContact { get; set; }

        public virtual string OutcomeOfContact { get; set; }

        [View(DisplayName = "Employee Acknowledged Review of Operations Related Documents")]
        public virtual bool EmployeeAcknowledgedTraining { get; set; }

        [StringLength(25)]
        public virtual string IsolationPointDescription { get; set; }

        public virtual Employee AuthorizedManagementPerson { get; set; }
        public virtual WayToRemoveLocks LockRemovalMethod { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
        public virtual string ContractorFirstName { get; set; }
        public virtual string ContractorLastName { get; set; }
        public virtual string ContractorPhone { get; set; }

        public virtual IList<LockoutFormAnswer> LockoutFormAnswers { get; set; }

        [DoesNotExport]
        public virtual IOrderedEnumerable<LockoutFormAnswer> LockoutConditionAnswers =>
            LockoutFormAnswers
               .Where(x => x.LockoutFormQuestion.Category.Id ==
                           LockoutFormQuestionCategory.Indices.LOCKOUT_CONDITIONS)
               .OrderBy(x =>
                    x.LockoutFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<LockoutFormAnswer> OutOfServiceConditionAnswers =>
            LockoutFormAnswers
               .Where(x => x.LockoutFormQuestion.Category.Id ==
                           LockoutFormQuestionCategory.Indices.OUT_OF_SERVICE)
               .OrderBy(x =>
                    x.LockoutFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<LockoutFormAnswer> ReturnToServiceAnswers =>
            LockoutFormAnswers
               .Where(x =>
                    x.LockoutFormQuestion.Category.Id ==
                    LockoutFormQuestionCategory.Indices.RETURN_TO_SERVICE)
               .OrderBy(x =>
                    x.LockoutFormQuestion.DisplayOrder);

        [DoesNotExport]
        public virtual IOrderedEnumerable<LockoutFormAnswer> ManagementAnswers =>
            LockoutFormAnswers
               .Where(x =>
                    x.LockoutFormQuestion.Category.Id ==
                    LockoutFormQuestionCategory.Indices.MANAGEMENT)
               .OrderBy(x =>
                    x.LockoutFormQuestion.DisplayOrder);

        #endregion

        #region Logical Properties

        #region Documents

        public virtual IList<LockoutFormDocument> LockoutFormDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return LockoutFormDocuments.Map(d => (IDocumentLink)d); }
        }

        public virtual IList<Document> Documents
        {
            get { return LockoutFormDocuments.Map(d => d.Document); }
        }

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        [DoesNotExport]
        public virtual string RecordUrlWorkOrder { get; set; }

        #endregion

        #region Notes

        public virtual IList<LockoutFormNote> LockoutFormNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return LockoutFormNotes.Map(d => (INoteLink)d); }
        }

        public virtual IList<Note> Notes
        {
            get { return LockoutFormNotes.Map(d => d.Note); }
        }

        #endregion

        [DoesNotExport]
        public virtual string TableName => nameof(LockoutForm) + "s";

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion

        #region Constructors

        public LockoutForm()
        {
            LockoutFormDocuments = new List<LockoutFormDocument>();
            LockoutFormNotes = new List<LockoutFormNote>();
            LockoutFormAnswers = new List<LockoutFormAnswer>();
        }

        #endregion
    }
}
