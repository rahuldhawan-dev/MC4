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
    public class EstimatingProjectMaterialController : ControllerBaseWithPersistence<IRepository<EstimatingProjectMaterial>, EstimatingProjectMaterial, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Constructors

        public EstimatingProjectMaterialController(
            ControllerBaseWithPersistenceArguments
                <IRepository<EstimatingProjectMaterial>, EstimatingProjectMaterial, User> args) : base(args) {}

        #endregion

        #region Private Methods

        private void SetShowLookupData(int id)
        {
            var estimatingProjectMaterial = Repository.Find(id);
            if (estimatingProjectMaterial != null)
            {
                var project =
                    _container.GetInstance<IEstimatingProjectRepository>()
                        .Find(estimatingProjectMaterial.EstimatingProject.Id);
                if (project != null)
                {
                    this.AddDynamicDropDownData<IMaterialRepository, Material, MaterialDisplayItem>(dataGetter:
                        x => x.FindActiveMaterialByOperatingCenter(project.OperatingCenter));
                }
            }
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEstimatingProjectMaterial>(id, null, (entity) => {
                // TODO: Remove redundant entity lookup
                SetShowLookupData(entity.Id);
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEstimatingProjectMaterial model)
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