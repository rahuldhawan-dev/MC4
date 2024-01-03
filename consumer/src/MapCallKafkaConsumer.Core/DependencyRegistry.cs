using MMSINC.Data.NHibernate;
using FluentNHibernate.Cfg;
using StructureMap;
using NHibernate;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions;
using log4net.Config;
using MMSINC.Data;
using StructureMap.Web;
using log4net;
using NHibernate.Cfg;
using NHibernate.AdoNet;
using StructureMap.Pipeline;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCallKafkaConsumer.Consumers.Ignition;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Producer;
using MMSINC.Utilities.Kafka.Consumer;
using MapCallKafkaConsumer.Consumers.Lims;
using MMSINC.Utilities;

namespace MapCallKafkaConsumer
{
    public class DependencyRegistry : Registry
    {
        #region Constants

        public const string DEFAULT_CONNECT_STRING = "Main";
        public const string UNIT_OF_WORK = MMSINC.Data.V2.NHibernate.UnitOfWorkFactory.CONTAINER_PROFILE;

        #endregion

        #region Properties

        public static bool IsInTestMode { get; set; }

        public static DatabaseConfiguration DatabaseConfiguration =>
            IsInTestMode ? (DatabaseConfiguration)new SQLiteConfiguration() : new MsSql2008Configuration(DEFAULT_CONNECT_STRING);

        #endregion

        #region Constructors

        public DependencyRegistry()
        {
            XmlConfigurator.Configure();

            For<IDateTimeProvider>().Use<DateTimeProvider>();

            // log4net
            For<ILog>().Use(context =>
                context.ParentType == null
                    ? LogManager.GetLogger("MapCallKafkaConsumer")
                    : LogManager.GetLogger(context.ParentType));

            // NHibernate
            For<IUnitOfWorkFactory>().Use<UnitOfWorkFactory>();
            For<ISessionFactory>().Singleton()
                                  .Use(ctx => CreateSessionFactory(ctx));

            For<ISession>().HybridHttpOrThreadLocalScoped()
                           .Use(context => context.GetInstance<ISessionFactory>().OpenSession());

            Profile(UNIT_OF_WORK,
                p => p.For<ISession>()
                      .LifecycleIs(new ContainerLifecycle())
                      .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession()));

            // everything from MapCallKafkaConsumer.Core (which meets the default
            // StructureMap conventions)
            Scan(s => {
                s.AssemblyContainingType<DependencyRegistry>();
                s.WithDefaultConventions();
            });

            // MapCall
            MapCallDependencies.RegisterRepositories(this);
            For<IRepository<SampleSite>>().Use<RepositoryBase<SampleSite>>();
            For<IRepository<SystemDeliveryEntry>>().Use<RepositoryBase<SystemDeliveryEntry>>();

            // Kafka
            For<IKafkaServiceFactory<IKafkaProducer>>().Use<KafkaServiceFactory<KafkaProducer>>();
            For<IKafkaServiceFactory<IKafkaConsumer>>().Use<KafkaServiceFactory<KafkaConsumer>>();
            For<ILimsConfiguration>().Use<LimsConfiguration>();
            For<IIgnitionConfiguration>().Use<IgnitionConfiguration>();
        }

        #endregion

        #region Private Methods

        private static ISessionFactory CreateSessionFactory(IContext context)
        {
            var config = Fluently.Configure()
                                 .Database(DatabaseConfiguration.Configuration)
                                 .Mappings(conf => {
                                      conf.FluentMappings
                                          .AddFromAssemblyOf<TownMap>()
                                          .AddDynamicMapping<TownMap>()
                                          .Conventions.AddFromAssemblyOf<TownMap>();
                                      conf.MergeMappings();
                                  }).ExposeConfiguration(c => {
                                      c.AddAuxiliaryDatabaseObjectsInAssemblyOf<TownMap>();
                                      c.DataBaseIntegration(prop => {
                                          prop.BatchSize = 100;
                                          prop.Batcher<SqlClientBatchingBatcherFactory>();
                                      });
                                  }).BuildConfiguration();

            config.SetInterceptor(context.GetInstance<ChangeTrackingInterceptor<User>>());
            config.AddAuxiliaryDatabaseObjectsInAssemblyOf<TownMap>();

            return config.BuildSessionFactory();
        }

        #endregion

        #region Exposed Methods

        public static IContainer Initialize()
        {
            return new Container(new DependencyRegistry());
        }

        #endregion
    }
}
