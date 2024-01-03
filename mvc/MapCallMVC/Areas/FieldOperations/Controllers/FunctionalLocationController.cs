using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using NHibernate.Util;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class FunctionalLocationController : ControllerBaseWithPersistence<IFunctionalLocationRepository, FunctionalLocation, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDropDownData<AssetType>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                   // this.AddDropDownData<ITownRepository, Town>(t => t.GetAllSorted(), t => t.Id, t => t.ShortName);
                    this.AddOperatingCenterDropDownData();
                    break;
                case ControllerAction.Search:
                    this.AddDropDownData<AssetType>("AssetType", f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    //this.AddDropDownData<ITownRepository, Town>("Town", t => t.GetAllSorted(), t => t.Id, t => t.ShortName);
                    this.AddOperatingCenterDropDownData();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups)]
        public ActionResult Search(SearchFunctionalLocation model)
        {
            return ActionHelper.DoSearch(model);
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups)]
        public ActionResult Index(SearchFunctionalLocation model)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(model));
                formatter.Excel(() => ActionHelper.DoExcel(model));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateFunctionalLocation(_container));
        }

        [HttpPost, RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Add)]
        public ActionResult Create(CreateFunctionalLocation model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditFunctionalLocation>(id);
        }

        [HttpPost, RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Edit)]
        public ActionResult Update(EditFunctionalLocation model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region ByTownForFacilityAssetType

        [HttpGet]
        public ActionResult ByTownForFacilityAssetType(int townId)
        {
            return new CascadingActionResult(Repository.GetByTownIdAndAssetTypeId(townId, AssetType.Indices.FACILITY), "Description", "Id");
        }

        [HttpGet]
        public ActionResult ByTownForSewerOpenings(int townId)
        {
            return new CascadingActionResult(
                Repository.GetByTownIdAndAssetTypeId(townId, AssetType.Indices.SEWER_OPENING).Concat(
                Repository.GetByTownIdAndAssetTypeId(townId, AssetType.Indices.SEWER_MAIN)), "Description", "Id");
        }

        [HttpGet]
        public ActionResult ActiveByTownForSewerOpenings(int townId)
        {
            return new CascadingActionResult(
                Repository.GetActiveByTownIdAndAssetTypeId(townId, AssetType.Indices.SEWER_OPENING).Concat(
                Repository.GetActiveByTownIdAndAssetTypeId(townId, AssetType.Indices.SEWER_MAIN)), "Description", "Id");
        }

        [HttpGet]
        public ActionResult ByTownForMainAsset(int townId)
        {
            return new CascadingActionResult(
                Repository.GetByTownIdAndAssetTypeId(townId, AssetType.Indices.MAIN), "Description", "Id");
        }

        [HttpGet]
        public ActionResult ByTownForSewerMainAsset(int townId)
        {
            return new CascadingActionResult(
                Repository.GetByTownIdAndAssetTypeId(townId, AssetType.Indices.SEWER_MAIN), "Description", "Id");
        }

        #endregion

        #region ByTownId

        [HttpGet]
        public ActionResult ByTownId(int townId)
        {
            return new CascadingActionResult(Repository.GetByTownId(townId), "Description", "Id");
        }

        #endregion

        #region ActiveByTownId

        [HttpGet]
        public ActionResult ActiveByTownId(int townId)
        {
            return new CascadingActionResult(Repository.GetActiveByTownId(townId), "Description", "Id");
        }

        [HttpGet]
        public ActionResult ActiveByTownIdForHydrantAssetType(int townId)
        {
            return new CascadingActionResult(Repository.GetActiveByTownId(townId).Where(fl => fl.AssetType.Id == AssetType.Indices.HYDRANT), "Description", "Id");
        }

        [HttpGet]
        public ActionResult ActiveByTownIdForSewerOpeningAssetType(int townId)
        {
            return new CascadingActionResult(Repository.GetActiveByTownId(townId).Where(fl => fl.AssetType.Id == AssetType.Indices.SEWER_OPENING), "Description", "Id");
        }

        [HttpGet]
        public ActionResult ActiveByTownIdForValveAssetType(int? townId, int? valveControlsId)
        {
            IEnumerable<FunctionalLocation> results = Enumerable.Empty<FunctionalLocation>();
            if (townId.HasValue)
            {
                if (valveControlsId.GetValueOrDefault() == ValveControl.Indices.BLOW_OFF_WITH_FLUSHING)
                {
                    results = Repository.GetActiveByTownId(townId.Value).Where(fl => fl.AssetType.Id == AssetType.Indices.HYDRANT || fl.AssetType.Id == AssetType.Indices.VALVE);
                }
                else
                {
                    results = Repository.GetActiveByTownId(townId.Value).Where(fl => fl.AssetType.Id == AssetType.Indices.VALVE);
                }
            }

            return new CascadingActionResult(results, "Description", "Id");
        }

        #endregion

        #region ByFacilityId

        [HttpGet]
        public ActionResult ByFacilityId(int facilityId)
        {
            return new CascadingActionResult(Repository.GetByFacilityId(facilityId), "Description", "Id");
        }

        #endregion

        public FunctionalLocationController(ControllerBaseWithPersistenceArguments<IFunctionalLocationRepository, FunctionalLocation, User> args) : base(args) {}
    }
}
