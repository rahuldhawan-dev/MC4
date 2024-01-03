using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class LocalController : ControllerBaseWithPersistence<ILocalRepository, Local, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesUnion;
        public const string LOCAL_NOT_FOUND = "Local not found.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Edit:
                    this.AddDropDownData<Union>(s => s.Id, s => s.BargainingUnit);
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;
                case ControllerAction.New:
                    this.AddDropDownData<Union>(s => s.Id, s => s.BargainingUnit);
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add);
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;
                case ControllerAction.Search:
                    this.AddDropDownData<Union>("Union", s => s.Id, s => s.BargainingUnit);
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;
            }
        }

        [RequiresRole(RoleModules.HumanResourcesUnion)]
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterId(id), "Name", "Id");
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchLocal local)
        {
            return ActionHelper.DoSearch(local);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchLocal local)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(local, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = true
                }));
                f.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, local));
                f.Excel(() => ActionHelper.DoExcel(local));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
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

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateLocal(_container));
        }

        [HttpPost, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Add)]
        public ActionResult Create(CreateLocal model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditLocal>(id);
        }

        [HttpPost, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Edit)]
        public ActionResult Update(EditLocal local)
        {
            return ActionHelper.DoUpdate(local);
        }

        #endregion

        #region ByUnionId

        [HttpGet]
        public ActionResult ByUnionId(int id)
        {
            return new CascadingActionResult(Repository.GetByUnionId(id), "Name", "Id");
        }

        #endregion

        public LocalController(ControllerBaseWithPersistenceArguments<ILocalRepository, Local, User> args) : base(args) {}
    }
}
