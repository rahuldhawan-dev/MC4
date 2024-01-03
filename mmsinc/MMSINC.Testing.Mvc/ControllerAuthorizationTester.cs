using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Authentication;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;
using RealAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MMSINC.Testing
{
    public enum UserType
    {
        /// <summary>
        /// Create a user with no special privileges(ie, not site admins)
        /// </summary>
        Normal,

        /// <summary>
        /// No user should be created. For anonymous access stuff.
        /// </summary>
        NoUser,

        /// <summary>
        /// Creates a user that is a site administrator.
        /// </summary>
        SiteAdmin
    }

    /// <summary>
    /// Test methods for asserting a user accessing a specific controller action has the correct authorization.
    /// This runs tests by actually creating a request and letting it run through the authorization filters. This
    /// is better at proving authorization occurs than checking for attributes.
    /// </summary>
    /// <typeparam name="TApp"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TAsserter"></typeparam>
    public abstract class ControllerAuthorizationTester<TApp, TUser, TAsserter>
        where TApp : MvcApplication, new()
        where TUser : class, IAdministratedUser
        where TAsserter : ControllerAuthorizationAsserter<TApp, TUser>
    {
        #region Private Members

        protected readonly ITestDataFactoryService _factoryService;
        protected readonly MvcApplicationTester<TApp> _application;

        #endregion

        #region Constructors

        protected ControllerAuthorizationTester(MvcApplicationTester<TApp> appTester,
            ITestDataFactoryService testFactoryService)
        {
            // Don't do any ObjectFactory.Container.injecting here. Because this is kind of a sub-test
            // for an actual test, we don't want to care about the order this is constructed
            // with other things in a [TestInitialize] scenario. ie: RoleService/AuthServ/something
            // getting injected here and then immediately overwritten somewhere else. Also don't
            // create a RoleService instance because it immediately gets an AuthServ instance.
            _application = appTester;
            _factoryService = testFactoryService;
        }

        #endregion

        #region Abstract Methods

        protected abstract TAsserter CreateAsserter();

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Runs all the assertions and then fails if any action for the controller has not been tested.
        /// </summary>
        public void Assert(Action<TAsserter> assertionAction)
        {
            var asserter = CreateAsserter();
            asserter.ActionsTested.Clear();
            assertionAction(asserter);

            var allActions = asserter.AllControllerActions ?? new string[0];
            if (!allActions.Any())
            {
                RealAssert.Fail("No actions were tested. Try harder next time!");
            }

            var missingInAction = allActions.Except(asserter.ActionsTested).ToArray();
            if (missingInAction.Any())
            {
                RealAssert.Fail("Authorization tests were not ran for the following actions: {0}." +
                                " If these are public methods that are not actions, make sure you put the NonActionAttribute on them.",
                    string.Join(", ", missingInAction));
            }
        }

        #endregion
    }

    public abstract class ControllerAuthorizationAsserter<TApp, TUser>
        where TApp : MvcApplication, new()
        where TUser : class, IAdministratedUser
    {
        #region Private Members

        protected readonly ITestDataFactoryService _factoryService;
        protected readonly IContainer _container;
        private Mock<IAuthenticationService<TUser>> _authServ;
        private string[] _allControllerActions;

        #endregion

        #region Properties

        protected MvcApplicationTester<TApp> Application { get; }
        protected FakeMvcHttpHandler Request { get; private set; }
        protected TUser User { get; private set; }

        /// <summary>
        /// Provides extra handling of the Request object after a request reset has occurred.
        /// </summary>
        public Action<FakeMvcHttpHandler> OnRequestReset { get; set; }

        public string[] AllControllerActions => _allControllerActions;
        public HashSet<string> ActionsTested { get; } = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        #endregion

        #region Constructors

        public ControllerAuthorizationAsserter(MvcApplicationTester<TApp> appTester,
            ITestDataFactoryService testFactoryService, IContainer container)
        {
            Application = appTester;
            _factoryService = testFactoryService;
            _container = container;
        }

        #endregion

        #region Private Methods

        protected TFactory GetFactory<TFactory>() where TFactory : TestDataFactory
        {
            return _factoryService.GetFactory<TFactory>();
        }

        /// <summary>
        /// Resets all the test fields. Call this before asserting anything.
        /// </summary>
        protected virtual void Reset(string requestPath, UserType userType)
        {
            // We don't want a User object to exist if the UserType is NoUser.
            User = (userType == UserType.NoUser ? null : CreateUser(userType));

            var isAuthenticated = userType != UserType.NoUser;

            _authServ = new Mock<IAuthenticationService<TUser>>();
            _authServ.Setup(x => x.CurrentUser).Returns(User);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(() => isAuthenticated && User.IsAdmin);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(isAuthenticated);

            _container.Inject(_authServ.Object);
            _container.Inject<IAuthenticationService>(_authServ.Object);
            _container.Inject<IAuthenticationService<IAdministratedUser>>(_authServ.Object);

            if (User is IUserWithProfile)
            {
                _container.Inject((IAuthenticationService<IUserWithProfile>)_authServ.Object);
            }

            Request = Application.CreateRequestHandler(requestPath);
            // This needs to be set because the base AuthorizeAttribute that LogonAuthorizeAttribute
            // uses specifically reads Request.User.IsAuthenticated.
            Request.UserIdentity.Setup(x => x.IsAuthenticated).Returns(isAuthenticated);
            ActionsTested.Add(Request.RouteContext.ActionName);

            if (_allControllerActions == null)
            {
                _allControllerActions = GetValidActions().ToArray();
            }

            OnRequestReset?.Invoke(Request);
        }

        private IEnumerable<string> GetValidActions()
        {
            foreach (var action in Request.RouteContext.ControllerDescriptor.GetCanonicalActions())
            {
                if (!action.GetCustomAttributes(typeof(NonActionAttribute), true).Any())
                {
                    yield return action.ActionName;
                }
            }
        }

        /// <summary>
        /// Returns true/false for whether a request is authorized for anonymous users.
        /// This does not test
        /// </summary>
        protected bool CanAccessAnonymously(string requestPath)
        {
            Reset(requestPath, UserType.NoUser);
            return Request.IsAuthorized;
        }

        /// <summary>
        /// Return true if a user can successfully.
        /// </summary>
        protected bool CanAccessAsSiteAdmin(string requestPath)
        {
            Reset(requestPath, UserType.SiteAdmin);
            return Request.IsAuthorized;
        }

        private IEnumerable<MvcAuthorizer> GetNonDefaultAuthorizers()
        {
            var globalFilters = Application.Filters.GlobalFilters;
            var authFilter = globalFilters.Select(x => x.Instance).OfType<AuthorizationFilterBase>().Single();
            foreach (var auth in authFilter.Authorizors)
            {
                if (!(auth is AnonymousAuthorizer || auth is FormsAuthenticationAuthorizer ||
                      auth is HttpAuthenticationAuthorizer || auth is AdminAuthorizer))
                {
                    yield return auth;
                }
            }
        }

        /// <summary>
        /// Asserts that a user needs to be authenticated to access the path.
        /// Also asserts that anonymous access is not allowed.
        /// </summary>
        /// <param name="requestPath"></param>
        /// <param name="removeOtherFilters">Set this to true if all other authorization filters should be ignored besides the LogonAuthorizeAttribute filter.</param>
        protected void RequiresUserToBeLoggedIn(string requestPath, bool removeOtherFilters)
        {
            RealAssert.IsFalse(CanAccessAnonymously(requestPath),
                "User should not be able to access '{0}' when they are not authenticated.", requestPath);

            Reset(requestPath, UserType.Normal);

            // We need to remove extra filters for now so that only the LogonAuthorizeAttribute
            // filter will run. Otherwise RoleAuthorizationFilter will start failing if
            // before we can create a role.
            var nonDefaultAuthorizers = GetNonDefaultAuthorizers().ToArray();

            if (removeOtherFilters)
            {
                //  globalFilters.Clear();

                foreach (var auth in nonDefaultAuthorizers)
                {
                    auth.IsEnabled = false;
                }
            }

            RealAssert.IsTrue(Request.IsAuthorized,
                "User should be allowed to access '{0}' when they are authenticated.", requestPath);

            if (removeOtherFilters)
            {
                foreach (var auth in nonDefaultAuthorizers)
                {
                    auth.IsEnabled = true;
                }
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Returns a *new* User object.
        /// </summary>
        protected abstract TUser CreateUser(UserType userType);

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Asserts that a user does not have to be authenticated to access the url.
        /// </summary>
        public void AllowsAnonymousAccess(string requestPath)
        {
            RealAssert.IsTrue(CanAccessAnonymously(requestPath),
                "User should be able to access '{0}' when they are not authenticated.", requestPath);
        }

        /// <summary>
        /// Asserts that a user must be authenticatedd to access a url.
        /// Note: This will fail if there are other authorization filters that 
        /// might fail if it they require more than authentication. You should
        /// add a specific method for testing if multiple filters are there and
        /// use the protected version of this method.
        /// </summary>
        public void RequiresLoggedInUserOnly(string requestPath)
        {
            RequiresUserToBeLoggedIn(requestPath, false);
        }

        /// <summary>
        /// Asserts that a user must be a site admin to access a url.
        /// Fails if anonymous or regular users are found to be authorized.
        /// </summary>
        public virtual void RequiresSiteAdminUser(string requestPath)
        {
            RealAssert.IsTrue(CanAccessAsSiteAdmin(requestPath), "User must be a site administrator to access '{0}'.",
                requestPath);

            Reset(requestPath, UserType.Normal);
            RealAssert.IsFalse(Request.IsAuthorized, "Non-admin users should not recieve access to '{0}'.",
                requestPath);

            Reset(requestPath, UserType.NoUser);
            RealAssert.IsFalse(Request.IsAuthorized, "Non-authenticated users should not recieve access to '{0}'.",
                requestPath);
        }

        public void RequiresUserWithPaymentProfile(string requestPath)
        {
            RequiresUserToBeLoggedIn(requestPath, true);
            Reset(requestPath, UserType.Normal);

            if (!(User is IUserWithProfile))
            {
                RealAssert.Fail("User class does not implement IUserWithProfile");
            }

            // Test user isn't authorized because they do not have a profile
            RealAssert.IsFalse(Request.IsAuthorized, "Request for '{0}' was authorized when it should not have been.");

            // Test a normal user with the role can access the page
            Reset(requestPath, UserType.Normal);
            var paymentUser = (IUserWithProfile)User;
            paymentUser.CustomerProfileId = 666;
            paymentUser.ProfileLastVerified = DateTime.Now;

            RealAssert.IsTrue(Request.IsAuthorized,
                "Request for '{0}' was not authorized. User does not have a valid payment profile.", requestPath);

            // Test that a normal user with some other role can not access the page.
            Reset(requestPath, UserType.Normal);
            paymentUser = (IUserWithProfile)User;
            paymentUser.ProfileLastVerified = null;
            RealAssert.IsFalse(Request.IsAuthorized,
                "Request for '{0}' was authorized, but user does not have a valid payment profile.", requestPath);
        }

        #endregion
    }
}
