using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class PersonnelAreaController : ControllerBaseWithPersistence<PersonnelArea, User>
    {
        #region Constants

        private const RoleModules ROLE = RoleModules.HumanResourcesEmployee;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchPersonnelArea model)
        {
            return ActionHelper.DoIndex(model);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchPersonnelArea>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreatePersonnelArea(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreatePersonnelArea model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditPersonnelArea>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditPersonnelArea model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region ByOperatingCenter

        [HttpGet]
        public ActionResult ByOperatingCenter(int id)
        {
            return new CascadingActionResult(Repository.Where(x => x.OperatingCenter.Id == id), "Description", "Id");
        }

        #endregion

        public PersonnelAreaController(ControllerBaseWithPersistenceArguments<IRepository<PersonnelArea>, PersonnelArea, User> args) : base(args) {}
    }
}
