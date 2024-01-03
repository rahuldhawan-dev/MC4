using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class ProductionWorkOrderController : SapSyncronizedControllerBaseWithPersisence<IProductionWorkOrderRepository, ProductionWorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        public const string
            PERMIT_NOTIFICATION = "Permit Related Equipment Work Order Created",
            CREATE_NOTIFICATION = "Production Work Order Created",
            ASSIGNED_NOTIFICATION = "Production Work Order Assigned",
            COMPLETED_NOTIFICATION = "Production Work Order Completed",
            CONFINED_SPACE_PREREQUISITE =
                "A confined space form is required but has not been entered. Please use the confined space form tab to create one.",
            CONFINED_SPACE_FORM_CREATED = "Confined space form has been created.",
            CONFINED_SPACE_FORM_COMPLETED = "Confined space form completed.",
            CONFINED_SPACE_FORM_COMPLETED_WITH_PERMIT =
                "Confined Space Form Completed, Permitted Authorized, Work May Commence",
            CONFINED_SPACE_FORM_COMPLETED_WITHOUT_PERMIT =
                "Confined Space Form Completed, No Permitted Required, Work May Commence",
            LOCKOUT_FORM_PREREQUISITE =
                "A lockout form is required but has not been entered. Please use the lockout form tab to create one.",
            LOCKOUT_FORM_CREATED = "Lockout Form Created",
            LOCKOUT_FORM_COMPLETED = "Lockout Form Completed",
            RED_TAG_PERMIT_CREATED = "Red Tag Permit Created",
            RED_TAG_PERMIT_COMPLETED = "Red Tag Permit Completed",
            SUPERVISOR_APPROVAL_REQUIRED = "Supervisor Approval Required",
            AS_LEFT_CONDITION_NEEDS_EMERGENCY_REPAIR = "As Left Condition Needs Emergency Repair";

        #endregion

        #region Constructors

        public ProductionWorkOrderController(ControllerBaseWithPersistenceArguments<IProductionWorkOrderRepository, ProductionWorkOrder, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private void AddEmployeeDropDownData(string key, ControllerAction action, Expression<Func<Employee, bool>> adminFilterFn = null, Func<RoleMatch, Expression<Func<Employee, bool>>> roleMatchFilterFnFn = null)
        {
            RoleMatch roleMatch;
            var currentUser = AuthenticationService.CurrentUser;

            Expression<Func<Employee, bool>> filter;
            var activeOnly = !new[] { ControllerAction.Edit, ControllerAction.Search }.Contains(action);

            adminFilterFn = adminFilterFn ?? (_ => true);
            roleMatchFilterFnFn = roleMatchFilterFnFn ?? (rm => e => rm.OperatingCenters.Contains(e.OperatingCenter.Id));

            if (currentUser.IsAdmin || (roleMatch = currentUser.GetQueryableMatchingRoles(_container.GetInstance<IRepository<AggregateRole>>(), ROLE, action.ToRoleAction())).HasWildCardMatch)
            {
                filter = adminFilterFn;
            }
            else
            {
                filter = roleMatchFilterFnFn(roleMatch);
            }

            this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>(key, r =>
                activeOnly ? r.GetActiveEmployeesForSelectWhere(filter) : r.Where(filter));
        }

        private void AddEmployeeDropDownDataByUserRole(string key, ControllerAction action, OperatingCenter operatingCenter)
        {
            var roleAction = action.ToRoleAction();
            Expression<Func<Employee, bool>> filterFn = e => e.User != null && e.User.HasAccess && e.User.Roles.Any(r =>
                r.Module.Id == (int)ROLE &&
                new[] {
                        (int)RoleActions.Edit,
                        (int)RoleActions.UserAdministrator
                    }
                   .Contains(r.Action.Id) &&
                (r.OperatingCenter == null ||
                 r.OperatingCenter.Id ==
                 operatingCenter.Id));

            AddEmployeeDropDownData(key, ControllerAction.Edit,
                filterFn, _ => filterFn);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<OrderType>();
                    this.AddDropDownData<ProductionWorkOrderPriority>("Priority");
                    this.AddDropDownData<MaintenancePlanTaskType>(x => x.Where(y => y.IsActive), x => x.Id, x => x.Description);
                    this.AddDropDownData<TaskGroup>("TaskGroupName", x => x.Id, x => x.TaskGroupName);
                    break;
                case ControllerAction.Show:
                    this.AddDropDownData<ProductionWorkOrderCancellationReason>("CancellationReason");
                    this.AddDropDownData<ProductionPrerequisite>();
                    this.AddDropDownData<AsLeftCondition>();
                    this.AddDropDownData<AsFoundCondition>();
                    this.AddDropDownData<AssetConditionReason>("AsFoundConditionReason", d => d.Where(x => x.ConditionDescription.ConditionType.Id == (int)ConditionType.Indices.AS_FOUND), d => d.Id, d => d.Description);
                    this.AddDropDownData<AssetConditionReason>("AsLeftConditionReason", d => d.Where(x => x.ConditionDescription.ConditionType.Id == (int)ConditionType.Indices.AS_LEFT), d => d.Id, d => d.Description);
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
                    break;
                case ControllerAction.New:
                    AddEmployeeDropDownData("RequestedBy", action);
                    this.AddDropDownData<ProductionWorkOrderPriority>("Priority",
                        x => x.Where((z => z.Id != (int)ProductionWorkOrderPriority.Indices.ROUTINE && z.Id != (int)ProductionWorkOrderPriority.Indices.ROUTINE_OFF_SCHEDULED)), x => x.Id,
                        x => x.Description);
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Read, "OperatingCenter", x => x.IsActive);
                    this.AddDynamicDropDownData<PlantMaintenanceActivityType, PlantMaintenanceActivityTypeDisplay>(
                        r => r.Id,
                        r => r.Display,
                        "PlantMaintenanceActivityTypeOverride",
                        filter: x => x.Id == PlantMaintenanceActivityType.Indices.RBS);
                    this.AddDropDownData<ProductionWorkOrderFrequency>(x => x.GetAllSorted(y => y.Id), x => x.Id,
                        x => x.Name);
                    break;
                case ControllerAction.Edit:
                    AddEmployeeDropDownData("RequestedBy", action);
                    this.AddDropDownData<ProductionWorkOrderPriority>("Priority",
                        x => x.Where((z => z.Id != (int)ProductionWorkOrderPriority.Indices.ROUTINE && z.Id != (int)ProductionWorkOrderPriority.Indices.ROUTINE_OFF_SCHEDULED)), x => x.Id,
                        x => x.Description);
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Read);
                    this.AddDynamicDropDownData<PlantMaintenanceActivityType, PlantMaintenanceActivityTypeDisplay>(
                        r => r.Id,
                        r => r.Display,
                        "PlantMaintenanceActivityTypeOverride",
                        filter: x => x.Id == PlantMaintenanceActivityType.Indices.RBS);
                    this.AddDropDownData<ProductionWorkOrderFrequency>(x => x.GetAllSorted(y => y.Id), x => x.Id,
                        x => x.Name);
                    break;
            }
        }

        #endregion

        #region Notifications

        private void DisplayNotifications(ProductionWorkOrder model)
        {
            if (model.Equipment != null)
            {
                foreach (var permit in model.Equipment.EnvironmentalPermits)
                {
                    DisplayNotification(
                        $"This piece of equipment needs to continue to function within the requirements of permit number {permit.PermitNumber}");
                }
            }

            if (model.IsEligibleForRedTagPermit && model.NeedsRedTagPermitAuthorization.HasValue && model.RedTagPermit != null)
            {
                if (model.IsRedTagPermitStillOpen)
                {
                    DisplaySuccessMessage(RED_TAG_PERMIT_CREATED);
                }
                else
                {
                    DisplaySuccessMessage(RED_TAG_PERMIT_COMPLETED);
                }
            }

            if (model.LockoutFormRequired)
            {
                if (!model.LockoutForms.Any())
                {
                    DisplayNotification(LOCKOUT_FORM_PREREQUISITE);
                }
                else if (model.LockoutForms.Any(x => x.ReturnedToServiceDateTime == null))
                {
                    DisplaySuccessMessage(LOCKOUT_FORM_CREATED);
                }
                else
                {
                    DisplaySuccessMessage(LOCKOUT_FORM_COMPLETED);
                }
            }

            if (model.ConfinedSpaceFormRequired)
            {
                if (!model.ConfinedSpaceForms.Any())
                {
                    DisplayNotification(CONFINED_SPACE_PREREQUISITE);
                }

                // both of these should be success messages:
                // If completed, do the completion notification
                // if not completed but one exists, do the created notification
                // This is coming in another ticket from Lori.
                else
                {
                    DisplaySuccessMessage(CONFINED_SPACE_FORM_CREATED);
                }

                if (model.ConfinedSpaceForms.Any(x => x.IsCompleted && x.IsSection5Completed))
                {
                    DisplaySuccessMessage(CONFINED_SPACE_FORM_COMPLETED_WITH_PERMIT);
                }

                if (model.ConfinedSpaceForms.Any(x => x.IsCompleted && !x.IsSection5Completed))
                {
                    DisplaySuccessMessage(CONFINED_SPACE_FORM_COMPLETED_WITHOUT_PERMIT);
                }
            }

            DisplaySapErrorIfApplicable(model);
        }

        private void MaybeSendPermitRelatedNotification(ViewModel<ProductionWorkOrder> model)
        {
            var order = Repository.Find(model.Id);

            order.RecordUrl = GetUrlForModel(order, "Show", "ProductionWorkOrder", "Production");

            if (order.Equipments.Any())
            {
                foreach (var productionWorkOrderEquipment in order.Equipments)
                {
                    productionWorkOrderEquipment.Equipment.RecordUrl = GetUrlForModel(productionWorkOrderEquipment.Equipment, "Show", "Equipment");
                    foreach (var equipmentEnvironmentalPermit in productionWorkOrderEquipment.Equipment.EnvironmentalPermits)
                    {
                        equipmentEnvironmentalPermit.RecordUrl = GetUrlForModel(equipmentEnvironmentalPermit, "Show", "EnvironmentalPermit", "Environmental");
                    }
                }
            }

            if (order.LinkedToEnvironmentalPermit)
            {
                var notifier = _container.GetInstance<INotificationService>();
                var args = new NotifierArgs {
                    OperatingCenterId = order.OperatingCenter.Id,
                    Module = ROLE,
                    Purpose = PERMIT_NOTIFICATION,
                    Data = order
                };
                notifier.Notify(args);
            }
        }

        private void SendCreateNotification(ViewModel<ProductionWorkOrder> model)
        {
            var templateModel = Repository.Find(model.Id);

            var newModel = new ProductionWorkOrderNotification {
                ProductionWorkOrder = templateModel,
                RecordUrl = GetUrlForModel(templateModel, "Show", "ProductionWorkOrder", "Production")
            };

            var notifier = _container.GetInstance<INotificationService>();

            var args = new NotifierArgs {
                OperatingCenterId = templateModel.OperatingCenter.Id,
                Module = ROLE,
                Purpose = CREATE_NOTIFICATION,
                Data = newModel
            };

            notifier.Notify(args);
        }

        private void SendCompleteNotification(CompleteProductionWorkOrder model)
        {
            var entity = Repository.Find(model.Id);

            var notifier = _container.GetInstance<INotificationService>();

            var newModel = new ProductionWorkOrderNotification {
                ProductionWorkOrder = entity,
                RecordUrl = GetUrlForModel(entity, "Show", "ProductionWorkOrder", "Production")
            };

            var args = new NotifierArgs {
                OperatingCenterId = entity.OperatingCenter.Id,
                Module = ROLE,
                Purpose = COMPLETED_NOTIFICATION,
                Data = newModel
            };

            notifier.Notify(args);
        }

        private void SendSupervisorApprovalRequiredNotification(CompleteProductionWorkOrder model)
        {
            var entity = Repository.Find(model.Id);
            var notifier = _container.GetInstance<INotificationService>();

            var newModel = new ProductionWorkOrderNotification {
                ProductionWorkOrder = entity,
                RecordUrl = GetUrlForModel(entity, "Show", "ProductionWorkOrder", "Production")
            };

            foreach (var employee in entity.EmployeeAssignments.Select(x => x.AssignedTo))
            {
                var supervisor = employee.ReportsTo;
                if (supervisor == null)
                {
                    DisplayNotification($"Unable to supervisor approval notification to {employee.FullName}'s supervisor because their employee record does not have a supervisor set.");
                }
                else if (string.IsNullOrWhiteSpace(supervisor.EmailAddress))
                {
                    DisplayNotification($"Unable to supervisor approval notification to {employee.FullName}'s supervisor({supervisor.FullName}) because the supervisor's employee record is missing an email address.");
                }
                else
                {
                    notifier.Notify(new NotifierArgs {
                        OperatingCenterId = entity.OperatingCenter.Id,
                        Module = ROLE,
                        Purpose = SUPERVISOR_APPROVAL_REQUIRED,
                        Data = newModel,
                        Address = supervisor.EmailAddress
                    });
                }
            }
        }

        #endregion

        #region SAP

        protected override void UpdateEntityForSap(ProductionWorkOrder entity)
        {
            if (entity.SendToSAP == false)
            {
                return;
            }

            var productionWorkOrder = new SAPCreateUnscheduledWorkOrder(entity);
            var repo = _container.GetInstance<ISAPCreateUnscheduledWorkOrderRepository>();
            repo.Save(productionWorkOrder);

            if (!string.IsNullOrWhiteSpace(productionWorkOrder.OrderNumber))
                entity.SAPWorkOrder = productionWorkOrder.OrderNumber;
            if (!string.IsNullOrWhiteSpace(productionWorkOrder.WBSElement))
                entity.WBSElement = productionWorkOrder.WBSElement;
            if ((!entity.SAPNotificationNumber.HasValue || entity.SAPNotificationNumber == 0) &&
                !string.IsNullOrEmpty(productionWorkOrder.NotificationNumber))
                entity.SAPNotificationNumber = long.Parse(productionWorkOrder.NotificationNumber);

            entity.SAPErrorCode = productionWorkOrder.SAPErrorCode;
        }

        protected void ProgressSAP(int id, RoleModules module)
        {
            var entity = Repository.Find(id);
            if (entity == null)
                throw new InvalidOperationException("The entity was reported as saved but could not be retrieved. SAP was not updated and the entity could not be marked as such.");
            if (entity.SendToSAP)
            {
                try
                {
                    var repo = _container.GetInstance<ISAPProgressUnscheduledWorkOrderRepository>();
                    var order = new SAPProgressUnscheduledWorkOrder(entity);
                    // remove any notifications, these are never updated on regular Progress.
                    order.SapProductionWorkOrderChildNotification = new List<SAPProductionWorkOrderChildNotification>();
                    // 11/27/2017 - setting to the order here because we're losing the values it's returning
                    order = repo.Save(order);
                    entity.SAPErrorCode = order.SAPErrorCode;
                }
                catch (Exception ex)
                {
                    entity.SAPErrorCode = SAP_UPDATE_FAILURE + ex.Message;
                }

                Repository.Save(entity);
                SendSapErrorNotification(entity, module);
            }
        }

        private void TryUpdateSapNotifications(ProductionWorkOrder entity, SAPProgressUnscheduledWorkOrder order)
        {
            if (order.SapProductionWorkOrderChildNotification == null)
                return;
            foreach (var x in order.SapProductionWorkOrderChildNotification)
            {
                var eq = entity.Equipments.FirstOrDefault(z =>
                    z.Equipment.SAPEquipmentId.ToString().TrimStart('0') == x.SAPEquipmentNumber);
                if (eq != null && !string.IsNullOrWhiteSpace(x.NotificationNumber))
                {
                    eq.SAPNotificationNumber = long.Parse(x.NotificationNumber);
                    if (x.CompleteFlag == "Y" && !eq.CompletedOn.HasValue)
                        eq.CompletedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                }

                foreach (var mp in x.SapProductionWorkOrderMeasuringPoints)
                {
                    var entityMPV = entity.ProductionWorkOrderMeasurementPointValues.FirstOrDefault(mpv => mpv.MeasurementPointEquipmentType.Description == mp.Unit1 && eq.Equipment == mpv.Equipment);
                    if (!string.IsNullOrWhiteSpace(mp.MeasuringDocument) && entityMPV != null)
                        entityMPV.MeasurementDocId = int.Parse(mp.MeasuringDocument.TrimStart('0'));
                }
            }
        }

        protected void FinalizeSAP(int id, RoleModules module)
        {
            var entity = Repository.Find(id);
            if (entity == null)
                throw new InvalidOperationException("The entity was reported as saved but could not be retrieved. SAP was not updated and the entity could not be marked as such.");
            if (entity.SendToSAP)
            {
                try
                {
                    var repo = _container.GetInstance<ISAPCompleteUnscheduledWorkOrderRepository>();
                    var order = new SAPCompleteUnscheduledWorkOrder(entity);
                    repo.Save(order);
                    entity.SAPErrorCode = order.SAPErrorCode;
                }
                catch (Exception ex)
                {
                    entity.SAPErrorCode = SAP_UPDATE_FAILURE + ex.Message;
                }

                Repository.Save(entity);
                SendSapErrorNotification(entity, module);
            }
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(CreateProductionWorkOrder model)
        {
            // TODO: Remove all the null model checks. The model will never be null. -Ross 1/23/2020
            if (model?.CapitalizedFrom != null)
            {
                model = ViewModelFactory.BuildWithOverrides<CreateProductionWorkOrder, ProductionWorkOrder>(Repository.Find(model.CapitalizedFrom.Value), new {
                    ProductionWorkDescription = (string)null,
                    model.CapitalizedFrom
                });
            }

            if (model != null && !model.OperatingCenter.HasValue)
            {
                var user = AuthenticationService.CurrentUser;
                model.OperatingCenter = user.DefaultOperatingCenter.Id;
                if (user.Employee != null)
                    model.RequestedBy = user.Employee.Id;
            }

            if (model != null && model.Coordinate == null && model.Latitude.HasValue && model.Longitude.HasValue)
            {
                var mapIcon = _container.GetInstance<IRepository<MapIcon>>().Find(MapIcon.Indices.WorkOrder);
                var coordinate =
                    _container.GetInstance<IRepository<Coordinate>>()
                              .Save(new Coordinate {
                                   Latitude = model.Latitude.Value,
                                   Longitude = model.Longitude.Value,
                                   Icon = mapIcon
                               });
                model.Coordinate = coordinate.Id;
            }
            else
            {
                model = model ?? new CreateProductionWorkOrder(_container);
            }

            ModelState.Clear();
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateProductionWorkOrder model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    UpdateSAP(model.Id, ROLE);
                    if (model.AssignToSelf)
                    {
                        ProgressSAP(model.Id, ROLE);
                    }

                    MaybeSendPermitRelatedNotification(model);

                    SendCreateNotification(model);
                    if (model.AssignToSelf)
                    {
                        var orderEntity = Repository.Find(model.Id);
                        EmployeeAssignmentController.SendAssignedNotification(this, _container, orderEntity, AuthenticationService.CurrentUser.Employee);
                    }

                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult CreateProductionWorkOrderFromPlan(CreateProductionWorkOrderFromPlan model)
        {
            model.Priority = (int)ProductionWorkOrderPriority.Indices.ROUTINE_OFF_SCHEDULED;
            model.Facility = model.Facility == 0 ? null : model.Facility;
            model.EquipmentType = model.EquipmentType == 0 ? null : model.EquipmentType;
            model.FunctionalLocation = model.FunctionalLocation ?? string.Empty;
            //WOs created from plan should have Breakdown Indicator false
            model.BreakdownIndicator = false;

            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    MaybeSendPermitRelatedNotification(model);
                    SendCreateNotification(model);

                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchProductionWorkOrder search = null)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult IndexForMaintenancePlanOrders(SearchProductionWorkOrderFromMaintenancePlan search)
        {
            if (ModelState.IsValid)
            {
                search.MaintenancePlanEntity = _container.GetInstance<IRepository<MaintenancePlan>>().Find(search.MaintenancePlan.Value);
            }

            return this.RespondTo(f => {
                f.Fragment(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    IsPartial = true,
                    ViewName = "_IndexForMaintenancePlan",
                    OnNoResults = () => PartialView("_NoResultsProductionWorkOrders", search)
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id, null, onModelFound: (entity) => {
                    SetDefaultDropdownValuesForAsLeftAsFoundCondition(entity);

                    this.AddDropDownData<IMaterialRepository, Material>("Material", 
                        r => r.FindActiveMaterialByOperatingCenter(entity.OperatingCenter));

                    this.AddDropDownData<IStockLocationRepository, StockLocation>("StockLocation", 
                        r => r.FindActiveByOperatingCenter(entity.OperatingCenter));
                    this.AddDropDownData<ProductionWorkOrderActionCode>("ActionCode");
                    this.AddDropDownData<ProductionWorkOrderFailureCode>("FailureCode");

                    if (entity.ProductionWorkDescription?.OrderType?.Id == OrderType.Indices.CORRECTIVE_ACTION_20 && entity.CanBeSupervisorApproved)
                    {
                        this.AddDropDownData<ProductionWorkOrderCauseCode>("CauseCode");
                    }
                    
                    DisplayNotifications(entity);
                }));
                x.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        private void SetDefaultDropdownValuesForAsLeftAsFoundCondition(ProductionWorkOrder entity)
        {
            var defaultAsLeftConditionDropdownValue = _container.GetInstance<IRepository<AsLeftCondition>>()
                                                                .Where(y => y.Id == AsLeftCondition.Indices.ACCEPTABLE_GOOD)
                                                                .FirstOrDefault();

            var defaultAsFoundConditionDropdownValue = _container.GetInstance<IRepository<AsFoundCondition>>()
                                                                 .Where(y => y.Id == AsFoundCondition.Indices.ACCEPTABLE_GOOD)
                                                                 .FirstOrDefault();

            // This is done here because we're in a Show action and 
            // the SetDefaults is never called.
            foreach (var equipment in entity.Equipments)
            {
                if (equipment.AsLeftCondition == null)
                {
                    equipment.AsLeftCondition = defaultAsLeftConditionDropdownValue;
                }

                if (equipment.AsFoundCondition == null)
                {
                    equipment.AsFoundCondition = defaultAsFoundConditionDropdownValue;
                }
            }
        }

        [RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult CreateProductionWorkOrderForEquipment(ProductionWorkOrderEquipmentViewModel model)
        {
            // model.ProductionWorkOrderId can't be null, because only a production work order id in the model can trigger the call to this controller action.
            var pwo = Repository.Find(model.ProductionWorkOrderId);
            
            var newPwo = _viewModelFactory.BuildWithOverrides<CreateProductionWorkOrder, ProductionWorkOrder>(pwo, new {
                Id = 0,
                model.Equipment,
                model.Priority,
                DateReceived = DateTime.Now,
                BreakdownIndicator = true,
                AssignToSelf = false,
                AutoCreatedCorrectiveWorkOrder = model.AsLeftCondition == AsLeftCondition.Indices.NEEDS_EMERGENCY_REPAIR ||
                                                 model.AsLeftCondition == AsLeftCondition.Indices.NEEDS_REPAIR,
                ProductionWorkDescription = _container.GetInstance<IProductionWorkDescriptionRepository>()
                                                      .GetCorrectiveActionWorkDescription(pwo.ProductionWorkDescription?.EquipmentType?.Id).Id
            });

            return ActionHelper.DoCreate(newPwo, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    UpdateSAP(newPwo.Id, ROLE);

                    if (newPwo.AssignToSelf)
                    {
                        ProgressSAP(newPwo.Id, ROLE);
                    }

                    MaybeSendPermitRelatedNotification(newPwo);
                    SendCreateNotification(newPwo);

                    if (newPwo.AssignToSelf)
                    {
                        var orderEntity = Repository.Find(newPwo.Id);
                        EmployeeAssignmentController.SendAssignedNotification(this, _container, orderEntity,
                            AuthenticationService.CurrentUser.Employee);
                    }

                    var recordUrl = GetUrlForModel(newPwo, "Show", "ProductionWorkOrder", "Production");
                    DisplaySuccessMessage($"{CREATE_NOTIFICATION} - <a href='{recordUrl}' target='_blank'>{newPwo.Id}</a>");
                    
                    if (model.AsLeftCondition != null && model.AsLeftCondition.Value == AsLeftCondition.Indices.NEEDS_EMERGENCY_REPAIR)
                    {
                        SendSupervisorNeedsEmergencyRepairNotification(model, newPwo);
                    }
                    
                    return RedirectToReferrerOr("Show", "ProductionWorkOrder", new { id = model.ProductionWorkOrderId, newProductionWorkOrderId = newPwo.Id }, "#EquipmentTab");
                }
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchProductionWorkOrder search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => {
                        Repository.SearchForDistinct(search);
                    }
                }));

                formatter.Fragment(() => {
                    search.EnablePaging = true;
                    return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                        IsPartial = true,
                        ViewName = "_Index",
                        OnNoResults = () => PartialView("_NoResults")
                    });
                });

                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.SearchForExcel(search).Select(e => new {
                        e.Id,
                        e.OperatingCenter,
                        e.PlanningPlant,
                        e.Facility,
                        e.FacilityArea,
                        e.FunctionalLocation,
                        e.EquipmentType,
                        e.Equipment,
                        e.Coordinate,
                        e.Priority,
                        e.IsOpen,
                        e.WorkDescription,
                        e.AirPermit,
                        e.HasLockoutRequirement,
                        e.HotWork,
                        e.IsConfinedSpace,
                        e.JobSafetyChecklist,
                        e.CapitalizedFrom,
                        e.RequestedBy,
                        e.Notes,
                        e.DateReceived,
                        e.BreakdownIndicator,
                        e.SAPWorkOrder,
                        e.SAPStatus,
                        e.SAPNotificationNumber,
                        e.WBSElement,
                        e.CapitalizationReason,
                        e.DateCompleted,
                        e.CompletedBy,
                        e.ApprovedOn,
                        e.ApprovedBy,
                        e.BasicStart,
                        e.DateCancelled,
                        e.CancellationReason,
                        e.PlantMaintenanceActivityTypeOverride,
                        e.CorrectiveOrderProblemCode,
                        e.OtherProblemNotes,
                        e.ActionCode,
                        e.FailureCode,
                        e.CauseCode,
                        e.ProductionWorkOrderRequiresSupervisorApproval,
                        e.MaterialsApproved,
                        e.Status,
                        e.OrderType,
                        e.SendToSap,
                        e.CanBeSupervisorApproved,
                        e.CanBeMaterialApproved,
                        e.CanBeCompleted,
                        e.CanBeCancelled,
                        e.CanBeMaterialPlanned,
                        e.CapitalizationCancelsOrder,
                        e.CurrentlyAssignedEmployee,
                        e.LockoutFormCreated,
                        e.LockoutForms,
                        e.LockoutDevices,
                        e.ConfinedSpaceFormCreated,
                        e.IsEligibleForRedTagPermit,
                        e.RedTagPermitCreated,
                        e.RedTagPermit,
                        e.EstimatedCompletionHours,
                        e.ActualCompletionHours,
                        e.TankInspections,
                        e.PlanNumber,
                        e.LocalTaskDescription
                    });

                    return this.Excel(results);
                });
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        [HttpGet]
        public ActionResult ByFacilityIdForLockoutForms(int id)
        {
            var results = Repository.GetByFacilityIdForLockoutForms(id);
            return new CascadingActionResult(results, "Id", "Id") { SortItemsByTextField = false };
        }

        [HttpGet]
        public ActionResult CorrectiveWorkOrdersForReplacedEquipment(int equipmentId)
        {
            var results = Repository.GetCorrectiveProductionWorkOrdersForReplacedEquipments(equipmentId);
            return new CascadingActionResult(results, "Id", "Id") { SortItemsByTextField = false };
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            var pwo = _container.GetInstance<IRepository<ProductionWorkOrder>>().Find(id);

            return pwo == null || pwo?.OrderType?.Id != OrderType.Indices.ROUTINE_13
                ? ActionHelper.DoEdit<EditProductionWorkOrder>(id, onModelFound: DisplayNotifications)
                : RedirectToAction("Show", new { id });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditProductionWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        if (model.CreateWorkOrder)
                        {
                            UpdateSAP(model.Id, ROLE);
                        }

                        if (model.ProgressWorkOrder)
                        {
                            ProgressSAP(model.Id, ROLE);
                        }
                        else if (model.FinalizeWorkOrder)
                        {
                            FinalizeSAP(model.Id, ROLE);
                        }
                    }

                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #region Completion

        // TODO: These belong to their own Controllers once they get created. 

        [HttpPost, RequiresSecureForm, RequiresRole(ROLE)]
        public ActionResult CompleteProductionWorkOrder(CompleteProductionWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var productionWorkOrder = Repository.Find(model.Id);
                    if ((productionWorkOrder.ProductionWorkDescription.OrderType.Id == OrderType.Indices.OPERATIONAL_ACTIVITY_10 ||
                         productionWorkOrder.ProductionWorkDescription.OrderType.Id == OrderType.Indices.CORRECTIVE_ACTION_20)
                        && !productionWorkOrder.ProductionWorkOrderMaterialUsed.Any())
                    {
                        ProgressSAP(model.Id, ROLE);
                        FinalizeSAP(model.Id, ROLE);
                    }
                    else
                    {
                        if (productionWorkOrder.ProductionWorkDescription.OrderType.Id == OrderType.Indices.PLANT_MAINTENANCE_WORK_ORDER_11)
                        {
                            ProgressSAP(model.Id, ROLE);
                            FinalizeSAP(model.Id, ROLE);
                        }
                        else
                        {
                            ProgressSAP(model.Id, ROLE);
                        }
                    }

                    SendCompleteNotification(model);

                    if (productionWorkOrder.CanBeSupervisorApproved)
                    {
                        SendSupervisorApprovalRequiredNotification(model);
                    }

                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresRole(ROLE)]
        public ActionResult RedTagPermitAuthorizationForProductionWorkOrder(RedTagPermitAuthorizationViewModel viewModel)
        {
            return ActionHelper.DoUpdate(viewModel, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Show", "ProductionWorkOrder", new { id = viewModel.Id }, "#RedTagPermitTab"),
                OnError = () => DoRedirectionToAction("Show", new { id = viewModel.Id })
            });
        }

        [HttpPost, RequiresRole(ROLE)]
        public ActionResult SupervisorApproveProductionWorkOrder(SupervisorApproveProductionWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var productionWorkOrder = Repository.Find(model.Id);
                    if (productionWorkOrder.ProductionWorkDescription.OrderType.Id == OrderType.Indices.CORRECTIVE_ACTION_20)
                    {
                        if (!productionWorkOrder.ProductionWorkOrderMaterialUsed.Any())
                        {
                            FinalizeSAP(model.Id, ROLE);
                        }
                        else
                        {
                            ProgressSAP(model.Id, ROLE);
                        }
                    }
                    else
                    {
                        FinalizeSAP(model.Id, ROLE);
                    }

                    return DoRedirectionToAction("Show", new { id = model.Id });
                },
                OnError = () => DoRedirectionToAction("Show", new { id = model.Id })
            });
        }

        [HttpPost, RequiresSecureForm, RequiresRole(ROLE)]
        public ActionResult RejectProductionWorkOrder(RejectProductionWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    ProgressSAP(model.Id, ROLE);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresSecureForm, RequiresRole(ROLE)]
        public ActionResult ApproveMaterialWorkOrder(ApproveMaterialWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    FinalizeSAP(model.Id, ROLE);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresSecureForm, RequiresRole(ROLE)]
        public ActionResult CancelProductionWorkOrder(CancelProductionWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    ProgressSAP(model.Id, ROLE);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresSecureForm, RequiresRole(ROLE)]
        public ActionResult CapitalizeProductionWorkOrder(CapitalizeProductionWorkOrder model)
        {
            //if there is time charged or no time charge/ material or no material still progress and complete need to call.
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    ProgressSAP(model.Id, ROLE);
                    FinalizeSAP(model.Id, ROLE);
                    var newModel = new CreateProductionWorkOrder(_container) { CapitalizedFrom = model.Id };

                    return RedirectToAction("New", newModel);
                }
            });
        }

        [HttpPost, RequiresSecureForm, RequiresRole(ROLE)]
        public ActionResult CompleteMaterialPlanning(CompleteProductionMaterialPlanning model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    ProgressSAP(model.Id, ROLE);
                    // Handle this if it fails to update SAP properly
                    var order = Repository.Find(model.Id);
                    {
                        if (order.SAPErrorCode != null && !order.SAPErrorCode.ToUpper().Contains("SUCCESS"))
                        {
                            order.MaterialsPlannedOn = null;
                            Repository.Save(order);
                        }
                    }
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Child Elements - Materials/Assignments/Prerequisites

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddEmployeeAssignment(AddEmployeeAssignmentProductionWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.ProgressWorkOrder)
                    {
                        ProgressSAP(model.Id, ROLE);
                    }

                    var orderEntity = Repository.Find(model.Id);
                    var assignedEmployee = _container.GetInstance<IRepository<Employee>>().Find(model.AssignedTo.Value);
                    EmployeeAssignmentController.SendAssignedNotification(this, _container, orderEntity, assignedEmployee);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveEmployeeAssignment(RemoveEmployeeAssignmentProductionWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.ProgressWorkOrder)
                    {
                        ProgressSAP(model.Id, ROLE);
                    }

                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveEmployeeAssignments(RemoveSchedulingEmployeeAssignments model)
        {
            return ActionHelper.DoUpdateForViewModelSet(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    foreach (var id in model.WorkOrdersToProgress)
                    {
                        ProgressSAP(id, ROLE);
                    }

                    return RedirectToReferrerOr("Search", "Scheduling", new { area = "Production" }, "#RemoveAssignmentsTab");
                },

                // By default it would redirect back to the ProductionWorkOrder section since we're operating on PWOs
                OnError = () => RedirectToReferrerOr("Search", "Scheduling", new { area = "Production" }, "#RemoveAssignmentsTab")
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddProductionWorkOrderMaterialUsed(AddProductionWorkOrderMaterialUsedProductionWorkOrder model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveProductionWorkOrderMaterialUsed(RemoveProductionWorkOrderMaterialUsedProductionWorkOrder model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddProductionWorkOrderProductionPrerequisite(AddProductionWorkOrderProductionPrerequisite model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region History

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult History(SearchProductionWorkOrder search)
        {
            return this.RespondTo((x) => {
                x.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    IsPartial = true,
                    ViewName = "History",
                    SearchOverrideCallback = () => Repository.SearchForProductionWorkOrderHistory(search),
                    OnNoResults = () => PartialView("History", search)
                }));
            });
        }

        #endregion

        #endregion

        #region Exposed Methods

        private void SendSupervisorNeedsEmergencyRepairNotification(ProductionWorkOrderEquipmentViewModel model, CreateProductionWorkOrder newPwo)
        {
            var orderRepo = _container.GetInstance<IProductionWorkOrderRepository>();
            var equipmentRepo = _container.GetInstance<IEquipmentRepository>();

            var pwo = orderRepo.Find(model.ProductionWorkOrderId);
            var equipment = equipmentRepo.Find(model.Equipment);

            if (pwo == null || equipment == null)
            {
                return;
            }

            var newModel = new ProductionWorkOrderEquipmentNotification {
                RoutineWorkOrder = pwo,
                ProductionWorkOrderId = newPwo.Id,
                ProductionWorkDescription = pwo.ProductionWorkDescription.ToString(),
                CorrectiveOrderProblemCode = pwo.CorrectiveOrderProblemCode,
                FacilityName = pwo.Facility.FacilityName,
                DateReceived = newPwo.DateReceived,
                FacilityUrl = GetUrlForModel(pwo.Facility, "Show", "Facility"),
                ProductionWorkOrderUrl = GetUrlForModel(newPwo, "Show", "ProductionWorkOrder", "Production"),
                EquipmentUrl = GetUrlForModel(equipment, "Show", "Equipment"),
                RoutineWorkOrderUrl = GetUrlForModel(pwo, "Show", "ProductionWorkOrder", "Production"),
                Equipment = equipment
            };

            var employees = pwo.EmployeeAssignments.Select(x => x.AssignedTo)
                               .Where(y => y.ReportsTo != null && !string.IsNullOrEmpty(y.ReportsTo.EmailAddress));

            var notifier = _container.GetInstance<INotificationService>();
            foreach (var employee in employees)
            {
                notifier.Notify(new NotifierArgs {
                    OperatingCenterId = pwo.OperatingCenter.Id,
                    Module = ROLE,
                    Purpose = AS_LEFT_CONDITION_NEEDS_EMERGENCY_REPAIR,
                    Data = newModel,
                    Subject =
                        $"Emergency Repair Needed for {equipment.Description} at {pwo.Facility.FacilityName} identified on {string.Format(CommonStringFormats.DATE, pwo.DateReceived)}",
                    Address = employee.ReportsTo.EmailAddress
                });
            }
        }

        #endregion
    }
}