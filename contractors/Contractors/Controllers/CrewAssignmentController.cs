using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using Contractors.Controllers.WorkOrder;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace Contractors.Controllers
{
    // BIG STINKIN' NOTE: Many actions here were originally using the Id parameter to reference the Crew. This is
    // a CrewAssignment controller so all the Id params should predictable reference a CrewAssignment. Use the Crew
    // parameter when referring to crews.

    public class CrewAssignmentController : SapControllerWithValidationBase<ICrewAssignmentRepository, CrewAssignment>
    {
        #region Constants

        public const string NO_WORK_ORDER_IDS_CHOSEN = "You must pick at least one work order to assign.",
                            NO_CREW_FOUND = "No such crew.",
                            NO_CREW_ASSIGNMENT_FOUND = "No such crew assignment",
                            INVALID_MARKOUT = "One or more of the work orders chosen does not have a markout that is valid on the scheduled date.",
                            INVALID_PERMIT = "One or more of the work orders chosen does not have a permit that is valid on the scheduled date.";

        public struct ViewDataKeys
        {
            public const string CREW = "Crew";
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This exists solely for the unit tests for this controller. These tests are doing validation testing
        /// rather than view model tests. They need to be rewritten.
        /// </summary>
        /// <param name="model"></param>
        internal void RunModelValidation(object model)
        {
            TryValidateModel(model);
        }

        private void PopulateCommonCalendarViewData()
        {
            this.AddDropDownData(ViewDataKeys.CREW,
                AuthenticationService.CurrentUser.Contractor.Crews,
                c => c.Id.ToString(), c => c.Description);
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

        private RedirectToRouteResult RedirectToFinalization(int workOrderId)
        {
            return RedirectToAction("Edit", "WorkOrderFinalization", new { id = workOrderId });
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

        [HttpPost]
        public ActionResult End(CrewAssignmentEnd model)
        {
            //The current user gets linked by both crew and work order to a given assignment.  
            //The repository security will only check for the crew, so create needs to make 
            // sure that the current user has access to both the given crew and the given order.

            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var workOrderId = Repository.Find(model.Id).WorkOrder.Id;
                    UpdateSAP(workOrderId);
                    return RedirectToFinalization(workOrderId);
                },
                OnNotFound = () => HttpNotFound(),
                OnError = () => {
                    var workOrderId = Repository.Find(model.Id).WorkOrder.Id;
                    return RedirectToFinalization(workOrderId);
                }
            });
        }

        #endregion

        #region Index

        [HttpGet, NoCache]
        public ActionResult Index(int workOrderId)
        {
            return IndexTabs(workOrderId);
        }

        // This is here so the action can be rendered in _Tabs regardless
        // of the action being post/get.
        public ActionResult IndexTabs(int workOrderId)
        {
            var wo = _container.GetInstance<IWorkOrderRepository>().Find(workOrderId);
            if (wo == null)
            {
                return NoSuchWorkOrder();
            }

            var model = _container.GetInstance<CrewAssignmentIndex>();
            model.Map(wo);
            model.IsFinalizationView = ReffererIsWorkOrderFinalization();
            return PartialView("_Index", model);
        }

        #endregion

        #region Start

        [HttpPost]
        public ActionResult Start(CrewAssignmentStart model)
        {
          //  var ca = Repository.Find(model.Id);
          //  var workOrderId = ca.WorkOrder.Id;
            //var model = new CrewAssignmentStart(ca);

            return ActionHelper.DoUpdate(model,
                new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                    OnSuccess = () => {
                        var workOrderId = Repository.Find(model.Id).WorkOrder.Id;
                        UpdateSAP(workOrderId);
                        return RedirectToFinalization(workOrderId);
                    },
                   // OnNotFound = () => HttpNotFound(),
                   OnError = () => {
                       var workOrderId = Repository.Find(model.Id).WorkOrder.Id;
                       return RedirectToFinalization(workOrderId);
                   }
                });
        }

        #endregion

        #region Create

        [HttpPost]
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
                    return RedirectToAction("Index", "WorkOrderScheduling");
                }
            });
        }

        #endregion

        #region Destroy/Delete

        [RequiresAdmin, HttpDelete]
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

        [RequiresAdmin, HttpGet]
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
            model.AssignmentsForDate = GetAllForCrewByDate(crew.Id, model.Date.Value);
            // TODO: Test Remaining is set
            var totalHours = model.AssignmentsForDate.Sum(a => a.WorkOrder.WorkDescription.TimeToComplete);
            model.Remaining = model.Availability - totalHours;
            return View(model);
        }

        #endregion

        #region UpdatePriority

        [RequiresAdmin, HttpPost]
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
                var assignmentsForDate = Repository.GetAllForCrewByDate(model.Crew.Value, model.Date.Value).ToDictionary(x => x.Id, x => x);
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

        public CrewAssignmentController(ControllerBaseWithPersistenceArguments<ICrewAssignmentRepository, CrewAssignment, ContractorUser> args) : base(args) { }
    }
}
