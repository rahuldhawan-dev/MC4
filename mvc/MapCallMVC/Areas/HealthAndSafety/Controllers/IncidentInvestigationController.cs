using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MapCallMVC.Controllers;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class IncidentInvestigationController : ControllerBaseWithPersistence<IncidentInvestigation, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = IncidentController.ROLE_MODULE; // Should always be the same as IncidentController.

        #endregion

        #region Constructor

        public IncidentInvestigationController(ControllerBaseWithPersistenceArguments<IRepository<IncidentInvestigation>, IncidentInvestigation, User> args) : base(args) { }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)] // Only users that can edit an incident can add/create an investigation
        public ActionResult New(CreateIncidentInvestigation model)
        {
            // NOTE: This action is coming from a tab on the Incident/Show view. 
            ModelState.Clear();
            return ActionHelper.DoNew(model, new ActionHelperDoNewArgs {
                IsPartial = true,
                ViewName = "New" // Not using "_New" because if Create fails it needs to use the full view.
            });
        }

        [HttpPost]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)] // Only users that can edit an incident can add/create an investigation
        public ActionResult Create(CreateIncidentInvestigation model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs
            {
                OnSuccess = () => RedirectToReferrerOr("Show", "Incident", new { area = string.Empty, Id = model.Incident.Value }, IncidentController.INVESTIGATIONS_TAB_FRAGMENT)
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditIncidentInvestigation>(id);
        }

        [HttpPost]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Update(EditIncidentInvestigation model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs
            {
                // This should redirect with a tab fragment. We can't do that at the moment because only RedirectToReferrer
                // supports that and using that here would redirect us back to the Edit view. No time to implement/test a RedirectToAction
                // with fragment correctly. -Ross 4/23/2020
                OnSuccess = () => RedirectToAction("Show", "Incident", new { area = string.Empty, Id = model.Incident.Value })
            });
        }

        #endregion
    }
}