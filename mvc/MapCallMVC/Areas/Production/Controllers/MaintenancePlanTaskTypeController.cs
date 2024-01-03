using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class MaintenancePlanTaskTypeController : ControllerBaseWithPersistence<MaintenancePlanTaskType, User>
    {
        public const RoleModules ROLE = RoleModules.ProductionDataAdministration;

        public MaintenancePlanTaskTypeController(ControllerBaseWithPersistenceArguments<IRepository<MaintenancePlanTaskType>, MaintenancePlanTaskType, User> args) : base(args) { }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchMaintenancePlanTaskType search) => ActionHelper.DoSearch(search);

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchMaintenancePlanTaskType search) => this.RespondTo((formatter) => {
            formatter.View(() => ActionHelper.DoIndex(search));
            formatter.Excel(() => ActionHelper.DoExcel(search));
        });

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id) => ActionHelper.DoShow(id);

        #endregion

        #region New/Create

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(MaintenancePlanTaskTypeViewModel model) => ActionHelper.DoCreate(model);

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New() => ActionHelper.DoNew(ViewModelFactory.Build<MaintenancePlanTaskTypeViewModel>());

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id) => ActionHelper.DoEdit<MaintenancePlanTaskTypeViewModel>(id);

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(MaintenancePlanTaskTypeViewModel model) => ActionHelper.DoUpdate(model);

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id) => ActionHelper.DoDestroy(id);

        #endregion
    }
}