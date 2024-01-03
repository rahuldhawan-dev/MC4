using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class EnvironmentalPermitFeeController : ControllerBaseWithPersistence<IRepository<EnvironmentalPermitFee>, EnvironmentalPermitFee, User>
    {
        #region Constants

        public const RoleModules ROLE = EnvironmentalPermitController.ROLE;

        #endregion

        #region New/Create

        [ActionBarVisible(false)] // Users are coming from EnvironmentalPermit/Show in order to create.
        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)] // Intentionally edit. Only users that can edit a permit can add fees.
        public ActionResult New(int environmentalPermit)
        {
            // NOTE: The view handles whether or not it should be a partial view or a full view.
            // Users seeing this in the EnvironmentalPermit/Show tab will get the partial version.
            // Users receive the full view if Create fails validation.
            return ActionHelper.DoNew(new CreateEnvironmentalPermitFee(_container) {
                EnvironmentalPermit = environmentalPermit, 
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)] // Intentionally edit. Only users that can edit a permit can add fees.
        public ActionResult Create(CreateEnvironmentalPermitFee model)
        {
            // This will return to the regular New view if there's errors. No need 
            // to redirect to the permit/show tab since that won't work.
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToAction("Show", "EnvironmentalPermit", new { id = model.EnvironmentalPermit.Value })
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEnvironmentalPermitFee>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEnvironmentalPermitFee model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "EnvironmentalPermit", new { id = model.EnvironmentalPermit.Value })
            });
        }

        #endregion

        public EnvironmentalPermitFeeController(ControllerBaseWithPersistenceArguments<IRepository<EnvironmentalPermitFee>, EnvironmentalPermitFee, User> args) : base(args) {}
    }
}
