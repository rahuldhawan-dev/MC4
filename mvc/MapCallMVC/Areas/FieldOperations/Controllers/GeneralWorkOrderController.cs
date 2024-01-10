using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder;
using MapCallMVC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;
using MMSINC.Results;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class GeneralWorkOrderController : SapSyncronizedControllerBaseWithPersisence<IGeneralWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = WorkOrderController.ROLE;
        public const int MAX_INDEX_RESULTS = 3000;
        public const string MAIN_BREAK_NOTIFICATION = "Main Break Entered",
                            SAMPLE_SITE_NOTIFICATION = "Work Order With Sample Site",
                            SEWER_OVERFLOW_CHANGED_NOTIFICATION = "Sewer Overflow Changed",
                            WORK_DESCRIPTION_CHANGED = "Work Order Changed to Main Break Repair/Replace";

        #endregion

        #region Properties

        protected IGeneralWorkOrderRepository _repository;

        public override IGeneralWorkOrderRepository Repository => _repository ?? (_repository = _container.GetInstance<IGeneralWorkOrderRepository>());

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<WorkOrderRequester>("RequestedBy");
                    this.AddDropDownData<IDocumentTypeRepository, DocumentType>("DocumentTypes", 
                        r => r.GetByTableName(nameof(WorkOrder) + "s").OrderBy(x => x.Name), t => t.Id, t => t.Name);
                    break;
                case ControllerAction.Show:
                    this.AddDropDownData<WorkOrderCancellationReason>("WorkOrderCancellationReason");
                    break;
                case ControllerAction.Edit:
                    this.AddDropDownData<WorkOrderCancellationReason>("WorkOrderCancellationReason");
                    this.AddDropDownData<
                        IRepository<PlantMaintenanceActivityType>, PlantMaintenanceActivityTypeDisplay>(
                        "PlantMaintenanceActivityTypeOverride",
                        z => z.Where(x => PlantMaintenanceActivityType.GetOverrideCodes().Contains(x.Id))
                              .Select(t => new PlantMaintenanceActivityTypeDisplay {
                                   Id = t.Id,
                                   Description = t.Description,
                                   Code = t.Code
                               }),
                        r => r.Id,
                        r => r.Display);
                    this.AddDropDownData<ServiceMaterial>("PreviousServiceLineMaterial", d => d.Where(x => x.Description != "UNKNOWN"), d => d.Id, d => d.Description);
                    this.AddDropDownData<ServiceMaterial>("CompanyServiceLineMaterial", d => d.Where(x => x.IsEditEnabled), d => d.Id, d => d.Description);
                    this.AddDropDownData<ServiceMaterial>("CustomerServiceLineMaterial", d => d.Where(x => x.Description != "UNKNOWN"), d => d.Id, d => d.Description);
                    this.AddDropDownData<PitcherFilterCustomerDeliveryMethod>("PitcherFilterCustomerDeliveryMethod");
                    this.AddDropDownData<ServiceSize>("PreviousServiceLineSize", x => x.GetAllSorted(y => y.SortOrder).Where(z => z.Service), x => x.Id, x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceSize>("CompanyServiceLineSize", x => x.GetAllSorted(y => y.SortOrder).Where(z => z.Service), x => x.Id, x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceSize>("CustomerServiceLineSize", x => x.GetAllSorted(y => y.SortOrder).Where(z => z.Service), x => x.Id, x => x.ServiceSizeDescription);
                    this.AddDropDownData<CustomerImpactRange>("CustomerImpact");
                    this.AddDropDownData<RepairTimeRange>("RepairTime");
                    break;
            }
        }

        protected override void UpdateEntityForSap(WorkOrder entity)
        {
            var sapWorkOrderStep = SAPWorkOrderStep.Indices.UPDATE;

            // if there's no previous sap work order number, then it's a create call to sap
            if (!entity.SAPWorkOrderNumber.HasValue || entity.SAPWorkOrderNumber == 0)
            {
                sapWorkOrderStep = SAPWorkOrderStep.Indices.CREATE;
            }

            if (!RequiresSapUpdate(entity))
            {
                return;
            }

            AddAuditLogEntry("SAPUpdate", entity.Id, "SAPUpdateStart", "Start Update", entity.MaterialsDocID);
            var sapRepo = _container.GetInstance<ISAPWorkOrderRepository>();
            var sapWorkOrderStepRepo = _container.GetInstance<IRepository<SAPWorkOrderStep>>();

            entity.UserId = AuthenticationService.CurrentUser.UserName;
            entity.SAPWorkOrderStep = sapWorkOrderStepRepo.Find(sapWorkOrderStep);

            if (sapWorkOrderStep == SAPWorkOrderStep.Indices.CREATE)
            {
                var createSapWorkOrder = sapRepo.Save(new SAPWorkOrder(entity));
                AddAuditLogEntry("SAPUpdate", entity.Id, "MaterialDocID", "CalledCreateSAPOrder", entity.MaterialsDocID);
                AddAuditLogEntry("SAPUpdate", entity.Id, "SAPErrorCode", "CalledCreateSAPOrder", createSapWorkOrder.SAPErrorCode);
                createSapWorkOrder.MapToWorkOrder(entity);
                entity.BusinessUnit = createSapWorkOrder.CostCenter;
                if (!string.IsNullOrEmpty(createSapWorkOrder.EquipmentNo))
                {
                    entity.SAPEquipmentNumber = long.Parse(createSapWorkOrder.EquipmentNo);
                }
                // if this was just created successfully, we need to change the step to update
                if (entity.SAPErrorCode.ToLower().EndsWith("successfully"))
                {
                    entity.SAPWorkOrderStep = sapWorkOrderStepRepo.Find(SAPWorkOrderStep.Indices.UPDATE);
                }
                return;
            }

            var progressWorkOrder = new SAPProgressWorkOrder(entity);
            var sapWorkOrder = sapRepo.Update(progressWorkOrder);
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

        private bool RequiresSapUpdate(WorkOrder workOrder)
        {
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
                    notifier.Notify(args);

                    foreach (var contact in
                             entity.Town.TownContacts
                                   .Where(tc => tc.ContactType.Id == ContactType.Indices.MAIN_BREAK_NOTIFICATION))
                    {
                        args.Address = contact.Contact.Email;
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

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoShow(id));
                formatter.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true, 
                    ViewName = "../WorkOrder/_ShowPopup"
                }));
                formatter.Json(() => {
                    var order = Repository.Find(id);

                    return order == null
                        ? (ActionResult)DoHttpNotFound($"WorkOrder with id {id} not found.")
                        : Json(new {
                            Data = order.ToJSONObject()
                        }, JsonRequestBehavior.AllowGet);
                });
                formatter.Pdf(() => {
                    var model = Repository.Find(id);

                    if (model == null)
                    {
                        return HttpNotFound();
                    }

                    var viewPath = this.GetStateViewPath(model, "Pdf");

                    return new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), viewPath, model);
                });
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, new SearchWorkOrder {
                    Id = id
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchWorkOrder search)
        {
            // Otherwise it's giving validation errors right off the bat, which might not make any 
            // sense when form-state populates the previous search.
            ModelState.Clear();
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchWorkOrder search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => {
                    var args = new ActionHelperDoIndexArgs();
                    if (!search.EnablePaging)
                    {
                        args.MaxResults = MAX_INDEX_RESULTS;
                    }
                    return ActionHelper.DoIndex(search, args);
                });

                // NOTE: The fragment endpoint is used by the Permits/Show/_WorkOrders view.
                // MC-6480 fixes a bug where searching didn't actually filter by the user's roles.
                // Doug said it was fine that this will filter on the Premise/Show page.
                formatter.Fragment(() => {
                    search.EnablePaging = false;
                    return ActionHelper.DoIndex(search,
                        new ActionHelperDoIndexArgs {
                            IsPartial = true,
                            ViewName = "_WorkOrders",
                            OnNoResults = () => PartialView("_NoResults")
                        });
                });
                formatter.Excel(() => ActionHelper.DoExcel(search, new ActionHelperDoIndexArgs {
                    MaxResults = MAX_INDEX_RESULTS
                }));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditGeneralWorkOrderModel>(id, onModelFound: order => {
                if (order.WorkDescription != null &&
                    WorkDescription.GetMainBreakWorkDescriptions().Contains(order.WorkDescription.Id))
                {
                    DisplayNotification(order.Town.CriticalMainBreakNotes);
                }
            });
        }

        [HttpGet, RequiresAdmin, Crumb(Action = "Edit")]
        public ActionResult EditFromIndex(int id)
        {
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<WorkOrder, EditGeneralWorkOrder>() {
                ViewName = "_Edit"
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditGeneralWorkOrderModel model)
        {
            var workDescription = model.WorkOrder.WorkDescription;
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    if (entity.IsSAPUpdatableWorkOrder)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }
                    SendWorkDescriptionChangedNotifications(entity, workDescription?.Id);
                    SendSampleSiteNotification(entity);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresAdmin]
        public ActionResult UpdateFromIndex(EditGeneralWorkOrder model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult UpdateTrafficControl(EditTrafficControl model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home"),
                OnError = () => {
                    DisplayModelStateErrors();
                    return RedirectToReferrerOr("Index", "Home");
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult UpdateAdditional(EditWorkOrderAdditional model)
        {
            var workDescription = model.WorkOrder.WorkDescription;
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    if (entity.IsSAPUpdatableWorkOrder)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }
                    SendWorkDescriptionChangedNotifications(entity, workDescription?.Id);
                    SendSampleSiteNotification(entity);
                    return RedirectToReferrerOr("Index", "Home");
                },
                OnError = () => {
                    DisplayModelStateErrors();
                    return RedirectToReferrerOr("Index", "Home");
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult UpdateComplianceData(EditWorkOrderComplianceData model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home"),
                OnError = () => {
                    DisplayModelStateErrors();
                    return RedirectToReferrerOr("Index", "Home");
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult UpdateServiceLineInfo(EditServiceLineInfo model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home"),
                OnError = () => {
                    DisplayModelStateErrors();
                    return RedirectToReferrerOr("Index", "Home");
                }
            });
        }

        #endregion

        #region RemovedAssignedContractor

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemovedAssignedContractor(RemoveContractor model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs() {
                OnSuccess = () => {
                    return RedirectToAction("Edit", new { model.Id });
                }
            });
        }

        #endregion

        public GeneralWorkOrderController(ControllerBaseWithPersistenceArguments<IGeneralWorkOrderRepository, WorkOrder, User> args) : base(args) {}
    }
}