using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.StructureMap;
using MMSINC.Validation;
using Moq;
using StructureMap;

namespace MapCall.Common.Testing
{
    /// <summary>
    /// This test base adds some common functionality that classes like controllers, helpers, and
    /// repositories often take advantage of.  It's not very specific to the mvc project, and is useful for
    /// testing things such as repositories in MapCall.Common. 
    /// </summary>
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    public abstract class MapCallMvcInMemoryDatabaseTestBase<TAssembly> : InMemoryDatabaseTest<TAssembly>
    {
        #region Private Members

        protected IViewModelFactory _viewModelFactory;
        protected Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void MapCallMvcInMemoryDatabaseTestBaseTestInitialize()
        {
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));

            _viewModelFactory = _container.GetInstance<IViewModelFactory>();

            ValidationAssert
               .RegisterValidatorAdapterWithDependencyInjection<EntityMustExistAttribute,
                    EntityMustExistAttributeAdapter>(_container);
            ValidationAssert
               .RegisterValidatorAdapterWithDependencyInjection<RequiredWhenAttribute, RequiredWhenAttributeAdapter>(
                    _container);
        }

        [TestCleanup]
        public void MapCallMvcInMemoryDatabaseTestBaseTestCleanup()
        {
            ValidationAssert.UnregisterValidatorAdapter<EntityMustExistAttribute>();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IViewModelFactory>().Use<ViewModelFactory>();
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(ActionFactory).Assembly).GetInstance<TestDataFactoryService>();
        }

        #endregion
    }

    /// <inheritdoc cref="MapCallMvcInMemoryDatabaseTestBase{TAssemblyOf}" />
    public abstract class
        MapCallMvcInMemoryDatabaseTestBase<TEntity, TRepository> : InMemoryDatabaseTest<TEntity, TRepository>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class
    {
        #region Private Members

        protected IViewModelFactory _viewModelFactory;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void MapCallMvcInMemoryDatabaseTestBaseTestInitialize()
        {
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();

            ValidationAssert
               .RegisterValidatorAdapterWithDependencyInjection<EntityMustExistAttribute,
                    EntityMustExistAttributeAdapter>(_container);
            ValidationAssert
               .RegisterValidatorAdapterWithDependencyInjection<RequiredWhenAttribute,
                    RequiredWhenAttributeAdapter>(_container);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
            e.For<IViewModelFactory>().Use<ViewModelFactory>();
        }

        [TestCleanup]
        public void MapCallMvcInMemoryDatabaseTestBaseTestCleanup()
        {
            ValidationAssert.UnregisterValidatorAdapter<EntityMustExistAttribute>();
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(ActionFactory).Assembly).GetInstance<TestDataFactoryService>();
        }

        #endregion
    }
}
