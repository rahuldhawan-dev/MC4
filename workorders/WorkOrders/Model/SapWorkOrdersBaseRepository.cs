using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using StructureMap;

namespace WorkOrders.Model
{
    // WorkOrderDetailPresenter handles all the work order updating already.
    // The rest will go through their own repositories. The views need to be updated to ensure
    // they are using the repository methods, not the base methods of the repository.
    // CrewAssignmentRepository - uses presenter
    // MaterialsUsedRepository - uses MvpObjectDataSource
    // MainBreakRepository - uses MvpObjectDataSource
    // no reason workorder repo can't use this so they all update sap the same and do the error emailings
    public abstract class SapWorkOrdersBaseRepository<TEntity> : WorkOrdersRepository<TEntity>
        where TEntity : class, new()
    {
        #region Constants

        public const string SAP_ERROR_CODE = "SAPErrorCode Occurred",
            SAP_UPDATE_FAILURE = "RETRY::UPDATE FAILURE: {0}";


        #endregion

        #region Private Members

        private IUser _currentUser;
        private IAuditLogEntryRepository _logger;

        #endregion

        #region Properties

        public IUser CurrentUser
        {
            get { return _currentUser ?? SecurityService.CurrentUser; }
            set { _currentUser = value; }
        }

        public IAuditLogEntryRepository Logger
        {
            get
            {
                return _logger ?? (_logger = DependencyResolver.Current.GetService<IAuditLogEntryRepository>());
            }
            set { _logger = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Use the existing work order and the changes that are being made to it to determine
        /// what step of the SAP process we need to notify
        /// </summary>
        /// <param name="workOrder">Existing Order</param>
        /// <param name="entity">Updated Order</param>
        /// <returns></returns>
        private int? GetSAPWorkOrderStep(MapCall.Common.Model.Entities.WorkOrder existingWorkOrder, WorkOrder updatedWorkOrder)
        {
            var ret = SAPWorkOrderStep.Indices.UPDATE;

            // if there's no previosu sap work order number, then it's a create call to sap
            if (!existingWorkOrder.SAPWorkOrderNumber.HasValue || existingWorkOrder.SAPWorkOrderNumber == 0)
                ret = SAPWorkOrderStep.Indices.CREATE;

            // are we approving materials?
            if (!updatedWorkOrder.CancelledAt.HasValue && !existingWorkOrder.MaterialsApprovedOn.HasValue && updatedWorkOrder.MaterialsApprovedOn.HasValue)
                ret = SAPWorkOrderStep.Indices.APPROVE_GOODS;

            // are we approving and truely completing the work ??
            if (!existingWorkOrder.ApprovedOn.HasValue && updatedWorkOrder.ApprovedOn.HasValue)
                ret = SAPWorkOrderStep.Indices.COMPLETE;

            // ok, but hold on a second. what if there was an error last time around and someone is just try to get it 
            // to resubmit to the same step as last time?
            // create will not have updated it at all so create will just go through to create again
            // if it was just an update before, then lets just update again
            // we're really just concerned with Complete and Stock
            if (existingWorkOrder.HasSAPErrorCode == true)
            {
                if (existingWorkOrder.SAPWorkOrderStep != null && existingWorkOrder.SAPWorkOrderStep.Id == SAPWorkOrderStep.Indices.COMPLETE)
                    ret = SAPWorkOrderStep.Indices.COMPLETE;
                if (existingWorkOrder.SAPWorkOrderStep != null && existingWorkOrder.SAPWorkOrderStep.Id == SAPWorkOrderStep.Indices.APPROVE_GOODS)
                    ret = SAPWorkOrderStep.Indices.APPROVE_GOODS;
            }

            return ret;
        }

        // Does this require an SAP update?
        private bool RequiresSapUpdate(MapCall.Common.Model.Entities.WorkOrder workOrder, WorkOrder entity)
        {
            // if we think we're creating but also cancelling, lets just get out because sap doesn't know about us
            if (entity.SAPWorkOrderStepID == SAPWorkOrderStep.Indices.CREATE && entity.CancelledAt.HasValue)
                return false;

            // if we are at the approval step but we don't have an approved by/on
            if (entity.SAPWorkOrderStepID == SAPWorkOrderStep.Indices.COMPLETE && (entity.ApprovedOn == null || entity.ApprovedByID == null))
                return false;

            // if we're at the stock to issue step but we don't have a stock issued by/on
            if (entity.SAPWorkOrderStepID == SAPWorkOrderStep.Indices.APPROVE_GOODS && (entity.MaterialsApprovedOn == null || entity.MaterialsApprovedByID == null))
                return false;

            // if we had an SAP error occur, we want to update.
            if (workOrder.HasSAPErrorCode == true)
                return true;

            // what if we've already approved materials?
            if (workOrder.MaterialsApprovedOn.HasValue)
                return false;

            // what if we've approved and have no materials to approve?
            if (workOrder.ApprovedOn.HasValue && !workOrder.MaterialsUsed.Any())
                return false;

            return true;
        }

        private void TryUpdateServiceInstallation(MapCall.Common.Model.Entities.WorkOrder workOrder, WorkOrder entity)
        {
            var workDescriptionId = workOrder.WorkDescription?.Id ?? entity.WorkDescriptionID;
            if (workOrder.ServiceInstallations != null && workOrder.ServiceInstallations.Any()
                && WorkDescriptionRepository.NEW_SERVICE_INSTALLATIONS.Contains(workDescriptionId))
            {
                // we've gotten this far, we need to mark it correctly for the scheduler in case it fails now.
                entity.SAPWorkOrderStepID = SAPWorkOrderStep.Indices.NMI; 
                var repo = DependencyResolver.Current.GetService<ISAPNewServiceInstallationRepository>();
                var result = repo.Save(new SAPNewServiceInstallation(workOrder.ServiceInstallations.FirstOrDefault()));
                AddAuditLogEntry("SAPUpdate", entity.Id, "MaterialDocID", "CalledUpdateWithNMI", entity.MaterialsDocID);
                AddAuditLogEntry("SAPUpdate", entity.Id, "SAPErrorCode", "CalledUpdateWithNMI", result.SAPStatus);

                workOrder.SAPErrorCode = result.SAPStatus;
                entity.SAPErrorCode = workOrder.SAPErrorCode;
            }
        }

        private void AddAuditLogEntry(string auditEntryType, int entityId, string fieldName, string oldValue, string newValue)
        {
            Logger.Save(new AuditLogEntry
            {
                AuditEntryType = auditEntryType,
                EntityId = entityId,
                EntityName = "WorkOrder",
                FieldName = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                Timestamp = DateTime.Now,
                User = new User
                {
                    Id = SecurityService.GetEmployeeID()
                }
            });
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// TODO: Refactor - It's fully tested atm.
        /// </summary>
        /// <param name="entityFrom271"></param>
        /// <param name="workOrder"></param>
        public void UpdateSAPWorkOrder(WorkOrder entityFrom271, MapCall.Common.Model.Entities.WorkOrder workOrder = null)
        {
            // NOTE: The workOrder variable may or may not have updated values because any changes made to entityFrom271
            // may not have been saved to the db yet. You need to trace where it's coming from in the code 
            // and follow all that logic to determine if it has old values or not.

            // Get a MapCall.Common General Work Order Repository
            // SAP doesn't know how to handle 271 WorkOrders
            var commonWorkOrderRepository = DependencyResolver.Current.GetService<IGeneralWorkOrderRepository>();

            // if we don't have a general work order, go get it
            if (workOrder == null)
                workOrder = commonWorkOrderRepository.Find(entityFrom271.WorkOrderID);

            // lets do a few check to see if we should continue to update SAP at all
            if (!workOrder.OperatingCenter.SAPEnabled 
                || !workOrder.OperatingCenter.SAPWorkOrdersEnabled
                || workOrder.OperatingCenter.IsContractedOperations
                || workOrder.CancelledAt.HasValue)
                return;

            entityFrom271.SAPWorkOrderStepID = GetSAPWorkOrderStep(workOrder, entityFrom271);

            if (!RequiresSapUpdate(workOrder, entityFrom271))
                return;

            // we've made it, lets send this work order over to sap now.
            AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "SAPUpdateStart",  "Start Update", entityFrom271.MaterialsDocID);
            var sapRepo = DependencyResolver.Current.GetService<ISAPWorkOrderRepository>();
            try
            {
                // refresh the work order because we saved it above.
                var alreadyDateCompleted = workOrder.DateCompleted.HasValue;
                commonWorkOrderRepository.SessionEvict(workOrder);
                workOrder = commonWorkOrderRepository.Find(workOrder.Id);
                workOrder.UserId = CurrentUser.Name;
                switch (entityFrom271.SAPWorkOrderStepID)
                {
                    case SAPWorkOrderStep.Indices.CREATE:
                        var createSapWorkOrder = sapRepo.Save(new SAPWorkOrder(workOrder));
                        AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "MaterialDocID", "CalledCreateSAPOrder", entityFrom271.MaterialsDocID);
                        AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "SAPErrorCode", "CalledCreateSAPOrder", createSapWorkOrder.SAPErrorCode);
                        createSapWorkOrder.MapToWorkOrder(entityFrom271);
                        break;
                    case SAPWorkOrderStep.Indices.APPROVE_GOODS:
                        var sapGoodsIssue = new SAPGoodsIssue(workOrder);
                        if (sapGoodsIssue.sapMaterialsUsed == null || !sapGoodsIssue.sapMaterialsUsed.Any())
                            return;
                        var goodsIssued = sapRepo.Approve(sapGoodsIssue);
                        foreach(var item in goodsIssued.Items) { 
                            AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "MaterialDocId", "CalledApproveGoods", item.MaterialDocument);
                            AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "SAPErrorCode", "CalledApproveGoods", item.Status);
                        }
                        if (goodsIssued != null)
                            goodsIssued.MapToWorkOrder(entityFrom271);
                        break;
                    case SAPWorkOrderStep.Indices.COMPLETE:
                        var completeSapWorkOrder = sapRepo.Complete(new SAPCompleteWorkOrder(workOrder));
                        AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "MaterialDocId", "CalledComplete", entityFrom271.MaterialsDocID);
                        AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "SAPErrorCode", "CalledComplete", completeSapWorkOrder.Status);
                        completeSapWorkOrder.MapToWorkOrder(entityFrom271);
                        break;
                    case SAPWorkOrderStep.Indices.UPDATE:
                        var progressWorkOrder = new SAPProgressWorkOrder(workOrder);
                        if (alreadyDateCompleted)
                            progressWorkOrder.sapCrewAssignments = null;
                        var sapWorkOrder = sapRepo.Update(progressWorkOrder);
                        AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "MaterialDocID", "CalledUpdate", entityFrom271.MaterialsDocID);
                        AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "SAPErrorCode", "CalledUpdate", progressWorkOrder.Status);
                        sapWorkOrder.MapToWorkOrder(entityFrom271);
                        // If it's being completed and wasn't already completed before (i.e. Rejected)
                        if (!alreadyDateCompleted && entityFrom271.DateCompleted.HasValue && sapWorkOrder.Status.Contains("Successful"))
                        {
                            if (!entityFrom271.DateRejected.HasValue && !workOrder.DateRejected.HasValue)
                            {
                                // Mark this correctly for the scheduler in case it fails now. Scheduler has to update and new meter install
                                entityFrom271.SAPWorkOrderStepID =
                                    SAPWorkOrderStep.Indices.UPDATE_WITH_NMI;
                                TryUpdateServiceInstallation(workOrder, entityFrom271);
                            }
                        }
                        break;
                    default: // update
                        throw new InvalidOperationException("No SAPWorkOrderStep was selected for the work order.");
                }
            }

            catch (Exception ex)
            {
                entityFrom271.SAPErrorCode = string.Format(SAP_UPDATE_FAILURE, ex);
                AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "SAPUpdateError", "Error Occurred during Update", entityFrom271.MaterialsDocID);

            }

            // update the entity again because we've set some sap values.
            var workOrderRepository = DependencyResolver.Current.GetService<IRepository<WorkOrder>>();
            workOrderRepository.UpdateCurrentEntity(entityFrom271);

            // lets send notifications if they need to be sent
            SendErrorNotification(entityFrom271, workOrder.OperatingCenter.Id, RoleModules.FieldServicesWorkManagement); // shouldn't this be WorkManagement?

            AddAuditLogEntry("SAPUpdate", entityFrom271.Id, "SAPUpdateEnd", "End Update", entityFrom271.MaterialsDocID);
        }

        //TODO: Figure out why the Notifier doesn't think the user is authenticated
        //      Also why is this static and/or public?
        public static void SendErrorNotification(WorkOrder entity, int operatingCenterId, RoleModules module)
        {
#if !DEBUG
            // current user isn't authenticated for some reason.
            if (!string.IsNullOrWhiteSpace(entity.SAPErrorCode) && !entity.SAPErrorCode.StartsWith("RETRY") && !entity.SAPErrorCode.ToUpper().Contains("SUCCESSFUL"))
            {
                var notifier = DependencyResolver.Current.GetService<INotificationService>();
                notifier.Notify(operatingCenterId, module, SAP_ERROR_CODE, new {
                    RecordUrl = new SAPEntity().GetShowUrl("WorkOrder", entity.WorkOrderID),
                    SAPErrorCode = entity.SAPErrorCode
                });
            }
#endif
        }

        #endregion

        #region Exposed Static Methods?

        //TODO: KILL ME OFF SOMEHOW
        public static void UpdateSAPWorkOrderStatic(WorkOrder entity, MapCall.Common.Model.Entities.WorkOrder workOrder = null)
        {
            // create an instance of myself
            var repo = new WorkOrderRepository();
            // call my instance method
            repo.UpdateSAPWorkOrder(entity, workOrder);
            // cry
        }

        #endregion
    }
}