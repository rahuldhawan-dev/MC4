using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.Presenter;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2016;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MMSINC.Utilities;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;
using static System.Int32;
using AssetTypeRepository = WorkOrders.Model.AssetTypeRepository;
using WorkDescriptionRepository = WorkOrders.Model.WorkDescriptionRepository;
using WorkOrder = WorkOrders.Model.WorkOrder;
using MMSINC.Data.NHibernate;
using WorkDescription = WorkOrders.Model.WorkDescription;
using WorkOrderRequester = MapCall.Common.Model.Entities.WorkOrderRequester;

namespace WorkOrders.Presenters.WorkOrders
{
    /// <summary>
    /// Presenter class for all views whose entity type is WorkOrders.Model.WorkOrder.
    /// </summary>
    public class WorkOrderDetailPresenter : DetailPresenter<WorkOrder>
    {
        #region Constants

        public struct SupervisorApprovalNotifications
        {
            public const string CURB_PIT_COMPLIANCE = "Curb-Pit Compliance",
                CURB_PIT_REVENUE = "Curb-Pit Revenue",
                CURB_PIT_ESTIMATE = "Curb-Pit Estimate",
                ASSET_ORDER_COMPLETED = AddNotificationPurposeForBug2876.PURPOSE;
        }

        public const string SUPERVISOR_APPROVAL_NOTIFICATION = "Supervisor Approval",
            MAIN_BREAK_COMPLETED_NOTIFICATION = "Main Break Completed",
            MAIN_BREAK_NOTIFICATION = "Main Break Entered",
            SERVICE_LINE_INSTALLATION_ENTERED_NOTIFICATION = "Service Line Installation Entered",
            SERVICE_LINE_RENEWAL_ENTERED_NOTIFICATION = "Service Line Renewal Entered",
            SERVICE_LINE_RENEWAL_LEAD_ENTERED_NOTIFICATION = "Service Line Renewal Lead Entered",
            SERVICE_LINE_RENEWAL_COMPLETED = "Service Line Renewal Completed",
            SERVICE_LINE_RENEWAL_LEAD_COMPLETED = "Service Line Renewal Lead Completed",
            SEWER_OVERFLOW_ENTERED_NOTIFICATION = "Sewer Overflow Entered",
            SEWER_OVERFLOW_CHANGED_NOTIFICATION = "Sewer Overflow Changed",
            EQUIPMENT_REPAIR = "Equipment Repair",
            FRCC_EMERGENCY_COMPLETED_NOTIFICATION = "FRCC Emergency Completed",
            SAMPLE_SITE_NOTIFICATION = "Work Order With Sample Site",
            ACOUSTIC_MONITORING_CREATED = "Acoustic Monitoring Order Created";

        public const string SAP_ERROR_CODE = "SAPErrorCode Occurred",
            SAP_UPDATE_FAILURE = "RETRY::UPDATE FAILURE: ",
            WORK_DESCRIPTION_CHANGED = "Work Order Changed to Main Break Repair/Replace";

        #endregion

        #region Private Members

        private IWorkOrderDetailView _detailView;
        private IWorkOrdersWorkOrderRepository _workOrderRepository;

        #endregion

        #region Properties

        protected IWorkOrderDetailView DetailView
        {
            get
            {
                if (_detailView == null)
                    _detailView = (IWorkOrderDetailView)View;
                return _detailView;
            }
        }

        public IWorkOrdersWorkOrderRepository WorkOrderRepository
        {
            get { return _workOrderRepository == null ? _workOrderRepository = DependencyResolver.Current.GetService<IWorkOrdersWorkOrderRepository>() : _workOrderRepository; }
            set { _workOrderRepository = value; }
        }

        #endregion

        #region Constructors

        public WorkOrderDetailPresenter(IWorkOrderDetailView view)
            : base(view)
        {
        }

        #endregion

        #region Private Methods

