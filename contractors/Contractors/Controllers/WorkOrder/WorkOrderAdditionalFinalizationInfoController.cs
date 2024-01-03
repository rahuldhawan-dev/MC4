using System.Linq;
using System.Web.Mvc;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.WebApi;
using IWorkDescriptionRepository = Contractors.Data.Models.Repositories.IWorkDescriptionRepository;
using IWorkOrderRepository = Contractors.Data.Models.Repositories.IWorkOrderRepository;

namespace Contractors.Controllers.WorkOrder
{
    public class WorkOrderAdditionalFinalizationInfoController : SapControllerWithValidationBase<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder>
    {
        #region Constants

        public const string SEWER_OVERFLOW_CHANGED_NOTIFICATION = "Sewer Overflow Changed", 
            PITCHER_FILTER_NOT_DELIVERED = "Pitcher Filter Not Delivered";

        #endregion

        public WorkOrderAdditionalFinalizationInfoController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) {}

        [HttpPost]
        public ActionResult Update(WorkOrderAdditionalFinalizationInfo model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    UpdateSAP(model.Id);
                    if (model.SendSewerOverflowChangedNotification)
                    {
                        SendSewerOverflowChangedNotification(model);
                    }
                    SendPitcherFilterNotDeliveredNotification(model);
                    // Return this instead of EmptyResult. Issue in Firefox
                    // http://stackoverflow.com/a/2208753/152168
                    return Json(string.Empty);
                },
                OnNotFound = () => HttpNotFound(),
                OnError = () => {
                    this.AddDropDownData<IWorkDescriptionRepository, WorkDescription>(
                        "WorkDescription",
                        r => r.GetByAssetTypeId(Repository.Find(model.Id).AssetType.Id)
                        , d => d.Id, d => d.Description);
                    return PartialView("_Edit", model);
                }
            });
        }

        #region Private Methods

        private void SendSewerOverflowChangedNotification(WorkOrderAdditionalFinalizationInfo model)
        {
            var entity = Repository.Find(model.Id);
            entity.RecordUrl = GetUrlForModel(entity, "Show", "GeneralWorkOrder", "FieldOperations");
            _container.GetInstance<INotificationService>().Notify(new NotifierArgs {
                OperatingCenterId = entity.OperatingCenter.Id,
                Module = RoleModules.FieldServicesWorkManagement,
                Purpose = SEWER_OVERFLOW_CHANGED_NOTIFICATION,
                Data = entity
            });
        }

        private void SendPitcherFilterNotDeliveredNotification(WorkOrderAdditionalFinalizationInfo model)
        {
            var entity = Repository.Find(model.Id);
            if (WorkDescription.PITCHER_FILTER_NOT_DELIVERED_WORK_DESCRIPTIONS.Contains(entity.WorkDescription?.Id ?? 0)
                && entity.HasPitcherFilterBeenProvidedToCustomer == false
                && ((entity.PreviousServiceLineMaterial?.Description == ServiceMaterial.Descriptions.GALVANIZED 
                     && entity.Town.State.Id == State.Indices.NJ) 
                    || entity.PreviousServiceLineMaterial?.Description == ServiceMaterial.Descriptions.LEAD))
            {
                var args = new NotifierArgs {
                    OperatingCenterId = entity.OperatingCenter.Id,
                    Module = RoleModules.FieldServicesWorkManagement,
                    Purpose = PITCHER_FILTER_NOT_DELIVERED,
                    TemplateName = PITCHER_FILTER_NOT_DELIVERED,
                    Data = entity,
                    Subject = PITCHER_FILTER_NOT_DELIVERED
                };
                   
                _container.GetInstance<INotificationService>().Notify(args);
            }
        }

        #endregion
    }
}
