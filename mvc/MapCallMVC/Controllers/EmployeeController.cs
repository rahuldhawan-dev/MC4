using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Engineering.Controllers;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.Production.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using StructureMap;

namespace MapCallMVC.Controllers
{
    public class EmployeeController : ControllerBaseWithPersistence<IEmployeeRepository, Employee, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesEmployee;
        public const string IMPORT_UPLOAD_TEMPDATA_KEY = "Import upload",
                            NEW_HIRE_EMAIL_NOTIFICATION = "New Hire Email Notification";
        #endregion

        #region Properties

        private const int PAGE_SIZE = 50;

        #endregion

        #region Private Methods

        private void SendNotification(IContainer container, Employee employee, string notificationPurpose)
        {
            var notifier = container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = employee.OperatingCenter.Id,
                Module = RoleModules.HumanResourcesEmployee,
                Purpose = notificationPurpose,
                Data = employee
            };
            notifier.Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            var getOpCenters = this.GetUserOperatingCentersFn(ROLE);

            Action allActions = () => {
                this.AddDropDownData<EmployeeStatus>("Status");
                this.AddDropDownData<EmployeeDepartment>("Department");
                this.AddDropDownData<CommercialDriversLicenseProgramStatus>();
                this.AddDropDownData<ScheduleType>();
            };

            Action newEditActions = () => {
                this.AddDropDownData<Gender>();
                this.AddDropDownData<ReasonForDeparture>();
                this.AddDropDownData<UnionAffiliation>();
                this.AddDropDownData<TCPAStatus>();
                this.AddDropDownData<DPCCStatus>();
                this.AddDropDownData<InstitutionalKnowledge>();
                this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>(
                    "ReportsTo");
                this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>(
                    "HumanResourcesManager");
                this.AddDynamicDropDownData<PositionGroup, PositionGroupDisplayItem>();
            };

