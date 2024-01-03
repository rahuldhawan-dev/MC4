using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallMVC.Controllers
{
    public class DivisionController : ControllerBaseWithPersistence<IDivisionRepository, Division, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.HumanResourcesUnion;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Search(SearchDivision search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchDivision search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateDivision(_container));
        }

        [HttpPost]
        [RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult Create(CreateDivision model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditDivision>(id);
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Update(EditDivision model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE_MODULE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region ByStateId

        [HttpGet]
        public ActionResult ByStateId(int stateId)
        {
            return new CascadingActionResult(Repository.GetByStateId(stateId), "Description", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        public DivisionController(ControllerBaseWithPersistenceArguments<IDivisionRepository, Division, User> args) : base(args) {}
    }
}
