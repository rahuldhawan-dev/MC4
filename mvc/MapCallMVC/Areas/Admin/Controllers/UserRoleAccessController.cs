using System.IO;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCallMVC.Areas.Admin.Models.ViewModels.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Admin.Controllers
{
    public class UserRoleAccessController : ControllerBaseWithPersistence<IUserRepository, User, User>
    {
        #region Consts

        public const string OPERATING_CENTER_STATES_VIEWDATA_KEY = "OperatingCentersStates";

        #endregion

        #region Constructor

        public UserRoleAccessController(ControllerBaseWithPersistenceArguments<IUserRepository, User, User> args) : base(args) {}

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            if (action == ControllerAction.New)
            {
                // Users can only add roles for the modules/operating centers that they have the UserAdministrator action for.
                // Multiselect for the module values makes sense for site admins, but not so much user admins. Here's why
                // If a user has the following roles:
                //     1. Module 1 - OPC 1 - User Admin
                //     2. Module 2 - OPC 2 - User Admin
                // Then the multiselects would show both modules and both operating centers as selectable. Server validation
                // of the model would make this a non-issue, but the UX is still a problem. The checkboxes would have to start
                // being disabled, and that may further confuse users as to why.
                //
                // The only thing that makes sense to do here is to make modules a single select dropdown instead. However, Doug
                // really wants the multi-select. He says that the only people with user admin access are going to have access
                // at a state level, so they should have user admin access for all the operating centers in a single state for
                // every module they have access to. In this case, if a user outside of this use case does have access, they're going
                // to get a huge list of validation errors and that's fine. I fully expect them to complain about this implementation
                // and then switch it back to requiring them to do one module at a time. 
                
                // Let the auto dropdown population thing do its work for site admins.
                if (!AuthenticationService.CurrentUser.IsAdmin)
                {
                    var roleAccess = AuthenticationService.CurrentUser.GetUserAdministrativeRoleAccess();
                    this.AddDropDownData("Modules", roleAccess.OperatingCentersByModule.Keys.OrderBy(x => x.Description), x => x.Id, x => x.Description);

                    // We only need to manually add the operating centers/states if the user is not a site admin
                    // and they don't have a wildcard role somewhere. Otherwise the auto dropdown population thing
                    // in ActionHelper will do the work for us.
                    var roleAccessOpc = roleAccess.OperatingCentersByModule.Values.SelectMany(x => x).Distinct().ToList();
                    if (!roleAccessOpc.Contains(null))
                    {
                        var states = roleAccessOpc.Select(x => x.State).Distinct().OrderBy(x => x.Abbreviation);
                        this.AddDropDownData("OperatingCenters", roleAccessOpc.OrderBy(x => x.OperatingCenterCode), x => x.Id, x => x.ToString());
                        this.AddDropDownData("States", states, x => x.Id, x => x.ToString());
                    }
                }
                
                var opcRepo = _container.GetInstance<IRepository<OperatingCenter>>();
                // This is json serialized on the frontend for some non-cascading interactions. This saves us
                // from having to write a bunch of extra ajax stuff.
                // Doug and Lori both requested that we don't allow for auto-selection of operating centers when they're contracted
                // operations because that "requires extra security" on their part.
                ViewData[OPERATING_CENTER_STATES_VIEWDATA_KEY] = opcRepo.Linq.Where(x => !x.IsContractedOperations).Select(x => new OperatingCenterState {
                    OperatingCenterId = x.Id,
                    StateId = x.State.Id
                }).ToList();
            }
            base.SetLookupData(action);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresUserAdmin]
        public ActionResult New(int id)
        {
            var currentUser = AuthenticationService.CurrentUser;
            var roleAccess = currentUser.GetUserAdministrativeRoleAccess();
            var viewModel = _viewModelFactory.BuildWithOverrides<CreateUserRoles>(new { Id = id });
            viewModel.UserCanAdministrateAllOperatingCenters = currentUser.IsAdmin || roleAccess.OperatingCentersByModule.Values.SelectMany(x => x).Contains(null);
            return ActionHelper.DoNew(viewModel, new MMSINC.Utilities.ActionHelperDoNewArgs {
                IsPartial = true
            });
        }

        // This action's needed solely for rendering the add roles dialog on
        // the role group page. 
        [HttpGet, RequiresAdmin]
        public ActionResult NewForRoleGroup()
        {
            var viewModel = _viewModelFactory.Build<CreateUserRoles>();
            viewModel.UserCanAdministrateAllOperatingCenters = true;
            return ActionHelper.DoNew(viewModel, new MMSINC.Utilities.ActionHelperDoNewArgs {
                IsPartial = true
            });
        }

        [HttpPost, RequiresUserAdmin]
        public ActionResult Create(CreateUserRoles model)
        {
            // Needs to be DoUpdate because we're not creating a new *User* object.
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "User", new { area = string.Empty, id = model.Id }),
                OnError = () => RedirectToAction("Show", "User", new { area = string.Empty, id = model.Id })
            });
        }

        #endregion

        #region Removing roles

        [HttpDelete, RequiresUserAdmin]
        public ActionResult Destroy(RemoveUserRoles model)
        {
            // Needs to be DoUpdate because we're not deleting a *User* object.
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "User", new { area = string.Empty, id = model.Id }),
                OnError = () => RedirectToAction("Show", "User", new { area = string.Empty, id = model.Id })
            });
        }

        #endregion
    }
}