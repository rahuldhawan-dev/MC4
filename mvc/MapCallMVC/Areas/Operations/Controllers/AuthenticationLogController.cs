using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Operations.Controllers
{
    public class AuthenticationLogController : ControllerBaseWithPersistence<IMapCallAuthenticationLogRepository, AuthenticationLog, User>
    {
        #region Constructor

        public AuthenticationLogController(ControllerBaseWithPersistenceArguments<IMapCallAuthenticationLogRepository, AuthenticationLog, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(RoleModules.ManagementGeneral)]
        public ActionResult Index(SearchAuthenticationLog search)
        {
            return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => {
                    Repository.SearchAuthenticationLogs(search);
                }
            });
        }

        [HttpGet, RequiresRole(RoleModules.ManagementGeneral)]
        public ActionResult Search()
        {
            this.AddOperatingCenterDropDownData();
            return ActionHelper.DoSearch<SearchAuthenticationLog>();
        }

        #endregion
    }
}