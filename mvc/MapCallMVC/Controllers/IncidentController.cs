using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Controllers
{
    public class IncidentController : ControllerBaseWithPersistence<IIncidentRepository, Incident, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.OperationsIncidents;
        public const string HS_INCIDENT_NOTIFICATION_PURPOSE = "HS Incident";
        public const string HS_INCIDENT_NOTIFICATION_PURPOSE_CLASSIFICATION = "HS Incident Classification Updated";
        public const string HS_INCIDENT_OSHA_RECORDABLE_NOTIFICATION_PURPOSE = "HS Incident OSHA Recordable";
        public const string INVESTIGATIONS_TAB_FRAGMENT = "#InvestigationsTab";

        #endregion

        #region Constructor

        public IncidentController(ControllerBaseWithPersistenceArguments<IIncidentRepository, Incident, User> args) :
            base(args) { }

        #endregion

        #region Private Methods

        private IEnumerable<Incident> GetSearchResultsWithoutPaging(SearchIncident model)
        {
            model.EnablePaging = false;
            return Repository.Search(model);
        }

        private void SendCreationsMostBodaciousNotification(Incident model, string notificationPurpose)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE_MODULE,
                Purpose = notificationPurpose,
                Data = model
            };

            var pdfResult = new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", model);
            var pdf = pdfResult.RenderPdfToBytes(ControllerContext);

            args.AddAttachment(String.Format("Incident{0}.pdf", model.Id), pdf);
            notifier.Notify(args);
        }

        private static ChartResult ToChartResult(Dictionary<string, int> data, string title)
        {
            var chart = new ChartBuilder<string, int>();
            chart.Title = title;
            // Series name doesn't matter because this is a SingleSeriesBar chart.
            chart.AddSeriesValues(title, data);
            chart.Type = ChartType.SingleSeriesBar;
            chart.SortType = ChartSortType.Alphabetical;
            chart.LegendPosition = ChartLegendPosition.None;
            chart.YMinValue = 0;
            return new ChartResult(chart);
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public ActionResult ByOperatingCenter(int townId)
        {
            return new CascadingActionResult(
                Repository.GetByOperatingCenter(townId).Select(x => new { Id = x.Id, Description = x.ToString() }),
                "Description", "Id");
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            Action newEditData = () => {
                var states = _container.GetInstance<IStateRepository>().GetAllSorted().ToArray();
                this.AddDropDownData("MedicalProviderState", states, state => state.Id, state => state.ToString());
                this.AddDropDownData("AccidentState", states, state => state.Id, state => state.ToString());
            };

            switch (action)
            {
                case ControllerAction.Show:
                    this.AddDropDownData<IncidentEmployeeAvailabilityType>("EmployeeAvailabilityType");
                    break;

                case ControllerAction.New:
                    newEditData();
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE_MODULE, RoleActions.Add,
                        extraFilterP: x => x.IsActive);
                    break;

                case ControllerAction.Edit:
                    newEditData();
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("Supervisor");
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE_MODULE, RoleActions.Edit);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchIncident model)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(model));
                formatter.Excel(() => {
                    model.EnablePaging = false;
                    var roleService = _container.GetInstance<IRoleService>();
                    var results = Repository.Search(model).Select(x => new {
                        x.Id,
                        x.Latitude,
                        x.Longitude,
                        x.AccidentCoordinate,
                        x.AccidentStreetName,
                        x.AccidentStreetNumber,
                        x.AccidentTown,
                        x.IncidentSummary,
                        x.AnyImmediateCorrectiveActionsApplied,
                        x.AthleticActivitiesInLastTwelveMonths,
                        x.CaseManager,
                        x.CaseNumber,
                        x.Claimant,
                        CreatedOn = x.CreatedAt,
                        x.CreatedBy,
                        x.DateInvestigationWillBeCompleted,
                        DrugAndAlcoholTestingDecision =
                            roleService.CanAccessRole(RoleModules.OperationsIncidentsDrugTesting, RoleActions.Read,
                                x.OperatingCenter)
                                ? x.DrugAndAlcoholTestingDecision
                                : null,
                        DrugAndAlcoholTestingResult =
                            roleService.CanAccessRole(RoleModules.OperationsIncidentsDrugTesting, RoleActions.Read,
                                x.OperatingCenter)
                                ? x.DrugAndAlcoholTestingResult
                                : null,
                        DrugAndAlcoholTestingNotes =
                            roleService.CanAccessRole(RoleModules.OperationsIncidentsDrugTesting, RoleActions.Read,
                                x.OperatingCenter)
                                ? x.DrugAndAlcoholTestingNotes
                                : null,
                        x.Employee,
                        x.EmployeeId,
                        x.Facility,
                        x.FiveWhysCompleted,
                        x.GeneralLiabilityCode,
                        x.IncidentClassification,
                        x.IncidentCommitteeReportCompletionDate,
                        x.IncidentCommitteeReportTargetCompletionDate,
                        x.IncidentCommitteeReportResults,
                        x.IncidentShift,
                        x.IncidentDate,
                        x.IncidentType,
                        x.EventExposureType,
                        x.IsChargeableMotorVehicleAccident,
                        x.IsInLitigation,
                        x.IsOvertime,
                        x.IsOSHARecordable,
                        x.IsSafetyCodeViolation,
                        x.SeriousInjuryOrFatalityType,
                        x.MarkoutNumber,
                        x.MedicalProviderName,
                        x.MedicalProviderPhone,
                        x.MedicalProviderTown,
                        x.MotorVehicleCode,
                        x.NatureOfPriorInjury,
                        x.NumberOfHoursOvertimeInPastWeek,
                        x.OperatingCenter,
                        x.OtherEmployers,
                        x.PoliceReportFiled,
                        x.Position,
                        x.PremiseNumber,
                        x.PriorInjuryDate,
                        x.PriorInjuryMedicalProvider,
                        x.QuestionEmployeeDoingBeforeIncidentOccurred,
                        x.QuestionWhatHappened,
                        x.QuestionInjuryOrIllness,
                        x.QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee,
                        x.Why1,
                        x.Why2,
                        x.Why3,
                        x.Why4,
                        x.Why5,
                        x.QuestionHaveHadSimilarInjuryBefore,
                        x.QuestionParticipatedInAthleticActivitiesInLastTwelveMonths,
                        x.QuestionHaveJobOutsideOfAmericanWater,
                        x.SoughtMedicalAttention,
                        x.Supervisor,
                        x.PersonnelArea,
                        x.Vehicle,
                        x.WitnessName,
                        x.WitnessPhone,
                        x.WorkOrderId,
                        x.MapCallWorkOrder,
                        x.TravelersReport,
                        x.Department,
                        x.IncidentStatus,
                        x.AtRiskBehaviorSection,
                        x.AtRiskBehaviorSubSection,
                        x.EmployeeSpokeWithNurse,
                        x.IncidentNurseRecommendationType,
                        x.RecommendedMedicalProvider,
                        x.NonMedicalTreatmentRecommendation,
                        x.EmployeeAcceptedRecommendationByNurse,
                        x.NursePhone,
                        x.AccidentAddress,
                        LastModifiedDate = x.UpdatedAt,
                        x.DateRecorded,
                        x.IncidentReportedDate,
                        x.NumberOfLostWorkDays,
                        x.NumberOfRestrictiveDutyDays,
                        x.LostWorkDay,
                        x.WorkersCompensationClaimStatus,
                        x.ClaimsCarrierId
                    });
                    return this.Excel(results);
                });
                formatter.Json(() => {
                    // TODO: Is this still used? Isn't this now down in the API project instead?

                    if (model.IncidentDate == null || model.IncidentDate.Operator != RangeOperator.Between ||
                        model.IncidentDate.End == null || model.IncidentDate.End.Value
                                                               .Subtract(model.IncidentDate.Start.Value).TotalDays > 30)
                    {
                        // NOTE: This error is useless to the people calling this method if it's being used as an API.
                        // They won't know what the error is. It should be sent back as a validation error. -Ross 7/13/2018
                        throw new InvalidOperationException(
                            "IncidentDateTime must be a 'between' search of a month or less.");
                    }

                    var results = GetSearchResultsWithoutPaging(model);
                    return Json(new {
                        Data = results.Select(i => new {
                            i.Id,
                            OperatingCenter = i.OperatingCenter.ToString(),
                            Facility = i.Facility.ToString(),
                            Department = i.Department?.ToString(),
                            Employee = i.Employee?.ToString(),
                            IncidentStatus = i.IncidentStatus?.Description,
                            i.IncidentDate,
                            i.DateInvestigationWillBeCompleted,
                            IncidentClassification = i.IncidentClassification?.Description,
                            IncidentType = i.IncidentType?.Description,
                            GeneralLiabilityCode = i.GeneralLiabilityCode?.Description,
                            i.CaseManager,
                            i.SeriousInjuryOrFatalityType,
                            i.IsOSHARecordable,
                            i.AnyImmediateCorrectiveActionsApplied
                        })
                    }, JsonRequestBehavior.AllowGet);
                });
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, model));
            });
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Pdf(() => ActionHelper.DoPdf(id));
                x.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true
                }));
                x.Map(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                    {
                        return HttpNotFound();
                    }

                    return _container.With((IEnumerable<IThingWithCoordinate>)new[] { model })
                                     .GetInstance<MapResultWithCoordinates>();
                });
            });
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchIncident>();
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult New()
        {
            var createViewModel = ViewModelFactory.BuildWithOverrides<CreateIncident>(new {
                WorkersCompensationClaimStatus = WorkersCompensationClaimStatus.Indices.NO_CLAIM
            });

            return ActionHelper.DoNew(createViewModel);
        }

        [HttpPost]
        [RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult Create(CreateIncident model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => { return SendNotification(model.Id, model.SendOSHANotification); },
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditIncident>(id);
        }

        [HttpPost]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Update(EditIncident model)
        {
            var originalEntity = Repository.Find(model.Id);
            var sendClassification = model.IncidentClassification != originalEntity?.IncidentClassification?.Id;

            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    // We don't send out the regular notification for edits except for when IsOSHARecordable becomes true.
                    if (model.SendOSHANotification)
                    {
                        return SendNotification(model.Id, model.SendOSHANotification);
                    }

                    if (sendClassification)
                    {
                        var entity = Repository.Find(model.Id);
                        SendCreationsMostBodaciousNotification(entity, HS_INCIDENT_NOTIFICATION_PURPOSE_CLASSIFICATION);
                    }

                    return DoRedirectionToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        [RequiresRole(ROLE_MODULE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        [RequiresAdmin]
        public ActionResult SendNotification(int id, bool sendOSHANotification)
        {
            // This is marked as RequiresAdmin for requests coming in explicitly to
            // this action. Create is not RequiresAdmin but needs to send a notification
            // out. RequiresAdmin won't prevent that, so that's a good thing.

            var entity = Repository.Find(id);
            if (entity == null)
            {
                DisplayErrorMessage($"A notification for incident #{id} can not be sent because it either does not exist or you do not have permission to access that incident.");
                return RedirectToAction("Index");
            }

            if (sendOSHANotification)
            {
                SendCreationsMostBodaciousNotification(entity, HS_INCIDENT_OSHA_RECORDABLE_NOTIFICATION_PURPOSE);
            }
            else
            {
                SendCreationsMostBodaciousNotification(entity, HS_INCIDENT_NOTIFICATION_PURPOSE);
            }

            DisplaySuccessMessage("Notification sent successfully!");

            return DoRedirectionToAction("Show", new { id = id });
        }

        #endregion

        #region ChartIncidentClassifications

        [HttpGet, NoCache, RequiresRole(ROLE_MODULE)]
        public ActionResult ChartIncidentClassifications(SearchIncident search)
        {
            var groupedIncidents = GetSearchResultsWithoutPaging(search)
                                  .GroupBy(x => x.IncidentClassification)
                                  .ToDictionary(x => x.Key.Description, x => x.Count());

            return ToChartResult(groupedIncidents, "Incident Classification Breakdown");
        }

        #endregion

        #region ChartIncidentTypes

        [HttpGet, NoCache, RequiresRole(ROLE_MODULE)]
        public ActionResult ChartIncidentTypes(SearchIncident search)
        {
            // I have no idea why there's a null check in ToDictionary, it shouldn't be getting nulls
            // in the first place. I added it and I still don't know why it's there. -Ross 9/10/2015
            var groupedIncidents = GetSearchResultsWithoutPaging(search).Where(x => x.IncidentType != null)
                                                                        .GroupBy(x => x.IncidentType)
                                                                        .ToDictionary(
                                                                             x => (x.Key != null
                                                                                 ? x.Key.Description
                                                                                 : "No Value"), x => x.Count());

            return ToChartResult(groupedIncidents, "Incident Type Breakdown");
        }

        #endregion

        #region ChartIncidentTypes

        [HttpGet, NoCache, RequiresRole(ROLE_MODULE)]
        public ActionResult ChartAtRiskBehaviors(SearchIncident search)
        {
            var groupedIncidents = GetSearchResultsWithoutPaging(search).Where(x => x.AtRiskBehaviorSection != null)
                                                                        .GroupBy(x => x.AtRiskBehaviorSection)
                                                                        .ToDictionary(x => x.Key.Description,
                                                                             x => x.Count());

            return ToChartResult(groupedIncidents, "At Risk Behavior Section Breakdown");
        }

        #endregion

        #region IncidentEmployeeAvailability

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult AddIncidentEmployeeAvailability(AddIncidentEmployeeAvailability model)
        {
            var args = new ActionHelperDoUpdateArgs();
            args.OnError = () => {
                DisplayModelStateErrors();
                return RedirectToAction("Show", new { id = model.Id });
            };
            return ActionHelper.DoUpdate(model, args);
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult RemoveIncidentEmployeeAvailability(RemoveIncidentEmployeeAvailability model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region IncidentInvestigation

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult RemoveIncidentInvestigation(RemoveIncidentInvestigation model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Show", "Incident", new { area = string.Empty, Id = model.Id },
                    INVESTIGATIONS_TAB_FRAGMENT),
                OnError = () => RedirectToReferrerOr("Show", "Incident", new { area = string.Empty, Id = model.Id },
                    INVESTIGATIONS_TAB_FRAGMENT),
            });
        }

        #endregion

        #region IncidentAccountabilityAction

        [HttpGet]
        public ActionResult ByEmployeeId(int id)
        {
            var results = Repository.GetByEmployeeId(id);
            return new CascadingActionResult(results, "Id", "Id") { SortItemsByTextField = false };
        }

        #endregion
    }
}
