using System;
using System.Security.Principal;
using Contractors.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Testing;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;
using ControllerBase = MMSINC.Controllers.ControllerBase;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace Contractors.Tests.Controllers
{
    public abstract class ContractorControllerTestBase<TController, TEntity, TRepository> :
        ControllerTestBase<MvcApplication, TController, TEntity, TRepository>
        where TController : ControllerBase
        where TEntity : class, new()
        where TRepository : class, IRepository<TEntity>
    {
        #region Private Members

        private ContractorAuthorizationTester _authTester;
        protected MockAuthenticationService<ContractorUser> _authenticationService;
        protected ContractorUser _currentUser;

        /// <summary>
        /// OperatingCenter created and used by anything the automatic controller tests
        /// needs it for. This is added to the _currentUser.Contractor.OperatingCenters list.
        /// </summary>
        protected OperatingCenter _automatedTestOperatingCenter;
        protected Mock<IPrincipal> _principalMock;
        protected Mock<IIdentity> _identityMock;
        protected TestDateTimeProvider _dateTimeProvider;
        protected DateTime _now;

        #endregion

        #region Properties

        protected ContractorAuthorizationTester Authorization =>
            _authTester ?? (_authTester =
                _container
                   .With(CreateFactoryService())
                   .GetInstance<ContractorAuthorizationTester>());
        
        #endregion

        #region Private Methods

        protected override IInMemoryDatabaseTestInterceptor CreateInterceptor() =>
            _container.GetInstance<ContractorsInMemoryDatabaseTestInterceptorWithChangeTracking>();

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container
                  .With(typeof(AssetTypeFactory).Assembly)
                  .GetInstance<TestDataFactoryService>();
        }
            
        protected virtual ContractorUser CreateUser()
        {
            return GetEntityFactory<ContractorUser>().Create();
        }

        [TestInitialize]
        public void ContractorsControllerTestBaseTestInitialize()
        {
            _currentUser = CreateUser();
            _principalMock.Setup(x => x.Identity).Returns((_identityMock = new Mock<IIdentity>()).Object);
            _authenticationService =
                (MockAuthenticationService<ContractorUser>)_container
                   .GetInstance<IAuthenticationService<ContractorUser>>();
            _authenticationService.SetUser(_currentUser);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeTestForCreateAndUpdateTests = () => {
                // All tests initialize by setting the current user with whatever CreateUser() returns.
                // For the automatic controller tests, due to the validation check, we need to use a site
                // admin user. Except we can't do this in contractors because there's almost zero use of
                // the IsAdmin flag. Repositories *all* filter by operating center no matter what. Most 
                // tests are going to need overrides to set the OperatingCenter property so it matches
                // what CurrentUser.Contractor.OperatingCenters has.

                // Several tests in Contractors already setup the operating center, but it's not done in 
                // a consistent manner. If they're already setup, those tests can also set the _automatedTestOperatingCenter
                // field and we'll just use that instead.
                if (_automatedTestOperatingCenter == null)
                { 
                    _automatedTestOperatingCenter = GetEntityFactory<OperatingCenter>().Create();
                    _currentUser.Contractor.OperatingCenters.Add(_automatedTestOperatingCenter);
                    Session.Save(_currentUser);
                    Session.Flush();
                }
            };
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            ContractorsDependencies.RegisterRepositories(e);
            e.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
            _principalMock = e.For<IPrincipal>().Mock();
            e.For<IDateTimeProvider>()
             .Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now))
             .Singleton();
            e.For<IAuthenticationService<ContractorUser>>().Singleton()
             .Use<MockAuthenticationService<ContractorUser>>();
            e.For<IAuthenticationService>().Use(ctx =>
                ctx.GetInstance<IAuthenticationService<ContractorUser>>());
            e.For<IAuthenticationCookieFactory>().Use<AuthenticationCookieFactory>();
            e.For<IDisplayItemService>().Use<DisplayItemService>();
            e.For<IHtmlToPdfConverter>().Mock();
            e.For<IIconSetRepository>().Mock();
        }

        protected void InitializeControllerForRequest(string virtualPath)
        {
            Request = Application.CreateRequestHandler(virtualPath);
            _target = Request.CreateAndInitializeController<TController>();
        }

        #endregion
    }

    public abstract class ContractorControllerTestBase<TController, TEntity>
        : ContractorControllerTestBase<TController, TEntity, RepositoryBase<TEntity>>
        where TController : ControllerBase
        where TEntity : class, new()
    {
        #region Private Methods

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(ContractorUserFactory).Assembly)
                             .GetInstance<TestDataFactoryService>();
        }

        #endregion
    }

    [Obsolete("Any tests using this should be using the version that inherits from ControllerTestBase.")]
    public abstract class ContractorsControllerTestBase<TEntity, TRepository>
        : InMemoryDatabaseTest<TEntity, TRepository>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class
    {
        protected MockAuthenticationService<ContractorUser>
            _authenticationService;

        protected IViewModelFactory _viewModelFactory;

        protected ContractorUser _currentUser;

        protected Mock<IDateTimeProvider> _dateTimeProvider;

        protected virtual ContractorUser CreateUser()
        {
            return GetEntityFactory<ContractorUser>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            ContractorsDependencies.RegisterRepositories(i);
            i.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
            i.For<IAuthenticationService<ContractorUser>>().Singleton().Use<MockAuthenticationService<ContractorUser>>();
            i.For<IAuthenticationService>().Use(ctx =>
                ctx.GetInstance<IAuthenticationService<ContractorUser>>());
            i.For<IViewModelFactory>().Use<ViewModelFactory>();
            _dateTimeProvider = i.For<IDateTimeProvider>().Mock();
        }

        [TestInitialize]
        public void ContractorsControllerTestBaseTestInitialize()
        {
            _authenticationService = (MockAuthenticationService<ContractorUser>)_container
               .GetInstance<IAuthenticationService<ContractorUser>>();
            _authenticationService.SetUser(CreateUser());
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();
            _currentUser = CreateUser();
            _authenticationService.SetUser(_currentUser);
        }
    }

    [Obsolete("Any tests using this should be using the version that inherits from ControllerTestBase.")]
    public abstract class ContractorsControllerTestBase<TAssemblyOf>
        : ContractorsControllerTestBase<TAssemblyOf, RepositoryBase<TAssemblyOf>>
        where TAssemblyOf : class { }
}
