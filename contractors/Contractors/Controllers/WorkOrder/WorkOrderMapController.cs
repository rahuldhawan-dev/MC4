using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;

namespace Contractors.Controllers.WorkOrder
{
    public class WorkOrderMapController : ControllerBaseWithValidation<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder>
    {
        public WorkOrderMapController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) { }

        [HttpGet] 
        public ActionResult Show(int id)
        {
            // Need to return a null view when null
            return ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                IsPartial = true,
                OnNotFound = () => PartialView("_Show") // Originally this was tested to return a view even if the model wasn't found. Dunno why.
            });
        }

    }
}
