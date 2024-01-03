using System;
using System.Security.Principal;
using log4net;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MMSINC.Authentication;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using Moq;
using MMSINC.Utilities;
using MMSINC.Utilities.ActiveMQ;
using MMSINC.Utilities.Pdf;
using NHibernate;
using StructureMap;

namespace MapCallMVC.Tests
{
    public abstract class MapCallMvcControllerTestBase<TController, TEntity, TRepository> :
        ControllerTestBase<MvcApplication, TController, TEntity, TRepository>
        where TController : ControllerBase
        where TEntity : class, new()
        where TRepository : class, IRepository<TEntity>
    {
        #region Private Members

        private MapCallMvcControllerAuthorizationTester _authTester;
        protected Mock<IAuthenticationService<User>> _authenticationService;
        protected User _currentUser;
        protected Mock<IPrincipal> _principalMock;
        protected Mock<IIdentity> _identityMock;
        protected Mock<IAssetCoordinateService> _assetCoordinateService;
        protected Mock<INotificationService> _notificationService;
        protected Mock<ILog> _log;
        protected DateTime _now;
        protected TestDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        protected MapCallMvcControllerAuthorizationTester Authorization =>
            _authTester ?? (_authTester =
                new MapCallMvcControllerAuthorizationTester(
                    Application,
                    Session,
                    CreateFactoryService(),
                    _container));

        protected Mock<IAuthenticationService<User>> AuthenticationService => _authenticationService;

        public User CurrentUserTest => _currentUser;

        #endregion

        #region Private Methods

        protected override IInMemoryDatabaseTestInterceptor CreateInterceptor() =>
            _container.GetInstance<MapCallMVCInMemoryDatabaseTestInterceptorWithChangeTracking>();

        protected virtual User CreateUser()
        {
            return GetEntityFactory<User>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeTestForCreateAndUpdateTests = () => {
                // All tests initialize by setting the current user with whatever CreateUser() returns.
                // For the automatic controller tests, due to the validation check, we need to use a site
                // admin user. We otherwise would need to create entities that fit specific roles and that
                // would become a major pain point in testing. Role-based stuff is already tested in the
                // functional tests.
                // 
                // Some of the tests override CreateUser to set the Employee as well, which we'll still need
                // even if the user's an admin.
                //
                // And one test, OperatingCenterControllerTest, has CreateUser return null because UserFactory
                // creates an OperatingCenter and that messes up all of those tests for some reason.
                if (_currentUser != null)
                {
                    _currentUser.IsAdmin = true;
                    Session.SaveOrUpdate(_currentUser);
                    _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(_currentUser.IsAdmin);
                }
            };
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(ActionFactory).Assembly).GetInstance<TestDataFactoryService>();
        }

        [TestInitialize]
        public void MapCallMvcControllerTestBaseTestInitialize()
        {
            _principalMock.Setup(x => x.Identity).Returns((_identityMock = new Mock<IIdentity>()).Object);
            _authenticationService.SetupGet(x => x.CurrentUser).Returns(_currentUser = CreateUser());
            _authenticationService.SetupGet(x => x.CurrentUserIsAuthenticated).Returns(true);
            // Some tests explicitly return null from CreateUser(). ex OperatingCenterControllerTest.
            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(_currentUser?.IsAdmin ?? false);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            MapCallDependencies.RegisterRepositories(e);
            _principalMock = e.For<IPrincipal>().Mock();
            e.For<ITokenRepository<SecureFormToken, SecureFormDynamicValue>>().Use<SecureFormTokenRepository>();
            e.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
            _authenticationService = e.For<IAuthenticationService<User>>().Mock();
            e.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            e.For<IRoleService>().Use(new Mock<IRoleService>().Object);
            e.For<IAuthenticationCookieFactory>().Use<AuthenticationCookieFactory>();
            e.For<IAssetCoordinateService>()
             .Use((_assetCoordinateService = new Mock<IAssetCoordinateService>()).Object);
            e.For<INotificationService>().Use((_notificationService = new Mock<INotificationService>()).Object);
            e.For<IActiveMQServiceFactory>().Use(new Mock<IActiveMQServiceFactory>().Object);
            e.For<IDisplayItemService>().Use<DisplayItemService>();
            e.For<ILog>().Use((_log = new Mock<ILog>()).Object);
            e.For<IHtmlToPdfConverter>().Use(new Mock<IHtmlToPdfConverter>().Object);
            e.For<IMapResultFactory>().Use<MapResultFactory>();
        }

        protected void InitializeControllerForRequest(string virtualPath)
        {
            Request = Application.CreateRequestHandler(virtualPath);
            _target = Request.CreateAndInitializeController<TController>();
        }

        #endregion
    }

    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    public abstract class MapCallMvcControllerTestBase<TController, TEntity> :
        MapCallMvcControllerTestBase<TController, TEntity, RepositoryBase<TEntity>>
        where TController : ControllerBase
        where TEntity : class, new()
    {
        #region Private Methods

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(ActionFactory).Assembly).GetInstance<TestDataFactoryService>();
        }

        #endregion
    }
}
