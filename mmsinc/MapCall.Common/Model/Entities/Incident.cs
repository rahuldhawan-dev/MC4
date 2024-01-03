using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Incident
        : IEntityWithCreationTimeTracking,
            IEntityWithUpdateTimeTracking,
            IThingWithNotes, 
            IThingWithDocuments,
            IThingWithCoordinate,
            IThingWithOperatingCenter,
            IThingWithActionItems
    {
        #region Consts

        public struct StringLengths
        {
            public const int MAX_CASE_NUMBER = 50,
                             MAX_CASE_MANAGER = 100,
                             MAX_CLAIMANT = 50,
                             MAX_CREATEDBY = CleanIncidentsTable.MAX_CREATEDBY_LENGTH,
                             MAX_MARKOUT_NUMBER = 50,
                             MAX_PREMISE_NUMBER = CleanIncidentsTable.MAX_PREMISE_NUMBER_LENGTH,
                             MAX_ACCIDENT_STREET_NAME = 50,
                             MAX_ACCIDENT_STREET_NUMBER = 10,
                             MAX_WORKORDER_ID = 50,
                             MAX_WITNESS_NAME = CleanIncidentsTable.MAX_WITNESS_NAME_LENGTH,
                             MAX_WITNESS_PHONE = CleanIncidentsTable.MAX_WITNESS_PHONE_LENGTH,
                             MAX_MED_PROVIDER_NAME = CleanIncidentsTable.MAX_MED_PROVIDER_LENGTH,
                             MAX_MED_PROVIDER_PHONE = CleanIncidentsTable.MAX_MED_PROVIDER_PHONE_LENGTH,
                             RECOMMENDED_MEDICAL_PROVIDER = 255,
                             NON_MEDICAL_TREATMENT_RECOMMENDATION = 255,
                             INCIDENT_SUMMARY = 130,
                             CONTRACTOR_NAME = 100,
                             CONTRACTOR_COMPANY = 100,
                             FIVE_WHYS = 255,
                             MAX_CLAIMS_CARRIER_ID = 255,
                             NURSE_PHONE = 14;
        }

        public struct DisplayNames
        {
            public const string FIVE_WHYS_COMPLETED = "5 Whys Completed",
                                QUESTION_EMPLOYEE_DOING_BEFORE_INCIDENT_OCCURRED =
                                    "What was the employee doing before the incident occurred?",
                                QUESTION_WHAT_HAPPENED = "What happened?",
                                QUESTION_INJURY_OR_ILLNESS = "What was the injury or illness?",
                                QUESTION_OBJECT_OR_SUBSTANCE = "What object or substance directly harmed the employee?",
                                QUESTION_HAVE_INJURED_BEFORE =
                                    "Have you ever injured or had treatment for this pain or body part before from a motor vehicle incident, workplace incident, or other incident?",
                                PRIOR_INJURY_MEDICAL_PROVIDER =
                                    "If yes, list the name and address of any medical provider(including chiropractors) who provided medical care.",
                                QUESTION_ATHLETICS =
                                    "Do you currently (in the last 12 months) participate in any athletic, recreational or sporting activities?",
                                ATHLETIC_ACTIVITIES = "If yes, list the activities you participate in.",
                                QUESTION_JOB_OUTSIDE_AMERICAN_WATER =
                                    "Do you volunteer or have a second occupation outside of American Water?",
                                OTHER_EMPLOYERS = "If yes, list the names and addresses of these employers.",
                                QUESTION_WHY1 = "Why Did the Incident Occur?",
                                QUESTION_WHY2345 = "Why?",
                                CONTRACTOR_COMPANY = "Contractor Company",
                                CONTRACTOR_NAME = "Contractor Name",
                                COMPENSATION_STATUS = "Workers' Compensation Claim Status",
                                CLAIMS_CARRIER_ID = "Claims Carrier Id",
                                MAP_CALL_WORK_ORDER = "MapCall Work Order",
                                IS_NOT_A_SAFETY_ACCOUNTABILITY_PRACTICE = "Is Not a Safety Accountability Practice";
        }

        public const int OSHA_DAYS_CAP = 180;

        #endregion

        #region Private Members

        [NonSerialized] private IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        Coordinate IThingWithCoordinate.Coordinate
        {
            get => AccidentCoordinate;
            set { }
        }

        public virtual decimal? Latitude => AccidentCoordinate?.Latitude;
        public virtual decimal? Longitude => AccidentCoordinate?.Longitude;

        [DoesNotExport]
        public virtual MapIcon Icon => AccidentCoordinate?.Icon;

        [View("Incident Coordinates")]
        public virtual Coordinate AccidentCoordinate { get; set; }

        [View("Incident Street Name")]
        public virtual string AccidentStreetName { get; set; }

        [View("Incident Street Number")]
        public virtual string AccidentStreetNumber { get; set; }

        [View("Incident Town")]
        public virtual Town AccidentTown { get; set; }

        [View("Incident Summary")]
        public virtual string IncidentSummary { get; set; }

        [Multiline]
        public virtual string AnyImmediateCorrectiveActionsApplied { get; set; }

        [View(DisplayNames.ATHLETIC_ACTIVITIES)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string AthleticActivitiesInLastTwelveMonths { get; set; }

        public virtual string CaseManager { get; set; }
        public virtual string CaseNumber { get; set; }
        public virtual string ContractorCompany { get; set; }
        public virtual string ContractorName { get; set; }
        public virtual string Claimant { get; set; }

        [View(FormatStyle.DateTimeWithoutSeconds)]
        public virtual DateTime CreatedAt { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? DateInvestigationWillBeCompleted { get; set; }
        public virtual IncidentDrugAndAlcoholTestingDecision DrugAndAlcoholTestingDecision { get; set; }
        public virtual IncidentDrugAndAlcoholTestingResult DrugAndAlcoholTestingResult { get; set; }

        [View("Testing Notes")]
        public virtual string DrugAndAlcoholTestingNotes { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
        [DoesNotExport]
        public virtual Employee ContractorObservedBy { get; set; }
        public virtual string EmployeeId => Employee?.EmployeeId;

        [View("Reporting Facility")]
        public virtual Facility Facility { get; set; }

        [View(DisplayNames.FIVE_WHYS_COMPLETED, Description = "Must be completed within 3 business days.")]
        public virtual bool FiveWhysCompleted { get; set; }

        public virtual GeneralLiabilityCode GeneralLiabilityCode { get; set; }
        public virtual IncidentClassification IncidentClassification { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? IncidentCommitteeReportCompletionDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? IncidentCommitteeReportTargetCompletionDate { get; set; }

        public virtual string IncidentCommitteeReportResults { get; set; }

        public virtual IncidentShift IncidentShift { get; set; }

        [View("Incident Date/Time", DisplayFormat = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime IncidentDate { get; set; }

        [View("Injury Type")]
        public virtual IncidentType IncidentType { get; set; }

        [View("Event/Exposure Type")]
        public virtual EventExposureType EventExposureType { get; set; }

        [View("Body Part(s)")]
        public virtual IList<BodyPart> BodyParts { get; set; }

        [View("Is Chargeable Motor Vehicle Incident")]
        public virtual bool IsChargeableMotorVehicleAccident { get; set; }

        public virtual bool IsInLitigation { get; set; }

        // IsOvertime is a required field but existing records will have null values.
        public virtual bool? IsOvertime { get; set; }
        public virtual bool IsOSHARecordable { get; set; }
        [View(DisplayNames.IS_NOT_A_SAFETY_ACCOUNTABILITY_PRACTICE)]
        public virtual bool IsSafetyCodeViolation { get; set; }

        [View("SIF")]
        public virtual SeriousInjuryOrFatalityType SeriousInjuryOrFatalityType { get; set; }
       
        public virtual string MarkoutNumber { get; set; }

        // If SoughtMedicalAttention is true, these MedicalProvider fields all must have values.
        public virtual string MedicalProviderName { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string MedicalProviderPhone { get; set; }

        public virtual Town MedicalProviderTown { get; set; }

        public virtual MotorVehicleCode MotorVehicleCode { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string NatureOfPriorInjury { get; set; }

        [View("# of OT Hours worked for previous consecutive days including day of incident")]
        public virtual decimal? NumberOfHoursOvertimeInPastWeek { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [View(DisplayNames.OTHER_EMPLOYERS)]
        public virtual string OtherEmployers { get; set; }

        public virtual bool PoliceReportFiled { get; set; }
        public virtual Position Position { get; set; }
        public virtual string PremiseNumber { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? PriorInjuryDate { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [View(DisplayNames.PRIOR_INJURY_MEDICAL_PROVIDER)]
        public virtual string PriorInjuryMedicalProvider { get; set; }

        [View(DisplayNames.QUESTION_EMPLOYEE_DOING_BEFORE_INCIDENT_OCCURRED)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string QuestionEmployeeDoingBeforeIncidentOccurred { get; set; }

        [View(DisplayNames.QUESTION_WHAT_HAPPENED)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string QuestionWhatHappened { get; set; }

        [View(DisplayNames.QUESTION_INJURY_OR_ILLNESS)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string QuestionInjuryOrIllness { get; set; }

        [View(DisplayNames.QUESTION_OBJECT_OR_SUBSTANCE)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee { get; set; }

        [ExcelExportColumn(UsePropertyName = true)]
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

        [View(DisplayNames.QUESTION_HAVE_INJURED_BEFORE)]
        public virtual bool QuestionHaveHadSimilarInjuryBefore { get; set; }

        [View(DisplayNames.QUESTION_ATHLETICS)]
        public virtual bool QuestionParticipatedInAthleticActivitiesInLastTwelveMonths { get; set; }

        [View(DisplayNames.QUESTION_JOB_OUTSIDE_AMERICAN_WATER)]
        public virtual bool QuestionHaveJobOutsideOfAmericanWater { get; set; }

        public virtual bool SoughtMedicalAttention { get; set; }

        /// <summary>
        /// Gets/sets the supervisor of the employee this incident is about *at the time of the incident*
        /// </summary>
        public virtual Employee Supervisor { get; set; }

        public virtual PersonnelArea PersonnelArea { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        /// <summary>
        /// This may contain more than one person's name.
        /// </summary>
        public virtual string WitnessName { get; set; }

        /// <summary>
        /// This may contain more than one phone number.
        /// </summary>
        public virtual string WitnessPhone { get; set; }

        [View("SAP Work Order")]
        public virtual string WorkOrderId { get; set; }

        [View(DisplayNames.MAP_CALL_WORK_ORDER)]
        public virtual WorkOrder MapCallWorkOrder { get; set; }

        [View("Insurance Carrier Report")]
        public virtual string TravelersReport { get; set; }

        public virtual EmployeeDepartment Department { get; set; }

        public virtual IncidentStatus IncidentStatus { get; set; }

        public virtual AtRiskBehaviorSection AtRiskBehaviorSection { get; set; }
        public virtual AtRiskBehaviorSubSection AtRiskBehaviorSubSection { get; set; }

        [View(DisplayNames.COMPENSATION_STATUS)]
        public virtual WorkersCompensationClaimStatus WorkersCompensationClaimStatus { get; set; }

        [View(DisplayNames.CLAIMS_CARRIER_ID)]
        public virtual string ClaimsCarrierId { get; set; }

        public virtual IList<IncidentInvestigation> IncidentInvestigations { get; set; }

        #region AccountabilityActions

        public virtual IList<EmployeeAccountabilityAction> EmployeeAccountabilityActions { get; set; }

        #endregion

        [SetterProperty]
        public virtual IDateTimeProvider DateTimeProvider
        {
            set => _dateTimeProvider = value;
        }

        #region Nurse stuff

        [View("Did the employee speak to nurse?")]
        public virtual EmployeeSpokeWithNurse EmployeeSpokeWithNurse { get; set; }

        [View("Recommendation by nurse")]
        public virtual IncidentNurseRecommendationType IncidentNurseRecommendationType { get; set; }

        public virtual string RecommendedMedicalProvider { get; set; }

        [View("Non-Medical Treatment Recommendation")]
        public virtual string NonMedicalTreatmentRecommendation { get; set; }

        [View("Did the Employee Accept the Recommendation by Nurse")]
        public virtual bool? EmployeeAcceptedRecommendationByNurse { get; set; }

        public virtual string NursePhone { get; set; }

        [Multiline]
        public virtual string ReasonEmployeeDidNotAcceptRecommendationByNurse { get; set; }

        #endregion

        #region Logical Props

        /// <summary>
        /// Placeholder property to return an Address object for the accident address.
        /// A new instance is returned everytime. This will be replaced to be a correct
        /// db ref. 
        /// </summary>
        public virtual Address AccidentAddress
        {
            get
            {
                var addy = new Address();
                addy.Town = AccidentTown;
                addy.Address1 = AccidentStreetNumber + " " + AccidentStreetName;
                return addy;
            }
        }

        public virtual DateTime UpdatedAt { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITHOUT_SECONDS)]
        public virtual DateTime DateRecorded => CreatedAt;

        [View("Incident Reported Date/Time", DisplayFormat = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime IncidentReportedDate { get; set; }

        public virtual IList<IncidentEmployeeAvailability> EmployeeAvailabilities { get; set; }
        public virtual IList<Document<Incident>> Documents { get; set; }
        public virtual IList<Note<Incident>> Notes { get; set; }
        public virtual IList<ActionItem<Incident>> ActionItems { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual IList<IActionItemLink> LinkedActionItems => ActionItems.Cast<IActionItemLink>().ToList();

        public virtual int NumberOfLostWorkDays
        {
            get
            {
                return EmployeeAvailabilities
                      .Where(x => x.EmployeeAvailabilityType.Id == IncidentEmployeeAvailabilityType.Indices.LOST_TIME)
                      .Sum(x => x.Duration);
            }
        }

        public virtual int NumberOfRestrictiveDutyDays
        {
            get
            {
                return EmployeeAvailabilities.Where(x =>
                                                  x.EmployeeAvailabilityType.Id == IncidentEmployeeAvailabilityType
                                                     .Indices.RESTRICTIVE_DUTY)
                                             .Sum(x => x.Duration);
            }
        }

        public virtual bool LostWorkDay => NumberOfLostWorkDays > 0;

        [DoesNotExport]
        public virtual string TableName => nameof(Incident) + "s";

        #endregion

        #endregion

        #region Constructors

        public Incident()
        {
            Documents = new List<Document<Incident>>();
            Notes = new List<Note<Incident>>();
            EmployeeAvailabilities = new List<IncidentEmployeeAvailability>();
            ActionItems = new List<ActionItem<Incident>>();
            EmployeeAccountabilityActions = new List<EmployeeAccountabilityAction>();
            IncidentInvestigations = new List<IncidentInvestigation>();
            BodyParts = new List<BodyPart>();
        }

        #endregion

        #region Private Methods

        private IEnumerable<IncidentEmployeeAvailability> GetLostWorkDayAvailabilities()
        {
            return EmployeeAvailabilities.Where(x =>
                x.EmployeeAvailabilityType.Id == IncidentEmployeeAvailabilityType.Indices.LOST_TIME);
        }

        private IEnumerable<IncidentEmployeeAvailability> GetRestrictiveDutyAvailabilities()
        {
            return EmployeeAvailabilities.Where(x =>
                x.EmployeeAvailabilityType.Id == IncidentEmployeeAvailabilityType.Indices.RESTRICTIVE_DUTY);
        }

        #endregion

        #region Public Methods

        public virtual int GetLostWorkDaysBetweenDates(DateTime? begin, DateTime end, RangeOperator operatorType)
        {
            switch (operatorType)
            {
                case RangeOperator.LessThan:
                    begin = Convert.ToDateTime("01/01/0001").EndOfDay();
                    break;
                case RangeOperator.LessThanOrEqualTo:
                    begin = Convert.ToDateTime("01/01/0001").BeginningOfDay();
                    break;
                case RangeOperator.GreaterThanOrEqualTo:
                    begin = end;
                    end = _dateTimeProvider.GetCurrentDate().BeginningOfDay();
                    break;
                case RangeOperator.GreaterThan:
                    begin = end;
                    end = _dateTimeProvider.GetCurrentDate().EndOfDay();
                    break;
                case RangeOperator.Equal:
                    begin = end;
                    break;
            }

            var lostWorkDayAvailabilities =
                (GetLostWorkDayAvailabilities().Sum(x => x.GetDurationBetweenDates(begin.Value, end)));
            return lostWorkDayAvailabilities > OSHA_DAYS_CAP ? OSHA_DAYS_CAP : lostWorkDayAvailabilities;
        }

        public virtual int GetCumulativeLostWorkDaysThroughEndDate(DateTime end, RangeOperator operatorType)
        {
            if (operatorType == RangeOperator.GreaterThanOrEqualTo || operatorType == RangeOperator.GreaterThan)
            {
                end = _dateTimeProvider.GetCurrentDate().BeginningOfDay();
            }

            var getLostWorkDayAvailabilities = GetLostWorkDayAvailabilities().Sum(x =>
                x.GetCumulativeDurationThroughEndDdate(_dateTimeProvider.GetCurrentDate()));
            return getLostWorkDayAvailabilities > OSHA_DAYS_CAP ? OSHA_DAYS_CAP : getLostWorkDayAvailabilities;
        }

        public virtual int GetRestrictiveDutyDaysBetweenDates(DateTime? begin, DateTime end, RangeOperator operatorType)
        {
            switch (operatorType)
            {
                case RangeOperator.LessThan:
                    begin = Convert.ToDateTime("01/01/0001").EndOfDay();
                    break;
                case RangeOperator.LessThanOrEqualTo:
                    begin = Convert.ToDateTime("01/01/0001").BeginningOfDay();
                    break;
                case RangeOperator.GreaterThanOrEqualTo:
                    begin = end;
                    end = _dateTimeProvider.GetCurrentDate().BeginningOfDay();
                    break;
                case RangeOperator.GreaterThan:
                    begin = end;
                    end = _dateTimeProvider.GetCurrentDate().EndOfDay();
                    break;
                case RangeOperator.Equal:
                    begin = end;
                    break;
            }

            var getRestrictiveDutyAvailabilities = GetRestrictiveDutyAvailabilities()
               .Sum(x => x.GetDurationBetweenDates(begin.Value, end));
            return getRestrictiveDutyAvailabilities > OSHA_DAYS_CAP ? OSHA_DAYS_CAP : getRestrictiveDutyAvailabilities;
        }

        public virtual bool GetDidIncidentHappenWithinTheRequestRange(DateTime? begin, DateTime end,
            RangeOperator operatorType, DateTime incidentDate)
        {
            incidentDate = incidentDate.AddHours(4);
            switch (operatorType)
            {
                case RangeOperator.LessThan:
                    begin = Convert.ToDateTime("01/01/0001").EndOfDay();
                    end = end.BeginningOfDay();
                    break;
                case RangeOperator.LessThanOrEqualTo:
                    begin = Convert.ToDateTime("01/01/0001").BeginningOfDay();
                    end = end.EndOfDay();
                    break;
                case RangeOperator.GreaterThanOrEqualTo:
                    begin = end.BeginningOfDay();
                    end = _dateTimeProvider.GetCurrentDate().EndOfDay();
                    break;
                case RangeOperator.GreaterThan:
                    begin = end.EndOfDay();
                    end = _dateTimeProvider.GetCurrentDate().BeginningOfDay();
                    break;
                case RangeOperator.Equal:
                    begin = end;
                    end = end.EndOfDay();
                    break;
            }

            var incidentInsideRange = (incidentDate > begin) && (incidentDate < end);
            return incidentInsideRange;
        }

        public virtual int GetCumulativeRestrictiveDutyDaysThroughEndDate(DateTime end, RangeOperator operatorType)
        {
            if (operatorType == RangeOperator.GreaterThanOrEqualTo || operatorType == RangeOperator.GreaterThan)
            {
                end = _dateTimeProvider.GetCurrentDate().EndOfDay();
            }

            var getRestrictiveDutyAvailabilities = GetRestrictiveDutyAvailabilities()
               .Sum(x => x.GetCumulativeDurationThroughEndDdate(end));
            return getRestrictiveDutyAvailabilities > OSHA_DAYS_CAP ? OSHA_DAYS_CAP : getRestrictiveDutyAvailabilities;
        }

        public virtual int GetCumulativeSumOfLostDaysRestrictiveDays(int restrictiveDays, int lostDays)
        {
            var cumulativeSumOfLostDaysRestrictiveDays = lostDays + restrictiveDays;
            return cumulativeSumOfLostDaysRestrictiveDays > OSHA_DAYS_CAP
                ? OSHA_DAYS_CAP
                : cumulativeSumOfLostDaysRestrictiveDays;
        }

        #endregion

        public virtual string ToJSONForActiveMQ()
        {
            return JavaScriptSerializerFactory.Build().Serialize(new {
                Id,
                OperatingCenter = OperatingCenter.ToString(),
                Facility = Facility.ToString(),
                Department = Department?.ToString(),
                Employee = Employee?.ToString(),
                EmployeId = Employee?.Id,
                PersonnelArea = PersonnelArea?.Description.ToString(),
                IncidentStatus = IncidentStatus?.Description,
                IncidentDate,
                DateInvestigationWillBeCompleted,
                IncidentClassification = IncidentClassification?.Description,
                IncidentType = IncidentType?.Description,
                GeneralLiabilityCode = GeneralLiabilityCode?.Description,
                CaseManager,
                SeriousInjuryOrFatalityType,
                IsOSHARecordable,
                AnyImmediateCorrectiveActionsApplied,
                AthleticActivitiesInLastTwelveMonths,
                Latitude = AccidentCoordinate?.Latitude,
                Longitude = AccidentCoordinate?.Longitude,
                AccidentStreetName,
                AccidentStreetNumber,
                AccidentTown = AccidentTown?.ToString(),
                CaseNumber,
                Claimant,
                CreatedOn = CreatedAt,
                CreatedBy,
                DrugAndAlcoholTestingDecision = DrugAndAlcoholTestingDecision?.ToString(),
                DrugAndAlcoholTestingResult = DrugAndAlcoholTestingResult?.ToString(),
                DrugAndAlcoholTestingNotes,
                IncidentCommitteeReportCompletionDate,
                IncidentCommitteeReportTargetCompletionDate,
                IncidentCommitteeReportResults,
                IncidentShift = IncidentShift?.ToString(),
                IsChargeableMotorVehicleAccident,
                IsInLitigation,
                IsOvertime,
                IsSafetyCodeViolation,
                SIF = SeriousInjuryOrFatalityType,
                MarkoutNumber,
                MedicalProviderName,
                MedicalProviderPhone,
                MedicalProviderTown = MedicalProviderTown?.ToString(),
                MotorVehicleCode = MotorVehicleCode?.Description,
                NatureOfPriorInjury,
                NumberOfHoursOvertimeInPastWeek,
                OtherEmployers,
                PoliceReportFiled,
                Position = Position?.ToString(),
                PremiseNumber,
                PriorInjuryDate,
                PriorInjuryMedicalProvider,
                QuestionEmployeeDoingBeforeIncidentOccurred,
                QuestionWhatHappened,
                QuestionInjuryOrIllness,
                QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee,
                QuestionHaveHadSimilarInjuryBefore,
                QuestionParticipatedInAthleticActivitiesInLastTwelveMonths,
                QuestionHaveJobOutsideOfAmericanWater,
                SoughtMedicalAttention,
                Supervisor = Supervisor?.ToString(),
                SupervisorEmployeeId = Supervisor?.EmployeeId,
                Vehicle = Vehicle?.ToString(),
                WitnessName,
                WitnessPhone,
                WorkOrderId,
                TravelersReport,
                AtRiskBehaviorSection = AtRiskBehaviorSection?.ToString(),
                AtRiskBehaviorSubSection = AtRiskBehaviorSubSection?.ToString(),
                //EmployeeSpokeToNurse,
                EmployeeAcceptedRecommendationByNurse,
                NurseRecommendation = IncidentNurseRecommendationType?.ToString(),
                ReasonEmployeeDidNotAcceptRecommendationByNurse,
                NonMedicalTreatmentRecommendation,
                RecommendedMedicalProvider,
                AccidentAddress = AccidentAddress?.ToString(),
                DateRecorded,
                IncidentReportedDate,
                NumberOfLostWorkDays,
                NumberOfRestrictiveDutyDays,
                LostWorkDay,
                LastModifiedDate = UpdatedAt,
                MapCallWorkOrder = MapCallWorkOrder?.ToString()
            });
        }
    }

    [Serializable]
    public class IncidentStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int OPEN = 1, CLOSED_DENIED = 2, CLOSED_ADMINISTRATIVELY_COMPLETE = 3;
        }
    }

    [Serializable]
    public class EmployeeSpokeWithNurse : ReadOnlyEntityLookup { }

    [Serializable]
    public class IncidentNurseRecommendationType : ReadOnlyEntityLookup { }

    [Serializable]
    public class IncidentEmployeeAvailabilityType : ReadOnlyEntityLookup
    {
        #region Structs

        public struct Indices
        {
            public const int LOST_TIME = 1, RESTRICTIVE_DUTY = 2;
        }

        #endregion
    }

    [Serializable]
    public class IncidentEmployeeAvailability : IEntity
    {
        #region Constants

        public const int CapDurationForRestrictiveDutyLostTime = 180; //MC-2312 Cap duration to 180 days

        #endregion

        #region Private Members

        [NonSerialized] private IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual Incident Incident { get; set; }
        public virtual IncidentEmployeeAvailabilityType EmployeeAvailabilityType { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime StartDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? EndDate { get; set; }

        public virtual int Duration
        {
            get
            {
                var end = EndDate ?? _dateTimeProvider.GetCurrentDate();
                // bug-3733 requests that the duration includes the end date day as part of the total.
                if (end.BeginningOfDay().Subtract(StartDate.BeginningOfDay()).Days + 1 >
                    CapDurationForRestrictiveDutyLostTime)
                {
                    return CapDurationForRestrictiveDutyLostTime;
                }

                return end.BeginningOfDay().Subtract(StartDate.BeginningOfDay()).Days + 1;
            }
        }

        [SetterProperty]
        public virtual IDateTimeProvider DateTimeProvider
        {
            set => _dateTimeProvider = value;
        }

        #endregion

        #region Public Methods

        public virtual int GetDurationBetweenDates(DateTime begin, DateTime end)
        {
            var realBegin = (begin > StartDate ? begin : StartDate);
            var possibleEnd = EndDate.GetValueOrDefault(_dateTimeProvider.GetCurrentDate());
            var realEnd = (end < possibleEnd ? end : possibleEnd);

            // Add 1 to the Days value as we need to include the last day in the range as a whole day. The dates being 
            // at midnight will cause it to be short a day.
            return Math.Max(0, (realEnd - realBegin).Days + 1);
        }

        public virtual int GetCumulativeDurationThroughEndDdate(DateTime end)
        {
            // This is from the "Report Changes" file in bug 3542:
            // CUMULATIVE  # of "RD" Days= The Count of Calendar Days  From the ORIGNAL Start of the RD (see below) & the & End Date of the RD. IF the END Date is "empty / null" then count to the END DATE of the Incident Date Range Search 
            var realBegin = StartDate;

            // Bug 3542 "Report Changes" file says the end date value needs to be the searched end date if the end date is not set yet.
            var realEnd = EndDate ?? end;

            // Add 1 to the Days value as we need to include the last day in the range as a whole day. The dates being 
            // at midnight will cause it to be short a day.
            return Math.Max(0, (realEnd - realBegin).Days + 1);
        }

        #endregion
    }

    [Serializable]
    public class IncidentDisplayItem : DisplayItem<Incident>
    {
        public override int Id { get; set; }
        public override string Display => Id.ToString();
    }
}
