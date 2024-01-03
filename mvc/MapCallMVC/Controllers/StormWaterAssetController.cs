using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class StormWaterAssetController : ControllerBaseWithPersistence<StormWaterAsset, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    break;
            }
            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Search(SearchStormWaterAsset search)
        {
            return ActionHelper.DoSearch(search);
        }

        // TODO: Caching must die
        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Index(SearchStormWaterAsset search)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Show(int id)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoShow(id));
                formatter.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs
                {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
            });
        }

        #endregion

        public StormWaterAssetController(ControllerBaseWithPersistenceArguments<IRepository<StormWaterAsset>, StormWaterAsset, User> args) : base(args) { }
    }

}
