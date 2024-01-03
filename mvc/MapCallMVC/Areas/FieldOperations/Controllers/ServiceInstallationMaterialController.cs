using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class ServiceInstallationMaterialController : ControllerBaseWithPersistence<ServiceInstallationMaterial, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesDataLookups;

        #endregion

        #region Controllers

        public ServiceInstallationMaterialController(ControllerBaseWithPersistenceArguments<IRepository<ServiceInstallationMaterial>, ServiceInstallationMaterial, User> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchServiceInstallationMaterial search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchServiceInstallationMaterial search)
        {
            return ActionHelper.DoIndex(search);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new ServiceInstallationMaterialViewModel(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(ServiceInstallationMaterialViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<ServiceInstallationMaterialViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(ServiceInstallationMaterialViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}