using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Operations.Controllers
{
    public class AtRiskBehaviorSectionController : ControllerBaseWithPersistence<AtRiskBehaviorSection, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.OperationsIncidents;

        #endregion

        #region Consructor

        public AtRiskBehaviorSectionController(ControllerBaseWithPersistenceArguments<IRepository<AtRiskBehaviorSection>, AtRiskBehaviorSection, User> args) : base(args) { }

        #endregion

        #region Public Methods

        // NOTE: No search, not enough values to warrant it.

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index()
        {
            return ActionHelper.DoIndex();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new AtRiskBehaviorSectionViewModel(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(AtRiskBehaviorSectionViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<AtRiskBehaviorSectionViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(AtRiskBehaviorSectionViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddAtRiskBehaviorSubSection(AddAtRiskBehaviorSubSectionToSection model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveAtRiskBehaviorSubsection(RemoveAtRiskBehaviorSubSectionFromSection model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
    }
}