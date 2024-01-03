using System;
using System.Security.Principal;
using log4net;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MMSINC.Authentication;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallIntranet.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.ActiveMQ;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallIntranet.Tests
{
    public abstract class MapCallIntranetControllerTestBase<TController, TEntity, TRepository> :
        ControllerTestBase<MvcApplication, TController, TEntity, TRepository>
        where TController : ControllerBase
        where TEntity : class, new()
        where TRepository : class, IRepository<TEntity>
    {
        #region Private Members

        private MapCallIntranetControllerAuthorizationTester _authTester;
        protected Mock<IAuthenticationService<User>> _authenticationService;
        protected User _currentUser;
        protected Mock<IPrincipal> _principalMock;
        protected Mock<IIdentity> _identityMock;
        protected Mock<INotificationService> _notificationService;
        protected Mock<IAssetCoordinateService> _assetCoordinateService;

        protected Mock<ILog> _log;
        protected DateTime _now;

        #endregion

        #region Properties

        protected virtual User CreateUser()
        {
            return GetEntityFactory<User>().Create();
        }

        protected MapCallIntranetControllerAuthorizationTester Authorization
        {
            get
            {
                return _authTester ?? (_authTester = new MapCallIntranetControllerAuthorizationTester(Application, Session,
                    CreateFactoryService(), _container));
            }
        }

        protected Mock<IAuthenticationService<User>> AuthenticationService
        {
            get { return _authenticationService; }
        }

        public User CurrentUserTest
        {
            get { return _currentUser; }
        }

        #endregion

        #region Private Methods

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(ActionFactory).Assembly).GetInstance<TestDataFactoryService>();
        }

        [TestInitialize]
        public void MapCallIntranetControllerTestBaseTestInitialize()
        {
            _principalMock.Setup(x => x.Identity).Returns((_identityMock = new Mock<IIdentity>()).Object);
            _authenticationService.SetupGet(x => x.CurrentUser).Returns(_currentUser = CreateUser());
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            MapCallDependencies.RegisterRepositories(e);
            _principalMock = e.For<IPrincipal>().Mock();
            e.For<ITokenRepository<SecureFormToken, SecureFormDynamicValue>>().Use<SecureFormTokenRepository>();
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_now = DateTime.Now));
            _authenticationService = e.For<IAuthenticationService<User>>().Mock();
            e.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            e.For<IRoleService>().Use(new Mock<IRoleService>().Object);
            e.For<IAuthenticationCookieFactory>().Use<AuthenticationCookieFactory>();
            e.For<IAssetCoordinateService>().Use((_assetCoordinateService = new Mock<IAssetCoordinateService>()).Object);
            e.For<INotificationService>().Use((_notificationService = new Mock<INotificationService>()).Object);
            e.For<IActiveMQServiceFactory>().Use(new Mock<IActiveMQServiceFactory>().Object);
            e.For<IDisplayItemService>().Use<DisplayItemService>();
            e.For<ILog>().Use((_log = new Mock<ILog>()).Object);
            e.For<IHtmlToPdfConverter>().Use(new Mock<IHtmlToPdfConverter>().Object);
            e.For<IMapResultFactory>().Use<MapResultFactory>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeTestForCreateAndUpdateTests = () => {
                // noop for intranet tests.
            };
        }

        protected void InitializeControllerForRequest(string virtualPath)
        {
            Request = Application.CreateRequestHandler(virtualPath);
            _target = Request.CreateAndInitializeController<TController>();
        }

        #endregion
    }

    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    public abstract class MapCallIntranetControllerTestBase<TController, TEntity> :
        MapCallIntranetControllerTestBase<TController, TEntity, RepositoryBase<TEntity>>
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
