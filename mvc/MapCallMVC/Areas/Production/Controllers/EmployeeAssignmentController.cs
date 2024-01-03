using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;

namespace MapCallMVC.Areas.Production.Controllers
{
    [DisplayName("Assignments")]
    public class EmployeeAssignmentController : ControllerBaseWithPersistence<IEmployeeAssignmentRepository, EmployeeAssignment, User>
    {
        #region Constants

        public const RoleModules ROLE = EmployeeAssignmentRepository.ROLE;

        public const string SAP_ERROR_CODE = "SAPErrorCode Occurred",
                            SAP_UPDATE_FAILURE = "RETRY::UPDATE FAILURE: ",
                            FINALIZATION_FRAGMENT = "#FinalizationTab";

        #endregion

        #region Private Methods

        //TODO: Duplication, consider using SAPSyncControWithPerBase
        protected void ProgressSAP(int id, RoleModules module)
        {
            var entity = Repository.Find(id);
            if (entity == null)
                throw new InvalidOperationException("The entity was reported as saved but could not be retrieved. SAP was not updated and the entity could not be marked as such.");

            try
            {
                var repo = _container.GetInstance<ISAPProgressUnscheduledWorkOrderRepository>();
                var order = new SAPProgressUnscheduledWorkOrder(entity.ProductionWorkOrder);
                // remove any notifications, these are never updated on regular Progress.
                order.SapProductionWorkOrderChildNotification = new List<SAPProductionWorkOrderChildNotification>();
                repo.Save(order);
                entity.ProductionWorkOrder.SAPErrorCode = order.SAPErrorCode;
            }
            catch (Exception ex)
            {
                entity.ProductionWorkOrder.SAPErrorCode = SAP_UPDATE_FAILURE + ex.Message;
            }

            _container.GetInstance<IProductionWorkOrderRepository>().Save(entity.ProductionWorkOrder);
            SendSapErrorNotification(entity.ProductionWorkOrder, module);
        }

        protected void SendSapErrorNotification(ProductionWorkOrder entity, RoleModules module)
        {
            //TODO: Move this to a logical formula property and add to the searches.
            if (!string.IsNullOrWhiteSpace(entity.SAPErrorCode) && !entity.SAPErrorCode.StartsWith("RETRY") && !entity.SAPErrorCode.ToUpper().Contains("SUCCESSFUL"))
            {
                var notifier = _container.GetInstance<INotificationService>();
                notifier.Notify(new NotifierArgs {
                    Subject = SAP_ERROR_CODE,
                    Purpose = SAP_ERROR_CODE,
                    Module = module,
                    Data = new {
                        RecordUrl = GetUrlForModel(entity, "Show", typeof(ProductionWorkOrder).Name, "FieldOperations"),
                        entity.SAPErrorCode
                    }
                });
            }
        }

