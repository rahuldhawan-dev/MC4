using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderStreetOpeningPermitProcessing;
using MapCallMVC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    [DisplayName("Street Opening Permits - Work Without Permit Attached")]
    public class WorkOrderStreetOpeningPermitProcessingController : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = WorkOrderController.ROLE;

        #endregion

        #region Constructors

        public WorkOrderStreetOpeningPermitProcessingController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion

        #region Public Methods

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

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchWorkOrderStreetOpeningPermitProcessing());
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchWorkOrderStreetOpeningPermitProcessing search)
        {
            return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetSopProcessingWorkOrders(search)
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new ActionHelperDoShowArgs() {
                GetEntityOverride = () => Repository.FindSopProcessingOrder(id)
            });
        }

        #endregion

        #region Edit

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator), ActionBarVisible(false)]
        public void Edit(int id) { }

        #endregion

        #endregion
    }
}
