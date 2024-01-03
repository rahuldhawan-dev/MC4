using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Engineering.Models.ViewModels.ArcFlash;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Engineering.Controllers
{
    public class ArcFlashStudyController : ControllerBaseWithPersistence<IRepository<ArcFlashStudy>, ArcFlashStudy, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EngineeringArcFlash;
        public const string ADD_ERROR_MESSAGE = "To add a new arc flash study, please select the facility and create the study from the Arc Flash Study tab";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.New)
            {
                this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchArcFlashStudy search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchArcFlashStudy search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(CreateArcFlashStudy model)
        {
            ModelState.Clear();
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateArcFlashStudy model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditArcFlashStudy>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditArcFlashStudy model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Constructors

        public ArcFlashStudyController(ControllerBaseWithPersistenceArguments<IRepository<ArcFlashStudy>, ArcFlashStudy, User> args) : base(args) { }

        #endregion
    }
}