        public static void SendAssignedNotification(MMSINC.Controllers.ControllerBase controller, StructureMap.IContainer container, ProductionWorkOrder entity, Employee assignedEmployee)
        {
            if (string.IsNullOrWhiteSpace(assignedEmployee.EmailAddress))
            {
                controller.DisplayNotification($"Unable to send assignment notification to {assignedEmployee.FullName} because their employee record is missing an email address.");
            }
            else
            {
                var newModel = new ProductionWorkOrderNotification {
                    ProductionWorkOrder = entity,
                    RecordUrl = controller.GetUrlForModel(entity, "Show", "ProductionWorkOrder", "Production")
                };

                var notifier = container.GetInstance<INotificationService>();

                var args = new NotifierArgs {
                    OperatingCenterId = entity.OperatingCenter.Id,
                    Module = ROLE,
                    Purpose = ProductionWorkOrderController.ASSIGNED_NOTIFICATION,
                    Data = newModel,
                    Address = assignedEmployee.EmailAddress
                };

                notifier.Notify(args);
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, onModelFound: (entity) => {
                this.AddDropDownData<IEmployeeRepository, Employee>("Employee",
                    r => r.GetActiveEmployeesWhere(x => x.OperatingCenter == entity.ProductionWorkOrder.OperatingCenter));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchEmployeeAssignment search)
        {
            SetLookupData(ControllerAction.Search);
            
            return View(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEmployeeAssignment search)
        {
            return this.RespondTo(f => {
                f.View(() => {
                    search.EnablePaging = false;
                    return ActionHelper.DoIndex(search);
                });
                f.Calendar(() => {
                    // TODO: Can we remove this then? -Ross 1/23/2020
                    throw new NotImplementedException("this is going to be tricky.");
                });
            });
        }

        #endregion

        #region New/Create

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Create(CreateEmployeeAssignment model)
        {
            var isModelStateValid = ModelState.IsValid;

            if (isModelStateValid)
            {
                var assignTo = _container.GetInstance<IEmployeeRepository>().Where(x => model.AssignedTo.Contains(x.Id));
                var assignBy = AuthenticationService.CurrentUser.Employee;
                var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                var orderRepo = _container.GetInstance<IProductionWorkOrderRepository>();

                var isDuplicateAssignment = new Func<EmployeeAssignment, bool>(assignment =>
                    assignment.AssignedFor.Date == model.AssignedFor && model.AssignedTo.Contains(assignment.AssignedTo.Id));

                var assignments = orderRepo
                                 .Where(x => model.ProductionWorkOrderIds.Contains(x.Id))
                                 .SelectMany(x => x.EmployeeAssignments);

                if (assignments.Any(isDuplicateAssignment))
                {
                    isModelStateValid = false;
                }

                foreach (var orderId in model.ProductionWorkOrderIds)
                {
                    var order = orderRepo.Find(orderId);

                    if (order.EmployeeAssignments.Any(isDuplicateAssignment))
                    {
                        ModelState.AddModelError("ProductionWorkOrderIds", $"Cannot add duplicate Employee Assignment(s) to Production Work Order with the ID: '{orderId}'");
                        isModelStateValid = false;
                    }

                    // Do not save any records if the validation failed, but we still want to continue scanning for validation errors
                    // so we can show the user all errors at once
                    if (!isModelStateValid)
                    {
                        continue;
                    }

                    foreach (var employee in assignTo)
                    {
                        Repository.Save(new EmployeeAssignment {
                            AssignedTo = employee,
                            AssignedBy = assignBy,
                            AssignedFor = model.AssignedFor.Value,
                            AssignedOn = now,
                            ProductionWorkOrder = order
                        });

                        SendAssignedNotification(this, _container, order, employee);
                    }
                }
            }
            
            if (!isModelStateValid)
            {
                DisplayModelStateErrors();
            }

            return RedirectToReferrerOr("Search", "Scheduling", new { area = "Production" }, string.Empty);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEmployeeAssignment>(id, null, onModelFound: (entity) => {
                this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("AssignedTo",
                    r => r.GetActiveEmployeesForSelectWhere(e =>
                        e.User != null && e.User.HasAccess && e.User.Roles.Any(role =>
                            role.Module.Id == (int)ROLE &&
                            new[] { (int)RoleActions.Edit, (int)RoleActions.UserAdministrator }
                               .Contains(role.Action.Id) &&
                            (role.OperatingCenter == null ||
                             role.OperatingCenter.Id == entity.ProductionWorkOrder.OperatingCenter.Id))));
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEmployeeAssignment model)
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

        #endregion

        #region End

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult End(EndEmployeeAssignment model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var dateTimeProvider = _container.GetInstance<IDateTimeProvider>();
                    _container.GetInstance<IRepository<Note>>().Save(new Note {
                        Text = model.Notes,
                        LinkedId = model.ProductionWorkOrder.Value, // This property is required, so it does not need a null check.
                        DataType = _container.GetInstance<IDataTypeRepository>()
                                             .GetByTableName(nameof(ProductionWorkOrder) + "s").First(),
                        CreatedBy = AuthenticationService.CurrentUser.UserName
                    });
                    ProgressSAP(model.Id, ROLE);
                    if (model.IsFinalAssignment != null && (bool)model.IsFinalAssignment && Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.Contains("ProductionWorkOrder/Show"))
                    {
                        return RedirectToReferrerOr("Show", "ProductionWorkOrder", new { id = model.ProductionWorkOrder },
                            FINALIZATION_FRAGMENT);
                    }
                    return RedirectToAction("Show", "ProductionWorkOrder", new { id = model.ProductionWorkOrder });
                },
                OnError = () => RedirectToAction("Show", "ProductionWorkOrder", new { id = model.ProductionWorkOrder })
            });
        }

        #endregion

        #region Start

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Start(StartEmployeeAssignment model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    ProgressSAP(model.Id, ROLE);
                    return RedirectToAction("Show", "ProductionWorkOrder", new { id = model.ProductionWorkOrder });
                }
            });
        }

        #endregion

        public EmployeeAssignmentController(ControllerBaseWithPersistenceArguments<IEmployeeAssignmentRepository, EmployeeAssignment, User> args) : base(args) { }
    }
}