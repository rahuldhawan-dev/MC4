using System;
using System.Security.Principal;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;
using ControllerBase = MMSINC.Controllers.ControllerBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MapCallApi.Tests
{
    public abstract class MapCallApiControllerTestBase<TController, TEntity, TRepository> :
        ControllerTestBase<MvcApplication, TController, TEntity, TRepository>
        where TController : ControllerBase
        where TEntity : class, new()
        where TRepository : class, IRepository<TEntity>
    {
        #region Private Members

        private MapCallApiControllerAuthorizationTester _authTester;
        protected Mock<IAuthenticationService<User>> _authenticationService;
        protected Mock<IMembershipHelper> _membershipHelper;
        protected User _currentUser;
        protected Mock<IPrincipal> _principalMock;
        protected Mock<IIdentity> _identityMock;
        protected Mock<INotificationService> _notificationService;
        protected DateTime _now;
        protected TestDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        protected MapCallApiControllerAuthorizationTester Authorization =>
            _authTester ?? (_authTester = new MapCallApiControllerAuthorizationTester(Application, _container,
                CreateFactoryService()));

        protected Mock<IAuthenticationService<User>> AuthenticationService => _authenticationService;

        #endregion

        #region Constructors

        public MapCallApiControllerTestBase()
        {
            MMSINC.MvcApplication.IsInTestMode = true;
            _dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now);
        }

        #endregion

        #region Private Methods

        protected override IInMemoryDatabaseTestInterceptor CreateInterceptor() =>
            _container.GetInstance<MapCallApiInMemoryDatabaseTestInterceptorWithChangeTracking>();

        protected virtual User CreateUser()
        {
            // just here to test c# 7 features in TeamCity, feel free to delete
            void throwaway() { }

            return GetEntityFactory<User>().Create();
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(ActionFactory).Assembly);
        }

        [TestInitialize]
        public void MapCallApiControllerTestBaseTestInitialize()
        {
            _principalMock.Setup(x => x.Identity).Returns((_identityMock = new Mock<IIdentity>()).Object);
            _authenticationService.SetupGet(x => x.CurrentUser).Returns(_currentUser = CreateUser());
            _authenticationService.SetupGet(x => x.CurrentUserIsAuthenticated).Returns(true);
        }

        [TestCleanup]
        public void MapCallApiControllerTestBaseTestCleanup()
        {
            MMSINC.MvcApplication.IsInTestMode = false;
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            MapCallDependencies.RegisterRepositories(i);
            _principalMock = i.For<IPrincipal>().Mock();
            i.For<ITokenRepository<SecureFormToken, SecureFormDynamicValue>>().Use<SecureFormTokenRepository>();
            i.For<IDateTimeProvider>().Use(() => _dateTimeProvider);
            _authenticationService = i.For<IAuthenticationService<User>>().Mock();
            i.For<IAuthenticationService>().Use(_authenticationService.Object);
            i.For<IAuthenticationCookieFactory>().Use<AuthenticationCookieFactory>();
            i.For<INotificationService>().Use((_notificationService = new Mock<INotificationService>()).Object);
            _membershipHelper = i.For<IMembershipHelper>().Mock();
            i.For<IImageToPdfConverter>().Use<ImageToPdfConverter>();
            i.For<IDisplayItemService>().Use<DisplayItemService>();
        }

        protected void InitializeControllerForRequest(string virtualPath)
        {
            Request = Application.CreateRequestHandler(virtualPath);
            _target = Request.CreateAndInitializeController<TController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeTestForCreateAndUpdateTests = () => {
                // noop for API project.
            };
        }

        protected void SetupHttpAuth(MapCallApiControllerAuthorizationAsserter a)
        {
            a.OnRequestReset = (req) => {
                req.RequestHeaders.Add("Authorization", "123456" + "user:pass".ToBase64String());
            };
            _membershipHelper.Setup(x => x.ValidateUser("user", "pass")).Returns(true);
        }

        #endregion
    }

    public abstract class MapCallApiControllerTestBase<TController, TEntity> :
        MapCallApiControllerTestBase<TController, TEntity, RepositoryBase<TEntity>>
        where TController : ControllerBase
        where TEntity : class, new()
    {
        #region Private Methods

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(ActionFactory).Assembly);
        }

        #endregion
    }
}
