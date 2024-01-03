using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class FacilitySubAreaController : ControllerBaseWithPersistence<FacilitySubArea, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionFacilityAreaManagement;

        #endregion

        #region Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchFacilitySubArea search)
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
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateFacilitySubArea>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateFacilitySubArea model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditFacilitySubArea>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditFacilitySubArea model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByFacilityArea

        [HttpGet]
        public ActionResult ByFacilityArea(int facilityAreaId)
        {
            return new CascadingActionResult(Repository.Where(x => x.Area.Id == facilityAreaId), "Description", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        public FacilitySubAreaController(ControllerBaseWithPersistenceArguments<IRepository<FacilitySubArea>, FacilitySubArea, User> args) : base(args) { }
    }
}