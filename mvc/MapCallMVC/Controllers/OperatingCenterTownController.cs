using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class OperatingCenterTownController : ControllerBaseWithPersistence<IRepository<OperatingCenterTown>, OperatingCenterTown, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.GeneralTowns;
        
        #endregion
        
        #region Constructors

        public OperatingCenterTownController(ControllerBaseWithPersistenceArguments<IRepository<OperatingCenterTown>, OperatingCenterTown, User> args): base(args) {}
        public OperatingCenterTownController() : this(null) { }

        #endregion

        #region Create
        
        [HttpPost, RequiresAdmin]
        public ActionResult Create(CreateOperatingCenterTown model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToAction("Show", "Town", new { area = "", id = model.Town })
            });
        }

        #endregion

        #region Destroy

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            var town = Repository.Find(id)?.Town.Id;
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                OnSuccess = () => RedirectToAction("Show", "Town", new { area = "", id = town })
            });
        }

        #endregion
    }
}