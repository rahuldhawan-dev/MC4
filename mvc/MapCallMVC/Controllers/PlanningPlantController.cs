using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class PlanningPlantController : ControllerBaseWithPersistence<PlanningPlant, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesDataLookups;

        #endregion

        #region Constructors

        public PlanningPlantController(ControllerBaseWithPersistenceArguments<IRepository<PlanningPlant>, PlanningPlant, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchPlanningPlant model)
        {
            return ActionHelper.DoIndex(model);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchPlanningPlant>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new PlanningPlantViewModel(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(PlanningPlantViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<PlanningPlantViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(PlanningPlantViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        [HttpGet]
        public ActionResult ByOperatingCenter(int? operatingCenter)
        {
            return
                new CascadingActionResult<PlanningPlant, PlanningPlantDisplayItem>(
                    Repository.Where(s => s.OperatingCenter.Id == operatingCenter), "Display", "Id");
        }

        [HttpGet]
        public ActionResult ByOperatingCenters(int[] operatingCenters)
        {
            return new CascadingActionResult<PlanningPlant, PlanningPlantDisplayItem>(
                Repository.Where(s => operatingCenters.Contains(s.OperatingCenter.Id)), "Display", "Id");
        }

        [HttpGet]
        public ActionResult ByState(int? state)
        {
            return
                new CascadingActionResult<PlanningPlant, PlanningPlantDisplayItem>(
                    Repository.Where(s => s.OperatingCenter.State.Id == state), "Display", "Id");
        }

        [HttpGet]
       public ActionResult ByOperatingCenterCodeAndNotId(int? operatingCenter)
        {
            return
                new CascadingActionResult<PlanningPlant, PlanningPlantDisplayItem>(
                    Repository.Where(s => s.OperatingCenter.Id == operatingCenter), "Display", "Code");
        }


        #endregion
    }
}