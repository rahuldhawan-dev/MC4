﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.CrewAssignments;
using MMSINC.ClassExtensions;
using MMSINC.Common;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class CrewAssignmentController : ControllerBaseWithPersistence<ICrewAssignmentRepository, CrewAssignment, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        public const string NO_WORK_ORDER_IDS_CHOSEN = "You must pick at least one work order to assign.",
                            NO_CREW_FOUND = "No such crew.",
                            NO_CREW_ASSIGNMENT_FOUND = "No such crew assignment",
                            INVALID_MARKOUT =
                                "One or more of the work orders chosen does not have a markout that is valid on the scheduled date.",
                            INVALID_PERMIT =
                                "One or more of the work orders chosen does not have a permit that is valid on the scheduled date.",
                            SAP_UPDATE_FAILURE = "RETRY::UPDATE FAILURE: ";

        public struct ViewDataKeys
        {
            public const string CREW = "Crew";
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This exists solely for the unit tests for this controller. The tests are doing view model tests
        /// and rely on the model validation to run, which doesn't happen by default.
        ///
        /// If you're having to do this for unit tests, you're likely testing view model validation in the wrong
        /// place.
        /// </summary>
        /// <param name="model"></param>
        internal void RunModelValidation(object model)
        {
            TryValidateModel(model);
        }

        private void PopulateCommonCalendarViewData()
        {
            this.AddDynamicDropDownData<Crew, CrewDisplayItem>(
                dataGetter: r =>
                    r.Linq.Where(c => c.Active && c.OperatingCenter != null)
                     .OrderBy(c => c.OperatingCenter.OperatingCenterCode)
                     .ThenBy(c => c.Description));
        }

        protected void UpdateSAP(int workOrderId)
        {
            var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(workOrderId);
            if (workOrder == null)
                throw new InvalidOperationException($"{workOrderId}: The entity was reported as saved but could not be retrieved. SAP was not updated and the entity could not be marked as such.");
            if (!workOrder.OperatingCenter.SAPEnabled || !workOrder.OperatingCenter.SAPWorkOrdersEnabled || workOrder.OperatingCenter.IsContractedOperations)
                return;
            try
            {
                var sapWorkOrder = _container.GetInstance<ISAPWorkOrderRepository>().Update(new SAPProgressWorkOrder(workOrder));
                workOrder.SAPErrorCode = sapWorkOrder.Status;
            }
            catch (Exception ex)
            {
                workOrder.SAPErrorCode = SAP_UPDATE_FAILURE + ex.Message;
            }
            // workOrder.SapWorkOrderStepId = SAPWorkOrderStep.Indices.UPDATE;
            workOrder.SAPWorkOrderStep = _container.GetInstance<IRepository<SAPWorkOrderStep>>().Find(SAPWorkOrderStep.Indices.UPDATE);
            _container.GetInstance<IWorkOrderRepository>().Save(workOrder);
            SendSapErrorNotification(workOrder, workOrderId);
        }

        protected void SendSapErrorNotification(ISAPEntity entity, int workOrderId)
        {
            if (!entity.SAPErrorCode.StartsWith("RETRY") && !entity.SAPErrorCode.ToUpper().Contains("SUCCESSFUL"))
            {
                using (var mm = _container.GetInstance<IMailMessageFactory>().Build())
                {
                    mm.Subject = "Contractors - Work Order - SAP Error";
                    mm.To.Add(!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AllEmailsGoTo"]) ? ConfigurationManager.AppSettings["AllEmailsGoTo"] : "mapcall@amwater.com");
                    mm.Sender = new MailAddress(ConfigurationManager.AppSettings[NotifierBase.FROM_ADDRESS_KEY]);
                    mm.Body = "Work Order: " + new SAPEntity().GetShowUrl("WorkOrder", workOrderId) + Environment.NewLine;
                    mm.Body += "Contractor Email: " + AuthenticationService.CurrentUser.Email;
                    using (var sc = _container.GetInstance<ISmtpClientFactory>().Build())
                    {
                        sc.Send(mm);
                    }
                }
            }
        }

        private void InitializeSearchViewModel(CrewAssignmentCalendarSearch model)
        {
            if (ModelState.IsValid && model.Crew.HasValue && model.Date.HasValue)
            {
                model.AvailabilityPercentagesForMonth = Repository.GetCrewTimePercentagesByMonth(model.Crew.Value, model.Date.Value);
            }
            else
            {
                model.AvailabilityPercentagesForMonth = new Dictionary<DateTime, decimal>();
            }
        }

        private Crew GetCrew(int crewId)
        {
            return _container
                .GetInstance<IRepository<Crew>>()
                .Find(crewId);
        }

        private HttpNotFoundResult NoCrewFound()
        {
            return HttpNotFound(NO_CREW_FOUND);
        }

        private HttpNotFoundResult NoCrewAssignmentFound()
        {
            return HttpNotFound(NO_CREW_ASSIGNMENT_FOUND);
        }

        private IEnumerable<CrewAssignment> GetAllForCrewByDate(int crewId, DateTime date)
        {
            return Repository.GetAllForCrewByDate(crewId, date).OrderBy(x => x.Priority);
        }

        private IEnumerable<CrewAssignment> GetUnstartedForCrewByDate(int crewId, DateTime date)
        {
            return Repository.GetAllForCrewByDate(crewId, date).Where(x => x.DateStarted == null).OrderBy(x => x.Priority);
        }

        private bool UrlIsWorkOrderFinalization(Uri uri)
        {
            var referrer = RouteTable.Routes.GetRouteData(uri);
            var action = referrer.Values["action"];
            var controller = referrer.Values["controller"];
            // Using this as a shortcut for checking for nulls and calling ToString a bunch.
            return
                String.Equals("WorkOrderFinalization", (string)controller,
                    StringComparison.InvariantCultureIgnoreCase) &&
                        string.Equals("Edit", (string)action,
                            StringComparison.InvariantCultureIgnoreCase);
        }

        private bool ReffererIsWorkOrderFinalization()
        {
            // If our request.url is finalization(because the tabs
            // are calling Html.Action) then let's go with that first.
            if (UrlIsWorkOrderFinalization(Request.Url))
            {
                return true;
            }

            // No referrer? Crazy talk!
            if (Request.UrlReferrer == null)
            {
                return false;
            }
            // If this is an ajax request, the referrer would come
            // from the finalization page.
            return UrlIsWorkOrderFinalization(Request.UrlReferrer);
        }

        #endregion

        #region Calendar

        [HttpGet, NoCache]
        [RequiresRole(ROLE)]
        public ActionResult ShowCalendar(CrewAssignmentCalendarSearch model)
        {
            model.Date = model.Date.GetValueOrDefault(_container.GetInstance<IDateTimeProvider>().GetCurrentDate().Date); // Date must be Today.

            if (model.Crew.HasValue && GetCrew(model.Crew.Value) != null)
            {
                model.AssignmentsForDate = GetAllForCrewByDate(model.Crew.Value, model.Date.Value);
            }

            PopulateCommonCalendarViewData();

            InitializeSearchViewModel(model);
            return View(model);
        }

        #endregion

        #region End
        
        [RequiresRole(ROLE)]
        public ActionResult End(CrewAssignmentEnd model)
        {
            //The current user gets linked by both crew and work order to a given assignment.  
            //The repository security will only check for the crew, so create needs to make 
            // sure that the current user has access to both the given crew and the given order.

            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
            {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    UpdateSAP(entity.WorkOrder.Id);
                    return RedirectToAction("Edit", "WorkOrderFinalization",
                        new { area = "FieldOperations", id = entity.WorkOrder.Id });
                },
                OnNotFound = () => HttpNotFound(),
                OnError = () => {
                    var entity = Repository.Find(model.Id);
                    return RedirectToAction("Edit", "WorkOrderFinalization",
                        new { area = "FieldOperations", id = entity.WorkOrder.Id });
                }
            });
        }

        #endregion

        #region Index

        [HttpGet, NoCache, ActionBarVisible(false)]
        [RequiresRole(ROLE)]
        public ActionResult Index(int workOrderId)
        {
            return IndexTabs(workOrderId);
        }

        // This is here so the action can be rendered in _Tabs regardless
        // of the action being post/get.
        [RequiresRole(ROLE)]
        public ActionResult IndexTabs(int workOrderId)
        {
            var wo = _container.GetInstance<IWorkOrderRepository>().Find(workOrderId);
            if (wo == null)
            {
                return HttpNotFound("No such work order.");
            }

            var model = _container.GetInstance<CrewAssignmentIndex>();
            model.Map(wo);
            model.IsFinalizationView = ReffererIsWorkOrderFinalization();
            return PartialView("_Index", model);
        }

        #endregion

        #region Start

        [HttpPost]
        [RequiresRole(ROLE)]
        public ActionResult Start(CrewAssignmentStart model)
        {
            //  var ca = Repository.Find(model.Id);
            //  var workOrderId = ca.WorkOrder.Id;
            //var model = new CrewAssignmentStart(ca);

            return ActionHelper.DoUpdate(model,
                new MMSINC.Utilities.ActionHelperDoUpdateArgs
                {
                    OnSuccess = () => {
                        var entity = Repository.Find(model.Id);
                        UpdateSAP(entity.WorkOrder.Id);

                        return RedirectToAction("New", "JobSiteCheckList", new { area = "HealthAndSafety", workOrderId = entity.WorkOrder.Id });
                    },
                    // OnNotFound = () => HttpNotFound(),
                    OnError = () => {
                        var entity = Repository.Find(model.Id);
                        return RedirectToAction("Edit", "WorkOrderFinalization",
                            new { area = "FieldOperations", id = entity.WorkOrder.Id });
                    }
                });
        }

        #endregion

        #region Create

        [RequiresRole(ROLE, RoleActions.Add), HttpPost]
        public ActionResult Create(SchedulingCrewAssignment model)
        {
            return ActionHelper.DoCreateForViewModelSet(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    foreach (var workOrderID in model.WorkOrderIDs)
                    {
                        UpdateSAP(workOrderID);
                    }

                    return RedirectToAction("ShowCalendar", new {
                        Crew = model.Crew,
                        // The date needs to be string formatted specifically for a regression test 
                        // that fails. Using a regular Date object ends up with a querystring parameter
                        // formatted with the time since the url generator knows nothing about our 
                        // value formatting. 
                        Date = model.AssignFor.Value.ToString("d")
                    });
                },
                OnError = () => {
                    DisplayModelStateErrors();
                    return RedirectToAction("Search", "WorkOrderScheduling");
                }
            });
        }

        #endregion

        #region Destroy/Delete

        [RequiresRole(ROLE, RoleActions.Delete), HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs
            {
                OnSuccess = () => this.HttpStatusCode(HttpStatusCode.NoContent),
                OnError = () => NoCrewAssignmentFound()
            });
        }

        #endregion

        #region Manage

        [RequiresRole(ROLE, RoleActions.Edit), HttpGet]
        public ActionResult Manage(CrewAssignmentManage model)
        {
            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();
                return NoCrewFound();
            }

            var crew = GetCrew(model.Crew.GetValueOrDefault());
            model.Availability = crew.Availability;
            model.CrewDescription = crew.Description;
            model.AssignmentsForDate = GetUnstartedForCrewByDate(crew.Id, model.Date.Value);
            // TODO: Test Remaining is set
            var totalHours = model.AssignmentsForDate.Sum(a => a.WorkOrder.WorkDescription.TimeToComplete);
            model.Remaining = model.Availability - totalHours;
            return View(model);
        }

        #endregion

        #region UpdatePriority

        [RequiresRole(ROLE, RoleActions.Edit), HttpPost]
        public ActionResult UpdatePriority(CrewAssignmentPriorityUpdate model)
        {
            // TODO: Why does this throw an exception instead of returning proper validation messages ot the user?
            // TODO: Redirect this back to the Manage page with the proper validation results.
            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();
                return NoCrewFound();
            }

            // TODO: Can this be done properly from a ViewModel? 
            if (model.CrewAssignments.Any())
            {
                var assignmentsForDate = GetUnstartedForCrewByDate(model.Crew.Value, model.Date.Value).ToDictionary(x => x.Id, x => x);
                if (assignmentsForDate.Count != model.CrewAssignments.Count() || assignmentsForDate.Keys.Except(model.CrewAssignments).Any())
                {
                    DisplayErrorMessage("The crew assignments sent to the server were out of date. Please try again.");
                    return RedirectToAction("Manage", new { Crew = model.Crew, date = model.Date });
                }

                for (var i = 0; i < model.CrewAssignments.Count(); i++)
                {
                    assignmentsForDate[model.CrewAssignments[i]].Priority = i + 1;
                }

                foreach (var ass in assignmentsForDate.Values)
                {
                    Repository.Save(ass);
                }
            }

            // Let's go back to the calendar page when we're all done.
            return RedirectToAction("ShowCalendar", new
            {
                Crew = model.Crew,
                // The date needs to be string formatted specifically for a regression test 
                // that fails. Using a regular Date object ends up with a querystring parameter
                // formatted with the time since the url generator knows nothing about our 
                // value formatting. 
                Date = model.Date.Value.ToString("d")
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<CrewAssignment, EditCrewAssignment>() {
                IsPartial = true,
                ViewName = "_Edit"
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditCrewAssignment model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs() {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home"),
                OnError = () => {
                    DisplayModelStateErrors();
                    return RedirectToReferrerOr("Index", "Home");
                }
            });
        }

        #endregion

        public CrewAssignmentController(ControllerBaseWithPersistenceArguments<ICrewAssignmentRepository, CrewAssignment, User> args) : base(args) { }
    }
}
