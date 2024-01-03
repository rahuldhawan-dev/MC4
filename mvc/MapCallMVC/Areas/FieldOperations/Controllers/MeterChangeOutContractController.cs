using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class MeterChangeOutContractController : ControllerBaseWithPersistence<MeterChangeOutContract, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesMeterChangeOuts;

        #endregion

        #region Constructor

        public MeterChangeOutContractController(ControllerBaseWithPersistenceArguments<IRepository<MeterChangeOutContract>, MeterChangeOutContract, User> args) : base(args) { }

        #endregion

        #region Actions

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchMeterChangeOutContract model)
        {
            return ActionHelper.DoIndex(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchMeterChangeOutContract());
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateMeterChangeOutContract(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateMeterChangeOutContract model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMeterChangeOutContract>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditMeterChangeOutContract model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult AddMeterChangeOuts(AddMeterChangeOutsToContract model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
    }
}