        private void FixNotes(WorkOrder entity)
        {
            var oldEntity = Repository.Get(entity.WorkOrderID);

            if (!String.IsNullOrWhiteSpace(entity.Notes) && entity.Notes != oldEntity.Notes)
            {
                entity.Notes = String.IsNullOrWhiteSpace(oldEntity.Notes) ?
                                   entity.Notes : oldEntity.Notes + entity.Notes;
            }
        }

        /// <summary>
        /// All the updates in the presenter now go through this method so the order can 
        /// be considered and updated for SAP if required.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="literally"></param>
        private void UpdateEntityConsideringSAP(WorkOrder entity, bool literally = false, bool dontUpdateSapAnotherCallWill = false)
        {
            if (literally)
            {
                Repository.UpdateCurrentEntityLiterally(entity);
                if (dontUpdateSapAnotherCallWill)
                    return;
            }

            // get a copy of the order before we update it.
            var workOrder = DependencyResolver.Current.GetService<IGeneralWorkOrderRepository>().Find(entity.WorkOrderID);
            // Finish the 271 update of the work order
            Repository.UpdateCurrentEntity(entity);
            TrySendMainBreakNotification(workOrder, entity);
            TrySendSampleSiteNotification(entity);
            TrySendSewerOverflowNotification(workOrder, entity);
            TrySendAcousticMonitoringNotification(workOrder, entity);

            // go through shared repository method now instead
            WorkOrderRepository.UpdateSAPWorkOrder(entity, workOrder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrder">Old WorkOrder</param>
        /// <param name="entity">New WorkOrder</param>
        private void TrySendMainBreakNotification(MapCall.Common.Model.Entities.WorkOrder workOrder, WorkOrder entity)
        {
            // if the old an new are both main break repair/replace we don't update
            if (WorkDescriptionRepository.MAIN_BREAKS.Contains(entity.WorkDescriptionID) && WorkDescriptionRepository.MAIN_BREAKS.Contains(workOrder.WorkDescription.Id))
                return;
            if (WorkDescriptionRepository.MAIN_BREAKS.Contains(entity.WorkDescriptionID)
                && workOrder.WorkDescription.Id != entity.WorkDescriptionID
                && !entity.CancelledAt.HasValue)
            {
                var notifier = DependencyResolver.Current.GetService<INotificationService>();
                // order doesn't always come in fully initialized, lets refresh it for this and the notification
                var fullOrder = Repository.Get(entity.WorkOrderID); 
                notifier.Notify(fullOrder.OperatingCenterID.Value,
                            RoleModules.FieldServicesWorkManagement,
                            WorkOrderDetailPresenter.MAIN_BREAK_NOTIFICATION,
                            fullOrder, WORK_DESCRIPTION_CHANGED, null);

                SendTownNotifications(notifier, fullOrder, MAIN_BREAK_NOTIFICATION);
            }
        }

        protected virtual void TrySendSampleSiteNotification( WorkOrder entity)
        {
            // if WO is modified and has a sample site, send the notification
            if (entity.IsPremiseLinkedToSampleSite)
            {
                var notifier = DependencyResolver.Current.GetService<INotificationService>();
                // order doesn't always come in fully initialized, lets refresh it for this and the notification
 
                // pulling the entity model for Workorder and SampleSites because i need entity model of samplesites for the notification
                var repo = DependencyResolver.Current.GetService<IRepository<MapCall.Common.Model.Entities.WorkOrder>>();
                var fullOrder = repo.Find(entity.Id);
                var repoForSampleSites = DependencyResolver.Current.GetService<IRepository<MapCall.Common.Model.Entities.SampleSite>>();
                var sampleSite = repoForSampleSites.Find(entity.SampleSite.Id);

                fullOrder.SampleSites.Clear();
                fullOrder.SampleSites.Add(sampleSite);
                fullOrder.RecordUrl = $"{ConfigurationManager.AppSettings["BaseUrl"]}modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg={fullOrder.Id}";
                notifier.Notify(fullOrder.OperatingCenter.Id,
                    RoleModules.FieldServicesWorkManagement,
                    WorkOrderDetailPresenter.SAMPLE_SITE_NOTIFICATION,
                    fullOrder, SAMPLE_SITE_NOTIFICATION, null);
            }
        }

        private void TrySendSewerOverflowNotification(MapCall.Common.Model.Entities.WorkOrder workOrder ,WorkOrder entity)
        {
            // Check to make sure were only sending if desription has been changed
            if (WorkDescriptionRepository.SEWER_OVERFLOW.Contains(entity.WorkDescriptionID) && WorkDescriptionRepository.SEWER_OVERFLOW.Contains(workOrder.WorkDescription.Id))
                return;

            if (WorkDescriptionRepository.SEWER_OVERFLOW.Contains(entity.WorkDescriptionID)
                && workOrder.WorkDescription.Id != entity.WorkDescriptionID
                && !entity.CancelledAt.HasValue)
            {
                var notifier = DependencyResolver.Current.GetService<INotificationService>();
                // order doesn't always come in fully initialized, lets refresh it for this and the notification
                var fullOrder = Repository.Get(entity.WorkOrderID);
                fullOrder.RecordUrl =
                    $"{ConfigurationManager.AppSettings["BaseUrl"]}modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg={fullOrder.Id}";
                notifier.Notify(fullOrder.OperatingCenterID.Value,
                    RoleModules.FieldServicesWorkManagement,
                    WorkOrderDetailPresenter.SEWER_OVERFLOW_CHANGED_NOTIFICATION,
                    fullOrder);
            }
        }

        protected virtual void TrySendAcousticMonitoringNotification(MapCall.Common.Model.Entities.WorkOrder workOrder, WorkOrder entity)
        {
            if (entity.RequesterID == WorkOrderRequester.Indices.ACOUSTIC_MONITORING && workOrder.RequestedBy.Id != WorkOrderRequester.Indices.ACOUSTIC_MONITORING)
            {
                var fullOrder = Repository.Get(entity.WorkOrderID);
                var notifier = DependencyResolver.Current.GetService<INotificationService>();
                
                notifier.Notify(fullOrder.OperatingCenterID.Value,
                    RoleModules.FieldServicesWorkManagement,
                    WorkOrderDetailPresenter.ACOUSTIC_MONITORING_CREATED,
                    fullOrder);
            }
        }

        private void UpdateForApproval(WorkOrder entity)
        {
            //get all the old values and only change what we need to change
            var oldEntity = Repository.Get(entity.WorkOrderID);
            if (entity.DateCompleted == null)
            {
                oldEntity.DateCompleted = null;
            }
            if (entity.CompletedByID == null)
            {
                oldEntity.CompletedByID = null;
            }

            //If we are approving then ApprovedByID / ApprovedOn will have a value
            oldEntity.ApprovedByID = entity.ApprovedByID;
            oldEntity.ApprovedOn = entity.ApprovedOn;
            oldEntity.AccountCharged = entity.AccountCharged;
            oldEntity.BusinessUnit = entity.BusinessUnit;
            oldEntity.UpdatedMobileGIS = entity.UpdatedMobileGIS;
            oldEntity.RequiresInvoice = entity.RequiresInvoice;

            //When rejecting add the reason to the notes
            var isApproved = entity.ApprovedByID != null;
            if (!isApproved)
            {
                oldEntity.DateRejected = DependencyResolver.Current.GetService<IDateTimeProvider>().GetCurrentDate();
                entity.DateRejected = DependencyResolver.Current.GetService<IDateTimeProvider>().GetCurrentDate();
                oldEntity.Notes += entity.Notes;
                entity.Notes = oldEntity.Notes;
            }

            // get a copy of the order before we update it.
            var workOrder = DependencyResolver.Current.GetService<IGeneralWorkOrderRepository>().Find(entity.WorkOrderID);
            //This allows the nulling of values
            UpdateEntityConsideringSAP(oldEntity, true, true);
            WorkOrderRepository.UpdateSAPWorkOrder(entity, workOrder);
        }

        private void UpdateAfterCompletion(WorkOrder entity)
        {
            var workOrder = DependencyResolver.Current.GetService<IGeneralWorkOrderRepository>().Find(entity.WorkOrderID);
            // MC-655 moved here from approval
            if (workOrder.Service != null)
            {
                // MC-449: Copy these four work order fields to the associated service upon supervisor approval.
                // Also it seems that this data does not need to be persisted to SAP because SAP already knows about
                // the changes or something. 
                var service = workOrder.Service;
                if (workOrder.PreviousServiceLineMaterial != null)
                {
                    service.PreviousServiceMaterial = workOrder.PreviousServiceLineMaterial;
                }
                if (workOrder.PreviousServiceLineSize != null)
                {
                    service.PreviousServiceSize = workOrder.PreviousServiceLineSize;
                }
                if (workOrder.CustomerServiceLineMaterial != null)
                {
                    service.CustomerSideMaterial = workOrder.CustomerServiceLineMaterial;
                }
                if (workOrder.CustomerServiceLineSize != null)
                {
                    service.CustomerSideSize = workOrder.CustomerServiceLineSize;
                }
                if (workOrder.CompanyServiceLineMaterial != null)
                {
                    service.ServiceMaterial = workOrder.CompanyServiceLineMaterial;
                }
                if (workOrder.CompanyServiceLineSize != null)
                {
                    service.ServiceSize = workOrder.CompanyServiceLineSize;
                }

                var serviceRepo = DependencyResolver.Current.GetService<IServiceRepository>();
                serviceRepo.Save(service);
            }
        }

        private void TryMaterialApprovalFix(WorkOrder entity)
        {
            if (entity.MaterialsApprovedBy == null && entity.MaterialsApprovedByID != null && !string.IsNullOrWhiteSpace(entity.SAPErrorCode) && !entity.SAPErrorCode.ToUpper().Contains("SUCCESS"))
            {
                var oldEntity = Repository.Get(entity.WorkOrderID);
                oldEntity.MaterialsApprovedOn = null;
                oldEntity.MaterialsApprovedByID = null;
                Repository.UpdateCurrentEntityLiterally(oldEntity);
            }
        }

        private void UpdateAccountStrings(WorkOrder entity)
        {
            var oldEntity = Repository.Get(entity.WorkOrderID);
            // we need to get out of here if we talk to sap, this should never happen
            if (oldEntity.OperatingCenter.SAPEnabled && oldEntity.OperatingCenter.SAPWorkOrdersEnabled && !oldEntity.OperatingCenter.IsContractedOperations)
                return;

            oldEntity.AccountCharged = entity.AccountCharged;
            oldEntity.BusinessUnit = entity.BusinessUnit;
            Repository.UpdateCurrentEntityLiterally(oldEntity);
        }

        private void UpdateWithNewAssetType(WorkOrder order)
        {
            var oldOrder = Repository.Get(order.WorkOrderID);
            switch (order.AssetTypeID)
            {
                case AssetTypeRepository.Indices.VALVE:
                    oldOrder.HydrantID = null;
                    oldOrder.SewerOpeningID = null;
                    oldOrder.PremiseNumber = null;
                    oldOrder.ServiceNumber = null;
                    oldOrder.ValveID = order.ValveID;
                    break;
                case AssetTypeRepository.Indices.HYDRANT:
                    oldOrder.ValveID = null;
                    oldOrder.SewerOpeningID = null;
                    oldOrder.PremiseNumber = null;
                    oldOrder.ServiceNumber = null;
                    oldOrder.HydrantID = order.HydrantID;
                    break;
                case AssetTypeRepository.Indices.SEWER_OPENING:
                    oldOrder.ValveID = null;
                    oldOrder.HydrantID = null;
                    oldOrder.PremiseNumber = null;
                    oldOrder.ServiceNumber = null;
                    oldOrder.SewerOpeningID = order.SewerOpeningID;
                    break;
                case AssetTypeRepository.Indices.SERVICE:
                    oldOrder.ValveID = null;
                    oldOrder.HydrantID = null;
                    oldOrder.SewerOpeningID = null;
                    oldOrder.PremiseNumber = order.PremiseNumber;
                    oldOrder.ServiceNumber = order.ServiceNumber;
                    break;
                case AssetTypeRepository.Indices.MAIN:
                    oldOrder.ValveID = null;
                    oldOrder.HydrantID = null;
                    oldOrder.SewerOpeningID = null;
                    oldOrder.PremiseNumber = null;
                    oldOrder.ServiceNumber = null;
                    // just in case this is for a main break
                    oldOrder.CustomerImpactRangeID = order.CustomerImpactRangeID;
                    oldOrder.RepairTimeRangeID = order.RepairTimeRangeID;
                    oldOrder.AlertID = order.AlertID;
                    oldOrder.SignificantTrafficImpact =
                        order.SignificantTrafficImpact;
                    break;
                default:
                    oldOrder.ValveID = null;
                    oldOrder.HydrantID = null;
                    oldOrder.SewerOpeningID = null;
                    oldOrder.PremiseNumber = null;
                    oldOrder.ServiceNumber = null;
                    break;
            }
            oldOrder.RequesterID = order.RequesterID;
            oldOrder.RequestingEmployeeID = order.RequestingEmployeeID;
            oldOrder.TownID = order.TownID;
            oldOrder.TownSectionID = order.TownSectionID;
            oldOrder.StreetID = order.StreetID;
            oldOrder.NearestCrossStreetID = order.NearestCrossStreetID;
            oldOrder.WorkDescriptionID = order.WorkDescriptionID;
            oldOrder.AssetTypeID = order.AssetTypeID;
            oldOrder.MarkoutRequirementID = order.MarkoutRequirementID;
            oldOrder.AccountCharged = order.AccountCharged;
            oldOrder.PlantMaintenanceActivityTypeOverride =
                order.PlantMaintenanceActivityTypeOverride;
            // need to update the asset type and id separately
            UpdateEntityConsideringSAP(oldOrder, true, true);
            // then account for any other changes
            //Repository.UpdateCurrentEntity(order);
            UpdateEntityConsideringSAP(order);
        }

        private void UpdateWithNoTownSection(WorkOrder order)
        {
            var oldOrder = Repository.Get(order.WorkOrderID);

            if (oldOrder.TownSection != null)
            {
                oldOrder.TownSection = null;
                oldOrder.TownID = order.TownID;
                oldOrder.StreetID = order.StreetID;
                oldOrder.NearestCrossStreetID = order.NearestCrossStreetID;
                oldOrder.WorkDescriptionID = order.WorkDescriptionID;
                oldOrder.AssetTypeID = order.AssetTypeID;
                oldOrder.AccountCharged = order.AccountCharged;
                oldOrder.PlantMaintenanceActivityTypeOverride = order.PlantMaintenanceActivityTypeOverride;

                // null out the TownSectionID
                UpdateEntityConsideringSAP(oldOrder, true, true);
            }

            // account for any other changes
            UpdateEntityConsideringSAP(order);
        }

        private bool HasNewAssetType(WorkOrder order)
        {
            var oldOrder = Repository.Get(order.WorkOrderID);
            return order.AssetTypeID != default(int) && order.AssetTypeID != oldOrder.AssetTypeID;
        }

        private bool HasTownSectionRemoved(WorkOrder order)
        {
            var oldOrder = Repository.Get(order.WorkOrderID);
            return order.TownSectionID == null && oldOrder.TownSectionID != null;
        }

        private bool HasAccountNumber(WorkOrder order)
        {
            return !String.IsNullOrEmpty(order.AccountCharged);
        }

        protected virtual void TryApprovalNotification(WorkOrder order)
        {
            var service = DependencyResolver.Current.GetService<INotificationService>();

            TryCurbPitNotification(order, service);
            TryAssetNotification(order, service);
        }

        private void TryAssetNotification(WorkOrder order, INotificationService service)
        {
            if (!WorkDescriptionRepository.ASSET_COMPLETION.Contains(
                    order.WorkDescriptionID))
            {
                return;
            }
            order.RecordUrl = $"{ConfigurationManager.AppSettings["BaseUrl"]}modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg={order.Id}";
            service.Notify(order.OperatingCenterID.Value,
                RoleModules.FieldServicesWorkManagement,
                SupervisorApprovalNotifications.ASSET_ORDER_COMPLETED, order);
        }

        private static void TryCurbPitNotification(WorkOrder order, INotificationService service)
        {
            if (!WorkDescriptionRepository.CURB_PIT.Contains(
                order.WorkDescriptionID))
            {
                return;
            }

            string purpose = null;

            if (WorkOrderPurposeRepository.Indices.COMPLIANCE == order.PurposeID)
            {
                purpose =
                    SupervisorApprovalNotifications.CURB_PIT_COMPLIANCE;
            }

            if (WorkOrderPurposeRepository.Indices.ESTIMATES == order.PurposeID)
            {
                purpose = SupervisorApprovalNotifications.CURB_PIT_ESTIMATE;
            }

            if (WorkOrderPurposeRepository.REVENUE.Contains(order.PurposeID))
            {
                purpose = SupervisorApprovalNotifications.CURB_PIT_REVENUE;
            }

            if (!String.IsNullOrEmpty(purpose))
            {
                service
                    .Notify(order.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement, purpose,
                        order);
            }
        }


        // Why does the Notifier think the user is not authenticated in DEV
        // because of some thing it isn't getting from MapCall proper
        public void TrySendSapErrorNotification(WorkOrder entity, RoleModules module)
        {
            // current user isn't authenticated for some reason.
            if (!string.IsNullOrWhiteSpace(entity.SAPErrorCode) && !entity.SAPErrorCode.StartsWith("RETRY") && !entity.SAPErrorCode.ToUpper().Contains("SUCCESSFUL"))
            {
                var notifier = DependencyResolver.Current.GetService<INotificationService>();
                notifier.Notify(entity.OperatingCenter.OperatingCenterID, module, SAP_ERROR_CODE, new { RecordUrl = new SAPEntity().GetShowUrl("WorkOrder", entity.WorkOrderID), SAPErrorCode = entity.SAPErrorCode });
            }
        }
        
        protected virtual void MaybeDefaultDigitalAsBuiltRequired(WorkOrder order)
        {
            var descriptionRepo = DependencyResolver
                                 .Current
                                 .GetService<MMSINC.Data.Linq.IRepository<WorkDescription>>();
            var description = descriptionRepo.Get(order.WorkDescriptionID);
            
            if (description != null && description.DigitalAsBuiltRequired)
            {
                order.DigitalAsBuiltRequired = true;
            }
        }

        protected virtual void TryCompletedNotification(WorkOrder order)
        {
            var workOrder = Repository.Get(order.WorkOrderID);
            if (workOrder != null && order.DateCompleted.HasValue)
            {
                workOrder.DateCompleted = order.DateCompleted;
                workOrder.RecordUrl = $"{ConfigurationManager.AppSettings["BaseUrl"]}modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg={workOrder.Id}";
                var notifier = DependencyResolver.Current.GetService<INotificationService>();

                if (WorkDescriptionRepository.MAIN_BREAKS.Contains(workOrder.WorkDescriptionID))
                {
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                    RoleModules.FieldServicesWorkManagement,
                    MAIN_BREAK_COMPLETED_NOTIFICATION,
                    workOrder);
                }
                if (workOrder.WorkDescriptionID == WorkDescription.SERVICE_LINE_RENEWAL_LEAD)
                {
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        SERVICE_LINE_RENEWAL_LEAD_COMPLETED,
                        workOrder);
                }
                else if (WorkDescriptionRepository.SERVICE_LINE_RENEWALS.Contains(workOrder.WorkDescriptionID))
                {
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                    RoleModules.FieldServicesWorkManagement,
                    SERVICE_LINE_RENEWAL_COMPLETED,
                    workOrder);
                }
                if (workOrder.RequesterID == WorkOrderRequesterRepository.Indices.FRCC
                    && workOrder.PriorityID == WorkOrderPriorityRepository.Indices.EMERGENCY)
                {
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        FRCC_EMERGENCY_COMPLETED_NOTIFICATION,
                        workOrder);
                }
            }
        }

        private void SendTownNotifications(INotificationService notifier, WorkOrder workorder, string notificationType)
        {
            foreach (var contact in workorder.Town.TownContacts.Where(x =>x.ContactType.ContactTypeID == 8)) //MapCall.Common.Model.Entities.ContactType.Indices.MAIN_BREAK_NOTIFICATION))
            {
                notifier.Notify(workorder.OperatingCenterID.Value,
                    RoleModules.FieldServicesWorkManagement, notificationType,
                    workorder, address: contact.Contact.Email);
            }
        }

        #endregion

        #region Event Handlers

        protected override void Repository_CurrentEntityChanged(object sender, EventArgs e)
        {
            LoadCurrentEntityOnView();
            View.SetViewControlsVisible(true);
            View.SetViewMode((DetailView.Phase == WorkOrderPhase.Finalization ||
                              DetailView.Phase == WorkOrderPhase.Approval ||
                              DetailView.Phase == WorkOrderPhase.StockApproval)
                                 ? DetailViewMode.Edit : DetailViewMode.ReadOnly);
        }

        protected void Repository_EntityUpdated(object sender, EntityEventArgs<WorkOrder> e)
        {
            if ((DetailView.Phase == WorkOrderPhase.Approval || DetailView.Phase == WorkOrderPhase.General) && e.Entity.ApprovedOn.HasValue)
            {
                var order = Repository.Get(e.Entity.WorkOrderID); // in general the order in approval is not fully populated
                TryApprovalNotification(order);
            }
        }

        private void Repository_EntityInserted(object sender, EntityEventArgs<WorkOrder> e)
        {
            if (DetailView.Phase == WorkOrderPhase.Input)
            {
                var notifications = new List<string>();
                var descript = e.Entity.WorkDescriptionID;

                if (WorkDescriptionRepository.MAIN_BREAKS.Contains(descript))
                {
                    notifications.Add(MAIN_BREAK_NOTIFICATION);
                    SendTownNotifications(DependencyResolver.Current.GetService<INotificationService>(), e.Entity, MAIN_BREAK_NOTIFICATION);
                }
                else if (WorkDescriptionRepository.SERVICE_LINE_INSTALLATIONS.Contains(descript))
                {
                    notifications.Add(SERVICE_LINE_INSTALLATION_ENTERED_NOTIFICATION);
                }
                else if (WorkDescriptionRepository.SERVICE_LINE_RENEWALS.Contains(descript))
                {
                    notifications.Add(SERVICE_LINE_RENEWAL_ENTERED_NOTIFICATION);
                }
                else if (WorkDescriptionRepository.SEWER_OVERFLOW.Contains(descript))
                {
                    notifications.Add(SEWER_OVERFLOW_ENTERED_NOTIFICATION);
                }

                // This not an else if. 

                if (AssetTypeRepository.Indices.EQUIPMENT == e.Entity.AssetTypeID)
                {
                    notifications.Add(EQUIPMENT_REPAIR);
                }

                if (notifications.Any())
                {
                    var notifier =
                        DependencyResolver.Current.GetService<INotificationService>();

                    foreach (var notification in notifications)
                    {
                        if (descript == WorkDescription.SERVICE_LINE_RENEWAL_LEAD)
                        {
                            notifier.Notify(e.Entity.OperatingCenterID.Value,
                                RoleModules.FieldServicesWorkManagement,
                                SERVICE_LINE_RENEWAL_LEAD_ENTERED_NOTIFICATION,
                                e.Entity);
                        }
                        else
                        {
                            notifier.Notify(e.Entity.OperatingCenterID.Value,
                                RoleModules.FieldServicesWorkManagement,
                                notification,
                                e.Entity);
                        }
                    }
                }
                WorkOrderRepository.UpdateSAPWorkOrder(e.Entity);
            }
        }

        protected override void View_Updating(object sender, EntityEventArgs<WorkOrder> e)
        {
            switch (DetailView.Phase)
            {
                case WorkOrderPhase.Approval:
                    UpdateForApproval(e.Entity);
                    break;
                case WorkOrderPhase.General:
                    MaybeDefaultDigitalAsBuiltRequired(e.Entity);

                    if (HasNewAssetType(e.Entity))
                    {
                        UpdateWithNewAssetType(e.Entity);
                    }
                    else if (HasTownSectionRemoved(e.Entity))
                    {
                        UpdateWithNoTownSection(e.Entity);
                    }
                    else
                    {
                        FixNotes(e.Entity);
                        goto default;
                    }

                    break;
                case WorkOrderPhase.Finalization:
                    FixNotes(e.Entity);
                    MaybeDefaultDigitalAsBuiltRequired(e.Entity);
                    UpdateEntityConsideringSAP(e.Entity);
                    TryCompletedNotification(e.Entity);
                    
                    // Something with the ListView's selectedIndex is making this switch the Repository's SelectedDataKey
                    if (e.Entity.WorkOrderID != Parse(Repository.SelectedDataKey.ToString()))
                        Repository.SetSelectedDataKey(e.Entity.WorkOrderID);
                    UpdateAfterCompletion(e.Entity);
                    break;
                default:
                    UpdateEntityConsideringSAP(e.Entity);
                    TryMaterialApprovalFix(e.Entity);
                    break;
            }

            LoadCurrentEntityOnView();
            DetailView.SetViewMode(
                e.Entity.WorkCompleted && DetailView.Phase != WorkOrderPhase.Approval
                    ? DetailViewMode.ReadOnly
                    : DetailViewMode.Edit);
        }

        protected override void View_DeleteClicked(object sender, EntityEventArgs<WorkOrder> e)
        {
            if (DetailView.Phase == WorkOrderPhase.General)
            {
                var entity = e.Entity;
                entity.CancelledByID = entity.CancelledByID.HasValue
                    ? (int?)null
                    : SecurityService.Instance.GetEmployeeID();
                entity.CancelledAt = entity.CancelledAt.HasValue
                    ? (DateTime?)null
                    : DependencyResolver.Current.GetService<IDateTimeProvider>()
                        .GetCurrentDate();

                UpdateEntityConsideringSAP(e.Entity);
            }
            else
            {
                base.View_DeleteClicked(sender, e);
            }
        }

        #endregion

        #region Event Passthroughs

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if (Repository != null)
            {
                Repository.EntityUpdated += Repository_EntityUpdated;
                Repository.EntityInserted += Repository_EntityInserted;
            }
        }

        #endregion

        #region Event Passthroughs

        #endregion

        #region Exposed Methods

        public override void OnViewInitialized()
        {
            if (View != null && !DetailView.ModeSet)
            {
                View.SetViewControlsVisible(false);

                switch (DetailView.Phase)
                {
                    case WorkOrderPhase.Input:
                        View.SetViewMode(DetailViewMode.Insert);
                        break;
                    case WorkOrderPhase.Finalization:
                        View.SetViewMode(DetailViewMode.Edit);
                        break;
                }
            }
        }

        #endregion
    }
}
