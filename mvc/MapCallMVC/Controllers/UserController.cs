using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AuthorizeNet;
using AuthorizeNet.Utility.NotProvided;
using JetBrains.Annotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCallMVC.Areas.Admin.Models.ViewModels.Users;
using MapCallMVC.Areas.Engineering.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Models.ViewModels.Users;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Common;
using MMSINC.Controllers;
using MMSINC.Results;

namespace MapCallMVC.Controllers
{
    public class UserController : ControllerBaseWithPersistence<IUserRepository, User, User>
    {
        #region Constants

        public const string COMMUNICATOR_URL = "/Content/AuthorizeNet/communicator.html";
        public const string ALREADY_EXISTS = @"Error processing request: E00039 - A duplicate record with ID {0} already exists.";
        public const string ALREADY_EXISTS_REGEX = @"Error processing request: E00039 - A duplicate record with ID (\d+) already exists.";

        #endregion

        #region Fields

        private IMembershipHelper _membershipHelper;

        #endregion

        #region Constructor

        public UserController(ControllerBaseWithPersistenceArguments<IUserRepository, User, User> args,
            IMembershipHelper membershipHelper) : base(args)
        {
            _membershipHelper = membershipHelper;
        }
        
        #endregion

        #region Private Members

        private IExtendedCustomerGateway _customerGateway;
        private string _communicatorUrl;

        #endregion
        
        #region Properties

        protected IExtendedCustomerGateway CustomerGateway
        {
            get
            {
                return _customerGateway ??
                    (_customerGateway =
                        _container.GetInstance<IExtendedCustomerGateway>());
            }
        }

