using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;

namespace Contractors.Controllers.WorkOrder
{
    public class WorkOrderReadOnlyController : ControllerBaseWithValidation<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder>
    {
        public WorkOrderReadOnlyController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) { }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs
            {
                OnNotFound = () => View() // For some reason we return a special view for 404s here.
            });
        }
    }
}
