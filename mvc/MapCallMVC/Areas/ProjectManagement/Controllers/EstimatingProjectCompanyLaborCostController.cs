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
    public class EstimatingProjectCompanyLaborCostController : ControllerBaseWithPersistence<IRepository<EstimatingProjectCompanyLaborCost>, EstimatingProjectCompanyLaborCost, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Constructors

        public EstimatingProjectCompanyLaborCostController(
            ControllerBaseWithPersistenceArguments
                <IRepository<EstimatingProjectCompanyLaborCost>, EstimatingProjectCompanyLaborCost, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData<AssetType>();
            this.AddDropDownData<CompanyLaborCost>();
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEstimatingProjectCompanyLaborCost>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEstimatingProjectCompanyLaborCost model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "EstimatingProject", new { area = "ProjectManagement", id = model.EstimatingProject })
            });
        }

        #endregion
    }
}