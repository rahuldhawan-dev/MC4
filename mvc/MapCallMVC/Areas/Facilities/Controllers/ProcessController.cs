using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class ProcessController : ControllerBaseWithPersistence<IProcessRepository, Process, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.ProductionFacilities;

        #endregion

        #region Constructor

        public ProcessController(ControllerBaseWithPersistenceArguments<IProcessRepository, Process, User> args) : base(args) { }

        #endregion

        #region

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddDropDownData<ProcessStage>();
                    break;
            }
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchProcess model)
        {
            // Not enough items to warrant paging.
            model.EnablePaging = false;
            return ActionHelper.DoIndex(model);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new ProcessViewModel(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(ProcessViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<ProcessViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(ProcessViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult ByProcessStage(int id)
        {
            var records = Repository.GetByProcessStage(id);
            return new CascadingActionResult(records, "Description", "Id");
        }

        #endregion
    }
}