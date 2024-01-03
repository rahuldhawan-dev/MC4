using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class PlanningPlantPublicWaterSupplyController : ControllerBaseWithPersistence<IRepository<PlanningPlantPublicWaterSupply>, PlanningPlantPublicWaterSupply, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Constructors

        public PlanningPlantPublicWaterSupplyController(ControllerBaseWithPersistenceArguments<IRepository<PlanningPlantPublicWaterSupply>, PlanningPlantPublicWaterSupply, User> args) : base(args) {}
        public PlanningPlantPublicWaterSupplyController() : this(null) {}

        #endregion

        #region Create

        [HttpPost, RequiresAdmin]
        public ActionResult Create(CreatePlanningPlantPublicWaterSupply model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToAction("Show", "PublicWaterSupply", new { area = "", id = model.PublicWaterSupply })
            });
        }

        #endregion

        #region Destroy

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            // Need to get this before the record is deleted.
            var pwsid = Repository.Find(id)?.PublicWaterSupply?.Id;
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                OnSuccess = () => RedirectToAction("Show", "PublicWaterSupply", new { area = "", id = pwsid })
            });
        }

        #endregion
    }
}
