using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Linq;
using Contractors.Data.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Data.NHibernate;

namespace Contractors.Controllers.WorkOrder
{
    public abstract class WorkOrderControllerBase<TSearchModel> : SapSyncronizedControllerBaseWithValidation<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder>
        where TSearchModel : IWorkOrderSearch
    {
        #region Constants

        public const string OVER_SEARCH_RESULT_LIMIT_ERROR =
            "The query you have entered will bring back more than {0} results.  Please refine your search.";
        public const int SEARCH_RESULT_LIMIT = 1000;
        public const string NO_SUCH_WORK_ORDER = "Work Order # {0} was not found to exist.";
        #endregion

        protected WorkOrderControllerBase(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) {}

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this
                       .AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>()
                       .AddDropDownData<IAssetTypeRepository, AssetType>(
                            "AssetType", t => t.Id, t => t.Description)
                       .AddDropDownData<WorkOrderPriority>(
                            "Priority", p => p.Id, p => p.Description)
                       .AddDropDownData<MarkoutRequirement>(
                            "MarkoutRequirement", r => r.Id, r => r.Description)
                       .AddDropDownData<WorkOrderPurpose>(
                            "Purpose", p => p.Id, p => p.Description)
                       .AddDropDownData<WorkOrderRequester>(
                            "Requester", r => r.Id, r => r.Description);
                    ViewData["CompletionTime"] = GetCompletionTimeArray();
                    break;
            }

        }

        [HttpGet] // TODO: Remove this, put it in proper controllers.
        public virtual ActionResult Search()
        {
            SetLookupData(ControllerAction.Search);
            return View();
        }

        protected SelectListItem[] GetCompletionTimeArray()
        {
            return new SelectListItem[] {
                new SelectListItem {
                    Text = WorkDescription.TimeEstimates.LESS_THAN_HOUR,
                    Value = WorkDescription.TimeEstimates.LESS_THAN_HOUR
                },
                new SelectListItem {
                    Text = WorkDescription.TimeEstimates.LESS_THAN_TWO_HOURS,
                    Value = WorkDescription.TimeEstimates.LESS_THAN_HOUR
                },
                new SelectListItem {
                    Text = WorkDescription.TimeEstimates.GREATER_THAN_TWO_HOURS,
                    Value = WorkDescription.TimeEstimates.LESS_THAN_HOUR
                }
            };
        }

        protected ActionResult RedirectToSearch()
        {
            return RedirectToAction("Search");
        }

        #region Exposed Methods

        /// <summary>
        /// This is being fed an entity after it's been updated.
        /// YOU MUST BE SURE YOU WANT TO UPDATE SAP AT THIS POINT
        /// </summary>
        /// <param name="entity"></param>
        protected override void UpdateEntityForSap(MapCall.Common.Model.Entities.WorkOrder entity)
        {
            entity.SAPWorkOrderStep = _container.GetInstance<IRepository<SAPWorkOrderStep>>().Find(SAPWorkOrderStep.Indices.UPDATE);
            var sapWorkOrder = _container.GetInstance<ISAPWorkOrderRepository>().Update(new SapProgressWorkOrder(entity));
            TryUpdateServiceInstallation(entity, sapWorkOrder);
            sapWorkOrder.MapToWorkOrder(entity);
        }

        private void TryUpdateServiceInstallation(MapCall.Common.Model.Entities.WorkOrder entity, SAPProgressWorkOrder sapWorkOrder)
        {
            //!alreadyDateCompleted 
            //entity.DateCompleted.HasValue
            //sapWorkOrder.Status.Contains("Successful"))
            //workOrder.ServiceInstallations != null
            //workOrder.ServiceInstallations.Any()
            //WorkDescriptionRepository.NEW_SERVICE_INSTALLATIONS.Contains(entity.WorkDescriptionID))
            
            // In contractors, we don't ever get here if it's already been date completed.
            // If it has been rejected before we don't send
            if (entity.DateRejected.HasValue)
                return;
            if (!sapWorkOrder.Status.Contains("Successful"))
                return;
            if (entity.ServiceInstallations == null || !entity.ServiceInstallations.Any())
                return;
            if (!WorkDescription.NEW_SERVICE_INSTALLATION.Contains(entity.WorkDescription.Id))
                return;
            var repo = _container.GetInstance<ISAPNewServiceInstallationRepository>();
            var model = _container
                       .With(entity.ServiceInstallations.FirstOrDefault())
                       .GetInstance<SAPNewServiceInstallation>();
            var result = repo.Save(model);
            entity.SAPWorkOrderStep = _container.GetInstance<IRepository<SAPWorkOrderStep>>().Find(SAPWorkOrderStep.Indices.NMI);
            entity.SAPErrorCode = result.SAPStatus;
            sapWorkOrder.Status = result.SAPStatus;
        }

        #endregion
    }
}