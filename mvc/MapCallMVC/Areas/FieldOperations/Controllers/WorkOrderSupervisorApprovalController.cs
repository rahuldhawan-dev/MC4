using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Controllers;
using MMSINC.Utilities;
using System.Web.Mvc;
using MapCallMVC.ClassExtensions;
using System.ComponentModel;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Helpers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.Entities;
using System.Linq;
using System;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderFinalization;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    [DisplayName("Work Order Supervisor Approval")]
    public class WorkOrderSupervisorApprovalController : SapSyncronizedControllerBaseWithPersisence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public struct SupervisorApprovalNotifications
        {
            public const string CURB_PIT_COMPLIANCE = "Curb-Pit Compliance",
                                CURB_PIT_REVENUE = "Curb-Pit Revenue",
                                CURB_PIT_ESTIMATE = "Curb-Pit Estimate",
                                ASSET_ORDER_COMPLETED = "Asset Order Completed",
                                REVISIT_WORK_ORDER_SUCCESS_MESSAGE = "Revisit work order <a href=\"{0}\" target=\"_blank\">{1}</a> created successfully!",
                                SAP_REVISIT_ORDER_ERROR_CODE = "RETRY::Created from Revisit";
        }

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        public const int MAX_RESULTS = 1000;

        #endregion

        #region Constructor

        public WorkOrderSupervisorApprovalController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion

        #region Private Methods

        protected override void UpdateEntityForSap(WorkOrder entity)
        {
            // NOTE: This controller *only* updates SAP for the APPROVE action.
            // Nothing needs to happens in SAP with the REJECT action.
            
            AddAuditLogEntry("SAPUpdate", entity.Id, "SAPUpdateStart", "Start Update", entity.MaterialsDocID);
            var sapCompletedStep = _container.GetInstance<IRepository<SAPWorkOrderStep>>().Find(SAPWorkOrderStep.Indices.COMPLETE);
            entity.SAPWorkOrderStep = sapCompletedStep;
            var sapRepo = _container.GetInstance<ISAPWorkOrderRepository>();
            var completeSapWorkOrder = sapRepo.Complete(new SAPCompleteWorkOrder(entity));
            completeSapWorkOrder.MapToWorkOrder(entity);
            AddAuditLogEntry("SAPUpdate", entity.Id, "MaterialDocId", "CalledComplete", entity.MaterialsDocID);
            AddAuditLogEntry("SAPUpdate", entity.Id, "SAPErrorCode", "CalledComplete", completeSapWorkOrder.Status);
            AddAuditLogEntry("SAPUpdate", entity.Id, "SAPUpdateEnd", "End Update", entity.MaterialsDocID);
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

        private void TryAssetNotification(WorkOrder order)
        {
            if (WorkDescriptionRepository.ASSET_COMPLETION.Contains(order.WorkDescription.Id))
            {
                order.RecordUrl = GetUrlForModel(order, "Show", "GeneralWorkOrder", "FieldOperations");
                this.SendNotification(order.OperatingCenter.Id, ROLE, SupervisorApprovalNotifications.ASSET_ORDER_COMPLETED, order);
            }
        }

        private void TryCurbPitNotification(WorkOrder order)
        {
            if (!WorkDescriptionRepository.CURB_PIT.Contains(order.WorkDescription.Id))
            {
                return;
            }

            string purposeNotification = null;

            var purpose = (WorkOrderPurpose.Indices)order.Purpose.Id;

            if (purpose == WorkOrderPurpose.Indices.COMPLIANCE)
            {
                purposeNotification = SupervisorApprovalNotifications.CURB_PIT_COMPLIANCE;
            }
            else if (purpose == WorkOrderPurpose.Indices.ESTIMATES)
            {
                purposeNotification = SupervisorApprovalNotifications.CURB_PIT_ESTIMATE;
            }
            else if (WorkOrderPurpose.REVENUE.Contains(purpose))
            {
                purposeNotification = SupervisorApprovalNotifications.CURB_PIT_REVENUE;
            }

            if (!String.IsNullOrEmpty(purposeNotification))
            {
                this.SendNotification(order.OperatingCenter.Id, ROLE, purposeNotification, order);
            }
        }

        private void CreateRevisitWorkOrder(WorkOrder originalWorkOrder)
        {
            if (originalWorkOrder.AssetType.Id == AssetType.Indices.SERVICE
                && originalWorkOrder.WorkDescription?.Id == (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION_PARTIAL)
            {
                // only create revisit work order if a revisit work order doesn't already exist
                if (Repository.FindByOriginalWorkOrderNumber(originalWorkOrder.Id) == null)
                {
                    var revisitWorkOrder = new CreateRevisitWorkOrderViewModel(_container) {
                        AssetType = AssetType.Indices.SERVICE,
                        WorkDescription = (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL,
                        OriginalOrderNumber = originalWorkOrder.Id,
                        OperatingCenter = originalWorkOrder.OperatingCenter?.Id,
                        Town = originalWorkOrder.Town?.Id,
                        RequestedBy = WorkOrderRequester.Indices.NSI,
                        Purpose = (int)WorkOrderPurpose.Indices.CUSTOMER,
                        Priority = (int)WorkOrderPriority.Indices.HIGH_PRIORITY,
                        MarkoutRequirement = (int)MarkoutRequirement.Indices.ROUTINE,
                        Notes = originalWorkOrder.Notes,
                        DigitalAsBuiltRequired = true,

                        // Service Id
                        DeviceLocation = originalWorkOrder.DeviceLocation,
                        SAPEquipmentNumber = originalWorkOrder.SAPEquipmentNumber,
                        Installation = originalWorkOrder.Installation,
                        PremiseNumber = originalWorkOrder.PremiseNumber,
                        ServiceNumber = originalWorkOrder.ServiceNumber,
                        Latitude = originalWorkOrder.Latitude,
                        Longitude = originalWorkOrder.Longitude,

                        // address
                        TownSection = originalWorkOrder.TownSection?.Id,
                        StreetNumber = originalWorkOrder.StreetNumber,
                        Street = originalWorkOrder.Street?.Id,
                        ApartmentAddtl = originalWorkOrder.ApartmentAddtl,
                        NearestCrossStreet = originalWorkOrder.NearestCrossStreet?.Id,
                        ZipCode = originalWorkOrder.ZipCode,

                        PlantMaintenanceActivityTypeOverride = originalWorkOrder.PlantMaintenanceActivityTypeOverride?.Id,
                        AccountCharged = originalWorkOrder.AccountCharged,

                        Service = originalWorkOrder.Service?.Id,

                        // sync to SAP
                        SAPErrorCode = SupervisorApprovalNotifications.SAP_REVISIT_ORDER_ERROR_CODE,
                        SAPWorkOrderStep = SAPWorkOrderStep.Indices.CREATE,
                        DateReceived = _dateTimeProvider.GetCurrentDate()
                    };

                    ActionHelper.DoCreate(revisitWorkOrder, new ActionHelperDoCreateArgs {
                        OnSuccess = () => {
                            var href = Url.Action("Show", "GeneralWorkOrder", new { id = revisitWorkOrder.Id });
                            DisplaySuccessMessage(string.Format(SupervisorApprovalNotifications.REVISIT_WORK_ORDER_SUCCESS_MESSAGE, href, revisitWorkOrder.Id));
                            return null;
                        }
                    });
                }
            }
        }

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownDataForRoleAndAction(RoleModules.FieldServicesWorkManagement,
                    extraFilterP: oc => oc.WorkOrdersEnabled);
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchWorkOrderSupervisorApproval());
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchWorkOrderSupervisorApproval search)
        {
            return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                MaxResults = MAX_RESULTS,
                SearchOverrideCallback = () => Repository.GetSupervisorApprovalWorkOrders(search)
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    GetEntityOverride = () => Repository.FindSupervisorApprovalWorkOrder(id)
                }));
                formatter.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true, 
                    ViewName = "../WorkOrder/_ShowPopup",
                    GetEntityOverride = () => Repository.FindSupervisorApprovalWorkOrder(id)
                }));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, () => {
                    var search = new SearchWorkOrderSupervisorApproval {
                        Id = id
                    };
                    return Repository.GetSupervisorApprovalWorkOrders(search);
                }));
            });
        }

        #endregion

        #region Approval/Rejection

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)] 
        public ActionResult Approve(SupervisorApproveWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                GetEntityOverride = () => Repository.FindSupervisorApprovalWorkOrder(model.Id),
                OnError = () => RedirectToAction("Show", new { id = model.Id }),
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    if (entity.IsSAPUpdatableWorkOrder)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }

                    TryCurbPitNotification(entity);
                    TryAssetNotification(entity);

                    CreateRevisitWorkOrder(entity);

                    // Approve can redirect back to the approval show page since approved
                    // work orders are searchable here.
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Reject(SupervisorRejectWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                GetEntityOverride = () => Repository.FindSupervisorApprovalWorkOrder(model.Id),
                OnError = () => RedirectToAction("Show", new { id = model.Id }),
                // Reject needs to redirect back to the general work order. Otherwise it'll
                // 404 since FindSupervisorApprovalWorkOrder won't return the record anymore.
                OnSuccess = () => RedirectToAction("Show", "GeneralWorkOrder", new { id = model.Id })
            });
        }

        #endregion

        #region Edit

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator), ActionBarVisible(false)]
        public void Edit(int id) { }

        #endregion
    }
}
