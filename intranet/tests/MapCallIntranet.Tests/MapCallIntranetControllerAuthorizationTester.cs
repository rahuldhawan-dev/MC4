using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallIntranet.Helpers;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using NHibernate;
using StructureMap;
using RealAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using UserType = MMSINC.Testing.UserType;

namespace MapCallIntranet.Tests
{
    public class MapCallIntranetControllerAuthorizationTester : ControllerAuthorizationTester<MvcApplication, User, MapCallIntranetControllerAuthorizationAsserter>
    {
        #region Private Members

        private readonly ISession _session;
        private readonly IContainer _container;

        #endregion

        #region Constructors

        public MapCallIntranetControllerAuthorizationTester(MvcApplicationTester<MvcApplication> appTester, ISession session, ITestDataFactoryService testFactoryService, IContainer container)
            : base(appTester, testFactoryService)
        {
            // Don't do any ObjectFactory.Container.injecting here. Because this is kind of a sub-test
            // for an actual test, we don't want to care about the order this is constructed
            // with other things in a [TestInitialize] scenario. ie: RoleService/AuthServ/something
            // getting injected here and then immediately overwritten somewhere else. Also don't
            // create a RoleService instance because it immediately gets an AuthServ instance.
            _session = session;
            _container = container;
        }

        #endregion

        #region Private Methods

        protected override MapCallIntranetControllerAuthorizationAsserter CreateAsserter()
        {
            return new MapCallIntranetControllerAuthorizationAsserter(_application, _factoryService, _session, _container);
        }

        #endregion
    }

    public class MapCallIntranetControllerAuthorizationAsserter : ControllerAuthorizationAsserter<MvcApplication, User>
    {
        #region Private Members

        private readonly ISession _session;
        private RoleService _roleService;

        #endregion

        #region Constructors

        public MapCallIntranetControllerAuthorizationAsserter(MvcApplicationTester<MvcApplication> appTester, ITestDataFactoryService testFactoryService, ISession session, IContainer container) : base(appTester, testFactoryService, container)
        {
            _session = session;
        }

        #endregion

        #region Private Methods

        private OperatingCenter _opc;

        protected override User CreateUser(UserType userType)
        {
            // Create and cache this once. There's no need to make a bazillion operating centers in this test.a
            if (_opc == null)
            {
                _opc = GetFactory<OperatingCenterFactory>().Create();
            }
            return GetFactory<UserFactory>().Create(new { DefaultOperatingCenter = _opc, IsAdmin = userType == UserType.SiteAdmin });
        }

        protected override void Reset(string requestPath, UserType userType)
        {
            base.Reset(requestPath, userType);

            // The base method creates our AuthServ instance. We can't create a RoleService
            // until after, otherwise RoleService will have the wrong instance.
            _roleService = _container.GetInstance<RoleService>();
            _container.Inject<IRoleService>(_roleService);
        }

