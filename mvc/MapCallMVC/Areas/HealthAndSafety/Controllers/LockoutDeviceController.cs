using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class LockoutDeviceController : ControllerBaseWithPersistence<IRepository<LockoutDevice>, LockoutDevice, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;
        //ticket mc-2265 specifically wants this role for Person dropdown:
        public const RoleModules ROLE_LOCKOUT_FORM = RoleModules.OperationsLockoutForms; 

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchLockoutDevice search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchLockoutDevice search)
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
            return ActionHelper.DoNew(new CreateLockoutDevice(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateLockoutDevice model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditLockoutDevice>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditLockoutDevice model)
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

        #region CustomActions

        [HttpGet] //role is not needed here; method is invoked on LockoutForm
        public ActionResult ByOperatingCenterForCurrentUserEmployee(int opCenterId)
        {
            return new CascadingActionResult<LockoutDevice, LockoutDeviceDisplayItem>(Repository.Where
                (device => device.OperatingCenter.Id == opCenterId && device.Person.Id == AuthenticationService.CurrentUser.Id));
        }

        #endregion

        public LockoutDeviceController(ControllerBaseWithPersistenceArguments<IRepository<LockoutDevice>, LockoutDevice, User> args) : base(args) {}
    }
}
