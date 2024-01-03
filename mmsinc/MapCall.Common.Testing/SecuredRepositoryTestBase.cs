using MapCall.Common.Testing.Utilities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCall.Common.Testing
{
    public class SecuredRepositoryTestBase<TEntity, TRepository, TUser> : InMemoryDatabaseTest<TEntity, TRepository>
        where TEntity : class
        where TRepository : SecuredRepositoryBase<TEntity, TUser>
        where TUser : class, IAdministratedUser, new()
    {
        private MockAuthenticationService<TUser> _authenticationService;
        private TUser _user;

        public MockAuthenticationService<TUser> AuthenticationService
        {
            get => _authenticationService ??
                   (AuthenticationService = new MockAuthenticationService<TUser>(User));
            private set => _authenticationService = value;
        }

        public TUser User
        {
            get => _user ?? (User = CreateUser());
            set => _user = value;
        }

        protected virtual TUser CreateUser()
        {
            return GetEntityFactory<TUser>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IAuthenticationService<TUser>>().Singleton().Use(_ => AuthenticationService.Object);
            i.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<TUser>>());
        }
    }

    public class
        MapCallMvcSecuredRepositoryTestBase<TEntity, TRepository, TUser> : SecuredRepositoryTestBase<TEntity,
            TRepository, TUser>
        where TEntity : class
        where TRepository : SecuredRepositoryBase<TEntity, TUser>
        where TUser : class, IAdministratedUser, new()
    {
        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(Data.ActionFactory).Assembly).GetInstance<TestDataFactoryService>();
        }
    }
}
