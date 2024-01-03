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
using MapCall.Common.Model.Repositories;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class TaskGroupCategoryController : ControllerBaseWithPersistence<IRepository<TaskGroupCategory>, TaskGroupCategory, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionDataAdministration;

        #endregion

        #region Constructor

        public TaskGroupCategoryController(ControllerBaseWithPersistenceArguments<IRepository<TaskGroupCategory>, TaskGroupCategory, User> args) : base(args) { }

        #endregion

        #region ByEquipmentTypeId

        [HttpGet]
        public ActionResult ByEquipmentTypeId(int equipmentTypeId)
        {
            // Because of all the many-to-many connections, we need to call
            // Distinct() on the results to avoid duplicates. NHibernate doesn't
            // support Distinct() on this kind of query when it's ran through the
            // SelectDynamic stuff for whatever reason and throws an exception,
            // so this also needs a ToList(). -Ross 5/1/2020
            var taskGroupCategories = (from cat in Repository.Where(x => true)
                                       from tg in cat.TaskGroups
                                       from classes in tg.EquipmentTypes
                                       where classes.Id == equipmentTypeId
                                       select new TaskGroupCategory {Type = cat.Type, Id = cat.Id}).Distinct().ToList();
            return new CascadingActionResult(taskGroupCategories, "Type", "Id");
        }

        [HttpGet]
        public ActionResult ByEquipmentTypeIds(int[] equipmentTypeIds)
        {
            // Because of all the many-to-many connections, we need to call
            // Distinct() on the results to avoid duplicates. NHibernate doesn't
            // support Distinct() on this kind of query when it's ran through the
            // SelectDynamic stuff for whatever reason and throws an exception,
            // so this also needs a ToList(). -Ross 5/1/2020
            var taskGroupCategories = (from cat in Repository.Where(x => true)
                                       from tg in cat.TaskGroups
                                       from classes in tg.EquipmentTypes
                                       where equipmentTypeIds.Contains(classes.Id)
                                       select new TaskGroupCategory {Type = cat.Type, Id = cat.Id}).Distinct().ToList();
            return new CascadingActionResult(taskGroupCategories, "Type", "Id");
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<TaskGroupCategoryViewModel>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(TaskGroupCategoryViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchTaskGroupCategory search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchTaskGroupCategory search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<TaskGroupCategoryViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(TaskGroupCategoryViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
    }
}
