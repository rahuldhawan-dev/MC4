using System;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;
using System.Web.Mvc;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderScheduling;
using MapCallMVC.ClassExtensions;
using System.ComponentModel;
using MMSINC.Data;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using System.Linq;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    [DisplayName("Work Order Scheduling")]
    public class WorkOrderSchedulingController : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        public const int MAX_RESULTS = 1000;

        #endregion

        #region Constructor

        public WorkOrderSchedulingController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownDataForRoleAndAction(RoleModules.FieldServicesWorkManagement,
                    extraFilterP: oc => oc.WorkOrdersEnabled);
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchWorkOrderScheduling());
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter => {
                formatter.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWorkOrderScheduling search)
        {
            search.EnablePaging = false;
            if (string.IsNullOrWhiteSpace(search.SortBy))
            {
                search.SortBy = "Id";
                search.SortAscending = true;
            }
            if (search.MarkoutExpirationDays.HasValue)
            {
                var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                search.ExpirationDate = new DateRange {
                    Start = now,
                    End = now.AddDays(search.MarkoutExpirationDays.Value),
                    Operator = RangeOperator.Between
                };
            }
            return this.RespondTo((formatter) => {
                formatter.View(() =>
                    ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                        MaxResults = MAX_RESULTS,
                        SearchOverrideCallback = () => Repository.GetSchedulingWorkOrders(search),
                        OnSuccess = () => {
                            var model = _container.GetInstance<SchedulingCrewAssignment>();
                            model.Search = search;
                            return View("Index", model);
                        }
                    }));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, () => {
                    search.EnablePaging = false;
                    return Repository.GetSchedulingWorkOrders(search);
                }));
            });
        }

        #endregion

        #region CanBeScheduled

        [HttpGet]
        public ActionResult CanBeScheduled(int id, DateTime assignedFor)
        {
            var result = new CanBeScheduledResult();
            var workOrder = Repository.Find(id);
            var today = _container.GetInstance<IDateTimeProvider>().GetCurrentDate().Date;

            if (!SchedulingCrewAssignment.MarkoutsAreValidForScheduling(workOrder, assignedFor, today))
            {
                result = new CannotBeScheduledResult(CrewAssignment.ModelErrors.INVALID_MARKOUT, id);
            }

            if (workOrder.StreetOpeningPermitRequired &&
                workOrder.Priority.WorkOrderPriorityEnum != WorkOrderPriorityEnum.Emergency &&
                (workOrder.CurrentStreetOpeningPermit?.DateIssued == null ||
                 workOrder.CurrentStreetOpeningPermit?.ExpirationDate == null ||
                 !assignedFor.IsBetween(
                     workOrder.CurrentStreetOpeningPermit.DateIssued.Value,
                     workOrder.CurrentStreetOpeningPermit.ExpirationDate.Value)))
            {
                result = new CannotBeScheduledResult(CrewAssignment.ModelErrors.INVALID_MARKOUT, id);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
