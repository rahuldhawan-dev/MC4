using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;

namespace Contractors.Controllers.WorkOrder
{
    public class WorkOrderInitialController : ControllerBaseWithValidation<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder>
    {
        public WorkOrderInitialController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) { }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs { IsPartial = true });
        }
    }
}
