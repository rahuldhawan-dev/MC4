using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class OperatingCenterServiceMaterialController : ControllerBaseWithPersistence<IRepository<OperatingCenterServiceMaterial>, OperatingCenterServiceMaterial, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesDataLookups;

        #endregion

        #region Contructors

        public OperatingCenterServiceMaterialController(ControllerBaseWithPersistenceArguments<IRepository<OperatingCenterServiceMaterial>, OperatingCenterServiceMaterial, User> args) : base(args) {}

        #endregion

        #region Create

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Create(CreateOperatingCenterServiceMaterial model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs
            {
                OnSuccess = () => RedirectToAction("Show", "ServiceMaterial", new { area = "", id = model.ServiceMaterial })
            });
        }

        #endregion

        #region Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Destroy(int id)
        {
            // Need to store this before it's deleted.
            var serviceMaterialId = Repository.Find(id)?.ServiceMaterial?.Id;
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                OnSuccess = () => RedirectToAction("Show", "ServiceMaterial", new { area = "", id = serviceMaterialId })
            });
        }

        #endregion
    }
}