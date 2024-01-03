using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class MarkoutPlanningController : ControllerBaseWithPersistence<IGeneralWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const string WORK_ORDER_NOT_FOUND = "Work Order not found.";

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchWorkOrder search)
        {
            return ActionHelper.DoSearch(search);
        }
        
        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchWorkOrder search)
        {
            var args = new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.SearchMarkoutPlanningOrders(search)
            };
            return ActionHelper.DoIndex(search, args); 
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<WorkOrder, EditMarkoutPlanning> {
                IsPartial = true,
                NotFound = WORK_ORDER_NOT_FOUND
            });
        }
        
        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditMarkoutPlanning model)
        {
            var args = new ActionHelperDoUpdateArgs {
                OnSuccess = () => PartialView("_Show", Repository.Find(model.Id)),
                OnError = () => PartialView("_Edit", model),
                OnNotFound = () => HttpNotFound(WORK_ORDER_NOT_FOUND)
            };

            return ActionHelper.DoUpdate(model, args);
        }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData();
            }
        }

        #endregion

        #region Constructors

        public MarkoutPlanningController(ControllerBaseWithPersistenceArguments<IGeneralWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion
    }
}
