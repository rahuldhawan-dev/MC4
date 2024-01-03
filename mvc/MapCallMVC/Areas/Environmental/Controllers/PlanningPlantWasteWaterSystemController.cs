using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class PlanningPlantWasteWaterSystemController : ControllerBaseWithPersistence<IRepository<PlanningPlantWasteWaterSystem>, PlanningPlantWasteWaterSystem, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Constructors

        public PlanningPlantWasteWaterSystemController(ControllerBaseWithPersistenceArguments<IRepository<PlanningPlantWasteWaterSystem>, PlanningPlantWasteWaterSystem, User> args) : base(args) {}
        public PlanningPlantWasteWaterSystemController() : this(null) {}

        #endregion

        #region Create

        [HttpPost, RequiresAdmin]
        public ActionResult Create(CreatePlanningPlantWasteWaterSystem model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToAction("Show", "WasteWaterSystem", new { area = "Environmental", id = model.WasteWaterSystem })
            });
        }

        #endregion

        #region Destroy

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            // Need to get this before the record is deleted.
            var wwsid = Repository.Find(id)?.WasteWaterSystem?.Id;
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                OnSuccess = () => RedirectToAction("Show", "WasteWaterSystem", new { area = "Environmental", id = wwsid })
            });
        }

        #endregion
    }
}