        private void SetupAuthorizationRoles(RoleModules module, RoleActions action, User user = null)
        {
            user = user ?? User;
            var role = GetFactory<RoleFactory>().Create(new {
                Module = GetFactory<ModuleFactory>().Create(new { Id = module }),
                Action = GetFactory<ActionFactory>().Create(new { Id = action }),
                User = user
            });

            _session.Save(user);
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Tests that a user must be authenticated and have the required role.
        /// </summary>
        /// <param name="requestPath"></param>
        /// <param name="module"></param>
        /// <param name="action"></param>
        public void RequiresRole(string requestPath, RoleModules module, RoleActions action = RoleActions.Read)
        {
            RequiresRoles(requestPath, new Dictionary<RoleModules, RoleActions> { { module, action } });
        }

        public void RequiresRoles(string requestPath, Dictionary<RoleModules, RoleActions> roles)
        {
            // Tests that anon access is not allowed and that 
            // a user needs to be logged in. Includes the hacky
            // workaround to disable other global filters, otherwise
            // it'll fail before we can setup the user roles.
            RequiresUserToBeLoggedIn(requestPath, true);

            // Test that a normal user with the role can access the page.
            Reset(requestPath, UserType.Normal);
            foreach (var role in roles)
            {
                SetupAuthorizationRoles(role.Key, role.Value);
            }
            RealAssert.IsTrue(Request.IsAuthorized, "Request for '{0}' was not authorized. User does not have valid roles.", requestPath);

            // Test that a user with a lower access action for the same module can not
            // access the page. This is only done if the required action is higher than "Read".

            if (!roles.Values.Contains(RoleActions.Read))
            {
                Reset(requestPath, UserType.Normal);
                foreach (var role in roles)
                {
                    SetupAuthorizationRoles(role.Key, RoleActions.Read);
                }
                RealAssert.IsFalse(Request.IsAuthorized, "Request for '{0}' was authorized when it shouldn't be. User has correct module but only Read action.", requestPath);
            }

            // Test that a normal user with some other role can not access the page.
            var otherModule = EnumExtensions.AnythingBut(roles.Keys.ToArray());
            Reset(requestPath, UserType.Normal);
            SetupAuthorizationRoles(otherModule, RoleActions.Read);
            RealAssert.IsFalse(Request.IsAuthorized, "Request for '{0}' was authorized, but user does not have valid roles.", requestPath);

            // Test that site admins can test regardless of role.
            RealAssert.IsTrue(CanAccessAsSiteAdmin(requestPath), "Site admins should be able to access '{0}' regardless of roles.", requestPath);
        }

        public override void RequiresSiteAdminUser(string requestPath)
        {
            base.RequiresSiteAdminUser(requestPath);

            var route = Request.RouteContext;
            var controllerAttr = route.ControllerDescriptor.GetCustomAttributes(typeof(RequiresRoleAttribute), true);
            var actionAttr = route.ActionDescriptor.GetCustomAttributes(typeof(RequiresRoleAttribute), true);
            var allAttr = controllerAttr.Concat(actionAttr).Cast<RequiresRoleAttribute>().ToArray();

            if (allAttr.Any())
            {
                Reset(requestPath, UserType.Normal);
                foreach (var role in allAttr)
                {
                    SetupAuthorizationRoles(role.Module, role.Action);
                }
                RealAssert.IsFalse(Request.IsAuthorized, "Request for '{0}' was authorized for non-admin users with roles. It should not be authorized.", requestPath);
            }
        }

        public void RequiresUserWithIntranetProfile(string requestPath)
        {
            Reset(requestPath, UserType.Normal);
            RealAssert.IsTrue(Request.IsAuthorized, "Request for '{0}' was not authorized.", requestPath);
        }
        public void RequiresUserWithProfileAndRole(string requestPath, RoleModules module, RoleActions action = RoleActions.Read)
        {
            RequiresUserToBeLoggedIn(requestPath, true);
            Reset(requestPath, UserType.Normal);

            // Test user isn't authorized because they do not have a profile
            RealAssert.IsFalse(Request.IsAuthorized, "Request for '{0}' was authorized when it should not have been.");

            // Test a normal user with the role can access the page
            Reset(requestPath, UserType.Normal);
            User.CustomerProfileId = 666;
            User.ProfileLastVerified = DateTime.Now;
            var roles = new Dictionary<RoleModules, RoleActions> { { module, action } };
            foreach (var role in roles)
            {
                SetupAuthorizationRoles(role.Key, role.Value, User);
            }
            RealAssert.IsTrue(Request.IsAuthorized, "Request for '{0}' was not authorized. User does not have valid roles.", requestPath);

            // Test that a normal user with some other role can not access the page.
            var otherModule = EnumExtensions.AnythingBut(roles.Keys.ToArray());
            Reset(requestPath, UserType.Normal);
            SetupAuthorizationRoles(otherModule, RoleActions.Read);
            RealAssert.IsFalse(Request.IsAuthorized, "Request for '{0}' was authorized, but user does not have valid roles.", requestPath);
        }

        #endregion
    }
}
