using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using Moq;
using NHibernate;
using StructureMap;
using NHibernateConfig = NHibernate.Cfg.Configuration;

namespace MMSINC.Testing.NHibernate
{
    /// <typeparam name="TAssemblyOf">
    /// Type who's assembly should be used to load the NHibernate model/map.
    /// </typeparam>
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    public class InMemoryDatabaseTest<TAssemblyOf>
    {
        #region Private Members

        private static readonly ConcurrentDictionary<string, IInMemTestConfig> _testConfigDictionary =
            new ConcurrentDictionary<string, IInMemTestConfig>();

        protected IContainer _container;
        private IInMemoryDatabaseTestInterceptor _interceptor;

        #endregion

        #region Properties

        public IContainer Container => _container;

        public ISession Session => _container.GetInstance<ISession>();
        protected IStatelessSession StatelessSession => _container.GetInstance<IStatelessSession>();

        protected IInMemoryDatabaseTestInterceptor Interceptor =>
            _interceptor ?? (_interceptor = CreateInterceptor());

        private IInMemTestConfig _testConfig;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InMemoryDatabaseTestInitialize()
        {
            _container = new Container(i => {
                i.For<DbConnection>().Singleton().Use(() => _testConfig.OpenConnection());
                i.For<IInterceptor>().Singleton()
                 .Use(ctx => _testConfig.Interceptor);
                i.For<IInMemoryDatabaseTestInterceptor>().Singleton().Use(_ => Interceptor);
                i.For<ISession>().Singleton().Use(ctx => GetSession(ctx));
                i.For<IStatelessSession>().Singleton().Use(ctx => GetStatelessSession(ctx));
                i.For<ISessionFactory>().Use(_ => _testConfig.SessionFactory);
                InitializeObjectFactory(i);
                i.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
                i.For<ITestDataFactoryService>().Singleton().Use(() => CreateFactoryService());
            });

            _testConfig = LoadTestConfig();
        }

        [TestCleanup]
        public void InMemoryDatabaseTestCleanup()
        {
            TestDataFactory.ResetAll();
            Session?.Dispose();
            StatelessSession?.Dispose();

            var connection = _container.GetInstance<DbConnection>();
            if (connection?.State == ConnectionState.Open)
            {
                connection?.Close();
            }

            connection?.Dispose();

            _container?.Dispose();
        }

        #endregion

        #region Private Methods

        protected virtual IInMemoryDatabaseTestInterceptor CreateInterceptor()
        {
            return _container.GetInstance<InMemoryDatabaseTestInterceptor>();
        }

        protected virtual void InitializeObjectFactory(ConfigurationExpression e)
        {
            // noop
        }

        private IInMemTestConfig LoadTestConfig()
        {
            var assemblyName = typeof(TAssemblyOf).Assembly.FullName;

            return _testConfigDictionary.ContainsKey(assemblyName)
                ? _testConfigDictionary[assemblyName]
                : _testConfigDictionary[assemblyName] =
                    InMemoryDatabaseTestUtility.GetConfig<TAssemblyOf>(Interceptor);
        }

        protected ISession GetSession(IContext ctx)
        {
            return new SessionWrapper(ctx.GetInstance<ISessionFactory>()
                                         .WithOptions()
                                         .Connection(ctx.GetInstance<DbConnection>())
                                         .Interceptor(Interceptor)
                                         .OpenSession());
        }

        private IStatelessSession GetStatelessSession(IContext ctx)
        {
            return ctx.GetInstance<ISessionFactory>()
                      .OpenStatelessSession(ctx.GetInstance<DbConnection>());
        }

        /// <summary>
        /// To be overridden should a different implementation of ITestDataFactory be required.
        /// </summary>
        protected virtual ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(GetType().Assembly).GetInstance<TestDataFactoryService>();
        }

        /// <summary>
        /// Use this only when you need a specific factory like UniqueOperatingCenterFactory
        /// otherwise GetEntityFactory&lt;TEntity&gt; is a better choice.
        /// </summary>
        public TFactory GetFactory<TFactory>()
            where TFactory : TestDataFactory
        {
            return _container.GetInstance<ITestDataFactoryService>().GetFactory<TFactory>();
        }

        public TestDataFactory<TEntity> GetEntityFactory<TEntity>()
            where TEntity : class, new()
        {
            return _container.GetInstance<ITestDataFactoryService>().GetEntityFactory<TEntity>();
        }

        public TEntity CreateEntity<TEntity>(object overrides = null)
            where TEntity : class, new()
        {
            return GetEntityFactory<TEntity>().Create(overrides);
        }

        public TEntity BuildEntity<TEntity>(object overrides = null)
            where TEntity : class, new()
        {
            return GetEntityFactory<TEntity>().Build(overrides);
        }

        public TestDataFactory GetEntityFactory(Type entityType)
        {
            return _container.GetInstance<ITestDataFactoryService>().GetEntityFactory(entityType);
        }

        #endregion
    }

    public class InMemoryDatabaseTest<TEntity, TRepository> : InMemoryDatabaseTest<TEntity>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class
    {
        private TRepository _repository;

        #region Properties

        public TRepository Repository
        {
            get => _repository ?? (Repository = CreateRepository());
            set => _repository = value;
        }

        #endregion

        #region Protected Methods

        protected TRepository CreateRepository()
        {
            return _container.GetInstance<TRepository>();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            if (typeof(TRepository) == typeof(IRepository<>).MakeGenericType(typeof(TEntity)))
            {
                return;
            }

            e.For<IRepository<TEntity>>().Singleton().Use<TRepository>();
            foreach (var type in typeof(TRepository).GetInterfaces()
                                                    .Where(iface =>
                                                         iface.GetInterfaces().Contains(typeof(IRepository<TEntity>))))
            {
                e.For(type).Use(ctx => ctx.GetInstance<IRepository<TEntity>>());
            }
        }

        #endregion
    }
}
