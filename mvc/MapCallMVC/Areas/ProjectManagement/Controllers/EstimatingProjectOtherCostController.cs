using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class EstimatingProjectOtherCostController : ControllerBaseWithPersistence<IRepository<EstimatingProjectOtherCost>, EstimatingProjectOtherCost, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Constructors

        public EstimatingProjectOtherCostController(
            ControllerBaseWithPersistenceArguments
                <IRepository<EstimatingProjectOtherCost>, EstimatingProjectOtherCost, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData<AssetType>();
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEstimatingProjectOtherCost>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEstimatingProjectOtherCost model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "EstimatingProject", new { area = "ProjectManagement", id = model.EstimatingProject })
            });
        }

        #endregion
    }
}