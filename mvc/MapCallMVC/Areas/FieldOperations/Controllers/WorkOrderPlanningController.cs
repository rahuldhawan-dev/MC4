using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderPlanning;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderPrePlanning;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    [DisplayName("Work Order Planning")]
    public class WorkOrderPlanningController
        : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        public const string TRAFFIC_CONTROL_FRAGMENT_IDENTIFIER = "#TrafficControl/NotesTab",
                            RETRY_MESSAGE = "RETRY::UPDATED Planned Completion Date";

        #endregion

        #region Constructors

        public WorkOrderPlanningController(
            ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args)
            : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownDataForRoleAndAction(RoleModules.FieldServicesWorkManagement,
                    extraFilterP: oc => oc.WorkOrdersEnabled);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchWorkOrderPlanning search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    GetEntityOverride = () => Repository.FindPlanningOrder(id)
                }));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, new SearchWorkOrder {
                    Id = id
                }));
                formatter.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true, 
                    ViewName = "_ShowPopup"
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchWorkOrderPlanning search)
        {
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetPlanningWorkOrders(search)
            };
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search, args));
                f.Excel(() => ActionHelper.DoExcel(search, args));
                f.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, () => {
                    search.EnablePaging = false;
                    return Repository.GetPlanningWorkOrders(search);
                }));
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            // This action method exists to allow the Documents tab to enable editing.
            return new EmptyResult();
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(UpdateWorkOrderPlanning model)
        {
            if (model.WorkOrderIds == null)
            {
                return RedirectToReferrerOr("Search", "WorkOrderPlanning");
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

            return RedirectToReferrerOr("Search", "WorkOrderPlanning");
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult UpdatePlanningForTrafficControl(UpdateWorkOrderPlanning model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Show", "WorkOrderPlanning", new { area = "FieldOperations", model.Id }, TRAFFIC_CONTROL_FRAGMENT_IDENTIFIER)
            });
        }

        #endregion
    }
}
