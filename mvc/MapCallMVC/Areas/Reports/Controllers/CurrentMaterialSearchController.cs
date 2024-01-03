using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.MostRecentlyInstalledServices;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class CurrentMaterialSearchController
        : ControllerBaseWithPersistence<
            IMostRecentlyInstalledServiceRepository,
            MostRecentlyInstalledService,
            User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public CurrentMaterialSearchController(
            ControllerBaseWithPersistenceArguments<
                IMostRecentlyInstalledServiceRepository,
                MostRecentlyInstalledService,
                User> args)
            : base(args) { }

        #endregion

        #region Private Methods

        private void GetResults(ISearchCurrentMaterial search)
        {
            Repository.GetCurrentMaterial(search);
        }

        private ActionResult GetMapResult(SearchCurrentMaterialForMap search)
        {
            if (Repository.GetCountForSearchSet(search) > SearchCurrentMaterialForMap.MAX_MAP_RESULT_COUNT)
            {
                return null;
            }

            return _container
                  .GetInstance<IMapResultFactory>()
                  .Build(ModelState, () => Repository.SearchForMap(search));
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            this.AddDropDownData<State>(s => s.Id, s => s.Abbreviation);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchCurrentMaterial());
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchCurrentMaterial search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => GetResults(search)
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => GetResults(search)
                }));
                formatter.Map(() => GetMapResult(search));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter =>
                formatter.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true
                })));
        }

        #endregion
    }
}
