using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class EmployeeViewModel : ViewModel<Employee>
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(EmployeeStatus)), EntityMap]
        public virtual int? Status { get; set; }

        [DropDown, EntityMustExist(typeof(OperatingCenter)), EntityMap]
        public virtual int? OperatingCenter { get; set; }

        [EntityMustExist(typeof(PersonnelArea)), EntityMap]
        [DropDown("", "PersonnelArea", "ByOperatingCenter", DependsOn = nameof(OperatingCenter), PromptText = "Select an Operating Center")]
        public virtual int? PersonnelArea { get; set; }

        [DropDown, EntityMustExist(typeof(EmployeeDepartment)), EntityMap]
        public virtual int? Department { get; set; }

        // This is a nullable field in the db because not all employees will ever
        // end up with a PositionGroup, but it is required now for editing any existing
        // employees or new employees.
        [Required, ComboBox, EntityMustExist(typeof(PositionGroup)), EntityMap]
        public int? PositionGroup { get; set; }

        [DropDown("Facility", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMustExist(typeof(Facility)), EntityMap]
        public virtual int? ReportingFacility { get; set; }

        [Required, StringLength(Employee.StringLengths.EMPLOYEE_ID)]
        public virtual string EmployeeId { get; set; }

        [Required, StringLength(Employee.StringLengths.FIRST_NAME)]
        public virtual string LastName { get; set; }

        [Required, StringLength(Employee.StringLengths.LAST_NAME)]
        public virtual string FirstName { get; set; }

        [StringLength(Employee.StringLengths.MIDDLE_NAME)]
        public virtual string MiddleName { get; set; }

        [DropDown, EntityMustExist(typeof(Gender)), EntityMap]
        public virtual int? Gender { get; set; }

        public virtual DateTime? DateHired { get; set; }
        public virtual DateTime? SeniorityDate { get; set; }
        public virtual int? SeniorityRanking { get; set; }
        public virtual DateTime? InactiveDate { get; set; }

        [DropDown, EntityMustExist(typeof(ReasonForDeparture)), EntityMap]
        public virtual int? ReasonForDeparture { get; set; }

        [DropDown, EntityMustExist(typeof(Employee)), EntityMap]
        public virtual int? ReportsTo { get; set; }
        [DropDown, EntityMustExist(typeof(Employee)), EntityMap]
        public virtual int? HumanResourcesManager { get; set; }
        [StringLength(Employee.StringLengths.LICENSE_WATER_TREATMENT)]
        public virtual string LicenseWaterTreatment { get; set; }
        [StringLength(Employee.StringLengths.T_LICENSE)]
        public virtual string TLicense { get; set; }
        public virtual DateTime? DateOfTLicense { get; set; }
        [StringLength(Employee.StringLengths.LICENSE_WATER_DISTRIBUTION)]
        public virtual string LicenseWaterDistribution { get; set; }
        [StringLength(Employee.StringLengths.W_LICENSE)]
        public virtual string WLicense { get; set; }
        public virtual DateTime? DateOfWLicense { get; set; }
        [StringLength(Employee.StringLengths.LICENSE_SEWER_COLLECTION)]
        public virtual string LicenseSewerCollection { get; set; }
        [StringLength(Employee.StringLengths.C_LICENSE)]
        public virtual string CLicense { get; set; }
        public virtual DateTime? DateOfCLicense { get; set; }
        [StringLength(Employee.StringLengths.LICENSE_SEWER_TREATMENT)]
        public virtual string LicenseSewerTreatment { get; set; }
        [StringLength(Employee.StringLengths.S_LICENSE)]
        public virtual string SLicense { get; set; }
        public virtual DateTime? DateOfSLicense { get; set; }
        [StringLength(Employee.StringLengths.LICENSE_INDUSTRIAL_DISCHARGE)]
        public virtual string LicenseIndustrialDischarge { get; set; }
        [StringLength(Employee.StringLengths.N_LICENSE)]
        public virtual string NLicense { get; set; }
        public virtual DateTime? DateOfNLicense { get; set; }
        [StringLength(Employee.StringLengths.CDL)]
        public virtual string CDL { get; set; }
        [StringLength(Employee.StringLengths.DRIVERS_LICENSE)]
        public virtual string DriversLicense { get; set; }
        [StringLength(Employee.StringLengths.EMERGENCY_CONTACT_NAME)]
        public virtual string EmergencyContactName { get; set; }
        [StringLength(Employee.StringLengths.EMERGENCY_CONTACT_PHONE)]
        public virtual string EmergencyContactPhone { get; set; }
        
        [Coordinate, View("Coordinates")]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public virtual int? CoordinateId { get; set; }

        [MMSINC.Validation.EmailAddress, StringLength(Employee.StringLengths.EMAIL_ADDRESS)]
        public virtual string EmailAddress { get; set; }

        [StringLength(Employee.StringLengths.ADDRESS)]
        public virtual string Address { get; set; }
        [StringLength(Employee.StringLengths.ADDRESS2)]
        public virtual string Address2 { get; set; }
        [StringLength(Employee.StringLengths.CITY)]
        public virtual string City { get; set; }
        [StringLength(Employee.StringLengths.STATE)]
        public virtual string State { get; set; }
        [StringLength(Employee.StringLengths.ZIP_CODE)]
        public virtual string ZipCode { get; set; }
        [StringLength(Employee.StringLengths.PHONE_PAGER)]
        public virtual string PhonePager { get; set; }
        [StringLength(Employee.StringLengths.PHONE_CELLULAR)]
        public virtual string PhoneCellular { get; set; }
        [StringLength(Employee.StringLengths.PHONE_HOME)]
        public virtual string PhoneHome { get; set; }
        [StringLength(Employee.StringLengths.PHONE_WORK)]
        public virtual string PhoneWork { get; set; }
        [StringLength(Employee.StringLengths.PHONE_PERSONAL_CELLULAR)]
        public virtual string PhonePersonalCellular { get; set; }

        [DropDown, EntityMustExist(typeof(UnionAffiliation)), EntityMap]
        public virtual int? UnionAffiliation { get; set; }
        [StringLength(Employee.StringLengths.PURCHASE_CARD_NUMBER)]
        public virtual string PurchaseCardNumber { get; set; }
        public virtual decimal? MonthlyDollarLimit { get; set; }
        public virtual decimal? SingleDollarLimit { get; set; }
        [DropDown, EntityMustExist(typeof(TCPAStatus)), EntityMap]
        public virtual int? TCPAStatus { get; set; }
        [DropDown, EntityMustExist(typeof(DPCCStatus)), EntityMap]
        public virtual int? DPCCStatus { get; set; }
        [DropDown("Employee", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMustExist(typeof(Employee)), EntityMap]
        public virtual int? PurchaseCardReviewer { get; set; }
        [DropDown("Employee", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMustExist(typeof(Employee)), EntityMap]
        public virtual int? PurchaseCardApprover { get; set; }
        [DropDown, EntityMustExist(typeof(InstitutionalKnowledge)), EntityMap]
        public virtual int? InstitutionalKnowledge { get; set; }
        [Multiline]
        public virtual string InstitutionalKnowledgeDescription { get; set; }

        [DropDown, EntityMustExist(typeof(CommercialDriversLicenseProgramStatus)), EntityMap, Required]
        public virtual int? CommercialDriversLicenseProgramStatus { get; set; }

        // TODO: This property doesn't make sense. It's a formula property yet we're
        // having the user check a checkbox to make the below field required? -Ross 5/5/2020
        public bool HasOneDayDoctorsNoteRestriction { get; set; }

        [RequiredWhen("HasOneDayDoctorsNoteRestriction", true, ErrorMessage="You must set the restriction end date if the employee has a one day doctor's note restriction.")]
        public DateTime? OneDayDoctorsNoteRestrictionEndDate { get; set; }

        [DropDown, EntityMustExist(typeof(ScheduleType)), EntityMap]
        public virtual int? ScheduleType { get; set; }

        // TODO: These two properties should be nullable? -Ross 5/5/2020
        public virtual bool ValidEssentialEmployeeCard { get; set; }
        public virtual bool GETSWPSCard { get; set; }

        #endregion

        #region Constructors

        public EmployeeViewModel(IContainer container) : base(container) {}

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateEmployeeIdUniqueness()
        {
            var existingEmployee = _container.GetInstance<IEmployeeRepository>().GetByEmployeeId(EmployeeId);
            if (existingEmployee != null && existingEmployee.Id != Id)
            {
                yield return new ValidationResult("The given employee ID is already being used by another employee.", new[] { nameof(EmployeeId) });
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateEmployeeIdUniqueness());
        }

        #endregion
    }

    public class EditEmployee : EmployeeViewModel
    {
        #region Constructors

        public EditEmployee(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateEmployee : EmployeeViewModel
    {
        #region Constructors

        public CreateEmployee(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchEmployee : SearchSet<Employee>
    {
        #region Properties

        [View("Employee ID")]
        public string EmployeeId { get; set; }

        [DropDown]
        public virtual int? OperatingCenter { get; set; }
        [DropDown("", "PersonnelArea", "ByOperatingCenter", DependsOn = nameof(OperatingCenter), PromptText = "Select an Operating Center")]
        public virtual int? PersonnelArea { get; set; }
        [DropDown]
        public int? Status { get; set; }
        public string LastName { get; set; }
        [DropDown, SearchAlias("CurrentPosition", "p", "Category")]
        public string Category { get; set; }
        [View("Initial Accountability Action Taken"), EntityMap, EntityMustExist(typeof(AccountabilityActionTakenType))]
        [DropDown, SearchAlias("EmployeeAccountabilityActions", "a", "AccountabilityActionTakenType.Id")]
        public int? AccountabilityActionTakenType { get; set; }
        [View("Modified Accountability Action Taken"), EntityMap, EntityMustExist(typeof(AccountabilityActionTakenType))]
        [DropDown, SearchAlias("EmployeeAccountabilityActions", "a", "ModifiedAccountabilityActionTakenType.Id")]
        public int? ModifiedAccountabilityActionTakenType { get; set; }
        [DropDown]
        public int? Department { get; set; }
        [DropDown, SearchAlias("CurrentPosition", "p", "Id")]
        public int? CurrentPosition { get; set; }

        [View("Date of Hire")]
        public DateRange DateHired { get; set; }
        [View("Reports To (last name only)")]
        [SearchAlias("ReportsTo", "LastName")]
        public string ReportsTo { get; set; }
        [View("Human Resource Manager (last name only)")]
        [SearchAlias("HumanResourcesManager", "LastName")]
        public string HumanResourcesManager { get; set; }
        [SearchAlias("CurrentPosition", "p", "Essential")]
        public bool? EssentialPosition { get; set; }
        [DropDown, SearchAlias("CurrentPosition", "p", "EmergencyResponsePriority.Id")]
        public int? EmergencyResponsePriority { get; set; }

        [DropDown]
        public int? CommercialDriversLicenseProgramStatus { get; set; }
        public bool? IsCDLCompliant { get; set; }
        public bool? HasOneDayDoctorsNoteRestriction { get; set; }
        [View(Employee.ACCOUNTABILITY_ACTION_RECORDS_ATTACHED), Search(ChecksExistenceOfChildCollection = true)]
        public bool? EmployeeAccountabilityActions { get; set; }

        #endregion
    }

    public class SearchEmployeeTraining : SearchSet<EmployeeTrainingReportItem>, ISearchEmployeeTraining
    {
        // TODO: Add other fields from Bug 2416

        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "opCntr", "Id")]
        public int? OperatingCenter { get; set; }

        [SearchAlias("positionGroupCommonName.TrainingRequirements", "trainingRequirement", "IsOSHARequirement")]
        [View("OSHA Requirement")]
        public bool? OSHARequirement { get; set; }

        [DropDown]
        [SearchAlias("positionGroupCommonName.TrainingRequirements", "trainingRequirement", "Id")]
        public int? TrainingRequirement { get; set; }

        [DropDown, View("Common Name")]
        [SearchAlias("positionGroup.CommonName", "positionGroupCommonName", "Id")]
        public int? PositionGroupCommonName { get; set; }
        
        [SearchAlias("PositionGroup", "positionGroup", "Id")]
        [View("Position"), DropDown("", "PositionGroup", "GetByCommonName", DependsOn = "PositionGroupCommonName")]
        public int? PositionGroup { get; set; }

        public string LastName { get; set; }

        [Search(CanMap = false)]
        public DateRange RecentTrainingDate { get; set; }
        [Search(CanMap = false)]
        public DateRange NextScheduledDate { get; set; }
        [Search(CanMap = false)]
        public DateRange NextDueByDate { get; set; }

        public int? EntityId { get; set; }

        #endregion
    }


    public class SearchEmployeeTrainingByDate : SearchSet<EmployeeTrainingByDateReportItem>, ISearchEmployeeTrainingByDate
    {
        // TODO: Add other fields from Bug 2416

        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "opCntr", "Id")]
        public int? OperatingCenter { get; set; }

        [SearchAlias("positionGroupCommonName.TrainingRequirements", "IsOSHARequirement")]
        [View("OSHA Requirement")]
        public bool? OSHARequirement { get; set; }

        [DropDown]
        [SearchAlias("positionGroupCommonName.TrainingRequirements", "Id")]
        public int? TrainingRequirement { get; set; }

        [DropDown, View("Common Name")]
        [SearchAlias("positionGroup.CommonName", "positionGroupCommonName", "Id")]
        public int? PositionGroupCommonName { get; set; }

        [SearchAlias("PositionGroup", "positionGroup", "Id")]
        [View("Position"), DropDown("", "PositionGroup", "GetByCommonName", DependsOn = "PositionGroupCommonName")]
        public int? PositionGroup { get; set; }

        public string LastName { get; set; }

        [Search(CanMap = false)]
        public DateRange RecentTrainingDate { get; set; }
        [Search(CanMap = false)]
        public DateRange NextScheduledDate { get; set; }
        [Search(CanMap = false)]
        public DateRange NextDueByDate { get; set; }
        [Search(CanMap = false)]
        public DateRange DateAttended { get; set; }

        public int? EntityId { get; set; }

        public override string DefaultSortBy => "attendedTrainingRecord.HeldOn";

        #endregion
    }

    public class SearchEmployeeUserReconciliation : SearchSet<EmployeeUserReconciliationReportItem>, ISearchEmployeeUserReconciliation
    {
        #region Properties

        [SearchAlias("OperatingCenter", "eOpCntr", "Id"), DropDown("OperatingCenter")]
        public int? EmployeeOperatingCenter { get; set; }
        [SearchAlias("user.DefaultOperatingCenter", "uOpCntr", "Id"), DropDown("OperatingCenter")]
        public int? UserOperatingCenter { get; set; }
        public string LastName { get; set; }

        #endregion
    }

    public class SearchEmployeeTrainingRecordExport : SearchSet<EmployeeTrainingRecordExportItem>
    {
        #region Properties

        [SearchAlias("TrainingRecord", "trainingRecord", "HeldOn")] 
        public DateRange HeldOn { get; set; }
        
        [SearchAlias("TrainingRecord", "trainingRecord", "ScheduledDate")]
        public DateRange ScheduledDate { get; set; }

        [SearchAlias("TrainingRecord", "trainingRecord", "AttendeesExportedDate")]
        public DateRange AttendeesExportedDate { get; set; }

        [DropDown]
        [SearchAlias("TrainingRecord", "Id")]
        public int? TrainingRecord { get; set; }

        // Training Module
        [DropDown]
        [SearchAlias("TrainingRecord", "trainingRecord", "TrainingModule.Id")]
        public int? TrainingModule { get; set; }
        
        // AW CourseNumber
        [SearchAlias("trainingRecord.TrainingModule", "trainingModule", "AmericanWaterCourseNumber")]
        public string AmericanWaterCourseNumber { get; set; }

        // Exported
        [SearchAlias("TrainingRecord", "trainingRecord", "Exported")]
        public bool? Exported { get; set; }

        #endregion

        // Date Exported
    }

    public class SearchEmployeeNJDEPLicense : SearchEmployee
    {
        [DropDown, Required, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("OperatingCenter", "opc","State.Id")]
        public int? State { get; set; }
        
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State", PromptText = "Please select a state")]
        public override int? OperatingCenter { get; set; }

        // TODO: Why is this not an override of the property that it's hiding? Or being forcibly set by ModifyValues?
        public new int? Status { get { return EmployeeStatus.Indices.ACTIVE; } }
    }
}
