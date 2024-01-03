using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class AssetTypeController : ControllerBaseWithPersistence<IAssetTypeRepository, AssetType, User>
    {
        #region Constructors

        public AssetTypeController(ControllerBaseWithPersistenceArguments<IAssetTypeRepository, AssetType, User> args) : base(args) {}

        #endregion

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterId(id), "Description", "Id")
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion
    }
}