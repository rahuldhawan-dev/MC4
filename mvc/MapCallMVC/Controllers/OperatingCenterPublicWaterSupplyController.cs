using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class OperatingCenterPublicWaterSupplyController : ControllerBaseWithPersistence<IRepository<OperatingCenterPublicWaterSupply>, OperatingCenterPublicWaterSupply, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Constructors

        public OperatingCenterPublicWaterSupplyController(ControllerBaseWithPersistenceArguments<IRepository<OperatingCenterPublicWaterSupply>, OperatingCenterPublicWaterSupply, User> args) : base(args) {}
        public OperatingCenterPublicWaterSupplyController() : this(null) {}

        #endregion

        #region Create

        [HttpPost, RequiresAdmin]
        public ActionResult Create(CreateOperatingCenterPublicWaterSupply model)
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