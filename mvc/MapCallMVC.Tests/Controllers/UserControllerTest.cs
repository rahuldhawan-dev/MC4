using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AuthorizeNet;
using AuthorizeNet.Utility.NotProvided;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest : MapCallMvcControllerTestBase<UserController, User>
    {
        #region Fields
        
        private Mock<IMembershipHelper> _membershipHelper;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return null;
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authenticationService = e.For<IAuthenticationService<User>>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            _membershipHelper = e.For<IMembershipHelper>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Request.CreateAndInitializeController<UserController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                ((CreateUser)vm).UserName = "some username";
            };
        }

        #endregion

        #region Private Methods

        private User CreateUserAndRole(RoleModules module, RoleActions action, OperatingCenter opc, bool hasAccess) 
        {
            var user = GetFactory<UserFactory>().Create(new { HasAccess = hasAccess });
            var mod = GetFactory<ModuleFactory>().Create(new { Id = (int)module });
            var act = GetFactory<ActionFactory>().Create(new { Id = (int)action });
            GetFactory<RoleFactory>().Create(new { User = user, Module = mod, Action = act, OperatingCenter = opc });
            return user;
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/User/ActiveFieldServicesAssetsUsersByOperatingCenter/");
                a.RequiresUserAdminUser("~/User/AddRoleGroup/");
                a.RequiresLoggedInUserOnly("~/User/AwiaUsersByOperatingCenterIdAndRole/");
                a.RequiresUserAdminUser("~/User/Create/");
                a.RequiresUserAdminUser("~/User/Edit/");
                a.RequiresLoggedInUserOnly("~/User/FieldServicesAssetsUsersByOperatingCenter/");
                a.RequiresLoggedInUserOnly("~/User/GetActiveUsersByStateId/");
                a.RequiresLoggedInUserOnly("~/User/GetActiveUsersWithOpCenterIdAndRoleForLockoutDevice/");
                a.RequiresLoggedInUserOnly("~/User/GetActiveUsersWithOpCenterIdAndRoleAndIsAssignedToLockOutDevices/");
                a.RequiresLoggedInUserOnly("~/User/GetAllByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/User/GetAllByStateOrOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/User/GetByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/User/GetByOperatingCenterIdAndPartialNameMatchForTDWorkOrders/");
                a.RequiresLoggedInUserOnly("~/User/GetToken/");
                a.RequiresUserAdminUser("~/User/Index/");
                a.RequiresLoggedInUserOnly("~/User/LockoutFormUsersByOperatingCenterId/");
                a.RequiresUserAdminUser("~/User/New/");
                a.RequiresUserAdminUser("~/User/RemoveRoleGroup/");
                a.RequiresSiteAdminUser("~/User/ResetApiPassword/");
                a.RequiresUserAdminUser("~/User/Search/");
                a.RequiresLoggedInUserOnly("~/User/Show/");
                a.RequiresSiteAdminUser("~/User/UnlockUser/");
                a.RequiresUserAdminUser("~/User/Update/");
                a.RequiresLoggedInUserOnly("~/User/FieldServicesWorkManagementUsersByOperatingCenter/");
            });
        }

        #region Cascades
        
        [TestMethod]
        public void TestGetByOperatingCenterIdReturnsUsersByOperatingCenterId()
        {
            var operatingCenter =
                GetFactory<OperatingCenterFactory>()
                    .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var otherOperatingCenter =
                GetFactory<OperatingCenterFactory>()
                    .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var validUser =
                GetEntityFactory<User>().Create(new {DefaultOperatingCenter = operatingCenter, UserName = "Kirwan"});
            var invalidUser =
                GetEntityFactory<User>().Create(new {DefaultOperatingCenter = otherOperatingCenter, UserName = "Keane"});

            var result = (CascadingActionResult)_target.GetByOperatingCenterId(operatingCenter.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(validUser.Id.ToString(), actual[1].Value);
        }

        [TestMethod]
        public void TestGetByOperatingCenterIdReturnsOnlyActiveUsers()
        {
            var operatingCenter =
                GetFactory<OperatingCenterFactory>()
                    .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var validUser =
                GetEntityFactory<User>()
                    .Create(new {DefaultOperatingCenter = operatingCenter, UserName = "Kirwan", HasAccess = true});
            var invalidUser =
                GetEntityFactory<User>()
                    .Create(new {DefaultOperatingCenter = operatingCenter, UserName = "Keane", HasAccess = false});

            var result = (CascadingActionResult)_target.GetByOperatingCenterId(operatingCenter.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(validUser.Id.ToString(), actual[1].Value);
        }

        [TestMethod]
        public void TestGetAllByOperatingCenterIdReturnsUsersByOperatingCenterIdAndIgnoresHasAccessValue()
        {
            var operatingCenter =
                GetFactory<OperatingCenterFactory>()
                    .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var validUser =
                GetEntityFactory<User>().Create(new {DefaultOperatingCenter = operatingCenter, UserName = "Kirwan"});
            var validUser2 =
                GetEntityFactory<User>().Create(new {DefaultOperatingCenter = operatingCenter, UserName = "Keane"});

            var result = (CascadingActionResult)_target.GetAllByOperatingCenterId(operatingCenter.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(2, actual.Count() - 1); // -1 for the empty list item that gets included.
            Assert.AreEqual(validUser.Id.ToString(), actual[1].Value);
            Assert.AreEqual(validUser2.Id.ToString(), actual[2].Value);
        }

        [TestMethod]
        public void TestGetAllByStateOrOperatingCenterIdReturnsUsersForAllScenarios()
        {
            var nj = GetFactory<StateFactory>().Create(new { Abbreviation = "NJ" });
            var ny = GetFactory<StateFactory>().Create(new { Abbreviation = "NY" });
            var nj7 = GetFactory<UniqueOperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury", State = nj });
            var nj4 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood", State = nj });
            var ny1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NY1", OperatingCenterName = "NYC", State = ny });
            var validUser = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = nj7, UserName = "Kirwan" });
            var validUser2 = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = nj4, UserName = "Keane" });
            var invalidUser = GetEntityFactory<User>()
               .Create(new { DefaultOperatingCenter = ny1, UserName = "Walters" });

            var result = (CascadingActionResult)_target.GetAllByStateOrOperatingCenterId(nj.Id, nj7.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1); // -1 for the empty list item that gets included.
            Assert.AreEqual(validUser.Id.ToString(), actual[1].Value);

            result = (CascadingActionResult)_target.GetAllByStateOrOperatingCenterId(nj.Id, null);
            actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(2, actual.Count() - 1); // -1 for the empty list item that gets included.
            Assert.AreEqual(validUser.Id.ToString(), actual[1].Value);
            Assert.AreEqual(validUser2.Id.ToString(), actual[2].Value);

            result = (CascadingActionResult)_target.GetAllByStateOrOperatingCenterId(null, null);
            actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(3, actual.Count() - 1); // -1 for the empty list item that gets included.
            Assert.AreEqual(validUser.Id.ToString(), actual[1].Value);
            Assert.AreEqual(validUser2.Id.ToString(), actual[2].Value);
            Assert.AreEqual(invalidUser.Id.ToString(), actual[3].Value);
        }

        [TestMethod]
        public void TestGetActiveUsersByStateIdReturnsUsersByStateId()
        {
            var state =
                GetEntityFactory<State>()
                   .Create(new { Abbreviation = "NJ" });
            var otherState =
                GetEntityFactory<State>()
                   .Create(new { Abbreviation = "PA" });
            var operatingCenter =
                GetFactory<OperatingCenterFactory>()
                   .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury", State = state });
            var otherOperatingCenter =
                GetFactory<OperatingCenterFactory>()
                   .Create(new { OperatingCenterCode = "PA4", OperatingCenterName = "Lakewood", State = otherState });
            var validUser =
                GetEntityFactory<User>().Create(new { DefaultOperatingCenter = operatingCenter });
            var invalidUser =
                GetEntityFactory<User>().Create(new { DefaultOperatingCenter = otherOperatingCenter });

            var result = (CascadingActionResult)_target.GetActiveUsersByStateId(operatingCenter.State.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(validUser.Id.ToString(), actual[1].Value);
        }

        [TestMethod]
        public void TestGetActiveUsersByStateIdReturnsOnlyActiveUsers()
        {
            var state =
                GetEntityFactory<State>()
                   .Create(new { Abbreviation = "NJ" });
            var operatingCenter =
                GetFactory<OperatingCenterFactory>()
                   .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury", State = state });
            var validUser =
                GetEntityFactory<User>().Create(new { DefaultOperatingCenter = operatingCenter, HasAccess = true });
            var inActiveUser =
                GetEntityFactory<User>().Create(new { DefaultOperatingCenter = operatingCenter, HasAccess = false });

            var result = (CascadingActionResult)_target.GetActiveUsersByStateId(operatingCenter.State.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(validUser.Id.ToString(), actual[1].Value);
        }

        [TestMethod]
        public void TestGetActiveUsersWithOpCenterIdAndRoleForLockoutDeviceReturnsOnlyActiveUsersWithOpCntrAndRole()
        {
            var operatingCenter =
                GetFactory<OperatingCenterFactory>()
                   .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var role = GetFactory<RoleFactory>().Create(RoleModules.OperationsLockoutForms, operatingCenter);
            var validUser =
                GetEntityFactory<User>()
                   .Create(new {
                        DefaultOperatingCenter = operatingCenter, UserName = "joe7", HasAccess = true,
                        Roles = new List<Role> { role }
                   });
            var invalidUser =
                GetEntityFactory<User>()
                   .Create(new { DefaultOperatingCenter = operatingCenter, UserName = "jane", HasAccess = false });

            _authenticationService.SetupGet(x => x.CurrentUser).Returns(validUser);

            var result = (CascadingActionResult)_target.GetActiveUsersWithOpCenterIdAndRoleForLockoutDevice(operatingCenter.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(validUser.FullName, actual[1].Text);
        }

        [TestMethod]
        public void TestGetByOperatingCenterIdAndPartialNameMatchForTDWorkOrdersReturnsOnlyUsersWithOpCenterOrRole()
        {
            var operatingCenter =
                GetFactory<OperatingCenterFactory>()
                   .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var otherOperatingCenter =
                GetFactory<OperatingCenterFactory>()
                   .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Howell" });
            var ocUser =
                GetEntityFactory<User>()
                   .Create(new
                    {
                        DefaultOperatingCenter = operatingCenter,
                        UserName = "ocUser",
                        FullName = "ocUser"
                    });
            var ocRoleUser =
                GetEntityFactory<User>()
                   .Create(new
                    {
                        DefaultOperatingCenter = otherOperatingCenter,
                        UserName = "ocRoleUser",
                        FullName = "ocRoleUser",
                   });
            var wildcardRoleUser =
                GetEntityFactory<User>()
                   .Create(new
                    {
                        DefaultOperatingCenter = otherOperatingCenter,
                        UserName = "wildcardRoleUser",
                        FullName = "wildcardRoleUser",
                        HasAccess = true,
                    });
            var invalidUser =
                GetEntityFactory<User>()
                   .Create(new {
                        DefaultOperatingCenter = otherOperatingCenter,
                        UserName = "invalidUser",
                        FullName = "invalidUser",
                        HasAccess = true
                    });
            var module = GetFactory<ModuleFactory>().Create(new { Id = (int)RoleModules.FieldServicesWorkManagement });
            var ocRole = GetFactory<RoleFactory>().Create(new { Module = module, OperatingCenter = operatingCenter, User = ocRoleUser });
            var wildcardRole = GetFactory<WildcardOpCenterRoleFactory>().Create(new { Module = module, User = wildcardRoleUser });

            var result =
                (AutoCompleteResult)_target.GetByOperatingCenterIdAndPartialNameMatchForTDWorkOrders("U",
                    operatingCenter.Id);
            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(3, actual.Count());

            foreach (var item in actual)
            {
                if (item.Id == invalidUser.Id)
                {
                    Assert.Fail("Invalid user was unexpected included in result");
                }
            }
        }

        [TestMethod]
        public void TestLockoutFormUsersByOperatingCenterIdReturnsActiveAndInactiveUsersWithTheLockoutFormsRoleForAGivenOperatingCenter()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<ActiveEmployeeFactory>().Create();
            var user = GetEntityFactory<User>().Create(new { Employee = employee });

            var readAction = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var addAction = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Add });
            var editAction = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Edit });
            var deleteAction = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Delete });
            var userAdminAction = GetFactory<ActionFactory>().Create(new { Id = RoleActions.UserAdministrator });

            // Test employee with user without any matching roles does not end up in list 
            var result = (CascadingActionResult)_target.LockoutFormUsersByOperatingCenterId(opc.Id);
            Assert.IsFalse(result.Data.Any());

            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsLockoutForms }),
                Action = readAction,
                User = user
            });

            void doTest(RoleAction action, OperatingCenter opcForRole)
            {
                role.OperatingCenter = opcForRole;
                role.Action = action;
                Session.Save(role);
                Session.Flush();

                var result1 = (CascadingActionResult)_target.LockoutFormUsersByOperatingCenterId(opc.Id);
                var testableResult = result1.Data.Any();
                Assert.IsTrue(testableResult, $"No result was found for action '{action.Description}' and operating center '{opcForRole}'.");
            }
            
            foreach(var action in new [] { readAction, addAction, editAction, deleteAction, userAdminAction})
            {
                // Do wildcard match
                doTest(addAction, null);
                // Do opc match
                doTest(action, opc);
            }

            // inactive user with role should show up
            employee.Status = GetFactory<InactiveEmployeeStatusFactory>().Create();
            Session.Save(employee);
            doTest(userAdminAction, null);
        }

        [TestMethod]
        public void TestGetActiveUsersWithOpCenterIdAndRoleAndIsAssignedToLockOutDevices()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodEmployee = GetFactory<ActiveEmployeeFactory>().Create();
            var goodUser = GetEntityFactory<User>().Create(new { Employee = goodEmployee });
            var badEmployee = GetFactory<ActiveEmployeeFactory>().Create();
            var badUser = GetEntityFactory<User>().Create(new { Employee = badEmployee });
            var goodLockoutdevice = GetEntityFactory<LockoutDevice>().Create(new {Person = goodUser, OperatingCenter = opc});
            var badLockoutdevice = GetEntityFactory<LockoutDevice>().Create(new {Person = badUser,  OperatingCenter = opc});
            
            var readAction = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });

            // Test employee with user without any matching roles does not end up in list 
            var result = (CascadingActionResult)_target.GetActiveUsersWithOpCenterIdAndRoleAndIsAssignedToLockOutDevices(opc.Id);
            Assert.IsFalse(result.Data.Any());

            var goodRole = GetFactory<RoleFactory>().Create(new
            {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsLockoutForms }),
                Action = readAction,
                User = goodUser,
                OperatingCenter = opc
            });

            var badRole = GetFactory<RoleFactory>().Create(new
            {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsLockoutForms }),
                Action = readAction,
                User = goodUser,
                OperatingCenter = opc
            });

            result = (CascadingActionResult)_target.GetActiveUsersWithOpCenterIdAndRoleAndIsAssignedToLockOutDevices(opc.Id);
            var actual = (IEnumerable<User>)result.Data;
           
            Assert.IsTrue(actual.Any());
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(goodLockoutdevice.Person.Id, actual.First().Id);
        }

        #endregion

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // noop: action only lets you view your own record. Covered in other tests.
        }

        [TestMethod]
        public void TestShowRedirectsToForbiddenIfCurrentUserIsNotUserAndNotSiteAdmin()
        {
            _currentUser = GetEntityFactory<User>().Create();
            var otherUser = GetEntityFactory<User>().Create();

            _authenticationService.SetupGet(x => x.CurrentUserId).Returns(_currentUser.Id);
            _authenticationService.SetupGet(x => x.CurrentUserIsAdmin).Returns(false);
            _authenticationService.SetupGet(x => x.CurrentUser.IsUserAdmin).Returns(false);

            var result = (RedirectResult)_target.Show(otherUser.Id);

            Assert.AreEqual(ControllerExtensions.Urls.FORBIDDEN, result.Url);
        }

        [TestMethod]
        public void TestShowShowsUserWithGivenIdIfUserExistsAndIsTheSameAsTheCurrentSiteUser()
        {
            _currentUser = GetEntityFactory<User>().Create();
            _authenticationService.SetupGet(x => x.CurrentUserId).Returns(_currentUser.Id);
            _authenticationService.SetupGet(x => x.CurrentUserIsAdmin).Returns(false);
            _authenticationService.SetupGet(x => x.CurrentUser.IsUserAdmin).Returns(false);

            var result = (ViewResult)_target.Show(_currentUser.Id);
            
            Assert.AreEqual(_currentUser, (User)result.Model);
        }

        [TestMethod]
        public void TestShowShowsUserWithGivenIdIfUserExistsAndCurrentSiteUserIsAdmin()
        {
            _currentUser = GetFactory<AdminUserFactory>().Create();
            _authenticationService.SetupGet(x => x.CurrentUserId).Returns(_currentUser.Id);
            _authenticationService.SetupGet(x => x.CurrentUserIsAdmin).Returns(true);
            _authenticationService.SetupGet(x => x.CurrentUser.IsUserAdmin).Returns(false);

            var otherUser = GetEntityFactory<User>().Create();

            var result = (ViewResult)_target.Show(otherUser.Id);

            Assert.AreEqual(otherUser, (User)result.Model);
        }

        [TestMethod]
        public void TestShowShowsUserWithGivenIdIfUserExistsAndCurrentSiteUserIsUserAdmin()
        {
            _currentUser = GetFactory<AdminUserFactory>().Create();
            _authenticationService.SetupGet(x => x.CurrentUserId).Returns(_currentUser.Id);
            _authenticationService.SetupGet(x => x.CurrentUserIsAdmin).Returns(false);
            _authenticationService.SetupGet(x => x.CurrentUser.IsUserAdmin).Returns(true);

            var otherUser = GetEntityFactory<User>().Create();

            var result = (ViewResult)_target.Show(otherUser.Id);

            Assert.AreEqual(otherUser, (User)result.Model);
        }

        #endregion

        #region Create

        [TestMethod]
        public void TestCreateCreatesNewMembershipUserOnSuccess()
        {
            _membershipHelper.Setup(x => x.CreateUser("test username", 
                                  "dummy password", 
                                  "some@email.com",
                                  "What was the email address used to setup your account?",
                                  "some@email.com",
                                  true))
                             .Returns(System.Web.Security.MembershipCreateStatus.Success);

            var model = _viewModelFactory.Build<CreateUser, User>(GetEntityFactory<User>().Create());
            model.UserName = "test username";
            model.Email = "some@email.com";

            _target.Create(model);

            _membershipHelper.Verify();
            _membershipHelper.Verify(x => x.ResetPassword("test username"));
        }

        [TestMethod]
        public void TestCreateThrowsInvalidOperationExceptionIfMembershipUserCreationIsNotSuccessul()
        {
            _membershipHelper.Setup(x => x.CreateUser("test username", 
                                  "dummy password", 
                                  "some@email.com",
                                  "What was the email address used to setup your account?",
                                  "some@email.com",
                                  true))
                             .Returns(System.Web.Security.MembershipCreateStatus.ProviderError);

            var model = _viewModelFactory.Build<CreateUser, User>(GetEntityFactory<User>().Create());
            model.UserName = "test username";
            model.Email = "some@email.com";

            MyAssert.Throws<InvalidOperationException>(() => _target.Create(model));
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<User>().Create();
            var expected = "some value";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditUser, User>(eq, x => {
                x.Address = expected;
            })) as RedirectToRouteResult;

            Assert.AreEqual(expected, Session.Get<User>(eq.Id).Address);
        }

        #endregion

        #region UnlockUser

        [TestMethod]
        public void TestUnlockUserDisplaysSuccessMessageIfMembershipHelperUnlockUserReturnsTrue()
        {
            var user = GetEntityFactory<User>().Create(new { UserName = "this test user" });    
            _membershipHelper.Setup(x => x.UnlockUser("this test user")).Returns(true);

            var result = _target.UnlockUser(user.Id);

            var successMessages = (List<string>)_target.TempData[MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY];
            Assert.IsTrue(successMessages.Contains("The Membership account for this test user has been unlocked."));
        }

        [TestMethod]
        public void TestUnlockUserDisplaysErrorMessageIfMembershipHelperUnlockUserReturnsFalse()
        {
            var user = GetEntityFactory<User>().Create(new { UserName = "this test user" });    
            _membershipHelper.Setup(x => x.UnlockUser("this test user")).Returns(false);

            var result = _target.UnlockUser(user.Id);

            var successMessages = (List<string>)_target.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY];
            Assert.IsTrue(successMessages.Contains("Unable to unlock user this test user."));
        }

        #endregion

        #region ResetApiPassword

        [TestMethod]
        public void TestResetApiPasswordReturnsNotFoundIfUserDoesNotExist()
        {
            MvcAssert.IsNotFound(_target.ResetApiPassword(0));
        }

        [TestMethod]
        public void TestResetApiPasswordResetsMembershipPassword()
        {
            _membershipHelper.Setup(x => x.ResetPassword("some user")).Returns("some string");
            var user = GetEntityFactory<User>().Create(new { UserName = "some user" });
            _target.ResetApiPassword(user.Id);

            _membershipHelper.Verify(x => x.ResetPassword("some user"));
        }

        #endregion

        #region GetToken

        [TestMethod]
        public void TestGetTokenReturnsTokenIfUserAlreadyHasProfileId()
        {
            const string token = "some long string the authorize.net site generated";
            _currentUser = GetEntityFactory<User>().Create(new { CustomerProfileId = 666 });
            _target.SetHiddenFieldValueByName("_communicatorUrl", "ORLY?");

            var gateway = new Mock<IExtendedCustomerGateway>();
            _container.Inject(gateway.Object);

            gateway.Setup(x => x.GetHostedSessionKey((uint)_currentUser.CustomerProfileId, _target.CommunicatorUrl, false)).Returns(token);
            
            var result = _target.GetToken(_currentUser.Id);

            Assert.AreEqual(token, ((ContentResult)result).Content);
        }

        [TestMethod]
        public void TestGetTokenReturnsTokenIfUserAlreadyHasProfileIdAtAuthorizeNet()
        {
            var profileId = "402";
            const string token = "some long string the authorize.net site generated";
            _currentUser = GetEntityFactory<User>().Create();
            _target.SetHiddenFieldValueByName("_communicatorUrl", "ORLY?");

            var gateway = new Mock<IExtendedCustomerGateway>();
            _container.Inject(gateway.Object);
            gateway.Setup(x => x.GetHostedSessionKey(uint.Parse(profileId), _target.CommunicatorUrl, false)).Returns(token);
            gateway.Setup(x => x.CreateCustomer(_currentUser.Email, _currentUser.DefaultOperatingCenter.ToString()))
                .Throws(new InvalidOperationException(string.Format(UserController.ALREADY_EXISTS, profileId)));
            gateway.Setup(x => x.GetCustomer(profileId)).Returns(new Customer {ProfileID = profileId});

            var result = _target.GetToken(_currentUser.Id);
            _currentUser = _container.GetInstance<IUserRepository>().Find(_currentUser.Id);

            Assert.AreEqual(profileId, _currentUser.CustomerProfileId.ToString());
            Assert.AreEqual(token, ((ContentResult)result).Content);
        }

        [TestMethod]
        public void TestGetTokenReturnsTokenIfUserDoesNotAlreadyHaveAProfileId()
        {
            var profileId = "402";
            const string token = "some long string the authorize.net site generated";
            _currentUser = GetEntityFactory<User>().Create();
            _target.SetHiddenFieldValueByName("_communicatorUrl", "ORLY?");

            var gateway = new Mock<IExtendedCustomerGateway>();
            _container.Inject(gateway.Object);

            gateway.Setup(x => x.GetHostedSessionKey(uint.Parse(profileId), _target.CommunicatorUrl, false)).Returns(token);
            gateway.Setup(x => x.CreateCustomer(
                                _currentUser.Email,
                                _currentUser.DefaultOperatingCenter.ToString()))
                            .Returns(new Customer{ ProfileID = profileId});
            
            var result = _target.GetToken(_currentUser.Id);
            _currentUser = _container.GetInstance<IUserRepository>().Find(_currentUser.Id);

            Assert.AreEqual(profileId, _currentUser.CustomerProfileId.ToString());
            Assert.AreEqual(token, ((ContentResult)result).Content);
        }

        #endregion

        #region ActiveFieldServicesAssetsUsersByOperatingCenter

        [TestMethod]
        public void TestActiveFieldServicesAssetsUsersByOperatingCenterReturnsAllActiveUsersWithMatchingRoleAndOperatingCenter()
        {
            var validOpc = GetFactory<OperatingCenterFactory>().Create();

            var validUserWithOperatingCenter = CreateUserAndRole(RoleModules.FieldServicesAssets, RoleActions.Read, validOpc, true);
            var validUserWithWildcardOperatingCenter = CreateUserAndRole(RoleModules.FieldServicesAssets, RoleActions.Read, null, true);
            var userWithWrongOperatingCenter = CreateUserAndRole(RoleModules.FieldServicesAssets, RoleActions.Read, GetFactory<UniqueOperatingCenterFactory>().Create(), true);
            var userWithWrongRole = CreateUserAndRole(RoleModules.BPUGeneral, RoleActions.Read, GetFactory<UniqueOperatingCenterFactory>().Create(), true);
            var userThatIsNotActive = CreateUserAndRole(RoleModules.FieldServicesAssets, RoleActions.Read, validOpc, false);

            var result = (CascadingActionResult)_target.ActiveFieldServicesAssetsUsersByOperatingCenter(validOpc.Id);
            var resultData = (IEnumerable<User>)result.Data;

            Assert.IsTrue(resultData.Contains(validUserWithOperatingCenter));
            Assert.IsTrue(resultData.Contains(validUserWithWildcardOperatingCenter));
            Assert.IsFalse(resultData.Contains(userThatIsNotActive));
            Assert.IsFalse(resultData.Contains(userWithWrongOperatingCenter));
            Assert.IsFalse(resultData.Contains(userWithWrongRole));
        }

        #endregion

        #region FieldServicesAssetsUsersByOperatingCenter

        [TestMethod]
        public void TestFieldServicesAssetsUsersByOperatingCenterReturnsAllUsersWithMatchingRoleAndOperatingCenterWhetherOrNotTheyAreActive()
        {
            var validOpc = GetFactory<OperatingCenterFactory>().Create();

            var validUserWithOperatingCenter = CreateUserAndRole(RoleModules.FieldServicesAssets, RoleActions.Read, validOpc, true);
            var validUserWithWildcardOperatingCenter = CreateUserAndRole(RoleModules.FieldServicesAssets, RoleActions.Read, null, true);
            var userWithWrongOperatingCenter = CreateUserAndRole(RoleModules.FieldServicesAssets, RoleActions.Read, GetFactory<UniqueOperatingCenterFactory>().Create(), true);
            var userWithWrongRole = CreateUserAndRole(RoleModules.BPUGeneral, RoleActions.Read, GetFactory<UniqueOperatingCenterFactory>().Create(), true);
            var userThatIsNotActive = CreateUserAndRole(RoleModules.FieldServicesAssets, RoleActions.Read, validOpc, false);

            var result = (CascadingActionResult)_target.FieldServicesAssetsUsersByOperatingCenter(validOpc.Id);
            var resultData = (IEnumerable<User>)result.Data;

            Assert.IsTrue(resultData.Contains(validUserWithOperatingCenter));
            Assert.IsTrue(resultData.Contains(validUserWithWildcardOperatingCenter));
            Assert.IsTrue(resultData.Contains(userThatIsNotActive));
            Assert.IsFalse(resultData.Contains(userWithWrongOperatingCenter));
            Assert.IsFalse(resultData.Contains(userWithWrongRole));
        }

        #endregion

        #region Active Users with Risk Register Role

        [TestMethod]
        public void TestAwiaUsersByOperatingCenterIdAndRole()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var role = GetFactory<RoleFactory>().Create(RoleModules.EngineeringRiskRegister, operatingCenter);
            var validUser = GetEntityFactory<User>().Create(new {
                        DefaultOperatingCenter = operatingCenter,
                        UserName = "joe7",
                        HasAccess = true,
                        Roles = new List<Role> { role }
                    });
            var invalidUser = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = operatingCenter, UserName = "jane", HasAccess = false });

            _authenticationService.SetupGet(x => x.CurrentUser).Returns(validUser);

            var result = (CascadingActionResult)_target.AwiaUsersByOperatingCenterIdAndRole(operatingCenter.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(validUser.FullName, actual[1].Text);
        }

        #endregion

        #endregion
    }
}