        public string CommunicatorUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_communicatorUrl))
                    _communicatorUrl = Request.GetRootUrl() + COMMUNICATOR_URL;    
                return _communicatorUrl;
            }
        }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            if (action == ControllerAction.Show)
            {
                this.AddDropDownData<RoleGroup>(x => x.Id, x => x.Name);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresUserAdmin]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchUser>();
        }

        [HttpGet, RequiresUserAdmin]
        public ActionResult Index(SearchUser search)
        {
            return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.SearchUsers(search)
            });
        }

        /// <summary>
        /// Users may see their own user page.
        /// Administrators will see the users page and access the old
        /// page through a link there.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet] 
        public ActionResult Show(int id)
        {
            if (AuthenticationService.CurrentUserId != id
                && !(AuthenticationService.CurrentUserIsAdmin || AuthenticationService.CurrentUser.IsUserAdmin))
            {
                return this.Forbidden();
            }

            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresUserAdmin]
        public ActionResult New()
        {
            return ActionHelper.DoNew<CreateUser>();
        }

        [HttpPost, RequiresUserAdmin]
        public ActionResult Create(CreateUser model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
#if DEBUG
                    // The Membership stuff doesn't work with our test db setup. The default MembershipProvider
                    // uses sql connection info from web.config. We can't easily fix this for functional tests
                    // without creating an entire custom provider. It's not worth the effort.
                    if (MMSINC.MvcApplication.IsInTestMode &&
                        MMSINC.MvcApplication.RegressionTestFlags.Contains("do not create membership user"))
                    {
                        return null;
                    }
#endif 
                    var entity = Repository.Find(model.Id);

                    // This password is used by the API/things that use Basic HTTP auth.
                    // The IsApproved param also needs to match HasAccess. If we revoke
                    // their access through the site, we need to revoke their Membership
                    // approval as well so that they can't continue to use the Basic HTTP auth.
                    var membershipCreateStatus = _membershipHelper.CreateUser(entity.UserName,
                        "dummy password",
                        entity.Email,
                        "What was the email address used to setup your account?",
                        entity.Email, 
                        entity.HasAccess);

                    if (membershipCreateStatus != MembershipCreateStatus.Success)
                    {
                        // We might want to have this delete the MapCall User record if the
                        // error comes up. It's incredibly unlikely that this error will ever
                        // be thrown, though.
                        throw new InvalidOperationException("Unable to create MembershipUser. Reason: " + membershipCreateStatus);
                    }

                    // Because we don't actually want to *use* the dummy password, we want to 
                    // immediately change it as well. If someone needs the password, though, they'll
                    // have to use the Reset API Password button.
                    _membershipHelper.ResetPassword(entity.UserName);

                    return null; // defer to the default result from ActionHelper.
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresUserAdmin]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditUser>(id);
        }

        [HttpPost, RequiresUserAdmin]
        public ActionResult Update(EditUser model)
        {
            // TODO: On success needs to update the MembershipUser's email and IsApproved values.
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    _membershipHelper.UpdateUserEmail(entity.UserName, entity.Email);
                    _membershipHelper.UpdateApproval(entity.UserName, entity.HasAccess);
                    return null; // defer to the default result from ActionHelper.
                }
            });
        }

        #endregion

        #region UnlockUser

        /// <summary>
        /// A user's Membership record will become locked out if they
        /// try to use Membership.ValidateUser too many times and fail.
        /// Same thing if they try to reset their password but the answer
        /// to the security question is invalid. This is not likely to
        /// happen, as the only thing using Membership is the API. If
        /// we ever switch that over to using okta, we can ditch this.
        /// </summary>
        [HttpPost, RequiresAdmin]
        public ActionResult UnlockUser(int id)
        {
            var user = Repository.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // UnlockUser returns false if it couldn't unlock the user.
            // It doesn't give us any useful information about why it couldn't.
            if (!_membershipHelper.UnlockUser(user.UserName))
            {
                DisplayErrorMessage($"Unable to unlock user {user.UserName}.");
            }
            else
            {
                DisplaySuccessMessage($"The Membership account for {user.UserName} has been unlocked.");
            }

            return RedirectToAction("Show", new { id = id });
        }

        #endregion

        #region ResetPassword

        /// <summary>
        /// Resets the user's Membership password. This password is only used for API access.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, RequiresAdmin]
        public ActionResult ResetApiPassword(int id)
        {
            var user = Repository.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var newPassword = _membershipHelper.ResetPassword(user.UserName);
            var encodedPassword = HttpUtility.HtmlEncode(newPassword);
            DisplaySuccessMessage($"The api password for {user.UserName} has been changed to: {encodedPassword}");

            return RedirectToAction("Show", new { id = id });
        }

        #endregion

        #region GetByOperatingCenterId

        [HttpGet] // TODO: This should be renamed to reflect that it's only active users being returned.
        public ActionResult GetByOperatingCenterId(int opCenterId)
        {
            var operatingCenter = _container.GetInstance<IOperatingCenterRepository>().Find(opCenterId);

            IEnumerable<User> results = Repository.Where(u => u.HasAccess && u.DefaultOperatingCenter.Id == opCenterId);

            if (operatingCenter.OperatedByOperatingCenter != null)
            {
                results =
                    results.MergeWith(
                        Repository.Where(
                            u =>
                                u.HasAccess &&
                                u.DefaultOperatingCenter.Id == operatingCenter.OperatedByOperatingCenter.Id));
            }
                
            return new CascadingActionResult(results, "FullName", "Id");
        }

        [HttpGet]
        public ActionResult GetAllByOperatingCenterId(int opCenterId)
        {
            return new CascadingActionResult(Repository.Where(u => u.DefaultOperatingCenter.Id == opCenterId), "FullName", "Id");
        }

        [HttpGet]
        public ActionResult GetAllByStateOrOperatingCenterId(int? stateId, int? opCenterId)
        {
            var results = Repository.GetAll();
            if (stateId != null)
            {
                results = results.Where(x => x.DefaultOperatingCenter.State.Id == stateId);
            }
            if (opCenterId != null)
            {
                results = results.Where(x => x.DefaultOperatingCenter.Id == opCenterId);
            }
            return new CascadingActionResult(results, "FullName", "Id");
        }

        [HttpGet]
        public ActionResult GetByOperatingCenterIdAndPartialNameMatchForTDWorkOrders(string partial, int operatingCenterId)
        {
            var results = Repository.FindByOperatingCenterIdAndPartialNameMatch(partial, operatingCenterId).Select(u => new {u.Id, u.FullName});
            return new AutoCompleteResult(results, "Id", "FullName");
        }

        [HttpGet]
        public ActionResult GetActiveUsersByStateId(int stateId)
        {
            // Greg - 4/9/2021 - if your working on this be wary of changing this to IEnumerable<User>, it has some bad performance implications
            var results = Repository.Where(u => u.HasAccess && u.DefaultOperatingCenter.State.Id == stateId);
            return new CascadingActionResult(results, "FullName", "Id");
        }

        [HttpGet]
        public ActionResult GetActiveUsersWithOpCenterIdAndRoleForLockoutDevice(int opCenterId)
        {
            var results = _container.GetInstance<IUserRepository>().GetUsersFilterByWithAndWithOutOperatingCenter((int)LockoutDeviceController.ROLE_LOCKOUT_FORM, opCenterId, (int)RoleActions.Read, true);
            return new CascadingActionResult(results.ToList(), "FullName", "Id") { SortItemsByTextField = false };
        }

        [HttpGet]
        public ActionResult GetActiveUsersWithOpCenterIdAndRoleAndIsAssignedToLockOutDevices(int opCenterId)
        {
            IEnumerable<User> results = Repository.GetUsersWithRole((int)LockoutDeviceController.ROLE_LOCKOUT_FORM, opCenterId, (int)RoleActions.Read, true).ToList();
            IEnumerable<LockoutDevice> lockoutDevices = _container.GetInstance<LockoutDeviceRepository>().Where(x => x.OperatingCenter.Id == opCenterId);
            results = results.Intersect(lockoutDevices.Select(x => x.Person));
            
            return new CascadingActionResult(results, "FullName", "Id");
        }

        [HttpGet]
        public ActionResult LockoutFormUsersByOperatingCenterId(int opCenterId)
        {
            var results = _container.GetInstance<IUserRepository>().GetUsersWithRole((int)LockoutFormController.ROLE, opCenterId, (int)RoleActions.Read, null).ToList();
            return new CascadingActionResult(results, "FullName", "Id");
        }

        #endregion

        #region GetFieldServicesAssetsUsersByOperatingCenter

        private CascadingActionResult GetFieldServicesAssetUsersByOperatingCenter(int operatingCenterId, bool? activeUsersOnly)
        {
            var results = _container.GetInstance<IUserRepository>().GetUsersWithRole((int)RoleModules.FieldServicesAssets, operatingCenterId, (int)RoleActions.Read, activeUsersOnly);
            return new CascadingActionResult(results, "FullName", "Id");
        }

        [HttpGet]
        public ActionResult ActiveFieldServicesAssetsUsersByOperatingCenter(int operatingCenterId)
        {
            return GetFieldServicesAssetUsersByOperatingCenter(operatingCenterId, true);
        }

        [HttpGet]
        public ActionResult FieldServicesAssetsUsersByOperatingCenter(int operatingCenterId)
        {
            return GetFieldServicesAssetUsersByOperatingCenter(operatingCenterId, null);
        }

        #endregion

        #region FieldServicesWorkManagementUsersByOperatingCenter

        [HttpGet]
        public ActionResult FieldServicesWorkManagementUsersByOperatingCenter(int operatingCenterId)
        {
            var result = Repository
                      .Where(e => e.DefaultOperatingCenter.Id == operatingCenterId ||
                                  e.DefaultOperatingCenter.OperatedByOperatingCenter.Id == operatingCenterId ||
                                  e.AggregateRoles.Any(r => r.Module.Id == (int)RoleModules.FieldServicesWorkManagement &&
                                                            (r.OperatingCenter == null || 
                                                             r.OperatingCenter.Id == operatingCenterId)))
                      .OrderBy(x => x.FullName).ToList();

            return new CascadingActionResult(result, "FullName", "Id");
        }

        #endregion

        #region GetToken

        [HttpPost]
        public ActionResult GetToken(int id)
        {
            var user = _container.GetInstance<IUserRepository>().Find(id);
            if (user == null)
                return HttpNotFound();

            var profileId = GetUserProfileId(user);
            return Content(
                    CustomerGateway.GetHostedSessionKey(
                        Convert.ToUInt32(profileId),
                        CommunicatorUrl,
                        HttpApplicationBase.IsProduction));
        }

        private int GetUserProfileId(User currentUser)
        {
            // TODO: THIS IS BAD NEWS BEARS TO BE HERE BUT IS A TEMPORARY FIX FOR REGRESSION TESTS.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (currentUser.CustomerProfileId.HasValue)
                return currentUser.CustomerProfileId.Value;

            // Error processing request: E00039 - A duplicate record with ID 37055111 already exists.
            Customer customer = null;

            try
            {
                customer = CustomerGateway
                    .CreateCustomer(currentUser.Email, currentUser.DefaultOperatingCenter.ToString());
            }
            catch (InvalidOperationException ex)
            {
                var match = new Regex(ALREADY_EXISTS_REGEX, RegexOptions.IgnoreCase).Match(ex.Message);
                {
                    if (match.Groups.Count > 1)
                    {
                        customer = CustomerGateway.GetCustomer(match.Groups[1].Value);
                    }
                }
            }

            currentUser.CustomerProfileId = Convert.ToInt32(customer.ProfileID);
            _container.GetInstance<IUserRepository>().Save(currentUser);

            return currentUser.CustomerProfileId.Value;
        }

        #region Awia Active Users with Risk Register Role

        [HttpGet]
        public ActionResult AwiaUsersByOperatingCenterIdAndRole(int operatingCenterId)
        {
            var results = Repository.GetUsersWithRole((int)AwiaComplianceController.ROLE, operatingCenterId, (int)RoleActions.Read, true).ToList();
            return new CascadingActionResult(results, "FullName", "Id");
        }

        #endregion

        #endregion

        #region Role Groups

        [HttpPost, RequiresUserAdmin]
        public ActionResult AddRoleGroup(AddUserRoleGroup model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "User", new { area = string.Empty, id = model.Id }),
                OnError = () => RedirectToAction("Show", "User", new { area = string.Empty, id = model.Id })
            });
        }

        [HttpDelete, RequiresUserAdmin]
        public ActionResult RemoveRoleGroup(RemoveUserRoleGroups model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "User", new { area = string.Empty, id = model.Id }),
                OnError = () => RedirectToAction("Show", "User", new { area = string.Empty, id = model.Id })
            });
        }

        #endregion
    }
}
