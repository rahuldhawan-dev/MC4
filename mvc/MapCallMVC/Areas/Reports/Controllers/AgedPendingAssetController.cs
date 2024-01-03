using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class AgedPendingAssetController : ControllerBaseWithPersistence<IHydrantRepository, Hydrant, User>
    {
        #region Consts

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public AgedPendingAssetController(ControllerBaseWithPersistenceArguments<IHydrantRepository, Hydrant, User> args) : base(args) {}

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData("OperatingCenter");
            }
        }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchAgedPendingAsset search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchAgedPendingAsset search)
        {
            return this.RespondTo(f => {
                search.EnablePaging = false;
                var valveRepository = _container.GetInstance<IValveRepository>();
                var hydrantAgedPendingAssets = Repository.GetAgedPendingAssets(search).ToList();
                var valveAgedPendingAssets = valveRepository.GetAgedPendingAssets(search).ToList();
                var results = valveAgedPendingAssets.Union(hydrantAgedPendingAssets).OrderBy(x => x.OperatingCenter.OperatingCenterCode);
                f.View(() => View("Index", results));
                f.Excel(() => this.Excel(results));
            });
        }

        #endregion
    }
}