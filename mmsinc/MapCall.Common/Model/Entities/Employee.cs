using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Employee : IThingWithDocuments, IThingWithNotes, IEntityLookup
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int ADDRESS = 255,
                             ADDRESS2 = 255,
                             CDL = 255,
                             C_LICENSE = 255,
                             CITY = 255,
                             DRIVERS_LICENSE = 255,
                             EMERGENCY_CONTACT_NAME = 255,
                             EMERGENCY_CONTACT_PHONE = 255,
                             EMPLOYEE_ID = 255,
                             EMAIL_ADDRESS = 255,
                             FIRST_NAME = 40,
                             LAST_NAME = 40,
                             LICENSE_WATER_TREATMENT = 255,
                             LICENSE_WATER_DISTRIBUTION = 255,
                             LICENSE_SEWER_COLLECTION = 255,
                             LICENSE_SEWER_TREATMENT = 255,
                             LICENSE_INDUSTRIAL_DISCHARGE = 255,
                             MIDDLE_NAME = 15,
                             N_LICENSE = 255,
                             PHONE_CELLULAR = 255,
                             PHONE_HOME = 255,
                             PHONE_PAGER = 255,
                             PHONE_PERSONAL_CELLULAR = 255,
                             PHONE_WORK = 255,
                             PURCHASE_CARD_NUMBER = 50,
                             S_LICENSE = 255,
                             STATE = 255,
                             T_LICENSE = 255,
                             W_LICENSE = 255,
                             ZIP_CODE = 255;

            #endregion
        }

        public const string FULL_NAME_FORMAT = "{0} {1} {2}",
                            PROPER_NAME_FORMAT = "{0}, {1} {2}",
                            DISPLAY_FORMAT = "{0} : {1}",
                            ACCOUNTABILITY_ACTION_RECORDS_ATTACHED = "Accountability Action Records Attached",
                            EMPLOYEE_STATUS = "Employee Status";

        #endregion

        #region Private Members

        private EmployeeDisplayItem _display;
        [NonSerialized] private IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MiddleName { get; set; }

        [View("Employee ID")]
        public virtual string EmployeeId { get; set; }

        [View("Date of Hire")]
        public virtual DateTime? DateHired { get; set; }

        public virtual DateTime? SeniorityDate { get; set; }
        public virtual int? SeniorityRanking { get; set; }
        public virtual DateTime? InactiveDate { get; set; }
        public virtual string LicenseWaterTreatment { get; set; }
        public virtual string TLicense { get; set; }
        public virtual DateTime? DateOfTLicense { get; set; }
        public virtual string LicenseWaterDistribution { get; set; }
        public virtual string WLicense { get; set; }
        public virtual DateTime? DateOfWLicense { get; set; }
        public virtual string LicenseSewerCollection { get; set; }
        public virtual string CLicense { get; set; }
        public virtual DateTime? DateOfCLicense { get; set; }
        public virtual string LicenseSewerTreatment { get; set; }
        public virtual string SLicense { get; set; }
        public virtual DateTime? DateOfSLicense { get; set; }
        public virtual string LicenseIndustrialDischarge { get; set; }
        public virtual string NLicense { get; set; }
        public virtual DateTime? DateOfNLicense { get; set; }
        public virtual string CDL { get; set; }
        public virtual string DriversLicense { get; set; }
        public virtual string EmergencyContactName { get; set; }
        public virtual string EmergencyContactPhone { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string Address { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual bool ValidEssentialEmployeeCard { get; set; }

        [View("GETS\\WPS Card")]
        public virtual bool GETSWPSCard { get; set; }

        // useful for setting null values in queries
        public virtual DateTime? Dummy { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string PhonePager { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string PhoneCellular { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string PhoneHome { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string PhoneWork { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string PhonePersonalCellular { get; set; }

        public virtual string PurchaseCardNumber { get; set; }
        public virtual decimal? MonthlyDollarLimit { get; set; }
        public virtual decimal? SingleDollarLimit { get; set; }
        public virtual string InstitutionalKnowledgeDescription { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? OneDayDoctorsNoteRestrictionEndDate { get; set; }

        #endregion

        #region References

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Facility ReportingFacility { get; set; }
        public virtual Employee ReportsTo { get; set; }
        public virtual Employee HumanResourcesManager { get; set; }
        public virtual Employee PurchaseCardReviewer { get; set; }
        public virtual Employee PurchaseCardApprover { get; set; }
        public virtual Coordinate Coordinate { get; set; }

        [View(EMPLOYEE_STATUS)]
        public virtual EmployeeStatus Status { get; set; }

        [View("Department Name")]
        public virtual EmployeeDepartment Department { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual ReasonForDeparture ReasonForDeparture { get; set; }
        public virtual UnionAffiliation UnionAffiliation { get; set; }
        public virtual TCPAStatus TCPAStatus { get; set; }
        public virtual DPCCStatus DPCCStatus { get; set; }
        public virtual InstitutionalKnowledge InstitutionalKnowledge { get; set; }
        public virtual Position CurrentPosition { get; set; }
        public virtual PositionGroup PositionGroup { get; set; }
        public virtual CommercialDriversLicenseProgramStatus CommercialDriversLicenseProgramStatus { get; set; }
        public virtual ScheduleType ScheduleType { get; set; }
        public virtual PersonnelArea PersonnelArea { get; set; }

        public virtual User User { get; set; }

        public virtual IList<Note<Employee>> EmployeeNotes { get; set; }
        public virtual IList<Document<Employee>> EmployeeDocuments { get; set; }
        public virtual IList<GrievanceEmployee> EmployeeGrievances { get; set; }
        public virtual IList<EmployeeAccountabilityAction> EmployeeAccountabilityActions { get; set; }
        public virtual IList<AbsenceNotification> AbsenceNotifications { get; set; }
        public virtual IList<GeneralLiabilityClaim> GeneralLiabilityClaims { get; set; }
        public virtual IList<Incident> Incidents { get; set; }

        public virtual IList<PositionHistory> PositionHistory { get; set; }
        public virtual IList<TailgateTalkEmployee> EmployeeTailgateTalks { get; set; }
        public virtual IList<JobObservationEmployee> EmployeeJobObservations { get; set; }
        public virtual IList<TrainingRecordScheduledEmployee> ScheduledTrainingRecords { get; set; }
        public virtual IList<TrainingRecordAttendedEmployee> AttendedTrainingRecords { get; set; }

        public virtual IList<DriversLicense> DriversLicenses { get; set; }

        public virtual IList<OperatorLicense> OperatorLicenses { get; set; }

        public virtual IList<MedicalCertificate> MedicalCertificates { get; set; }
        public virtual IList<ViolationCertificate> ViolationCertificates { get; set; }
        public virtual IList<HepatitisBVaccination> HepatitisBVaccinations { get; set; }

        public virtual IList<FamilyMedicalLeaveActCase> FamilyMedicalLeaveActCases { get; set; }

        public virtual IList<EmployeeProductionSkillSet> ProductionSkillSets { get; set; }

        public virtual IList<EmployeeAssignment> ProductionAssignments { get; set; }
        public virtual IList<EmployeeAssignment> RelatedProductionAssignments { get; set; }
        public virtual IList<ScheduledAssignment> ScheduledAssignments { get; set; }

        #endregion

        #region Logical Properties

        public virtual string FullName =>
            String.Format(FULL_NAME_FORMAT, FirstName, MiddleName, LastName).Replace("  ", " ");

        public virtual string ProperName => String.Format(PROPER_NAME_FORMAT, LastName, FirstName, MiddleName);

        public virtual string Display => String.Format(DISPLAY_FORMAT, ProperName, EmployeeId).Replace("  ", " ");

        public virtual string Description => (_display ?? (_display = new EmployeeDisplayItem {
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            EmployeeId = EmployeeId,
            OperatingCenter = OperatingCenter?.OperatingCenterCode
        })).Display;

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return EmployeeNotes.Map(x => (INoteLink)x); }
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return EmployeeDocuments.Map(x => (IDocumentLink)x); }
        }

        public virtual IList<Grievance> LinkedGrievances
        {
            get { return EmployeeGrievances.Map(g => g.Grievance); }
        }

        public virtual IList<TailgateTalk> LinkedTailgateTalks
        {
            get { return EmployeeTailgateTalks.Map(x => x.TailgateTalk); }
        }

        public virtual IList<JobObservation> LinkedJobObservations
        {
            get { return EmployeeJobObservations.Map(x => x.JobObservation); }
        }

        public virtual string TableName => FixTableAndColumnNamesForBug1623.OldTableNames.EMPLOYEES;

        [View("Documents")]
        public virtual int DocumentCount => EmployeeDocuments.Count;

        [View("Notes")]
        public virtual int NoteCount => EmployeeNotes.Count;

        [View("Grievances")]
        public virtual int GrievanceCount => LinkedGrievances.Count;

        public virtual bool IsActive => Status != null && Status.Description == "Active";

        public virtual bool IsLicensed => !String.IsNullOrWhiteSpace(TLicense)
                                          || !String.IsNullOrWhiteSpace(WLicense)
                                          || !String.IsNullOrWhiteSpace(CLicense)
                                          || !String.IsNullOrWhiteSpace(SLicense)
                                          || !String.IsNullOrWhiteSpace(NLicense);

        public virtual string LicenseTypes => String.Join(", ", BuildLicenseTypeArray());

        #region CDL / DriversLicenses

        public virtual bool IsCDLCompliant { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? MedicalCertificateExpirationDate { get; set; }

        public virtual int MedicalCertificateDaysOverdue =>
            (MedicalCertificateExpirationDate.HasValue && MedicalCertificateExpirationDate < DateTime.Today)
                ? (DateTime.Today - MedicalCertificateExpirationDate.Value.BeginningOfDay()).Days
                : 0;

        [View(FormatStyle.Date)]
        public virtual DateTime? DriversLicenseIssuedDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DriversLicenseRenewalDate { get; set; }

        public virtual int DriversLicenseRenewalDaysOverdue =>
            (DriversLicenseRenewalDate.HasValue && DriversLicenseRenewalDate < DateTime.Today)
                ? (DateTime.Today - DriversLicenseRenewalDate.Value.BeginningOfDay()).Days
                : 0;

        [View("Violation Certificate Expiration", FormatStyle.Date)]
        public virtual DateTime? ViolationCertificateExpirationDate { get; set; }

        public virtual int ViolationCertificateCertificateDaysOverdue
        {
            get
            {
                var today = _dateTimeProvider.GetCurrentDate().BeginningOfDay();
                return (ViolationCertificateExpirationDate.HasValue && ViolationCertificateExpirationDate < today)
                    ? (today - ViolationCertificateExpirationDate.Value.BeginningOfDay()).Days
                    : 0;
            }
        }

        #endregion

        public virtual IEnumerable<DriversLicense> CommercialDriversLicenses
        {
            get { return DriversLicenses.Where(x => x.DriversLicenseClass.Id != 4); }
        }

        /// <summary>
        /// This is a formula field.
        /// </summary>
        public virtual bool HasOneDayDoctorsNoteRestriction { get; protected set; }

        public virtual string ReportsToName => ReportsTo?.FullName;

        public virtual string ReportsToEmployeeId => ReportsTo?.EmployeeId;

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

        public Employee()
        {
            EmployeeNotes = new List<Note<Employee>>();
            EmployeeAccountabilityActions = new List<EmployeeAccountabilityAction>();
            EmployeeDocuments = new List<Document<Employee>>();
            EmployeeGrievances = new List<GrievanceEmployee>();
            PositionHistory = new List<PositionHistory>();
            EmployeeTailgateTalks = new List<TailgateTalkEmployee>();
            DriversLicenses = new List<DriversLicense>();
            MedicalCertificates = new List<MedicalCertificate>();
            ViolationCertificates = new List<ViolationCertificate>();
            HepatitisBVaccinations = new List<HepatitisBVaccination>();
            ScheduledTrainingRecords = new List<TrainingRecordScheduledEmployee>();
            AttendedTrainingRecords = new List<TrainingRecordAttendedEmployee>();
            AbsenceNotifications = new List<AbsenceNotification>();
            ProductionSkillSets = new List<EmployeeProductionSkillSet>();
            RelatedProductionAssignments = new List<EmployeeAssignment>();
            ProductionAssignments = new List<EmployeeAssignment>();
            OperatorLicenses = new List<OperatorLicense>();
            Incidents = new List<Incident>();
            GeneralLiabilityClaims = new List<GeneralLiabilityClaim>();
            ScheduledAssignments = new List<ScheduledAssignment>();
        }

        #endregion

        #region Private Methods

        private string[] BuildLicenseTypeArray()
        {
            var ret = new List<string>();

            Action<string, string> addIfHasLicense = (license, type) => {
                if (!String.IsNullOrWhiteSpace(license))
                {
                    ret.Add(type);
                }
            };

            addIfHasLicense(LicenseWaterTreatment, "Water Treatment");
            addIfHasLicense(LicenseWaterDistribution, "Water Distribution");
            addIfHasLicense(LicenseSewerCollection, "Sewer Collection");
            addIfHasLicense(LicenseSewerTreatment, "Sewer Treatment");
            addIfHasLicense(LicenseIndustrialDischarge, "Industrial Discharge");
            addIfHasLicense(TLicense, "T");
            addIfHasLicense(WLicense, "W");
            addIfHasLicense(CLicense, "C");
            addIfHasLicense(SLicense, "S");
            addIfHasLicense(NLicense, "N");

            return ret.ToArray();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return FullName;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public virtual object ToJson()
        {
            return new {
                Id,
                Status = Status.Description,
                PositionGroup = PositionGroup?.Description,
                FirstName,
                LastName,
                EmployeeId,
                OperatingCenter = OperatingCenter?.Description,
                EmailAddress,
                ReportsToName,
                ReportsToEmployeeId
            };
        }

        #endregion
    }

    [Serializable]
    public class EmployeeDisplayItem : DisplayItem<Employee>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmployeeId { get; set; }

        [SelectDynamic("OperatingCenterCode")]
        public string OperatingCenter { get; set; }

        public override string Display =>
            $"{LastName}, {FirstName} {MiddleName} - {EmployeeId} - {OperatingCenter}".Replace("  ", " ");
    }

    [Serializable]
    public class EmployeeDisplayItemWithStatus : EmployeeDisplayItem
    {
        [SelectDynamic("Description")]
        public string Status { get; set; }

        public override string Display => $"{base.Display} - {Status}".Replace("  ", " ");
    }

    /// <summary>
    /// Normally, this is for when you have a single set of employees linked to your model
    /// (see Grievance, JobObservation, and TailgateTalk for examples).
    /// 
    /// If, however, you have more than one context for linking employees, they need to be
    /// differentiated by DataTypeName (see TrainingRecord Scheduled/AttendedEmployees).
    /// 
    /// Honestly, if you need the latter, this system should probably be rethought.  Look
    /// at Tab#ExtraData (and its usages), TabBuilder#WithEmployees (TabBuilderExtensions.cs),
    /// and Views\Shared\Employee\Index.cshtml and marvel at the all duct tape and superglue.
    /// </summary>
    public interface IThingWithEmployees
    {
        #region Abstract Properties

        int Id { get; }
        string TableName { get; }
        string DataTypeName { get; }
        IList<IEmployeeLink> LinkedEmployees { get; }
        bool AllowNotifications { get; }
        string RoleModule { get; }
        string NotificationPurpose { get; }
        string EntityType { get; }
        int OperatingCenterId { get; }

        /// <summary>
        /// Boolean returned should indicate whether the view should render the "Link Employees" control to link more employees.
        /// </summary>
        /// <param name="dataTypeName"></param>
        /// <returns></returns>
        bool AllowMoreEmployeesFor(string dataTypeName);

        #endregion
    }

    public interface IEmployeeLink : IEntity
    {
        #region Abstract Properties

        Employee Employee { get; set; }
        DataType DataType { get; set; }
        int LinkedId { get; set; }
        DateTime LinkedOn { get; set; }
        string LinkedBy { get; set; }

        #endregion
    }
}
