using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using OperatingCenterSpoilRemovalCost = MapCall.Common.Model.Entities.OperatingCenterSpoilRemovalCost;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class OperatingCenterSpoilRemovalCostController : ControllerBaseWithPersistence<IRepository<OperatingCenterSpoilRemovalCost>, OperatingCenterSpoilRemovalCost, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<IStateRepository, State>(r => r.GetAll());
            }

            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search(SearchOperatingCenterSpoilRemovalCost search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchOperatingCenterSpoilRemovalCost search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateOperatingCenterSpoilRemovalCost(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateOperatingCenterSpoilRemovalCost model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditOperatingCenterSpoilRemovalCost>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditOperatingCenterSpoilRemovalCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public OperatingCenterSpoilRemovalCostController(ControllerBaseWithPersistenceArguments<IRepository<OperatingCenterSpoilRemovalCost>, OperatingCenterSpoilRemovalCost, User> args) : base(args) { }
    }
}