using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    // NOTE: The old mapcall page for this existed in the Admin section. However they asked that we don't
    // make the CRUD actions require site admin so they can hand off the page to non-site-admin users. So
    // all CRUD actions require the UserAdministrator role.
    public class BusinessUnitController : ControllerBaseWithPersistence<BusinessUnit, User>
    {
        #region Consts

        public const RoleModules ROLE = RoleModules.FieldServicesProjects;

        #endregion

        #region Constructor

        public BusinessUnitController(ControllerBaseWithPersistenceArguments<IRepository<BusinessUnit>, BusinessUnit, User> args) : base(args) {}

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateBusinessUnit>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateBusinessUnit model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search(SearchBusinessUnit search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchBusinessUnit search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditBusinessUnit>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditBusinessUnit model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Finding things by operating center

        [HttpGet]
        public ActionResult FindByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.Where(bu => bu.OperatingCenter.Id == operatingCenterId), "BU", "Id");
        }

        [HttpGet]
        public ActionResult FindByOperatingCenterIdForWasteWaterOrCFS(int operatingCenterID)
        {
            return new CascadingActionResult(Repository.Where(bu => bu.OperatingCenter.Id == operatingCenterID && new[] {
                Department.Indices.WASTE_WATER,
                Department.Indices.CFS
            }.Contains(bu.Department.Id)), "BU", "Id");
        }

        [HttpGet]
        public ActionResult FindByOperatingCenterIdForTDWorkOrders(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.FindByOperatingCenterForTDWorkOrders(operatingCenterId), "BU", "Id");
        }

        #endregion
    }
}