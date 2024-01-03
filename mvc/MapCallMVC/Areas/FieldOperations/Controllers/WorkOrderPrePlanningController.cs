using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderPrePlanning;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    [DisplayName("Work Order Pre Planning")]
    public class WorkOrderPrePlanningController
        : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = WorkOrderController.ROLE;
        public const int MAX_RESULTS = 1000;
        public const string
            RETRY_MESSAGE = "RETRY::UPDATED Planned Completion Date",
            STREET_PERMIT_NOTIFICATION_MESSAGE = "Orders with no permit data cannot be selected for assigning";

        #endregion

        #region Constructor

        public WorkOrderPrePlanningController(
            ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args)
            : base(args) { }

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
            return ActionHelper.DoSearch(new SearchWorkOrderPrePlanning());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                GetEntityOverride = () => Repository.FindPrePlanningOrder(id)
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWorkOrderPrePlanning search)
        {
            // TODO: Is there a reason we limit max results for the page but not the excel export?
            search.EnablePaging = false;
            return this.RespondTo((formatter) => {
                formatter.View(() =>
                    ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                        MaxResults = MAX_RESULTS,
                        SearchOverrideCallback = () => Repository.GetPrePlanningWorkOrders(search),
                        OnSuccess = () => {
                            DisplayNotification(STREET_PERMIT_NOTIFICATION_MESSAGE);
                            return null; // defer to default.
                        }
                    }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Assign(AssignWorkOrderPrePlanning assignment)
        {
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            if (assignment.WorkOrderIds == null)
            {
                return RedirectToReferrerOr("Search", "WorkOrderPrePlanning");
            }

            foreach (var id in assignment.WorkOrderIds)
            {
                var order = Repository.FindPrePlanningOrder(id);

                if (assignment.AssignedTo.HasValue)
                {
                    order.OfficeAssignment = new User {Id = assignment.AssignedTo.Value};
                    order.OfficeAssignedOn = now;
                }
                else
                {
                    order.AssignedContractor = new Contractor {Id = assignment.ContractorAssignedTo.Value};
                    order.AssignedToContractorOn = now;
                }

                Repository.Save(order);
            }

            return RedirectToReferrerOr("Search", "WorkOrderPrePlanning");
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(UpdateWorkOrderPrePlanning model)
        {
            if (model.WorkOrderIds == null)
            {
                return RedirectToReferrerOr("Search", "WorkOrderPrePlanning");
            }

            var orders = new List<WorkOrder>();
            foreach (var id in model.WorkOrderIds)
            {
                var order = Repository.Find(id);
                order.PlannedCompletionDate = model.PlannedCompletionDate;
                order.SAPErrorCode = RETRY_MESSAGE;
                orders.Add(order);
            }

            if (orders.Count > 0)
            {
                Repository.Save(orders);
            }

            return RedirectToReferrerOr("Search", "WorkOrderPrePlanning");
        }
    }
}
