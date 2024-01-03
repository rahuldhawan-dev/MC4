using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels.FireDistricts;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class FireDistrictController : ControllerBaseWithPersistence<IFireDistrictRepository, FireDistrict, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesDataLookups;

        #endregion

        #region Constructor

        public FireDistrictController(ControllerBaseWithPersistenceArguments<IFireDistrictRepository, FireDistrict, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet]
        public ActionResult ByTownId(int townId)
        {
            return new CascadingActionResult(Repository.GetByTownId(townId).Select(x => new{ Id = x.Id, Description = x.ToString()}), "Description", "Id");
        }

        [HttpGet]
        public ActionResult GetPremiseNumber(int fireDistrictId)
        {
            var fd = Repository.Find(fireDistrictId) ?? new FireDistrict();
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            res.Data = new {
                premiseNumber = fd.PremiseNumber
            };

            return res;
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchFireDistrict search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchFireDistrict search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<FireDistrictViewModel>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(FireDistrictViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<FireDistrictViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(FireDistrictViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete), ActionBarVisible(false)]
        public ActionResult Destroy(DeleteFireDistrict district)
        {
            return ActionHelper.DoDestroy(district.Id);
        }

        #endregion
    }
}