using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class WorkOrderController
        : SapSyncronizedControllerBaseWithPersisence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        public struct NotificationPurposes
        {
            public const string
                ACOUSTIC_MONITORING_CREATED = "Acoustic Monitoring Order Created",
                EQUIPMENT_REPAIR = "Equipment Repair",
                FRCC_EMERGENCY_COMPLETED = "FRCC Emergency Completed",
                FRCC_EMERGENCY_CREATED = "FRCC Emergency Created",
                MAIN_BREAK_COMPLETED = "Main Break Completed",
                MAIN_BREAK = "Main Break Entered",
                SERVICE_LINE_INSTALLATION_ENTERED = "Service Line Installation Entered",
                SERVICE_LINE_RENEWAL_ENTERED = "Service Line Renewal Entered",
                SERVICE_LINE_RENEWAL_LEAD_ENTERED = "Service Line Renewal Lead Entered",
                SERVICE_LINE_RENEWAL_COMPLETED = "Service Line Renewal Completed",
                SEWER_OVERFLOW_ENTERED = "Sewer Overflow Entered",
                SUPERVISOR_APPROVAL = "Supervisor Approval",
                SAMPLE_SITE_NOTIFICATION = "Work Order With Sample Site";
        }

        public const string
            NOT_FOUND = "WorkOrder with the id '{0}' was not found.",
            WORK_DESCRIPTIONS_VIEWDATA_KEY = "WorkDescriptions";
        
        #endregion

        #region Private Members

        private readonly INotificationService _notificationService;

        #endregion

        #region Private Methods

        private void SendCreateNotificationMaybe(CreateWorkOrder viewModel)
        {
            var workDescriptionRegistry = new[] {
                new Tuple<int[], string>(
                    WorkDescriptionRepository.MAIN_BREAKS,
                    NotificationPurposes.MAIN_BREAK),
                new Tuple<int[], string>(
                    WorkDescriptionRepository.SERVICE_LINE_INSTALLATIONS,
                    NotificationPurposes.SERVICE_LINE_INSTALLATION_ENTERED),
                new Tuple<int[], string>(
                    WorkDescriptionRepository.SERVICE_LINE_RENEWALS,
                    NotificationPurposes.SERVICE_LINE_RENEWAL_ENTERED),
                new Tuple<int[], string>(
                    WorkDescriptionRepository.SEWER_OVERFLOW,
                    NotificationPurposes.SEWER_OVERFLOW_ENTERED),
            };

            var notifications = (from tup in workDescriptionRegistry
                                 where tup.Item1.Contains(viewModel.WorkDescription.Value)
                                 select tup.Item2).ToList();

            if (viewModel.AssetType == AssetType.Indices.EQUIPMENT)
            {
                notifications.Add(NotificationPurposes.EQUIPMENT_REPAIR);
            }

            var entity = Repository.Find(viewModel.Id);

            entity.RecordUrl = GetUrlForModel(entity, "Show", "WorkOrder", "FieldOperations");

            foreach (var notification in notifications)
            {
                var args = new NotifierArgs {
                    OperatingCenterId = viewModel.OperatingCenter.Value,
                    Module = ROLE,
                    Purpose = notification,
                    Data = entity
                };
                if (viewModel.WorkDescription.Value == (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL_LEAD)
                {
                    args.Purpose = NotificationPurposes.SERVICE_LINE_RENEWAL_LEAD_ENTERED;
                }
                _notificationService.Notify(args);
            }

            if (WorkDescriptionRepository.MAIN_BREAKS.Contains(viewModel.WorkDescription.Value))
            {
                foreach (var contact in
                         entity
                            .Town.TownContacts
                            .Where(tc => tc.ContactType.Id == ContactType.Indices.MAIN_BREAK_NOTIFICATION))
                {
                    _notificationService.Notify(
                        viewModel.OperatingCenter.Value,
                        ROLE,
                        NotificationPurposes.MAIN_BREAK,
                        entity,
                        address: contact.Contact.Email);
                }
            }
            if (entity.HasSampleSite == true)
            {
                _notificationService.Notify(
                    entity.OperatingCenter.Id,
                    ROLE,
                    NotificationPurposes.SAMPLE_SITE_NOTIFICATION,
                    entity);
            }

            if (viewModel.RequestedBy == WorkOrderRequester.Indices.ACOUSTIC_MONITORING)
            {
                _notificationService.Notify(viewModel.OperatingCenter.Value, ROLE, NotificationPurposes.ACOUSTIC_MONITORING_CREATED, entity);
            }

            // FRCC Emergency Orders
            if (entity.RequestedBy.Id == WorkOrderRequester.Indices.FRCC &&
                entity.Priority.Id == (int)WorkOrderPriority.Indices.EMERGENCY)
            {
                _notificationService.Notify(
                    viewModel.OperatingCenter.Value,
                    ROLE,
                    NotificationPurposes.FRCC_EMERGENCY_CREATED,
                    entity);
            }
        }

        private void SendEditNotificationMaybe(EditWorkOrder viewModel)
        {
            var entity = Repository.Find(viewModel.Id);
            if (entity.HasSampleSite == true)
            {
                _notificationService.Notify(
                    entity.OperatingCenter.Id,
                    ROLE,
                    NotificationPurposes.SAMPLE_SITE_NOTIFICATION,
                    entity);
            }
        }

        protected override void UpdateEntityForSap(WorkOrder entity)
        {
            entity.UserId = AuthenticationService.CurrentUser.UserName;

            if (entity.SAPWorkOrderNumber.HasValue && entity.SAPWorkOrderStep != null &&
                (entity.SAPWorkOrderStep.Id == SAPWorkOrderStep.Indices.CREATE ||
                 entity.SAPWorkOrderStep.Id == SAPWorkOrderStep.Indices.UPDATE))
            {
                entity.SAPWorkOrderStep =
                    _container.GetInstance<IRepository<SAPWorkOrderStep>>()
                              .Find(SAPWorkOrderStep.Indices.UPDATE);
                var workOrder = new SAPProgressWorkOrder(entity);
                var sapWorkOrder = _container.GetInstance<ISAPWorkOrderRepository>().Update(workOrder);
                entity.BusinessUnit = sapWorkOrder.CostCenter;
                if (!string.IsNullOrWhiteSpace(sapWorkOrder.OrderNumber))
                    entity.SAPWorkOrderNumber = long.Parse(sapWorkOrder.OrderNumber);
                if (!string.IsNullOrWhiteSpace(sapWorkOrder.WBSElement))
                    entity.AccountCharged = sapWorkOrder.WBSElement;
                if ((!entity.SAPNotificationNumber.HasValue || entity.SAPNotificationNumber == 0) &&
                    !string.IsNullOrEmpty(sapWorkOrder.NotificationNumber))
                    entity.SAPNotificationNumber = long.Parse(sapWorkOrder.NotificationNumber);
                if (!string.IsNullOrEmpty(sapWorkOrder.MaterialDocument))
                    entity.MaterialsDocID = sapWorkOrder.MaterialDocument;
                entity.SAPErrorCode = sapWorkOrder.Status;
            }
            else
            {
                entity.SAPWorkOrderStep =
                    _container.GetInstance<IRepository<SAPWorkOrderStep>>()
                              .Find(SAPWorkOrderStep.Indices.CREATE);

                var workOrder = new SAPWorkOrder(entity);
                var sapWorkOrder = _container.GetInstance<ISAPWorkOrderRepository>().Save(workOrder);
                entity.BusinessUnit = sapWorkOrder.CostCenter;
                if (!string.IsNullOrWhiteSpace(sapWorkOrder.OrderNumber))
                    entity.SAPWorkOrderNumber = long.Parse(sapWorkOrder.OrderNumber);
                if (!string.IsNullOrWhiteSpace(sapWorkOrder.WBSElement))
                    entity.AccountCharged = sapWorkOrder.WBSElement;
                if ((!entity.SAPNotificationNumber.HasValue || entity.SAPNotificationNumber == 0) &&
                    !string.IsNullOrEmpty(sapWorkOrder.NotificationNumber))
                    entity.SAPNotificationNumber = long.Parse(sapWorkOrder.NotificationNumber);
                if (!string.IsNullOrEmpty(sapWorkOrder.EquipmentNo))
                {
                    entity.SAPEquipmentNumber = long.Parse(sapWorkOrder.EquipmentNo);
                }
                entity.SAPErrorCode = sapWorkOrder.SAPErrorCode;
                // if this was just created successfully, we need to change the step to update
                if (entity.SAPErrorCode.ToLower().EndsWith("successfully"))
                {
                    entity.SAPWorkOrderStep = new SAPWorkOrderStep { Id = SAPWorkOrderStep.Indices.UPDATE };
                }
            }
        }

        
        
        private void SwapEntityLookupIdForName<TEntityLookup>(RouteValueDictionary rvd, string param)
            where TEntityLookup : IEntityLookup
        {
            if (rvd.ContainsKey(param))
            {
                var paramValue = rvd[param] as int?;
                if (paramValue.HasValue)
                {
                    rvd[param] = _container.GetInstance<IRepository<TEntityLookup>>()
                                           .Find(paramValue.Value).Description;
                }
            }
        }

        private bool TryCreatingService(int workOrderId)
        {
            var workOrder = Repository.Find(workOrderId);

            if (workOrder != null && workOrder.WorkDescription != null &&
                WorkDescription.AUTO_CREATE_SERVICE_WORK_DESCRIPTIONS.Contains(workOrder.WorkDescription.Id))
            {
                var model = new CreateService(_container, workOrder);

                if (!TryValidateModel(model))
                {
                    return false;
                }

                var service = model.MapToEntity(new Service());

                service = _container.GetInstance<IServiceRepository>().Save(service);

                workOrder.Service = service;

                Repository.Update(workOrder);

                return true;
            }

            return true;
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownData(oc => oc.WorkOrdersEnabled);
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
                    ViewData[WORK_DESCRIPTIONS_VIEWDATA_KEY] =
                        _container
                           .GetInstance<IRepository<WorkDescription>>()
                           .Where(x => x.IsActive)
                           .Select(x => WorkDescription.WorkDescriptionToJson(x))
                           .ToList();
                    break;
                case ControllerAction.Edit:
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
                    ViewData[WORK_DESCRIPTIONS_VIEWDATA_KEY] =
                        _container
                           .GetInstance<IRepository<WorkDescription>>()
                           .Where(x => x.IsActive)
                           .Select(x => WorkDescription.WorkDescriptionToJson(x))
                           .ToList();
                    break;
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<WorkDescription>();
                    this.AddDropDownData<WorkOrderRequester>("RequestedBy");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => RedirectToAction("Show", "GeneralWorkOrder", new { area = "FieldOperations", id = id }));
                formatter.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true, 
                    ViewName = "_ShowPopup"
                }));
                formatter.Json(() => {
                    var order = Repository.Find(id);

                    return order == null
                        ? (ActionResult)DoHttpNotFound($"WorkOrder with id {id} not found.")
                        : Json(new {
                            Data = order.ToJSONObject()
                        }, JsonRequestBehavior.AllowGet);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(CreateWorkOrder model)
        {
            if (model.CoordinateId == null && model.Latitude.HasValue && model.Longitude.HasValue)
            {
                var mapIcon = _container
                             .GetInstance<IRepository<MapIcon>>().Find(MapIcon.Indices.WorkOrder);
                var coordinate =
                    _container
                       .GetInstance<IRepository<Coordinate>>()
                       .Save(new Coordinate {
                            Latitude = model.Latitude.Value,
                            Longitude = model.Longitude.Value,
                            Icon = mapIcon
                        });
                model.CoordinateId = coordinate.Id;
            }

            if (model.SAPNotificationNumber != null)
            {
                model.Notes = 
                    TempData[SapNotificationController.TEMP_DATA_CREATE_WORK_ORDER_NOTES]?.ToString() ??
                    string.Empty;

                model.SpecialInstructions =
                    TempData[SapNotificationController.TEMP_DATA_CREATE_WORK_ORDER_SPECIAL_INSTRUCTIONS]?.ToString() ??
                    string.Empty;
            }
            
            // Is DateReceived passed in at some point?
            model.DateReceived = model.DateReceived ??
                                 _container.GetInstance<IDateTimeProvider>()
                                           .GetCurrentDate().BeginningOfDay(); 

            ModelState.Clear();
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateWorkOrder model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }
                    SendCreateNotificationMaybe(model);

                    if (!TryCreatingService(model.Id))
                    {
                        return RedirectToAction("NewFromWorkOrder", "Service", new { area = "FieldOperations", workOrderId = model.Id });
                    }

                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            var workOrder = Repository.Find(id);
            if (workOrder == null)
                return DoHttpNotFound(string.Format(NOT_FOUND, id));

            if (workOrder.SAPWorkOrderStep != null &&
                (workOrder.SAPWorkOrderStep.Id == SAPWorkOrderStep.Indices.APPROVE_GOODS ||
                 workOrder.SAPWorkOrderStep.Id == SAPWorkOrderStep.Indices.COMPLETE))
                return RedirectToAction("Show", new { id = workOrder.Id});
            return ActionHelper.DoEdit<EditWorkOrder>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }
                    SendEditNotificationMaybe(model);
                    return RedirectToAction("Edit", "GeneralWorkOrder", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult CompleteMaterialPlanning(CompleteMaterialPlanning model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    UpdateSAP(model.Id, ROLE);
                    // Handle this if it fails to update SAP properly
                    var order = Repository.Find(model.Id);
                    if (order.SAPErrorCode != null && !order.SAPErrorCode.ToUpper().Contains("SUCCESS"))
                    {
                        order.MaterialPlanningCompletedOn = null;
                        Repository.Save(order);
                    }
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Cancel(CancelWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var order = Repository.Find(model.Id);
                    if (order.IsSAPUpdatableWorkOrder)
                    {
                        UpdateSAP(model.Id, ROLE);
                        // Handle this if it fails to update SAP properly
                        order = Repository.Find(model.Id);
                        if (order.SAPErrorCode != null && !order.SAPErrorCode.ToUpper().Contains("SUCCESS"))
                        {
                            order.CancelledAt = null;
                            order.WorkOrderCancellationReason = null;
                            Repository.Save(order);
                            DisplayErrorMessage("SAP Error: " + order.SAPErrorCode);
                        }
                    }

                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region ByTownIdForServices

        [HttpGet]
        public ActionResult ByTownIdForServices(int id)
        {
            var results = Repository.GetByTownIdForServices(id);
            return new CascadingActionResult(results, "Id", "Id") { SortItemsByTextField = false};
        }

        [HttpGet]
        public ActionResult ByTownIdForMainBreaks(int id)
        {
            var results = Repository.GetByTownIdForMainBreaks(id);
            return new CascadingActionResult(results, "Id", "Id") { SortItemsByTextField = false };
        }

        [HttpGet]
        public ActionResult ByTownId(int id)
        {
            var results = Repository.GetByTownId(id);
            return new CascadingActionResult(results, "Id", "Id") { SortItemsByTextField = false };
        }

        #endregion

        #region ByWorkOrderId

        [HttpGet]
        public ActionResult ByWorkOrderId(SearchWorkOrderId model)
        {
            var result = new Dictionary<string, object>();
            result["success"] = false;

            if (!ModelState.IsValid)
            {
                result["message"] = "Invalid search parameters.";
            }
            else
            {
                var workOrder = Repository.Find(model.WorkOrderId.Value);
                if (workOrder == null)
                {
                    result["message"] = "There are no work orders that match this WorkOrderID";
                }
                else
                {
                    result["location"] = $"{workOrder.StreetAddress} {workOrder.TownAddress}";
                    result["workOrderId"] = workOrder.Id;
                    result["operatingCenterId"] = workOrder.OperatingCenter.Id;
                    result["latitude"] = workOrder.Latitude;
                    result["longitude"] = workOrder.Longitude;
                    result["description"] = workOrder.WorkDescription.ToString();
                    if (workOrder.AssignedContractor != null)
                        result["jobCategoryId"] = JobCategory.Indices.CONTRACTOR;
                    else
                        result["jobCategoryId"] = JobCategory.Indices.T_AND_D;
                    result["success"] = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FindByPartialWorkOrderIDMatch

        [HttpGet]
        public ActionResult FindByPartialWorkOrderIDMatch(string workOrderID)
        {
            var results = Repository.FindByPartialWorkOrderIDMatch(workOrderID);

            return new AutoCompleteResult(results, "Id", "Id");
        }

        #endregion

        #region FindBySAPWorkOrderNumber

        [HttpPost, RequiresSecureForm(false)]
        public ActionResult FindBySAPWorkOrderNumber(long? sapWorkOrderNumber)
        {
            if (!sapWorkOrderNumber.HasValue)
            {
                return Json(new {success = false,});
            }

            var result = Repository.FindBySAPWorkOrderNumber(sapWorkOrderNumber.Value);
            if (result == null)
            {
                return Json(new {success = false,});
            }

            var resultModel = new Dictionary<string, object>();
            resultModel["success"] = true;
            resultModel["workOrderId"] = result.Id;
            resultModel["operatingCenterId"] = result.OperatingCenter.Id;
            resultModel["address"] = result.StreetAddress + ", " + result.TownAddress;

            var coord = new Coordinate {
                Latitude = Convert.ToDecimal(result.Latitude),
                Longitude = Convert.ToDecimal(result.Longitude),
                Icon = _container.GetInstance<IIconSetRepository>().GetDefaultIconSet(
                    _container.GetInstance<IRepository<MapIcon>>()).DefaultIcon
            };

            _container.GetInstance<IRepository<Coordinate>>().Save(coord);

            resultModel["coordinateId"] = coord.Id;

            return Json(resultModel);
        }

        #endregion

        #region GetAccountingCode

        [HttpGet]
        public ActionResult GetAccountingCode(int id)
        {
            var workOrder = Repository.Find(id);
            if (workOrder == null)
                return HttpNotFound();

            return Json(new {accountingCode = workOrder.AccountCharged}, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region History

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult History(HistoryWorkOrder search)
        {
            HistoryWorkOrder.FixAssetInfo(search);

            return ActionHelper.DoIndex(search,
                new ActionHelperDoIndexArgs {
                    IsPartial = true,
                    ViewName = "History",
                    OnNoResults = () => PartialView("History", search)
                });
        }

        #endregion

        public WorkOrderController(
            ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args,
            INotificationService notificationService)
            : base(args)
        {
            _notificationService = notificationService;
        }
    }
}