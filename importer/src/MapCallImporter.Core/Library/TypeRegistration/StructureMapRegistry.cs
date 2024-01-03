using System;
using System.Diagnostics.CodeAnalysis;
using FluentNHibernate.Cfg;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Data.V2;
using MMSINC.Data.V2.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.AdoNet;
using StructureMap;
using StructureMap.Pipeline;
using MsSqlConfiguration = FluentNHibernate.Cfg.Db.MsSqlConfiguration;
using UnitOfWork = MMSINC.Data.V2.NHibernate.UnitOfWork;

namespace MapCallImporter.Library.TypeRegistration
{
    [ExcludeFromCodeCoverage]
    public class StructureMapRegistry : StructureMapRegistryBase
    {
        #region Constants

        public const string CONNECTION_STRING_KEY = "Main";

        #endregion

        #region Constructors

        public StructureMapRegistry()
        {
            For(typeof(MMSINC.Data.V2.IRepository<>)).Use(typeof(Repository<>));
            For<IUnitOfWorkFactory>().Use<Data.UnitOfWorkFactory>();
            For<ISessionFactory>().Singleton().Use(ctx => CreateSessionFactory(ctx));
            For<IDateTimeProvider>().Use<DateTimeProvider>();
            For<IAssemblyInfoService>().Use(_ => new AssemblyInfoService());
            For<IServiceRepository>().Use<ServiceRepository>();

            Profile(nameof(UnitOfWork),
                p => p.For<ISession>().LifecycleIs<ContainerLifecycle>()
                      .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession()));
        }

        #endregion

        #region Private Methods

        private ISessionFactory CreateSessionFactory(IContext context)
        {
            try
            {
                var config = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c =>
                        c.FromConnectionStringWithKey(CONNECTION_STRING_KEY)))
                    .Mappings(m => {
                        m.HbmMappings.AddFromAssemblyOf<NotificationConfiguration>();
                        m.FluentMappings
                         .AddFromAssemblyOf<NotificationConfiguration>()
                         .AddDynamicMapping<NotificationConfiguration>()
                         .Conventions.AddFromAssemblyOf<NotificationConfiguration>();
                    })
                    .ExposeConfiguration(c => c.DataBaseIntegration(prop => {
                        prop.BatchSize = 100;
                        prop.Batcher<SqlClientBatchingBatcherFactory>();
                    }))
                    .BuildConfiguration();
                config.SetInterceptor(context.GetInstance<ChangeTrackingInterceptor<User>>());
                config.AddAuxiliaryDatabaseObjectsInAssemblyOf<NotificationConfiguration>();
                return config.BuildSessionFactory();
            }
            catch (FluentConfigurationException e)
            {
                throw new Exception($"Potential Reasons:\n{string.Join("\n", e.PotentialReasons)}");
            }
        }

        #endregion
    }
}