using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallApi.Controllers
{
    public class IncidentController : ControllerBaseWithPersistence<IIncidentRepository, Incident, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.OperationsIncidents;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchIncident model)
        {
            // This has been temporarily removed because there are now more than one search field and also there's
            // only like 400 incident records. The tests that tested this are also commented out. -Ross 8/30/2017

            //if (model.IncidentDate == null || model.IncidentDate.Operator != RangeOperator.Between ||
            //    model.IncidentDate.End == null || model.IncidentDate.End.Value
            //        .Subtract(model.IncidentDate.Start.Value).TotalDays > 30)
            //{
            //    throw new InvalidOperationException(
            //        "IncidentDateTime must be a 'between' search of a month or less.");
            //}
            model.EnablePaging = false;
            return Json(
                // TODO: We should consider taking the thing that does Excel exports and having it work for json stuff too.
                //       It already flattens this stuff down and would mean not having to write the below select code.
                Repository.Search(model).Select(i => new {
                    i.Id,
                    OperatingCenter = i.OperatingCenter.ToString(),
                    Facility = i.Facility.ToString(),
                    Department = i.Department?.ToString(),
                    Employee = i.Employee?.ToString(),
                    EmployeId = i.Employee?.Id, 
                    IncidentStatus = i.IncidentStatus?.Description,
                    i.IncidentDate,
                    i.DateInvestigationWillBeCompleted,
                    IncidentClassification = i.IncidentClassification?.Description,
                    IncidentType = i.IncidentType?.Description,
                    GeneralLiabilityCode = i.GeneralLiabilityCode?.Description,
                    i.CaseManager,
                    i.SeriousInjuryOrFatalityType,
                    i.IsOSHARecordable,
                    i.AnyImmediateCorrectiveActionsApplied,
                    i.AthleticActivitiesInLastTwelveMonths,
                    Latitude = i.AccidentCoordinate?.Latitude,
                    Longitude = i.AccidentCoordinate?.Longitude,
                    i.AccidentStreetName,
                    i.AccidentStreetNumber,
                    AccidentTown = i.AccidentTown?.ToString(),
                    i.CaseNumber,
                    i.Claimant,
                    CreatedOn = i.CreatedAt,
                    i.CreatedBy,
                    DrugAndAlcoholTestingDecision = i.DrugAndAlcoholTestingDecision?.ToString(),
                    DrugAndAlcoholTestingResult = i.DrugAndAlcoholTestingResult?.ToString(),
                    i.DrugAndAlcoholTestingNotes,
                    i.IncidentCommitteeReportCompletionDate,
                    i.IncidentCommitteeReportTargetCompletionDate,
                    i.IncidentCommitteeReportResults,
                    IncidentShift = i.IncidentShift?.ToString(),
                    i.IsChargeableMotorVehicleAccident,
                    i.IsInLitigation,
                    i.IsOvertime,
                    i.IsSafetyCodeViolation,
                    SIF = i.SeriousInjuryOrFatalityType,
                    i.MarkoutNumber,
                    i.MedicalProviderName,
                    i.MedicalProviderPhone,
                    MedicalProviderTown = i.MedicalProviderTown?.ToString(),
                    MotorVehicleCode = i.MotorVehicleCode?.Description,
                    i.NatureOfPriorInjury,
                    i.NumberOfHoursOvertimeInPastWeek,
                    i.OtherEmployers,
                    i.PoliceReportFiled,
                    Position = i.Position?.ToString(),
                    i.PremiseNumber,
                    i.PriorInjuryDate,
                    i.PriorInjuryMedicalProvider,
                    i.QuestionEmployeeDoingBeforeIncidentOccurred,
                    i.QuestionWhatHappened,
                    i.QuestionInjuryOrIllness,
                    i.QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee,
                    i.QuestionHaveHadSimilarInjuryBefore,
                    i.QuestionParticipatedInAthleticActivitiesInLastTwelveMonths,
                    i.QuestionHaveJobOutsideOfAmericanWater,
                    i.SoughtMedicalAttention,
                    Supervisor = i.Supervisor?.ToString(),
                    SupervisorEmployeeId = i.Supervisor?.EmployeeId,
                    Vehicle = i.Vehicle?.ToString(),
                    i.WitnessName,
                    i.WitnessPhone,
                    i.WorkOrderId,
                    i.TravelersReport,
                    AtRiskBehaviorSection = i.AtRiskBehaviorSection?.ToString(),
                    AtRiskBehaviorSubSection = i.AtRiskBehaviorSubSection?.ToString(),
                    i.EmployeeSpokeWithNurse,
                    i.EmployeeAcceptedRecommendationByNurse,
                    NurseRecommendation = i.IncidentNurseRecommendationType?.ToString(),
                    i.ReasonEmployeeDidNotAcceptRecommendationByNurse,
                    i.NursePhone,
                    i.NonMedicalTreatmentRecommendation,
                    i.RecommendedMedicalProvider,
                    AccidentAddress = i.AccidentAddress?.ToString(),
                    i.DateRecorded,
                    i.IncidentReportedDate,
                    i.NumberOfLostWorkDays,
                    i.NumberOfRestrictiveDutyDays,
                    i.LostWorkDay,
                    LastModifiedDate = i.UpdatedAt
                }), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public IncidentController(ControllerBaseWithPersistenceArguments<IIncidentRepository, Incident, User> args) : base(args) { }
    }
}