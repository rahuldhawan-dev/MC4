using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Management.Models;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Management.Controllers
{
    public class UserViewedController : ControllerBaseWithPersistence<IUserViewedRepository, UserViewed, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ManagementGeneral;

        #endregion

        #region Constructor

        public UserViewedController(ControllerBaseWithPersistenceArguments<IUserViewedRepository, UserViewed, User> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchUserViewed search)
        {
            // The view does a lot of grouping and weirdness.
            search.EnablePaging = false;
            return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
            {
                SearchOverrideCallback = () => Repository.SearchWithImages(search)
                // Do something for OnNoResults or invalid model?
            });
        }

        #endregion
    }
}