using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Reports.Controllers
{
    // NOTE: The data that needs to be searched on and the data that's displayed does not fit
    //       into the pattern of one resource = one controller = one repository. Nothing about this
    //       can actually work with ActionHelper or the search mapper without a lot of hacks and workarounds.
    [RequiresAdmin] 
    public class UserRoleController : ControllerBaseWithPersistence<IRepository<AggregateRole>, AggregateRole, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData();
                this.AddDynamicDropDownData<Module, ModuleDisplayItem>();
                this.AddDropDownData<RoleAction>(x => x.Id, x => x.Name);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Index(SearchUserRole model)
        {
            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();
                return RedirectToAction("Search");
            }

            var users = _container.GetInstance<IUserRepository>().GetUsersWithRole(model.Module.GetValueOrDefault(), model.OperatingCenter, model.RoleAction, model.UserHasAccess).ToList();
            return View(users);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchUserRole>();
        }

        #endregion

        public UserRoleController(ControllerBaseWithPersistenceArguments<IRepository<AggregateRole>, AggregateRole, User> args) : base(args) {}
    }
}
