using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using System.Web.Mvc;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class TaskGroupController : ControllerBaseWithPersistence<IRepository<TaskGroup>, TaskGroup, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionDataAdministration;

        #endregion

        #region Constructor

        public TaskGroupController(ControllerBaseWithPersistenceArguments<IRepository<TaskGroup>, TaskGroup, User> args) : base(args) { }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<TaskGroupViewModel>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(TaskGroupViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchTaskGroup search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchTaskGroup search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = false
                }));
                f.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository
                       .Search(search)
                       .Select(x => new {
                            x.Id,
                            x.TaskGroupId,
                            x.TaskGroupName,
                            x.MaintenancePlanTaskType,
                            x.TaskDetails,
                            x.TaskGroupCategory,
                            EquipmentDLifespans = string.Join(", ",
                                x.EquipmentLifespans.Select(y => y.Description).ToArray()),
                            EquipmentPurposes = string.Join(", ", x.EquipmentPurposes.Select(y => y.Description).ToArray()),
                            EquipmentTypes = string.Join(", ", x.EquipmentTypes.Select(y => y.Display).ToArray())
                        }).ToList();
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<TaskGroupViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(TaskGroupViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByTaskGroupCategoryIdOrAll

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult ByTaskGroupCategoryIdOrAll(int? taskGroupCategoryId)
        {
            if (taskGroupCategoryId == null)
            {
                return new CascadingActionResult<TaskGroup, TaskGroupDisplayItem>(_repository.GetAllSorted(x => x.TaskGroupId));
            }
            return new CascadingActionResult<TaskGroup, TaskGroupDisplayItem>(_repository.Where(x => x.TaskGroupCategory.Id == taskGroupCategoryId).OrderBy(x => x.TaskGroupId));
        }

        #endregion

        #region ByMaintenancePlanTaskTypeIds

        [HttpGet]
        public ActionResult ByMaintenancePlanTaskTypeIds(int[] maintenancePlanTaskTypeIds)
        {
            var query = _repository.Where(x => maintenancePlanTaskTypeIds.Contains(x.MaintenancePlanTaskType.Id));
            var taskGroups = query.Distinct().ToList();
            return new CascadingActionResult(taskGroups, "TaskGroupName", "Id");
        }

        #endregion

        #region ByTaskGroupCategoryIdByEquipmentTypeId

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult ByTaskGroupCategoryIdByEquipmentTypeId(int taskGroupCategoryId, int equipmentTypeId)
        {
            var query = _repository.Where(x =>
                x.TaskGroupCategory.Id == taskGroupCategoryId && x.EquipmentTypes.Any(t => t.Id == equipmentTypeId));
            var taskGroups = query.Distinct().ToList();

            return new CascadingActionResult(taskGroups, "TaskGroupName", "Id");
        }

        #endregion

        #region ByTaskGroupCategoryIdByEquipmentTypeIds

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult ByTaskGroupCategoryIdByEquipmentTypeIds(int taskGroupCategoryId, int[] equipmentTypeIds)
        {
            var query = _repository.Where(x =>
                x.TaskGroupCategory.Id == taskGroupCategoryId &&
                x.EquipmentTypes.Any(t => equipmentTypeIds.Contains(t.Id)));
            var taskGroups = query.Distinct().ToList();

            return new CascadingActionResult(taskGroups, "TaskGroupName", "Id");
        }

        #endregion

        #region ByTaskTypes

        [HttpGet]
        public ActionResult ByTaskTypesForTaskGroupNames(int[] taskTypeIds)
        {
            var query = _repository.Where(x =>
                taskTypeIds.Contains(x.MaintenancePlanTaskType.Id));
            var taskGroupNames = query.Distinct().ToList();
            return new CascadingActionResult(taskGroupNames, "TaskGroupName", "TaskGroupName");
        }

        #endregion

        #region Children

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult GetTaskDetailAndPlanTypeInformation(int taskGroupId)
        {
            var results = Container.GetInstance<IRepository<TaskGroup>>().Find(taskGroupId);
            return this.RespondTo((formatter) => {
                formatter.Json(() => {
                    return Json(new {
                        Data = new {
                            results.TaskDetails,
                            results.TaskDetailsSummary,
                            PlanType = results.MaintenancePlanTaskType?.Description ?? string.Empty
                        }
                    }, JsonRequestBehavior.AllowGet);
                });
            });
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}