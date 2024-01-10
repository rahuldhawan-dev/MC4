using System.ComponentModel;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderFinalization;
using MMSINC.Utilities;
using MapCallMVC.ClassExtensions;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Data.NHibernate;
using MapCall.Common.Utility.Notifications;
using MMSINC.ClassExtensions;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    [DisplayName("Work Order Finalization")]
    public class WorkOrderFinalizationController : SapSyncronizedControllerBaseWithPersisence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = WorkOrderController.ROLE;

        public const string CONTRACTOR_ASSIGNED_NOTIFICATION = "This work order is assigned to a contractor, please proceed if authorized to do so.",
                            MAIN_BREAK_NOTIFICATION = "Main Break Entered", 
                            MAIN_BREAK_COMPLETED_NOTIFICATION = "Main Break Completed",
                            SAMPLE_SITE_NOTIFICATION = "Work Order With Sample Site",
                            SERVICE_LINE_RENEWAL_COMPLETED = "Service Line Renewal Completed",
                            SERVICE_LINE_RENEWAL_LEAD_COMPLETED = "Service Line Renewal Lead Completed",
                            SEWER_OVERFLOW_CHANGED_NOTIFICATION = "Sewer Overflow Changed",
                            FRCC_EMERGENCY_COMPLETED_NOTIFICATION = "FRCC Emergency Completed",
                            WORK_DESCRIPTION_CHANGED = "Work Order Changed to Main Break Repair/Replace",
                            WORK_ORDER_NOT_FOUND = "Work Order {0} is not eligible for finalization.",
                            PITCHER_FILTER_NOT_DELIVERED = "Pitcher Filter Not Delivered";

        #endregion

        #region Constructors

        public WorkOrderFinalizationController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownDataForRoleAndAction(RoleModules.FieldServicesWorkManagement,
                    extraFilterP: oc => oc.WorkOrdersEnabled);
            }
            else if (action == ControllerAction.Edit)
            {
                this.AddDropDownData<ServiceMaterial>("PreviousServiceLineMaterial", d => d.Where(x => x.Description != "UNKNOWN"), d => d.Id, d => d.Description);
                this.AddDropDownData<ServiceMaterial>("CompanyServiceLineMaterial", d => d.Where(x => x.IsEditEnabled), d => d.Id, d => d.Description);
                this.AddDropDownData<ServiceMaterial>("CustomerServiceLineMaterial", d => d.Where(x => x.Description != "UNKNOWN"), d => d.Id, d => d.Description);
            }
        }

        protected override void UpdateEntityForSap(WorkOrder entity)
        {
            var sapWorkOrderStepRepo = _container.GetInstance<IRepository<SAPWorkOrderStep>>();
            entity.SAPWorkOrderStep = sapWorkOrderStepRepo.Find(SAPWorkOrderStep.Indices.UPDATE);
            AddAuditLogEntry("SAPUpdate", entity.Id, "SAPUpdateStart", "Start Update", entity.MaterialsDocID);
            entity.UserId = AuthenticationService.CurrentUser.UserName;
            var progressWorkOrder = new SAPProgressWorkOrder(entity);
            var sapWorkOrder = _container.GetInstance<ISAPWorkOrderRepository>().Update(progressWorkOrder);
            AddAuditLogEntry("SAPUpdate", entity.Id, "MaterialDocID", "CalledUpdate", entity.MaterialsDocID);
            AddAuditLogEntry("SAPUpdate", entity.Id, "SAPErrorCode", "CalledUpdate", progressWorkOrder.Status);
            sapWorkOrder.MapToWorkOrder(entity);

            if (sapWorkOrder != null && sapWorkOrder.Status.Contains("Successful") && !entity.DateRejected.HasValue)
            {
                var workDescriptionId = entity.WorkDescription?.Id;
                if (workDescriptionId != null && 
                    entity.ServiceInstallations != null && 
                    entity.ServiceInstallations.Any() &&
                    WorkDescriptionRepository.NEW_SERVICE_INSTALLATIONS.Contains(workDescriptionId.Value))
                {
                    entity.SAPWorkOrderStep = sapWorkOrderStepRepo.Find(SAPWorkOrderStep.Indices.NMI);
                    var repo = DependencyResolver.Current.GetService<ISAPNewServiceInstallationRepository>();
                    var result = repo.Save(new SAPNewServiceInstallation(entity.ServiceInstallations.FirstOrDefault()));
                    AddAuditLogEntry("SAPUpdate", entity.Id, "MaterialDocID", "CalledUpdateWithNMI", entity.MaterialsDocID);
                    AddAuditLogEntry("SAPUpdate", entity.Id, "SAPErrorCode", "CalledUpdateWithNMI", result.SAPStatus);
                    entity.SAPErrorCode = result.SAPStatus;
                }
            }
        }

        private void AddAuditLogEntry(string auditEntryType, int entityId, string fieldName, string oldValue, string newValue)
        {
            var auditRepo = _container.GetInstance<IAuditLogEntryRepository>();
            auditRepo.Save(new AuditLogEntry {
                AuditEntryType = auditEntryType,
                EntityId = entityId,
                EntityName = nameof(WorkOrder),
                FieldName = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                Timestamp = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                User = AuthenticationService.CurrentUser
            });
        }

        // Below Notifications are sent, once a work order is finalized
        // 1. Main Break Completed
        // 2. Service Line Renewal Completed
        // 3. FRCC Emergency Completed
        private void SendNotifications(WorkOrder entity)
        {
            if (entity.DateCompleted.HasValue)
            {
                entity.RecordUrl = GetUrlForModel(entity, "Show", "GeneralWorkOrder", "FieldOperations");
                var notifier = _container.GetInstance<INotificationService>();
                var args = new NotifierArgs {
                    OperatingCenterId = entity.OperatingCenter.Id,
                    Module = ROLE,
                    Data = entity
                };

                if (entity.WorkDescription != null && WorkDescription.GetMainBreakWorkDescriptions().Contains(entity.WorkDescription.Id))
                {
                    args.Purpose = MAIN_BREAK_COMPLETED_NOTIFICATION;
                    notifier.Notify(args);
                }
                else if (entity.WorkDescription != null && WorkDescription.SERVICE_LINE_RENEWALS.Contains(entity.WorkDescription.Id))
                {
                    args.Purpose = SERVICE_LINE_RENEWAL_COMPLETED;
                    notifier.Notify(args);
                }

                if (entity.RequestedBy?.Id == WorkOrderRequester.Indices.FRCC
                    && entity.Priority?.Id == (int)WorkOrderPriority.Indices.EMERGENCY)
                {
                    args.Purpose = FRCC_EMERGENCY_COMPLETED_NOTIFICATION;
                    notifier.Notify(args);
                }

                if (WorkDescription.PITCHER_FILTER_NOT_DELIVERED_WORK_DESCRIPTIONS.Contains(entity.WorkDescription?.Id ?? 0)
                    && entity.HasPitcherFilterBeenProvidedToCustomer == false
                    && ((entity.PreviousServiceLineMaterial?.Description == ServiceMaterial.Descriptions.GALVANIZED 
                         && entity.Town?.State?.Id == State.Indices.NJ) 
                        || entity.PreviousServiceLineMaterial?.Description == ServiceMaterial.Descriptions.LEAD))
                {
                    args.Purpose = PITCHER_FILTER_NOT_DELIVERED;
                    args.TemplateName = PITCHER_FILTER_NOT_DELIVERED;
                    args.Subject = PITCHER_FILTER_NOT_DELIVERED;
                    
                    notifier.Notify(args);
                }
            }
        }

        private void SendWorkDescriptionChangedNotifications(WorkOrder entity, int? finalWorkDescription = null)
        {
            if (finalWorkDescription.HasValue && 
                entity.WorkDescription != null &&
                finalWorkDescription.Value != entity.WorkDescription.Id
                && !entity.CancelledAt.HasValue)
            {
                entity.RecordUrl = GetUrlForModel(entity, "Show", "GeneralWorkOrder", "FieldOperations");
                var notifier = _container.GetInstance<INotificationService>();
                var args = new NotifierArgs {
                    OperatingCenterId = entity.OperatingCenter.Id,
                    Module = ROLE,
                    Data = entity
                };

                // Main Break Entered Notification: Sent when a work order description is
                // changed to main break repair / replace.
                // if the old and new are both main break repair / replace we don't send notification
                if (!WorkDescription.GetMainBreakWorkDescriptions().Contains(finalWorkDescription.Value) &&
                    WorkDescription.GetMainBreakWorkDescriptions().Contains(entity.WorkDescription.Id))
                {
                    args.Purpose = MAIN_BREAK_NOTIFICATION;
                    args.Subject = WORK_DESCRIPTION_CHANGED;

                    foreach (var contact in
                             entity.Town.TownContacts
                                   .Where(tc => tc.ContactType.Id == ContactType.Indices.MAIN_BREAK_NOTIFICATION))
                    {
                        notifier.Notify(args);
                    }
                }

                // Sewer Overflow Changed Notification: Sent when a work order description is
                // changed to sewer main/service overflow.
                // if the old and new are both sewer main/service overflow we don't send notification
                if (!WorkDescription.SEWER_OVERFLOW.Contains(finalWorkDescription.Value) &&
                    WorkDescription.SEWER_OVERFLOW.Contains(entity.WorkDescription.Id))
                {
                    args.Purpose = SEWER_OVERFLOW_CHANGED_NOTIFICATION;
                    notifier.Notify(args);
                }
            }
        }

        // if WO is modified and has a sample site, send the notification
        private void SendSampleSiteNotification(WorkOrder entity)
        {
            if (entity.HasSampleSite.GetValueOrDefault())
            {
                entity.RecordUrl = GetUrlForModel(entity, "Show", "GeneralWorkOrder", "FieldOperations");
                var notifier = _container.GetInstance<INotificationService>();
                var args = new NotifierArgs {
                    OperatingCenterId = entity.OperatingCenter.Id,
                    Module = ROLE,
                    Data = entity,
                    Purpose = SAMPLE_SITE_NOTIFICATION
                };
                notifier.Notify(args);
            }
        }

        private void SetDefaultMeterLocation(WorkOrder workOrder)
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

        private void UpdateServiceInstallationMeterLocation(WorkOrder workOrder)
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
                            si.MeterLocation = new MeterSupplementalLocation() { Id = meterLocationId.Value };
                            var repo = _container.GetInstance<IRepository<ServiceInstallation>>();
                            repo.Save(si);
                        }
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchWorkOrderFinalization());
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchWorkOrderFinalization search)
        {
            return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetFinalizingWorkOrders(search)
            });
        }

        /// <summary>
        /// Adding bypass check field to view finalizing work order when accessing
        /// from other pages
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bypassCheck"></param>
        /// <returns></returns>
        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id, bool? bypassCheck = false)
        {
            return ActionHelper.DoShow(id, new ActionHelperDoShowArgs() {
                GetEntityOverride = () => {
                    if (bypassCheck.GetValueOrDefault())
                    {
                        return Repository.Find(id);
                    }

                    return Repository.FindFinalizationOrder(id);
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            // 1.If a work order is not ready for finalization - Display a valid error message
            // 2.If a work order is ready for finalization - Open in Edit mode
            // 3.If a work order is already finalized - Open in Show mode
            // 4.If a work order is already finalized and supervisor approved - Open in Show mode
            
            var entity = Repository.Find(id);
            if (entity != null)
            {
                // 4.If a work order is already finalized and supervisor approved - Open in Show mode
                if (entity.ApprovedOn.HasValue)
                {
                    return RedirectToAction("Show", new { id, bypassCheck = true });
                }
                // 3.If a work order is already finalized - Open in Show mode
                if (entity.DateCompleted.HasValue)
                {
                    return RedirectToAction("Show", new { id });
                }
                
            }
            // 2.If a work order is ready for finalization - Open in Edit mode
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<WorkOrder, EditWorkOrderFinalization> {
                GetEntityOverride = () =>
                {
                    var finalizationOrder = Repository.FindFinalizationOrder(id);
                    SetDefaultMeterLocation(finalizationOrder);
                    return finalizationOrder;
                },
                // 1.If a work order is not ready for finalization - Display a valid error message
                NotFound = string.Format(WORK_ORDER_NOT_FOUND, id)
            }, onModelFound: order => {
                if (order.AssignedContractor != null)
                {
                    DisplayNotification(CONTRACTOR_ASSIGNED_NOTIFICATION);
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditWorkOrderFinalization model)
        {
            var workDescription = model.WorkOrder.WorkDescription;
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    UpdateServiceInstallationMeterLocation(entity);
                    if (entity.IsSAPUpdatableWorkOrder)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }
                    SendWorkDescriptionChangedNotifications(entity, workDescription?.Id);
                    SendSampleSiteNotification(entity);
                    SendNotifications(entity);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #endregion
    }
}
