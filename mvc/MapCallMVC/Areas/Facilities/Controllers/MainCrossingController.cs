using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    [DisplayName("Main Crossing and Easement Inspection")]
    public class MainCrossingController : ControllerBaseWithPersistence<IMainCrossingRepository, MainCrossing, User>
    {
        #region Constants

        public const string NOT_FOUND = "Main Crossing with the id '{0}' was not found.";

        #endregion

        #region Private Methods

        private MapResult GetMapResult(SearchMainCrossing search)
        {
            var result = _container.GetInstance<AssetMapResult>();

            if (ModelState.IsValid)
            {
                search.EnablePaging = false;

                // Bug 2558: If loading related assets from AssetMap, do not include the RequiresInspection search.
                if (search.IsRelatedAssetSearch)
                {
                    search.RequiresInspection = null;
                }

                var coords = Repository.Search(search).Select(x => x.ToAssetCoordinate());
                if (search.EntityId.HasValue)
                {
                    result.Initialize(coords);
                }
                else
                {
                    var valveSearchRvd = new RouteValueDictionary();
                    valveSearchRvd[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatterExtensions.MAP_ROUTE_EXTENSION;

                    foreach (var ms in ModelState)
                    {
                        valveSearchRvd[ms.Key] = ms.Value.Value.AttemptedValue;
                    }
                    valveSearchRvd["IsRelatedAssetSearch"] = true;
                    result.RelatedAssetsUrl = Url.Action("Index", "Valve", valveSearchRvd);
                    result.Initialize(coords);
                }
            }

            return result;
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<PipeMaterial>("MainMaterial", d => d.GetAllSorted(x => x.Description), d => d.Id, d => d.Description);
                    this.AddDropDownData<PipeDiameter>("MainDiameter", d => d.GetAllSorted(x => x.Diameter), d => d.Id, d => d.Diameter);
                    this.AddDropDownData<RecurringFrequencyUnit>("InspectionFrequencyUnit");
                    this.AddDropDownData<TypicalOperatingPressureRange>(d => d.GetAllSorted(x => x.Id), d => d.Id, d => d.Description);
                    this.AddDropDownData<PressureSurgePotentialType>(d => d.GetAllSorted(x => x.Id), d => d.Id, d => d.Description);
                    break;
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownData(x => x.IsActive);
                    this.AddDropDownData<PipeMaterial>("MainMaterial", d => d.GetAllSorted(x => x.Description), d => d.Id, d => d.Description);
                    this.AddDropDownData<PipeDiameter>("MainDiameter", d => d.GetAllSorted(x => x.Diameter), d => d.Id, d => d.Diameter);
                    this.AddDropDownData<RecurringFrequencyUnit>("InspectionFrequencyUnit");
                    this.AddDropDownData<TypicalOperatingPressureRange>(d => d.GetAllSorted(x => x.Id), d => d.Id, d => d.Description);
                    this.AddDropDownData<PressureSurgePotentialType>(d => d.GetAllSorted(x => x.Id), d => d.Id, d => d.Description);
                    break;
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<BodyOfWater>(d => d.GetAllSorted(x => x.Name), d => d.Id, d => d.Name);
                    this.AddDropDownData<TypicalOperatingPressureRange>(d => d.GetAllSorted(x => x.Id), d => d.Id, d => d.Description);
                    this.AddDropDownData<PressureSurgePotentialType>(d => d.GetAllSorted(x => x.Id), d => d.Id, d => d.Description);
                    break;
            }
            this.AddDropDownData<MainCrossingInspectionType>("InspectionType");
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Search(SearchMainCrossing search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Show(int id)
        {
            return this.RespondTo((f) => {
                f.View(() => ActionHelper.DoShow(id));
                f.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true 
                }));
            });
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Index(SearchMainCrossing search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs { RedirectSingleItemToShowView = false }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => GetMapResult(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateMainCrossing(_container));
        }

        [HttpPost, RequiresRole(RoleModules.FieldServicesAssets, RoleActions.Add)]
        public ActionResult Create(CreateMainCrossing model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMainCrossing>(id);
        }

        [HttpPost, RequiresRole(RoleModules.FieldServicesAssets, RoleActions.Edit)]
        public ActionResult Update(EditMainCrossing model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region ByOperatingCenterId

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult<MainCrossing, MainCrossingDisplayItem>(Repository.GetByOperatingCenterForSelect(id), "Display", "Id");
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult ByOperatingCenterIds(int[] ids)
        {
            return new CascadingActionResult<MainCrossing, MainCrossingDisplayItem>(Repository.GetByOperatingCentersForSelect(ids).OrderBy(x => x.OperatingCenter.OperatingCenterCode), "Display", "Id");
        }
        
        #endregion

        #region ByTownId

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult ByTownIdForWorkOrders(int townId)
        {
            return new CascadingActionResult<MainCrossing, MainCrossingDisplayItem>(Repository.GetByTownIdForWorkOrders(townId), "Display", "Id")
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        public MainCrossingController(ControllerBaseWithPersistenceArguments<IMainCrossingRepository, MainCrossing, User> args) : base(args) {}
    }
}
