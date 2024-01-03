using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    [DisplayName("Municipalities")]
    public class TownController : ControllerBaseWithPersistence<ITownRepository, Town, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.GeneralTowns;
        public const string TOWN_NOT_FOUND = "Town not found.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDropDownData<State>(s => s.Id, s => s.Name);
                    this.AddDropDownData<AbbreviationType>();
                    break;
                case ControllerAction.Search:
                    this.AddDropDownData<State>("State", s => s.Id, s => s.Name);
                    break;
                case ControllerAction.Show:
                    this.AddOperatingCenterDropDownData();
                    this.AddDynamicDropDownData<PublicWaterSupply, PublicWaterSupplyDisplayItem>();
                    this.AddDynamicDropDownData<WasteWaterSystem, WasteWaterSystemDisplayItem>();
                    this.AddDropDownData<ContactType>();
                    this.AddDropDownData<Gradient>();
                    break;
            }

            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Search(SearchTown town)
        {
            return ActionHelper.DoSearch(town);
        }

        [HttpGet]
        public ActionResult Index(SearchTown search)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
                {
                    RedirectSingleItemToShowView = true
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, onModelFound: (town) => {
                this.AddDropDownData<IFireDistrictRepository, FireDistrict>("FireDistrict", r => r.GetByStateId(town.State.Id),
                    fd => fd.Id, fd => string.Format("{0} - {1}", fd.AddressCity, fd.DistrictName));
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTown>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditTown town)
        {
            return ActionHelper.DoUpdate(town);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresAdmin]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateTown(_container));
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Create(CreateTown model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region ByCountyId

        [HttpGet]
        public ActionResult ByCountyId(int countyId)
        {
            return new CascadingActionResult(Repository.GetByCountyId(countyId), "ShortName", "Id");
        }

        #endregion

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterId(id), "ShortName", "Id");
        }

        [HttpGet]
        public ActionResult ByOperatingCenterIds(int[] ids)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterIds(ids), "ShortName", "Id");
        }

        #endregion

        [HttpGet]
        public ActionResult ByStateId(int stateId)
        {
            return new CascadingActionResult(Repository.GetByStateId(stateId), "ShortName", "Id");
        }

        [HttpGet]
        public ActionResult ByStateIdWithCounty(int stateId)
        {
            return new CascadingActionResult(Repository.GetByStateId(stateId), "NameWithCounty", "Id");
        }

        #region WithFacilitiesByStateId

        [HttpGet]
        public ActionResult WithFacilitiesByStateId(int stateId)
        {
            return new CascadingActionResult(Repository.GetWithFacilitiesByStateId(stateId), "ShortName", "Id");
        }

        #endregion

        #region AddPublicWaterSupply

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddPublicWaterSupply(AddPublicWaterSupplyTown model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region RemovePublicWaterSupply

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemovePublicWaterSupply(RemovePublicWaterSupplyTown model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region CreateTownContact

        [HttpPost, RequiresRole(RoleModules.GeneralTowns, RoleActions.Edit)]
        public ActionResult CreateTownContact(int id, CreateTownContact model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region DestroyTownContact

        [HttpDelete, RequiresRole(RoleModules.GeneralTowns, RoleActions.Edit)]
        public ActionResult DestroyTownContact(int id, DestroyTownContact model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region WasteWaterSystems

        [HttpPost, RequiresAdmin]
        public ActionResult AddWasteWaterSystem(AddWasteWaterSystemTown model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresAdmin]
        public ActionResult RemoveWasteWaterSystem(RemoveWasteWaterSystemTown model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Gradients

        [HttpPost, RequiresAdmin]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult AddGradient(AddTownGradient model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresAdmin]
        [RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult RemoveGradient(RemoveTownGradient model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region GetState

        [HttpGet]
        public ActionResult GetState(int id)
        {
            var town = Repository.Find(id);
            if (town == null)
            {
                return HttpNotFound();
            }

            return Json(new { stateId = town.State.Id, state = town.State.Abbreviation }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region GetTownCriticalMainBreakNotes

        [HttpGet]
        public ActionResult GetTownCriticalMainBreakNotes(int id)
        {
            return Content(Repository.Find(id).CriticalMainBreakNotes);
        }

        #endregion

        public TownController(ControllerBaseWithPersistenceArguments<ITownRepository, Town, User> args) : base(args) { }
    }
}