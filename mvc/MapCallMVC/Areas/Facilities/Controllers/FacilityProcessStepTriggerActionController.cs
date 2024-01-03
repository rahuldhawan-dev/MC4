using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class FacilityProcessStepTriggerActionController : ControllerBaseWithPersistence<FacilityProcessStepTriggerAction, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.ProductionFacilities;

        #endregion

        #region Constructor

        public FacilityProcessStepTriggerActionController(ControllerBaseWithPersistenceArguments<IRepository<FacilityProcessStepTriggerAction>, FacilityProcessStepTriggerAction, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add), ActionBarVisible(false)]
        public ActionResult New(int? triggerId)
        {
            return ActionHelper.DoNew(new FacilityProcessStepTriggerActionViewModel(_container)
            {
                Trigger = triggerId
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(FacilityProcessStepTriggerActionViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<FacilityProcessStepTriggerActionViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(FacilityProcessStepTriggerActionViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            var parentStep = Repository.Find(id)?.Trigger.Id;
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                OnSuccess = () => RedirectToAction("Show", "FacilityProcessStepTrigger", new { id = parentStep })
            });
        }

        #endregion
    }
}