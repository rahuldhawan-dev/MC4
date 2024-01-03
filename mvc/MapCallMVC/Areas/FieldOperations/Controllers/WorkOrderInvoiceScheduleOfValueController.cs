using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class WorkOrderInvoiceScheduleOfValueController : ControllerBaseWithPersistence<IRepository<WorkOrderInvoiceScheduleOfValue>, WorkOrderInvoiceScheduleOfValue, User>
    {
        #region Constructors

        public WorkOrderInvoiceScheduleOfValueController(
            ControllerBaseWithPersistenceArguments
                <IRepository<WorkOrderInvoiceScheduleOfValue>, WorkOrderInvoiceScheduleOfValue, User> args) : base(args) {}

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.FieldServicesWorkManagement, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditWorkOrderInvoiceScheduleOfValue>(id);
        }

        [HttpPost, RequiresRole(RoleModules.FieldServicesWorkManagement, RoleActions.Edit)]
        public ActionResult Update(EditWorkOrderInvoiceScheduleOfValue model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
            {
               OnSuccess = () => RedirectToAction("Show", "WorkOrderInvoice", new { area = "FieldOperations", id = model.WorkOrderInvoice })
            });
        }

        #endregion
    }
}