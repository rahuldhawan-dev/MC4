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
    public class BappTeamController : ControllerBaseWithPersistence<IBappTeamRepository, BappTeam, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.BAPPTeamSharingGeneral;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDynamicDropDownData<IRepository<OperatingCenter>, OperatingCenter, OperatingCenterDisplayItem>("OperatingCenters");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchBappTeam search)
        {
            return ActionHelper.DoIndex(search);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateBappTeam(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateBappTeam model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditBappTeam>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditBappTeam model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByOperatingCenterId

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterId(id), "Description", "Id");
        }

        #endregion

        public BappTeamController(ControllerBaseWithPersistenceArguments<IBappTeamRepository, BappTeam, User> args) : base(args) {}
    }
}