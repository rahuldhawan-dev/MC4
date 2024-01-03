using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class FacilityProcessStepTriggerController : ControllerBaseWithPersistence<FacilityProcessStepTrigger, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.ProductionFacilities;

        #endregion

        #region Constructor

        public FacilityProcessStepTriggerController(ControllerBaseWithPersistenceArguments<IRepository<FacilityProcessStepTrigger>, FacilityProcessStepTrigger, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            this.AddDropDownData<FacilityProcessStepTriggerType>("TriggerType");
            this.AddDropDownData<FacilityProcessStepTriggerLevel>("TriggerLevel");
            this.AddDropDownData<FacilityProcessStepAlarm>("Alarm");
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [RequiresRole(ROLE, RoleActions.Add)]
        [HttpGet, ActionBarVisible(false)]
        public ActionResult New(int? facilityProcessStepId)
        {
            return ActionHelper.DoNew(new FacilityProcessStepTriggerViewModel(_container) {
                FacilityProcessStep = facilityProcessStepId
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(FacilityProcessStepTriggerViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<FacilityProcessStepTriggerViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(FacilityProcessStepTriggerViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            var parentStep = Repository.Find(id)?.FacilityProcessStep.Id;
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                OnSuccess = () => RedirectToAction("Show", "FacilityProcessStep", new { id = parentStep })
            });
        }

        #endregion
    }
}