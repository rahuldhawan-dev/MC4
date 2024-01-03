using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;

namespace Contractors.Controllers.WorkOrder
{
    [RequiresAdmin]
    public class WorkOrderSchedulingController : WorkOrderControllerBase<WorkOrderSchedulingSearch>
    {
        #region Exposed Methods

        [HttpGet]
        public ActionResult Index(WorkOrderSchedulingSearch search)
        {
            return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.SearchSchedulingOrders(search),
                OnSuccess = () => {
                    this.AddDropDownData<Crew>("Crew", c => c.Id, c => c.Description);
                    var model = _container.GetInstance<SchedulingCrewAssignment>();
                    model.Search = search;
                    return View("Index", model);
                }
            });
        }

        #endregion

        public WorkOrderSchedulingController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) {}
    }
}
