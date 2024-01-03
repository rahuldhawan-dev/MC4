using System.Linq;
using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using NHibernate.Criterion;
using IWorkDescriptionRepository = Contractors.Data.Models.Repositories.IWorkDescriptionRepository;
using IWorkOrderRepository = Contractors.Data.Models.Repositories.IWorkOrderRepository;

namespace Contractors.Controllers.WorkOrder
{
    public class WorkOrderFinalizationController : WorkOrderControllerBase<WorkOrderFinalizationSearch>
    {
        #region Private Methods

        /// <summary>
        /// This exists solely for the unit tests for this controller. These tests are doing validation testing
        /// rather than view model tests. They need to be rewritten.
        /// </summary>
        /// <param name="model"></param>
        internal void RunModelValidation(object model)
        {
            TryValidateModel(model);
        }
		
        private void SetDefaultMeterLocation(MapCall.Common.Model.Entities.WorkOrder workOrder)
        {
            if (workOrder != null && workOrder.AssetType.Id == AssetType.Indices.SERVICE &&
                workOrder.MeterLocation == null && workOrder.PremiseNumber != null)
            {
                // default the Meter Location to corresponding Premise's Meter Location if not set already
                workOrder.MeterLocation = _container.GetInstance<IRepository<Premise>>()
                                                    .FindActivePremiseByPremiseNumberDeviceLocationAndInstallation(workOrder.PremiseNumber, workOrder.DeviceLocation?.ToString(), workOrder.Installation?.ToString())
                                                    .FirstOrDefault()?.MeterLocation;
            }
        }

        private void UpdateServiceInstallationMeterLocation(MapCall.Common.Model.Entities.WorkOrder workOrder)
        {
            // map WorkOrder.MeterLocation.Id to ServiceInstallation's MeterSupplementalLocation.Id
            // Inside and Outside MeterLocation maps to Inside and Outside respectively in MeterSupplementalLocation in ServiceInstallation
            // in case ServiceInstallation has anything other than Inside and Outside like SecureAccess/LS then don't update
            if (workOrder.AssetType.Id == AssetType.Indices.SERVICE)
            {
                foreach (var si in workOrder.ServiceInstallations)
                {
                    if (workOrder.MeterLocation?.Id != si.MeterLocation?.Id)
                    {
                        int? meterLocationId;
                        switch (si.MeterLocation?.Id)
                        {
                            case MeterSupplementalLocation.Indices.INSIDE:
                            case MeterSupplementalLocation.Indices.OUTSIDE:
                                meterLocationId = workOrder.MeterLocation.Id;
                                break;
                            default:
                                meterLocationId = null; // don't update if anything other than Inside and Outside like SecureAccess/LS etc
                                break;
                        }

                        if (meterLocationId.HasValue)
                        {
                            si.MeterLocation = new MeterSupplementalLocation { Id = meterLocationId.Value };
                            var repo = _container.GetInstance<IRepository<ServiceInstallation>>();
                            repo.Save(si);
                        }
                    }
                }
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Edit:
                    this.AddDropDownData<IWorkDescriptionRepository, WorkDescription>("WorkDescription", d => d.Id, d => d.Description);
                    this.AddDropDownData<IRepository<ServiceSize>, ServiceSize>("PreviousServiceLineSize", d => d.Id, d => d.ServiceSizeDescription);
                    this.AddDropDownData<IRepository<ServiceSize>, ServiceSize>("CustomerServiceLineSize", d => d.Id, d => d.ServiceSizeDescription);
                    this.AddDropDownData<IServiceMaterialRepository, ServiceMaterial>("PreviousServiceLineMaterial", d => d.GetAllButUnknown(), d => d.Id, d => d.Description);
                    this.AddDropDownData<IServiceMaterialRepository, ServiceMaterial>("CustomerServiceLineMaterial", d => d.GetAllButUnknown(), d => d.Id, d => d.Description);
                    break;
            }
        }

        [HttpGet]
        public ActionResult Index(WorkOrderFinalizationSearch search)
        {
            return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
            {
                SearchOverrideCallback = () => Repository.SearchFinalizationOrders(search)
            });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(id, new MMSINC.Utilities.ActionHelperDoEditArgs<MapCall.Common.Model.Entities.WorkOrder, WorkOrderFinalizationDetails> {
                GetEntityOverride = () => {
                    var finalizationOrder = Repository.FinalizationOrders
                                                      .Add(Restrictions.IdEq(id))
                                                      .UniqueResult<MapCall.Common.Model.Entities.WorkOrder>();
                    SetDefaultMeterLocation(finalizationOrder);
                    return finalizationOrder;
                },
                NotFound = string.Format(NO_SUCH_WORK_ORDER, id)
            });
        }

        [HttpPost]
        public ActionResult Update(WorkOrderFinalizationDetails model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    UpdateServiceInstallationMeterLocation(entity);
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id);
                    }
                    return RedirectToAction("ShowCalendar", "CrewAssignment");
                },
                OnError = () => RedirectToAction("Edit", new { model.Id })
            });
        }

        #endregion

        public WorkOrderFinalizationController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) {}
    }
}
