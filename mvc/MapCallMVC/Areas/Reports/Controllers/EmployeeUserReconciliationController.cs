using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class EmployeeUserReconciliationController : ControllerBaseWithPersistence<IEmployeeRepository, Employee, User>
    {
        public const RoleModules ROLE = RoleModules.HumanResourcesEmployee;

        public EmployeeUserReconciliationController(ControllerBaseWithPersistenceArguments<IEmployeeRepository, Employee, User> args) : base(args) {}

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchEmployeeUserReconciliation search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEmployeeUserReconciliation search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetMismatchedUsers(search)
                }));

                f.Excel(() => ActionHelper.DoExcel(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetMismatchedUsers(search)
                }));
            });
        }
    }
}