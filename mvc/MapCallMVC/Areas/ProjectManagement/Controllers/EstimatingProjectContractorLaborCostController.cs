using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class EstimatingProjectContractorLaborCostController : ControllerBaseWithPersistence<IRepository<EstimatingProjectContractorLaborCost>, EstimatingProjectContractorLaborCost, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Constructors

        public EstimatingProjectContractorLaborCostController(
            ControllerBaseWithPersistenceArguments
                <IRepository<EstimatingProjectContractorLaborCost>, EstimatingProjectContractorLaborCost, User> args)
            : base(args) { }

        #endregion

        #region Private Methods

        private void SetShowLookupData(int id)
        {
            var entity = Repository.Find(id);
            if (entity != null)
            {
                var project = _container.GetInstance<IEstimatingProjectRepository>().Find(entity.EstimatingProject.Id);
                if (project != null)
                {
                    this.AddDynamicDropDownData<ContractorLaborCost, ContractorLaborCostDisplayItem>(filter: c => c.OperatingCenters.Contains(project.OperatingCenter));
                }
            }
            this.AddDropDownData<AssetType>();
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEstimatingProjectContractorLaborCost>(id, null, onModelFound:
                (entity) => {
                    // TODO: This lookup is redundant.
                    SetShowLookupData(entity.Id);
                });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEstimatingProjectContractorLaborCost model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "EstimatingProject", new { area = "ProjectManagement", id = model.EstimatingProject }),
                OnError = () => {
                    SetShowLookupData(model.Id);
                    return null;
                }
            });
        }

        #endregion
    }
}