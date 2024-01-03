using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class CompletedWorkOrdersWithMaterialController : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        // NOTE: This needs to be a role that 271 users will have access to.
        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchCompletedWorkOrdersWithMaterial search)
        {
            // Paging isn't supported because of the grouping in the repository.
            search.EnablePaging = false;
            var args = new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetCompletedWorkOrderMaterialCounts(search)
            };
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, args));
                formatter.Excel(() => ActionHelper.DoExcel(search, args));
            });
        }

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.UserAdministrator)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchCompletedWorkOrdersWithMaterial());
        }

        #endregion

        public CompletedWorkOrdersWithMaterialController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }
    }
}