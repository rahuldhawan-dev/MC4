using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Operations.Controllers
{
    // TODO: This functionality can probably all be part of UserController when Users.aspx is finally converted to MVC.

    [DisplayName("User Tracking")]
    public class UserTrackingController : ControllerBaseWithPersistence<IUserRepository, User, User>
    {
        #region Constructor

        public UserTrackingController(ControllerBaseWithPersistenceArguments<IUserRepository, User, User> args) : base(args) {}

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(MapCall.Common.Model.Entities.RoleModules.ManagementGeneral)]
        public ActionResult Search()
        {
            this.AddOperatingCenterDropDownData("DefaultOperatingCenter");
            return ActionHelper.DoSearch<SearchUserTracking>();
        }

        [HttpGet, RequiresRole(MapCall.Common.Model.Entities.RoleModules.ManagementGeneral)]
        public ActionResult Index(SearchUserTracking search)
        {
            return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
            {
                SearchOverrideCallback = () =>
                {
                    Repository.SearchUserTracking(search);
                }
            });
        }

        #endregion
    }
}