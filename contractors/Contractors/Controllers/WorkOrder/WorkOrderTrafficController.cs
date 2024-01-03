using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;

namespace Contractors.Controllers.WorkOrder
{
    public class WorkOrderTrafficController : ControllerBaseWithValidation<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder>
    {
        public WorkOrderTrafficController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) {}

        [HttpGet]
        public ActionResult Show(int workOrderId)
        {
            return ActionHelper.DoShow(workOrderId, new MMSINC.Utilities.ActionHelperDoShowArgs {
                IsPartial = true,
                OnNotFound = () => PartialView("_Show") // Don't know why we're returning a view with a null model instead of a 404
            });
        }

        [HttpGet]
        public ActionResult Edit(int workOrderId)
        {
            return ActionHelper.DoEdit<WorkOrderTrafficDetails>(workOrderId, new MMSINC.Utilities.ActionHelperDoEditArgs<MapCall.Common.Model.Entities.WorkOrder, WorkOrderTrafficDetails> {
                IsPartial = true,
                OnNotFound = () => PartialView("_Edit") // Don't know why we're returning a view with a null model instead of a 404
            });
        }

        [HttpPost]
        public ActionResult Update(WorkOrderTrafficDetails model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => Show(model.Id),
                OnNotFound = () => HttpNotFound(),
                OnError = () => {
                    // We want the edit view back instead of forwarding back to Show
                    // if there's any problems. Need to refresh these properties.
                    var newModel =
                        _container.GetInstance<WorkOrderTrafficDetails>();
                    newModel.Map(Repository.Find(model.Id));
                    return PartialView("_Edit", newModel);
                }
            });
        }
    }
}
