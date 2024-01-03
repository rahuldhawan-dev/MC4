using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class PositionGroupCommonNameController : ControllerBaseWithPersistence<IPositionGroupCommonNameRepository, PositionGroupCommonName, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.OperationsTrainingModules;

        #endregion

        public PositionGroupCommonNameController(ControllerBaseWithPersistenceArguments<IPositionGroupCommonNameRepository, PositionGroupCommonName, User> args) : base(args) {}

        #region Show/Index

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index()
        {
            return ActionHelper.DoIndex();
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreatePositionGroupCommonName(_container));
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult Create(CreatePositionGroupCommonName model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditPositionGroupCommonName>(id);
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Update(EditPositionGroupCommonName model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
    }
}
