using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Controllers;
using NHibernate.Criterion;

namespace Contractors.Controllers.WorkOrder
{
    [RequiresAdmin]
    public class WorkOrderPlanningController : WorkOrderControllerBase<WorkOrderPlanningSearch>
    {
        [HttpGet]
        public ActionResult Index(WorkOrderPlanningSearch search)
        {
            return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
            {
                SearchOverrideCallback = () => Repository.SearchPlanningOrders(search)
            });
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs
            {
                GetEntityOverride = () => Repository.PlanningOrders
                                            .Add(Restrictions.IdEq(id))
                                            .UniqueResult<MapCall.Common.Model.Entities.WorkOrder>()
            });
        }

        public WorkOrderPlanningController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) {}
    }
}
