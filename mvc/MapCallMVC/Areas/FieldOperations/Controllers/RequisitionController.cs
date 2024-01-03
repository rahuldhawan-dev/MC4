using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.NHibernate;
using MapCall.Common.Metadata;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Requisitions;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System.Net;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class RequisitionController : ControllerBaseWithPersistence<Requisition, User>
    {
        #region Consts

        // Creating/editing a requisition can only be done from the supervisor approval screen,
        // so all actions should match the role access in that controller.
        public const RoleModules ROLE = WorkOrderSupervisorApprovalController.ROLE;

        #endregion

        #region Constructor

        public RequisitionController(ControllerBaseWithPersistenceArguments<IRepository<Requisition>, Requisition, User> args) : base(args) { }

        #endregion

        #region Show

        [HttpGet, NoCache, RequiresRole(WorkOrderSupervisorApprovalController.ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                IsPartial = true,
                ViewName = "_ShowAjaxTableRow"
            });
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult New(int workOrderId)
        {
            var model = ViewModelFactory.Build<CreateWorkOrderRequisition>();
            model.WorkOrder = workOrderId;
            return ActionHelper.DoNew(model, new MMSINC.Utilities.ActionHelperDoNewArgs {
                IsPartial = true,
                ViewName = "_New"
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateWorkOrderRequisition model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => PartialView("_ShowAjaxTableRow", Repository.Find(model.Id)),
                OnError = () => PartialView("_New", model)
            });
        }

        #endregion

        #region Edit/Update

        [NoCache]
        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(id, new MMSINC.Utilities.ActionHelperDoEditArgs<Requisition, EditWorkOrderRequisition> {
                IsPartial = true
            });
        }

        // Creating a requisition only occurs on the work order supervisor approval page,
        // so this role should match that page's role access.
        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditWorkOrderRequisition model)
        {
            var workOrderId = Repository.Find(model.Id)?.WorkOrder.Id;
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => PartialView("_ShowAjaxTableRow", Repository.Find(model.Id)),
                OnError = () => PartialView("_Edit", model),
            });
        }

        #endregion

        #region Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs {
                NotFound = "Requisition not found.",
                OnSuccess = () => new HttpStatusCodeResult(HttpStatusCode.NoContent)
            });
        }

        #endregion
    }
}