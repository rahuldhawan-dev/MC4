using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using DataType = System.ComponentModel.DataAnnotations.DataType;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public abstract class BaseIncidentModel : ViewModel<Incident>
    {
        #region Constants

        public const int TARGET_COMPLETION_DATE_DEFAULT_DATEDIFF = 10; // MC-2407 Now 10 days

        #endregion

        #region Properties

        [Coordinate(IconSet = IconSets.Incidents, AddressCallback = "Incident.getAddress")]
        [Required(ErrorMessage = "The Incident Coordinates field is required."), EntityMustExist(typeof(Coordinate))]
        [EntityMap]
        public virtual int? AccidentCoordinate { get; set; }

        [Required, StringLength(Incident.StringLengths.MAX_ACCIDENT_STREET_NAME)]
        public string AccidentStreetName { get; set; }

        [Required, StringLength(Incident.StringLengths.MAX_ACCIDENT_STREET_NUMBER)]
        public string AccidentStreetNumber { get; set; }

        [View("Incident State"), Required, DropDown, DoesNotAutoMap("Used for cascades. Manually mapped.")]
        public int? AccidentState { get; set; }

        [DoesNotAutoMap("Used for cascades. Manually mapped.")]
        [View("Incident County"), Required, DropDown("", "County", "ByStateId", DependsOn = "AccidentState", PromptText = "Select a state above")]
        public int? AccidentCounty { get; set; }

        [EntityMap]
        [EntityMustExist(typeof(Town))]
        [Required, DropDown("", "Town", "ByCountyId", DependsOn = "AccidentCounty", PromptText = "Select a county above")]
        [View("Incident Town")]
        public int? AccidentTown { get; set; }

        [Required, StringLength(Incident.StringLengths.INCIDENT_SUMMARY)]
        public virtual string IncidentSummary { get; set; }

        public virtual string AnyImmediateCorrectiveActionsApplied { get; set; }

        [DataType(DataType.MultilineText)]
        [RequiredWhen("QuestionParticipatedInAthleticActivitiesInLastTwelveMonths", true, ErrorMessage = "This field is required.")]
        public string AthleticActivitiesInLastTwelveMonths { get; set; }

        [StringLength(Incident.StringLengths.MAX_CASE_MANAGER)]
        public string CaseManager { get; set; }

        [StringLength(Incident.StringLengths.MAX_CASE_NUMBER)]
        public string CaseNumber { get; set; }

        [StringLength(Incident.StringLengths.MAX_CLAIMANT)]
        public string Claimant { get; set; }

        [StringLength(Incident.StringLengths.CONTRACTOR_COMPANY)]
        [RequiredWhen("EmployeeType", ComparisonType.EqualTo, MapCall.Common.Model.Entities.EmployeeType.Indices.CONTRACTOR)]
        public string ContractorCompany { get; set; }

        [StringLength(Incident.StringLengths.CONTRACTOR_NAME)]
        [RequiredWhen("EmployeeType", ComparisonType.EqualTo, MapCall.Common.Model.Entities.EmployeeType.Indices.CONTRACTOR)]
        public string ContractorName { get; set; }

        [RequiredWhen("IsOSHARecordable", true)]
        public DateTime? DateInvestigationWillBeCompleted { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(int.MaxValue)]
        public string DrugAndAlcoholTestingNotes { get; set; }

        [View("Decision")]
        [Required(ErrorMessage = "The Decision field is required."), DropDown, EntityMap]
        [EntityMustExist(typeof(IncidentDrugAndAlcoholTestingDecision))]
        public int? DrugAndAlcoholTestingDecision { get; set; }

        // TODO: RequiredIf changes here.
        [EntityMap, DropDown]
        [EntityMustExist(typeof(IncidentDrugAndAlcoholTestingResult))]
        public int? DrugAndAlcoholTestingResult { get; set; }

        [EntityMap]
        [EntityMustExist(typeof(Employee))]
        [RequiredWhen("EmployeeType", ComparisonType.EqualTo, MapCall.Common.Model.Entities.EmployeeType.Indices.EMPLOYEE)]
        public virtual int? Employee { get; set; }

        [EntityMap, Required]
        [DropDown, EntityMustExist(typeof(EmployeeType))]
        public virtual int? EmployeeType { get; set; }

        [Required, EntityMap] 
        [EntityMustExist(typeof(Facility))]
        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public virtual int? Facility { get; set; }

        [Required]
        public bool? FiveWhysCompleted { get; set; }

        [DropDown, EntityMap]
        [EntityMustExist(typeof(GeneralLiabilityCode))]
        public int? GeneralLiabilityCode { get; set; }

        public DateTime? IncidentCommitteeReportCompletionDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string IncidentCommitteeReportResults { get; set; }

        [DropDown, Required, EntityMap]
        [EntityMustExist(typeof(IncidentClassification))]
        public int? IncidentClassification { get; set; }

        [Required, DateTimePicker]
        public DateTime? IncidentDate { get; set; }

        [Required, DateTimePicker]
        public DateTime? IncidentReportedDate { get; set; }

        [DropDown, EntityMap]
        [Required, EntityMustExist(typeof(IncidentType))]
        public int? IncidentType { get; set; }

        [DropDown, EntityMap, Required]
        [EntityMustExist(typeof(EventExposureType))]
        public int? EventExposureType { get; set; }
        
        [MultiSelect, EntityMap]
        [EntityMustExist(typeof(BodyPart))]
        public int[] BodyParts { get; set; }

        [DropDown, EntityMap, Required]
        [EntityMustExist(typeof(IncidentShift))]
        public int? IncidentShift { get; set; }

        public bool IsChargeableMotorVehicleAccident { get; set; }

        public bool IsInLitigation { get; set; }
        public bool IsOSHARecordable { get; set; }
        public bool IsSafetyCodeViolation { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SeriousInjuryOrFatalityType))]
        public int? SeriousInjuryOrFatalityType { get; set; }

        [Required]
        public bool? IsOvertime { get; set; }

        [StringLength(Incident.StringLengths.MAX_MARKOUT_NUMBER)]
        public string MarkoutNumber { get; set; }

        [DropDown, EntityMap]
        [EntityMustExist(typeof(MotorVehicleCode))]
        public int? MotorVehicleCode { get; set; }

        [StringLength(Incident.StringLengths.MAX_MED_PROVIDER_NAME)]
        [RequiredWhen("SoughtMedicalAttention", true)]
        public string MedicalProviderName { get; set; }

        [StringLength(Incident.StringLengths.MAX_MED_PROVIDER_PHONE)]
        [RequiredWhen("SoughtMedicalAttention", true)]
        public string MedicalProviderPhone { get; set; }

        [DropDown, DoesNotAutoMap("Used for cascade. Manually set.")]
        [RequiredWhen("SoughtMedicalAttention", true)]
        public int? MedicalProviderState { get; set; }

        [RequiredWhen("SoughtMedicalAttention", true), DoesNotAutoMap("Used for cascade. Manually set.")]
        [DropDown("", "County", "ByStateId", DependsOn = "MedicalProviderState", PromptText = "Select a state above")]
        public int? MedicalProviderCounty { get; set; }

        [RequiredWhen("SoughtMedicalAttention", true)]
        [EntityMap]
        [EntityMustExist(typeof(Town))]
        [DropDown("", "Town", "ByCountyId", DependsOn = "MedicalProviderCounty", PromptText = "Select a county above")]
        public int? MedicalProviderTown { get; set; }

        [DataType(DataType.MultilineText)]
        [RequiredWhen("QuestionHaveHadSimilarInjuryBefore", true)]
        public string NatureOfPriorInjury { get; set; }
        
        [RequiredWhen("IsOvertime", true)]
        public decimal? NumberOfHoursOvertimeInPastWeek { get; set; }

        [DropDown, Required, EntityMap] 
        [EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [DataType(DataType.MultilineText)]
        [RequiredWhen("QuestionHaveJobOutsideOfAmericanWater", true, ErrorMessage = "This field is required.")]
        public string OtherEmployers { get; set; }

        public bool PoliceReportFiled { get; set; }

        [StringLength(Incident.StringLengths.MAX_PREMISE_NUMBER)]
        public string PremiseNumber { get; set; }

        [RequiredWhen("QuestionHaveHadSimilarInjuryBefore", true)]
        public virtual DateTime? PriorInjuryDate { get; set; }

        [DataType(DataType.MultilineText)]
        [RequiredWhen("QuestionHaveHadSimilarInjuryBefore", true, ErrorMessage = "This field is required.")]
        public string PriorInjuryMedicalProvider { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Multiline]
        public string QuestionEmployeeDoingBeforeIncidentOccurred { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Multiline]
        public string QuestionWhatHappened { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Multiline]
        public string QuestionInjuryOrIllness { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Multiline]
        public string QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool QuestionHaveHadSimilarInjuryBefore { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool QuestionParticipatedInAthleticActivitiesInLastTwelveMonths { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool QuestionHaveJobOutsideOfAmericanWater { get; set; }

        [Required]
        public bool SoughtMedicalAttention { get; set; }

        [DataType(DataType.MultilineText)]
        [Multiline]
        public virtual string TravelersReport { get; set; }

        [EntityMap]
        [EntityMustExist(typeof(Vehicle))]
        [DropDown("FleetManagement", "Vehicle", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public virtual int? Vehicle { get; set; }

        [StringLength(Incident.StringLengths.MAX_WITNESS_NAME)]
        public string WitnessName { get; set; }

        [StringLength(Incident.StringLengths.MAX_WITNESS_PHONE)]
        public string WitnessPhone { get; set; }

        [StringLength(Incident.StringLengths.MAX_WORKORDER_ID)]
        public string WorkOrderId { get; set; }

        [AutoComplete("FieldOperations", nameof(WorkOrder), "FindByPartialWorkOrderIDMatch")]
        [EntityMap]
        [EntityMustExist(typeof(WorkOrder))]
        public int? MapCallWorkOrder { get; set; }

        [RequiredWhen("AtRiskBehaviorSubSection", ComparisonType.NotEqualTo, null)]
        [DropDown, EntityMap, EntityMustExist(typeof(AtRiskBehaviorSection))]
        public int? AtRiskBehaviorSection { get; set; }

        [DropDown("Operations", "AtRiskBehaviorSubSection", "ByAtRiskBehaviorSectionId", DependsOn = "AtRiskBehaviorSection")]
        [EntityMap, EntityMustExist(typeof(AtRiskBehaviorSubSection))]
        public int? AtRiskBehaviorSubSection { get; set; }

        [DoesNotAutoMap("Used by controller, set in MapToEntity.")]
        public bool SendOSHANotification { get; set; }

        [RequiredWhen("FiveWhysCompleted", true, ErrorMessage = "This field is required when 'Five Whys Completed' is Yes.")]
        [StringLength(Incident.StringLengths.FIVE_WHYS)]
        public virtual string Why1 { get; set; }

        [StringLength(Incident.StringLengths.FIVE_WHYS)]
        public virtual string Why2 { get; set; }

        [StringLength(Incident.StringLengths.FIVE_WHYS)]
        public virtual string Why3 { get; set; }

        [StringLength(Incident.StringLengths.FIVE_WHYS)]
        public virtual string Why4 { get; set; }

        [StringLength(Incident.StringLengths.FIVE_WHYS)]
        public virtual string Why5 { get; set; }

        [RequiredWhen("FiveWhysCompleted", true, ErrorMessage = "This field is required when 'Five Whys Completed' is Yes.")]
        public virtual DateTime? DateSubmitted { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WorkersCompensationClaimStatus)), Required]
        public virtual int? WorkersCompensationClaimStatus { get; set; }

        [StringLength(Incident.StringLengths.MAX_CLAIMS_CARRIER_ID)]
        [RequiredWhen(nameof(WorkersCompensationClaimStatus), ComparisonType.EqualToAny, nameof(GetClaimsCarrierIdRequiredWorkerCompensationClaimStatus), typeof(BaseIncidentModel), FieldOnlyVisibleWhenRequired = true)]
        public string ClaimsCarrierId { get; set; }

        #region Nurse stuff

        [Required, DropDown, EntityMap, EntityMustExist(typeof(EmployeeSpokeWithNurse))]
        public virtual int? EmployeeSpokeWithNurse { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(IncidentNurseRecommendationType))]
        public int? IncidentNurseRecommendationType { get; set; }

        public string RecommendedMedicalProvider { get; set; }
        public string NonMedicalTreatmentRecommendation { get; set; }

        public bool? EmployeeAcceptedRecommendationByNurse { get; set; }

        [StringLength(Incident.StringLengths.NURSE_PHONE)]
        public string NursePhone { get; set; }

        public string ReasonEmployeeDidNotAcceptRecommendationByNurse { get; set; }

        #endregion

        #endregion

        #region Constructors

        protected BaseIncidentModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateTestingResult()
        {
            if (!IncidentCommitteeReportCompletionDate.HasValue)
            {
                // None of this matters until CompletionDate is set.
                yield break;
            }

            if (!DrugAndAlcoholTestingResult.HasValue && DrugAndAlcoholTestingDecision.HasValue)
            {
                // TestingResult is never required if TestingDecision has a "NO TEST..." value.
                var decision =
                    _container.GetInstance<MMSINC.Data.NHibernate.IRepository<IncidentDrugAndAlcoholTestingDecision>>
                        ().Find(DrugAndAlcoholTestingDecision.Value);

                // Decision is required, so if that fails this method won't be called anyway.
                if (decision != null && decision.RequiresTesting)
                {
                    yield return
                        new ValidationResult(
                            "A drug and alcohol testing result is required when a decision that requires testing is entered.",
                            new[] {"DrugAndAlcoholTestingResult"});
                }
            }
        }

        public static int[] GetClaimsCarrierIdRequiredWorkerCompensationClaimStatus() => MapCall.Common.Model.Entities.WorkersCompensationClaimStatus.CLAIMS_CARRIER_ID;

        //private IEnumerable<ValidationResult> ValidateIncidentType()
        //{
        //    if (!IncidentCommitteeReportCompletionDate.HasValue)
        //    {
        //        // None of this matters until CompletionDate is set.
        //        yield break;
        //    }

        //    if (!IncidentType.HasValue)
        //    {
        //        yield return new ValidationResult("Incident Type must be entered when the completion date is set.", new[] { "IncidentType" });
        //    }
        //}

        #endregion

        #region Exposed Methods

        public override void Map(Incident entity)
        {
            base.Map(entity);

            AccidentCounty = AccidentTown.HasValue ? entity.AccidentTown.County.Id : (int?)null;
            AccidentState = AccidentTown.HasValue ? entity.AccidentTown.County.State.Id : (int?)null;
            MedicalProviderCounty = MedicalProviderTown.HasValue ? entity.MedicalProviderTown.County.Id : (int?)null;
            MedicalProviderState = MedicalProviderTown.HasValue ? entity.MedicalProviderTown.County.State.Id : (int?)null;
        }

        public override Incident MapToEntity(Incident entity)
        {
            // Only want to send this notification if IsOSHARecordable gets set to true. This includes during editing.
            // NOTE: Need to get the entity value *before* mapping.
            SendOSHANotification = (!entity.IsOSHARecordable && IsOSHARecordable);

            if (IncidentDate.HasValue)
            {
                entity.IncidentCommitteeReportTargetCompletionDate = IncidentDate.Value.AddDays(TARGET_COMPLETION_DATE_DEFAULT_DATEDIFF);
            }

            if (EmployeeType == MapCall.Common.Model.Entities.EmployeeType.Indices.EMPLOYEE)
            {
                ContractorName = null;
                ContractorCompany = null;
            }
            else
            {
                Employee = null;
            }

            return base.MapToEntity(entity);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateTestingResult());
        }

        #endregion
    }

    public class CreateIncident : BaseIncidentModel
    {
        #region Constants

        public const string VALIDATION_ERROR_INACTIVE_EMPLOYEE = "Employee must be active.";

        #endregion

        #region Properties

        // For create, employees must filter by supervisor and/or role operating center.
        [DropDown("", "Employee", "ActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterId",
            DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public override int? Employee
        {
            get => base.Employee;
            set => base.Employee = value;
        }

        #endregion

        #region Constructors

        public CreateIncident(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateActiveEmployee()
        {
            var emp = _container.GetInstance<IEmployeeRepository>().Find(Employee.GetValueOrDefault());
            if (emp != null && !emp.IsActive && EmployeeType == MapCall.Common.Model.Entities.EmployeeType.Indices.EMPLOYEE)
            {
                yield return new ValidationResult(VALIDATION_ERROR_INACTIVE_EMPLOYEE, new[] { "Employee" });
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            // These values can not be set in the Map method as the Map method
            // is not called when null is passed in the constructor.
            // MC-2137 requests this be false by default.
            FiveWhysCompleted = false;

            // MC-2576 requests this to be IMMEDIATE_MEDICAL_TREATMENT_NOT_REQUIRED by default
            DrugAndAlcoholTestingDecision = IncidentDrugAndAlcoholTestingDecision.Indices.IMMEDIATE_MEDICAL_TREATMENT_NOT_REQUIRED;
        }

        public override Incident MapToEntity(Incident entity)
        {
            var mapped = base.MapToEntity(entity);
            mapped.Supervisor = mapped.Employee?.ReportsTo;
            mapped.PersonnelArea = mapped.Employee?.PersonnelArea;
            mapped.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
            if (entity.EmployeeType?.Id == MapCall.Common.Model.Entities.EmployeeType.Indices.CONTRACTOR)
            {
                mapped.ContractorObservedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;
            }
            mapped.Position = mapped.Employee?.CurrentPosition;
            mapped.IncidentStatus = _container.GetInstance<IRepository<IncidentStatus>>().Find(IncidentStatus.Indices.OPEN);
            if (Employee.HasValue)
            {
                mapped.Department = _container.GetInstance<IEmployeeRepository>().Find(Employee.Value).Department;
            }

            return mapped;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateActiveEmployee());
        }

        #endregion
    }

    public class EditIncident : BaseIncidentModel
    {
        #region Constants

        internal const string VALIDATION_NON_ADMIN_EMPLOYEE = "Only admins may change the employee on an existing incident record.",
                              VALIDATION_NON_ADMIN_FACILITY = "Only admins may change the facility on an existing incident record.",
                              VALIDATION_NON_ADMIN_OPERATING_CENTER = "Only admins may change the operating center on an existing incident record.",
                              VALIDATION_NON_ADMIN_SUPERVISOR = "Only admins may change the supervisor on an existing incident record.";

        #endregion

        // Employee, Facility, and OperatingCenter must be
        // manually mapped as their values can only be
        // changed by admins

        #region Properties

        [DropDown, EntityMap]
        [EntityMustExist(typeof(Employee))]
        public int? Supervisor { get; set; }

        // This needs to be cascading whether or not the OperatingCenter is editable.
        [DropDown("FleetManagement", "Vehicle", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public override int? Vehicle
        {
            get { return base.Vehicle; }
            set { base.Vehicle = value; }
        }

        // For edit, which is only available to super admins, we need to display employees only filtered
        // by operating center.
        [DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an Operating Center above")]
        public override int? Employee
        {
            get => base.Employee;
            set => base.Employee = value;
        }

        [DoesNotAutoMap("Display only. Used for ValidateNonAdminFields")]
        public Employee CurrentEmployee => GetOriginalIncident().Employee;

        [DoesNotAutoMap("Display only. Used for ValidateNonAdminFields")]
        public Facility CurrentFacility => GetOriginalIncident().Facility;

        [DoesNotAutoMap("Display only. Used for ValidateNonAdminFields")]
        public OperatingCenter CurrentOperatingCenter => GetOriginalIncident().OperatingCenter;
      
        [DoesNotAutoMap("Display only. Used for ValidateNonAdminFields")]
        public Employee CurrentSupervisor => GetOriginalIncident().Supervisor;
       
        [DoesNotAutoMap("Display only. Used for ValidateNonAdminFields")]
        public EmployeeDepartment CurrentDepartment => GetOriginalIncident().Department;

        [DoesNotAutoMap("Display only. Used for ValidateNonAdminFields")]
        [DisplayName(Incident.DisplayNames.CONTRACTOR_NAME)]
        public string CurrentContractorName => GetOriginalIncident().ContractorName;
        [DoesNotAutoMap("Display only. Used for ValidateNonAdminFields")]
        [DisplayName(Incident.DisplayNames.CONTRACTOR_COMPANY)]
        public string CurrentContractorCompany => GetOriginalIncident().ContractorCompany;

        [EntityMap, EntityMustExist(typeof(EmployeeDepartment))]
        [DropDown, Secured(AppliesToAdmins = false)]
        public int? Department { get; set; }

        [DropDown, Required, EntityMap, EntityMustExist(typeof(IncidentStatus))]
        public int? IncidentStatus { get; set; }

        #endregion

        #region Constructors

        public EditIncident(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private Incident GetOriginalIncident()
        {
            return _container.GetInstance<IRepository<Incident>>().Find(Id);
        }

        private IEnumerable<ValidationResult> ValidateNonAdminFields()
        {
            if (_container.GetInstance<IAuthenticationService<User>>().CurrentUserIsAdmin)
            {
                yield break;
            }

            if (CurrentEmployee?.Id != Employee && EmployeeType == MapCall.Common.Model.Entities.EmployeeType.Indices.EMPLOYEE)
            {
                yield return new ValidationResult(VALIDATION_NON_ADMIN_EMPLOYEE, new[] {"Employee"});
            }

            if (CurrentFacility.Id != Facility)
            {
                yield return new ValidationResult(VALIDATION_NON_ADMIN_FACILITY, new[] {"Facility"});
            }

            if (CurrentOperatingCenter.Id != OperatingCenter)
            {
                yield return new ValidationResult(VALIDATION_NON_ADMIN_OPERATING_CENTER, new[] {"OperatingCenter"});
            }

            var supervisorId = CurrentSupervisor != null ? CurrentSupervisor.Id : (int?)null;
            if (supervisorId != Supervisor)
            {
                yield return new ValidationResult(VALIDATION_NON_ADMIN_SUPERVISOR, new[] {"Supervisor"});
            }
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateNonAdminFields());
        }

        public override Incident MapToEntity(Incident entity)
        {
            var currentUserRoles = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Roles;

            if (currentUserRoles != null)
            {
                var hasDrugSectionEditAccess = currentUserRoles.Any(x =>
                    x.Module.Id == (int)RoleModules.OperationsIncidentsDrugTesting &&
                    x.OperatingCenter.Id == OperatingCenter &&
                    (x.Action.Id == (int)RoleActions.Edit ||
                     x.Action.Id == (int)RoleActions.UserAdministrator));

                if (!hasDrugSectionEditAccess)
                {
                    DrugAndAlcoholTestingDecision = entity.DrugAndAlcoholTestingDecision?.Id;
                    DrugAndAlcoholTestingNotes = entity.DrugAndAlcoholTestingNotes;
                    DrugAndAlcoholTestingResult = entity.DrugAndAlcoholTestingResult?.Id;
                }
            }

            return base.MapToEntity(entity);
        }

        #endregion
    }

    public class SearchIncidentOSHARecordableSummary : SearchSet<Incident>, ISearchIncidentOSHARecordableSummary
    {
        [MultiSelect]
        public int[] OperatingCenter { get; set; }

        [Required]
        public DateRange IncidentDate { get; set; }
    }

    public class SearchIncident : SearchSet<Incident>
    {
        #region Properties

        [View("IncidentID")]
        public int? EntityId { get; set; }

        [EntityMap, EntityMustExist(typeof(State))]
        [DropDown, SearchAlias("OperatingCenter", "State.Id", Required = true)]
        public virtual int? State { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateIdOrAll", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }

        [View("Reporting Facility")]
        [DropDown("", "Facility", "ByOperatingCenterIds", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Facility { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EmployeeType))]
        public int? EmployeeType { get; set; }

        [DropDown("", "Employee", "ActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Employee { get; set; }

        public string ContractorCompany { get; set; }

        public DateRange IncidentDate { get; set; }

        public DateRange IncidentCommitteeReportCompletionDate { get; set; }

        public bool? IsCompleted { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(IncidentClassification))]
        public int? IncidentClassification { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(IncidentType)), View("Injury Type")]
        public int? IncidentType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EventExposureType)), View("Event/Exposure Type")]
        public int? EventExposureType { get; set; }
     
        [DropDown, EntityMap, EntityMustExist(typeof(IncidentStatus))]
        public int? IncidentStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(IncidentDrugAndAlcoholTestingDecision))]
        public int? DrugAndAlcoholTestingDecision { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(IncidentDrugAndAlcoholTestingResult))]
        public int? DrugAndAlcoholTestingResult { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Department))]
        public int? Department { get; set; }

        public bool? FiveWhysCompleted { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(IncidentShift))]
        public int? IncidentShift { get; set; }

        public bool? IsOvertime { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(AtRiskBehaviorSection))]
        public int[] AtRiskBehaviorSection { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SeriousInjuryOrFatalityType))]
        [View("SIF")]
        public int? SeriousInjuryOrFatalityType { get; set; }

        public bool? IsOSHARecordable { get; set; }

        public DateRange DateInvestigationWillBeCompleted { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WorkersCompensationClaimStatus))]
        public virtual int? WorkersCompensationClaimStatus { get; set; }

        [View(Incident.DisplayNames.CLAIMS_CARRIER_ID)]
        public SearchString ClaimsCarrierId { get; set; }

        [AutoComplete("FieldOperations", nameof(WorkOrder), "FindByPartialWorkOrderIDMatch")]
        [EntityMap]
        [EntityMustExist(typeof(WorkOrder))]
        public int? MapCallWorkOrder { get; set; }

        #endregion

        #region Exposed Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            var props = mapper.MappedProperties;

            if (props.ContainsKey("IsCompleted"))
            {
                var isSignedOff = props["IsCompleted"].Value as bool?;
                if (isSignedOff.HasValue)
                {
                    props["IsCompleted"].Value = null;
                    props["IncidentCommitteeReportCompletionDate"].Value = !isSignedOff.Value ? SearchMapperSpecialValues.IsNull : SearchMapperSpecialValues.IsNotNull;
                }
            }
        }

        #endregion
    }

    public class AddIncidentEmployeeAvailability : ViewModel<Incident>
    {
        // NOTE: The whole setup for this model only works because there can only ever be 
        //       one IncidentEmployeeAvailability record with the EndDate not set. A new
        //       record is not allowed to be entered until the existing one has an EndDate
        //       set. If they messed up a record when closing it, they can remove it and 
        //       add one again. 

        #region Properties

        // This is only used when updating an existing record.
        [DoesNotAutoMap("Not a property on Incident"), EntityMustExist(typeof(IncidentEmployeeAvailability))]
        public int? ExistingEmployeeAvailability { get; set; }

        [View("Type")]
        [Required, DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(IncidentEmployeeAvailabilityType))]
        public int? EmployeeAvailabilityType { get; set; }

        [Required, View(FormatStyle.Date), DoesNotAutoMap("Manually set in MapToEntity.")]
        public DateTime? StartDate { get; set; }

        [DoesNotAutoMap("Manually set in MapToEntity.")]
        [CompareTo(nameof(StartDate), ComparisonType.GreaterThanOrEqualTo, TypeCode.DateTime, IgnoreNullValues = true)]
        public DateTime? EndDate { get; set; }

        #endregion

        #region Constructors

        public AddIncidentEmployeeAvailability(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Incident MapToEntity(Incident entity)
        {
            // Do not call base MapToEntity method as these properties don't map to an Incident.

            IncidentEmployeeAvailability iea;

            if (ExistingEmployeeAvailability.HasValue)
            {
                iea = entity.EmployeeAvailabilities.Single(x => x.Id == ExistingEmployeeAvailability.Value);
            }
            else
            {
                iea = new IncidentEmployeeAvailability();
                entity.EmployeeAvailabilities.Add(iea);
            }

            iea.Incident = entity;
            iea.EmployeeAvailabilityType = _container.GetInstance<IRepository<IncidentEmployeeAvailabilityType>>().Find(EmployeeAvailabilityType.Value);
            iea.StartDate = StartDate.Value;
            iea.EndDate = EndDate;

            switch (EmployeeAvailabilityType.Value)
            {
                case IncidentEmployeeAvailabilityType.Indices.RESTRICTIVE_DUTY:
                    if (entity.IncidentClassification.Id > IncidentClassification.Indices.FATALITY &&
                        entity.IncidentClassification.Id < IncidentClassification.Indices.LOST_TIME)
                    {
                        entity.IncidentClassification = _container
                                                        .GetInstance<IRepository<IncidentClassification>>()
                                                        .Find(IncidentClassification.Indices.RESTRICTED_DUTY);
                    }

                    break;
                case IncidentEmployeeAvailabilityType.Indices.LOST_TIME:
                    if (entity.IncidentClassification.Id > IncidentClassification.Indices.FATALITY &&
                        entity.IncidentClassification.Id < IncidentClassification.Indices.LOST_TIME ||
                        entity.IncidentClassification.Id == IncidentClassification.Indices.RESTRICTED_DUTY)
                    {
                        entity.IncidentClassification = _container
                                                        .GetInstance<IRepository<IncidentClassification>>()
                                                        .Find(IncidentClassification.Indices.LOST_TIME);
                    }

                    break;
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateTimes());
        }

        private IEnumerable<ValidationResult> ValidateTimes()
        {
            var incident = _container.GetInstance<IIncidentRepository>().Find(Id);

            var avails = incident.EmployeeAvailabilities.ToList();

            // Need to remove the record being updated(if there is one) so validation works correctly.
            if (ExistingEmployeeAvailability.HasValue)
            {
                avails.RemoveAll(x => x.Id == ExistingEmployeeAvailability.Value);
            }

            // The EndDate is not required, however no new records are allowed to be entered until all existing records for an incident have an EndDate set.
            if (avails.Any(x => !x.EndDate.HasValue))
            {
                yield return new ValidationResult("An employee availability record exists that does not have an end date specified. This record must be closed before continuing.");
                yield break;
            }

            foreach (var lost in avails)
            {
                if (lost.StartDate <= StartDate.Value)
                {
                    if (StartDate.Value < lost.EndDate)
                    {
                        yield return new ValidationResult("The given employee availability start date overlaps with an existing record.");
                    }
                }

                if (EndDate.HasValue)
                {
                    if (lost.StartDate <= EndDate.Value)
                    {
                        if (EndDate.Value <= lost.EndDate)
                        {
                            yield return new ValidationResult("The given employee availability end date overlaps with an existing record.");
                        }
                    }

                    if (StartDate.Value <= lost.StartDate && lost.EndDate <= EndDate.Value)
                    {
                        yield return new ValidationResult("The given employee availability date range is already encompassed by another record.");
                    }
                }
            }
        }

        #endregion
    }

    public class RemoveIncidentEmployeeAvailability : ViewModel<Incident>
    {
        #region Properties

        [Required, DoesNotAutoMap]
        public int? EmployeeAvailabilityId { get; set; }

        #endregion

        #region Constructors

        public RemoveIncidentEmployeeAvailability(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Incident MapToEntity(Incident entity)
        {
            // Do not call base.MapToEntity.
            var toRemove = entity.EmployeeAvailabilities.Single(x => x.Id == EmployeeAvailabilityId.Value);
            entity.EmployeeAvailabilities.Remove(toRemove);

            return entity;
        }

        #endregion
    }

    public class RemoveIncidentInvestigation : ViewModel<Incident>
    {
        #region Properties

        [Required, DoesNotAutoMap, EntityMustExist(typeof(IncidentInvestigation))]
        public int? IncidentInvestigationId { get; set; }

        #endregion

        #region Constructors

        public RemoveIncidentInvestigation(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Incident MapToEntity(Incident entity)
        {
            // Do not call base.MapToEntity.
            var toRemove = entity.IncidentInvestigations.Single(x => x.Id == IncidentInvestigationId.Value);
            entity.IncidentInvestigations.Remove(toRemove);
            return entity;
        }

        #endregion
    }
}