            switch (action)
            {
                case ControllerAction.Show:
                    this.AddDropDownData<ProductionSkillSet>();
                    break;
                case ControllerAction.Search:
                    allActions();
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    this.AddDropDownData<IPositionRepository, Position>(
                        "Category",
                        r => r.GetDistinctCategories(),
                        p => p.Category,
                        p => p.Category);
                    this.AddDropDownData<IPositionRepository, Position>("CurrentPosition",
                        r => r.GetDistinctPositions(),
                        p => p.Id,
                        p => p.Description);
                    this.AddDropDownData<EmergencyResponsePriority>("EmergencyResponsePriority");
                    break;
                case ControllerAction.New:
                    allActions();
                    newEditActions();
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add);
                    break;
                case ControllerAction.Edit:
                    allActions();
                    newEditActions();
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    break;
            }
        }

        public ActionResult GetByOperatingCenterId(int[] operatingCenterId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetByOperatingCenterId(operatingCenterId));
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchEmployee search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEmployee search)
        {
            if (string.IsNullOrWhiteSpace(search.SortBy))
            {
                search.SortBy = "Id";
                search.SortAscending = true;
            }
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = true
                }));
                f.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.HumanResourcesEmployee, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateEmployee(_container));
        }

        [HttpPost, RequiresRole(RoleModules.HumanResourcesEmployee, RoleActions.Add)]
        public ActionResult Create(CreateEmployee employee)
        {
            return ActionHelper.DoCreate(employee, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendNotification(_container, Repository.Find(employee.Id), NEW_HIRE_EMAIL_NOTIFICATION);
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEmployee>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEmployee employee)
        {
            return ActionHelper.DoUpdate(employee);
        }

        #region ProductionSkillSets

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddProductionSkillSets(AddEmployeeProductionSkillSet model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveProductionSkillSets(RemoveEmployeeProductionSkillSet model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #endregion

        #region Lookups

        #region ActiveEmployeesByOperatingCenterId

        [HttpGet]
        public ActionResult EmployeesWithCurrentProductionAssignmentsByOperatingCenterIds(
            int[] operatingCenterIds)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetActiveEmployeesWhere(e =>
                    e.RelatedProductionAssignments.Any(a =>
                        operatingCenterIds.Contains(a.ProductionWorkOrder.OperatingCenter.Id)) ||
                    e.ProductionAssignments.Any(a =>
                        operatingCenterIds.Contains(a.ProductionWorkOrder.OperatingCenter.Id))));
        }

        [HttpGet]
        public ActionResult EmployeesWithCurrentProductionAssignmentsByOperatingCenterId(
            int operatingCenterId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetActiveEmployeesWhere(e =>
                    e.RelatedProductionAssignments.Any(a =>
                        a.ProductionWorkOrder.OperatingCenter.Id == operatingCenterId) ||
                    e.ProductionAssignments.Any(a =>
                        a.ProductionWorkOrder.OperatingCenter.Id == operatingCenterId)));
        }

        [HttpGet]
        public ActionResult ActiveProductionWorkManagementEmployeesByOperatingCenterId(
            int operatingCenterId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetActiveEmployeesByUserRole(
                    operatingCenterId,
                    RoleModules.ProductionWorkManagement,
                    RoleActions.Edit,
                    RoleActions.UserAdministrator));
        }

        [HttpGet]
        public ActionResult ProductionWorkManagementEmployeesByOperatingCenterId(int operatingCenterId)
        {
            IQueryable<Employee> employeesByOperatingCenter = Repository.GetEmployeesByUserRole(
                operatingCenterId,
                RoleModules.ProductionWorkManagement,
                RoleActions.Edit,
                RoleActions.UserAdministrator);
            var employeesMatches = GetSortedEmployees(operatingCenterId, employeesByOperatingCenter);
            CascadingActionResult<Employee, EmployeeDisplayItem> employees =
                new CascadingActionResult<Employee, EmployeeDisplayItem>(employeesMatches.AsQueryable()) { 
                    SortItemsByTextField = false
                };
            
            return employees;
        }

        [HttpGet]
        public ActionResult ActiveFieldServicesWorkManagementEmployeesByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetActiveEmployeesByUserRole(
                    operatingCenterId,
                    RoleModules.FieldServicesWorkManagement,
                    RoleActions.Edit,
                    RoleActions.UserAdministrator));
        }

        [HttpGet]
        public ActionResult ActiveEmployeesByOperatingCenterId(int id)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetActiveEmployeesByOperatingCenterId(id));
        }

        [HttpGet]
        public ActionResult GetEmployeesForProductionWorkOrderSchedulingByOperatingCenterAndProductionSkillSet(int? id, int? skillSetId)
        {
            RoleModules ROLE = ProductionWorkOrderController.ROLE;
            CascadingActionResult<Employee, EmployeeDisplayItem> employees;
            IQueryable<Employee> employeesByOperatingCenter;

            if (skillSetId > 0)
            {
                employeesByOperatingCenter = Repository.GetActiveEmployeesForSelectWhere(e =>
                    e.User != null && e.User.HasAccess && e.User.Roles.Any(role =>
                        role.Module.Id == (int)ROLE &&
                        new[] { (int)RoleActions.Edit, (int)RoleActions.UserAdministrator }
                           .Contains(role.Action.Id) &&
                        (role.OperatingCenter == null ||
                         role.OperatingCenter.Id == id) &&
                        e.ProductionSkillSets.Any(s =>
                            s.ProductionSkillSet.Id == skillSetId)));
            }
            else
            {
                employeesByOperatingCenter = Repository.GetActiveEmployeesForSelectWhere(e =>
                    e.User != null && e.User.HasAccess && e.User.Roles.Any(role =>
                        role.Module.Id == (int)ROLE &&
                        new[] { (int)RoleActions.Edit, (int)RoleActions.UserAdministrator }
                           .Contains(role.Action.Id) &&
                        (role.OperatingCenter == null ||
                         role.OperatingCenter.Id == id)));
            }

            var employeesMatches = GetSortedEmployees(id, employeesByOperatingCenter);
            employees = new CascadingActionResult<Employee, EmployeeDisplayItem>(employeesMatches.AsQueryable());
            employees.SortItemsByTextField = false;
            return employees;
        }

        [HttpGet]
        public ActionResult GetEmployeesForProductionWorkOrderSchedulingByOperatingCenter(int? id)
        {
            RoleModules ROLE = ProductionWorkOrderController.ROLE;

            IQueryable<Employee> employeesByOperatingCenter = Repository.GetActiveEmployeesForSelectWhere(e =>
                e.User != null && e.User.HasAccess && e.User.Roles.Any(role =>
                    role.Module.Id == (int)ROLE &&
                    new[] { (int)RoleActions.Edit, (int)RoleActions.UserAdministrator }
                       .Contains(role.Action.Id) &&
                    (role.OperatingCenter == null ||
                     role.OperatingCenter.Id == id)));

            var employeesMatches = GetSortedEmployees(id, employeesByOperatingCenter);
            CascadingActionResult<Employee, EmployeeDisplayItem> employees =
                new CascadingActionResult<Employee, EmployeeDisplayItem>(employeesMatches.AsQueryable());
            employees.SortItemsByTextField = false;
            return employees;
        }

        private static List<Employee> GetSortedEmployees(int? id, IQueryable<Employee> employees)
        {
            List<Employee> employeesByOperatingCenter = new List<Employee>();
            List<Employee> employeesOthers = new List<Employee>();

            employees.ToList().ForEach(emp => {
                if (emp.OperatingCenter != null && emp.OperatingCenter.Id == id)
                {
                    employeesByOperatingCenter.Add(emp);
                }
            });

            employees.ToList().ForEach(emp => {
                if (emp.OperatingCenter != null && emp.OperatingCenter.Id != id)
                {
                    employeesOthers.Add(emp);
                }
            });

            employeesByOperatingCenter.AddRange(employeesOthers.OrderBy(e => e.LastName));
            return employeesByOperatingCenter;
        }

        [HttpGet]
        public ActionResult ActiveEmployeesByEmployeePartialFirstOrLastName(string partialFirstOrLastName)
        {
            var results = Enumerable.Empty<Employee>();
            if (!string.IsNullOrEmpty(partialFirstOrLastName) && partialFirstOrLastName.Length >= 3)
            {
                results = Repository.Where(e =>
                    e.Status.Id == EmployeeStatus.Indices.ACTIVE &&
                    (e.FirstName.StartsWith(partialFirstOrLastName) ||
                     e.LastName.StartsWith(partialFirstOrLastName)));
            }

            return new AutoCompleteResult(results, "Id", nameof(Employee.FullName));
        }

        [HttpGet]
        public ActionResult ActiveEmployeesDescriptionByEmployeePartialFirstOrLastName(string partialFirstOrLastName)
        {
            var results = Enumerable.Empty<Employee>();
            if (!string.IsNullOrEmpty(partialFirstOrLastName) && partialFirstOrLastName.Length >= 3)
            {
                results = Repository.Where(e =>
                    e.Status.Id == EmployeeStatus.Indices.ACTIVE &&
                    (e.FirstName.StartsWith(partialFirstOrLastName) ||
                     e.LastName.StartsWith(partialFirstOrLastName)));
            }

            return new AutoCompleteResult(results, "Id", "Description");
        }

        [HttpGet]
        public ActionResult AllEmployeesByEmployeePartialFirstOrLastName(string partialFirstOrLastName)
        {
            var results = Enumerable.Empty<Employee>();
            if (!string.IsNullOrEmpty(partialFirstOrLastName) && partialFirstOrLastName.Length >= 3)
            {
                results = Repository.Where(e => e.FirstName.StartsWith(partialFirstOrLastName) ||
                                                e.LastName.StartsWith(partialFirstOrLastName));
            }

            return new AutoCompleteResult(
                results,
                "Id",
                nameof(Employee.FullName),
                nameof(Employee.FullName));
        }

        [HttpGet]
        public ActionResult ActiveEmployeesByOperatingCenterIdWithStatus(int id)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItemWithStatus>(
                Repository.GetActiveEmployeesByOperatingCenterId(id));
        }

        [HttpGet]
        public ActionResult ActiveEmployeesByOperatingCenterIds(int[] ids)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetActiveEmployeesByOperatingCenterId(ids));
        }

        [HttpGet]
        public ActionResult ActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterId(int id)
        {
            var curUser = AuthenticationService.CurrentUser;
            var roleRepo = _container.GetInstance<IRepository<AggregateRole>>();
            var opCenterIds = curUser.GetQueryableMatchingRoles(
                                          roleRepo,
                                          IncidentController.ROLE_MODULE,
                                          RoleActions.UserAdministrator)
                                     .OperatingCenters;
            var results = Repository.GetActiveEmployeesByOperatingCenterId(id);
            if (!AuthenticationService.CurrentUser.IsAdmin)
            {
                var curEmpId = AuthenticationService.CurrentUser.Employee.Id;
                // NOTE: This should match with IncidentRepository in how many times
                // the nested ReportsTo is checked.

                if (opCenterIds.Any())
                {
                    results = results.Where(x => opCenterIds.Contains(x.OperatingCenter.Id) ||
                                                 x.ReportsTo.Id == curEmpId ||
                                                 x.ReportsTo.ReportsTo.Id == curEmpId ||
                                                 x.ReportsTo.ReportsTo.ReportsTo.Id == curEmpId);
                }
                else
                {
                    results = results.Where(x => x.ReportsTo.Id == curEmpId ||
                                                 x.ReportsTo.ReportsTo.Id == curEmpId ||
                                                 x.ReportsTo.ReportsTo.ReportsTo.Id == curEmpId);
                }
            }
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(results);
        }

        [HttpGet]
        public ActionResult EmployeesByStateIdOrOperatingCenterIdAndStatusIdWithStatus(int? stateId, int? operatingCenterId, int? statusId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItemWithStatus>(
                Repository.GetByStateIdOrOperatingCenterIdAndStatusId(stateId, operatingCenterId, statusId));
        }

        [HttpGet]
        public ActionResult EmployeesByOperatingCenterIdAndStatusId(int operatingCenterId, int statusId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetByOperatingCenterIdAndStatusId(operatingCenterId, statusId));
        }

        #endregion

        #region Employees with Lockout Form Role

        [HttpGet]
        public ActionResult LockoutFormEmployeesByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetEmployeesByUserRole(operatingCenterId, LockoutFormController.ROLE));
        }

        [HttpGet]
        public ActionResult ActiveLockoutFormEmployeesByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetActiveEmployeesByUserRole(operatingCenterId, LockoutFormController.ROLE));
        }

        #endregion

        #region ActiveEmployeesByOperatingCenterIdForJobSiteCheckLists

        [HttpGet]
        public ActionResult ActiveEmployeesByOperatingCenterIdForJobSiteCheckLists(int id)
        {
            // bug-3248: If the operating center is SOV it needs to return NJ6 employees.
            // bug-3533: This is now controlled by OperatingCenter.OperatedByOperatingCenter.

            // NOTE: This needs to display *both* collections of employees from the given operating center
            //       as well as the OperatedByOperatingCenter for old JSCL records that were entered prior
            //       to bug 3248.
            var opcRepo = _container.GetInstance<IOperatingCenterRepository>();
            var opc = opcRepo.Find(id);
            IQueryable<Employee> results;

            if (opc.OperatedByOperatingCenter != null)
            {
                results =
                    Repository.GetActiveEmployeesWhere(e =>
                        e.OperatingCenter.Id == id ||
                        e.OperatingCenter.Id == opc.OperatedByOperatingCenter.Id ||
                        e.User.Roles.Any(r =>
                            r.Module.Id == (int)RoleModules.FieldServicesWorkManagement &&
                            (r.OperatingCenter == null || r.OperatingCenter.Id == id ||
                             r.OperatingCenter.Id == opc.OperatedByOperatingCenter.Id)));
            }
            else
            {
                results =
                    Repository.GetActiveEmployeesWhere(e =>
                        e.OperatingCenter.Id == id ||
                        e.User.Roles.Any(r =>
                            r.Module.Id == (int)RoleModules.FieldServicesWorkManagement &&
                            (r.OperatingCenter == null || r.OperatingCenter.Id == id)));
            }

            return new CascadingActionResult<Employee, EmployeeDisplayItem>(results);
        }

        #endregion

        #region ActiveEmployeesByOperatingCenterIdForPreJobSafetyBriefs

        [HttpGet]
        public ActionResult ActiveEmployeesByOperatingCenterIdForPreJobSafetyBriefs(int operatingCenterId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetActiveEmployeesByUserRole(
                    operatingCenterId,
                    ProductionPreJobSafetyBriefController.ROLE));
        }

        #endregion

        #region ActiveEmployeesWithNoCdlByOperatingCenterId

        [HttpGet]
        public ActionResult ActiveEmployeesWithNoCdlByOperatingCenterId(int? id)
        {
            return
                new CascadingActionResult<Employee, EmployeeDisplayItem>(
                    Repository.GetActiveEmployeesWhere(
                        e => e.OperatingCenter.Id == id && e.DriversLicense == null));
        }

        #endregion

        #region ActiveEmployeesByState

        [HttpGet]
        public ActionResult ActiveEmployeesByStateId(int id)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.Where(e => e.OperatingCenter.State.Id == id &&
                                      e.Status.Id == EmployeeStatus.Indices.ACTIVE));
        }

        [HttpGet]
        public ActionResult ActiveEmployeesByStateIdAndPartial(string partial, int id)
        {
            var results = Repository.Where(e =>
                e.OperatingCenter.State.Id == id &&
                e.Status.Id == EmployeeStatus.Indices.ACTIVE &&
                (e.FirstName.Contains(partial) || e.LastName.Contains(partial)));
            return new AutoCompleteResult(results, "Id", "Description");
        }

        #endregion

        #region AutoCompleteGeneric

        [HttpGet]
        public ActionResult HealthAndSafetyActiveEmployeesByRoleAndPartial(string partial)
        {
            var curUser = AuthenticationService.CurrentUser;
            var query = Repository.GetActiveEmployeesByOperatingCentersForRole(
                curUser,
                RoleModules.OperationsHealthAndSafety);
            var results = query
                         .Where(e => e.FirstName.Contains(partial) || e.LastName.Contains(partial))
                         .Select(e => new Employee {
                             FirstName = e.FirstName,
                             MiddleName = e.MiddleName,
                             LastName = e.LastName,
                             EmployeeId = e.EmployeeId,
                             Id = e.Id,
                             OperatingCenter = new OperatingCenter {
                                 OperatingCenterCode = e.OperatingCenter.OperatingCenterCode
                             }
                         })
                         .ToList();
            return new AutoCompleteResult(results, "Id", "Description");
        }

        [HttpGet]
        public ActionResult PlanActiveEmployeesByRoleAndPartial(string partial)
        {
            var curUser = AuthenticationService.CurrentUser;
            var query = Repository.GetActiveEmployeesByOperatingCentersForRole(
                curUser,
                EmergencyResponsePlanController.ROLE);
            var results = query
                         .Where(e => e.FirstName.Contains(partial) || e.LastName.Contains(partial))
                         .Select(e => new Employee {
                             FirstName = e.FirstName,
                             MiddleName = e.MiddleName,
                             LastName = e.LastName,
                             EmployeeId = e.EmployeeId,
                             Id = e.Id,
                             OperatingCenter = new OperatingCenter {
                                 OperatingCenterCode = e.OperatingCenter.OperatingCenterCode
                             }
                         })
                         .ToList();
            return new AutoCompleteResult(results, "Id", "Description");
        }

        #endregion

        #region HasOneDayDoctorsNoteRestriction

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult HasOneDayDoctorsNoteRestriction(int employeeId)
        {
            var data = new Dictionary<string, object>();
            var emp = Repository.Find(employeeId);
            if (emp == null)
            {
                data["success"] = false;
            }
            else
            {
                data["success"] = true;
                data["hasRestriction"] = emp.HasOneDayDoctorsNoteRestriction;

                if (emp.HasOneDayDoctorsNoteRestriction)
                {
                    // This is for display and the json serializer serializes dates all stupidly.
                    data["restrictionEndDate"] =
                        emp.OneDayDoctorsNoteRestrictionEndDate.Value.ToString(CommonStringFormats
                           .DATE_WITHOUT_PARAMETER);
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Covid Issue

        [HttpGet]
        public ActionResult CovidIssueActiveEmployeesByState(int stateId)
        {
            var curUser = AuthenticationService.CurrentUser;
            var query = Repository.GetActiveEmployeesByOperatingCentersForRole(
                curUser,
                RoleModules.HumanResourcesCovid);
            var results = query.Where(e => e.OperatingCenter.State.Id == stateId);
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(results);
        }

        [HttpGet]
        public ActionResult ByOperatingCentersOrStateForHumanResourcesCovid(
            int? stateId,
            int[] operatingCenters)
        {
            var curUser = AuthenticationService.CurrentUser;
            var query = Repository.GetActiveEmployeesByOperatingCentersForRole(
                curUser,
                RoleModules.HumanResourcesCovid);
            var results = (operatingCenters != null && operatingCenters.Any())
                    ? query.Where(e => operatingCenters.Contains(e.OperatingCenter.Id))
                    : query.Where(e => e.OperatingCenter.State.Id == stateId)
                ;
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(results);
        }

        #endregion

        #region ByOperatingCentersOrState

        public ActionResult ByOperatingCentersOrState(int? stateId, int[] operatingCenters)
        {
            IQueryable<Employee> ret;
            if (operatingCenters != null && operatingCenters.Any())
            {
                ret = Repository.GetEmployeesByOperatingCenters(operatingCenters);
            }
            else if (stateId.HasValue)
            {
                ret = Repository.Where(e => e.OperatingCenter.State.Id == stateId.Value);
            }
            else
            {
                ret = Array.Empty<Employee>().AsQueryable();
            }

            return new CascadingActionResult<Employee, EmployeeDisplayItem>(ret);
        }

        #endregion

        #region GetEmployeeByOperatingCentersWhoHaveEnteredSystemDeliveryEquipmentEntries

        [HttpGet]
        public ActionResult GetEmployeeByOperatingCentersWhoHaveEnteredSystemDeliveryEntries(
            int[] operatingCenterIds)
        {
            var facilityEntryRepo = _container.GetInstance<IRepository<SystemDeliveryFacilityEntry>>();
            IEnumerable<SystemDeliveryFacilityEntry> systemDeliveryFacilityEntries =
                facilityEntryRepo.Where(x => operatingCenterIds.Contains(x.Facility.OperatingCenter.Id));
            IEnumerable<Employee> employeeList =
                systemDeliveryFacilityEntries.Select(x => x.EnteredBy).Distinct();

            return new CascadingActionResult(employeeList, "Display", "Id");
        }

        #endregion

        #region Employees with Risk Register Role

        [HttpGet]
        public ActionResult RiskRegisterEmployeesByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(
                Repository.GetEmployeesByUserRole(operatingCenterId, RiskRegisterAssetController.ROLE));
        }

        #endregion

        #endregion

        #region Constructors

        public EmployeeController(
            ControllerBaseWithPersistenceArguments<IEmployeeRepository, Employee, User> args)
            : base(args) {}

        #endregion
    }
}