using System;
using System.Collections.Specialized;
using System.Configuration;
using FluentNHibernate.Cfg;
using Historian.Data.Client.Repositories;
using log4net;
using log4net.Config;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.AssetUploads;
using MapCall.Common.Utility.Notifications;
using MapCall.LIMS.Client;
using MapCall.LIMS.Configuration;
using MapCall.SAP.Model.Repositories;
using MapCallImporter.Library;
using MapCallImporter.Library.Excel;
using MapCallScheduler.JobHelpers.GISMessageBroker;
using MapCallScheduler.Library.Data.NHibernate;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Library.Quartz;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Common;
using MMSINC.Data.NHibernate;
using MMSINC.Data.V2;
using MMSINC.Data.V2.NHibernate;
using MMSINC.Interface;
using MMSINC.Utilities;
using MMSINC.Utilities.ActiveMQ;
using MMSINC.Utilities.APIM;
using MMSINC.Utilities.Documents;
using MMSINC.Utilities.ErrorHandling;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Consumer;
using MMSINC.Utilities.Kafka.Producer;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Utilities.Pdf;
using NHibernate;
using NHibernate.AdoNet;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using RazorEngine.Templating;
using StructureMap;
using StructureMap.Pipeline;
using UnitOfWorkFactory = MapCallScheduler.Library.Data.NHibernate.UnitOfWorkFactory;

namespace MapCallScheduler
{
    public class DependencyRegistry : Registry
    {
        #region Constants

        public const string DEFAULT_CONNECTION_STRING = "Main",
                            REGISTER_ISESSION = "SchedulerJob - RegisterISession",
                            NO_REGISTER_ISESSION = "SchedulerJob - NoRegisterISession",
                            UNIT_OF_WORK = MMSINC.Data.V2.NHibernate.UnitOfWorkFactory.CONTAINER_PROFILE;

        #endregion

        #region Properties

        public static bool IsInTestMode { get; set; }

        public static DatabaseConfiguration DatabaseConfiguration
        {
            get
            {
                return IsInTestMode
                    ? (DatabaseConfiguration)new SQLiteConfiguration()
                    : new MsSql2008Configuration(DEFAULT_CONNECTION_STRING);
            }
        }

        #endregion

        #region Constructors

        public DependencyRegistry()
        {
            XmlConfigurator.Configure();

            // Quartz.net
            For<IJobFactory>().Use<MapCallJobFactory>();
            For<IScheduler>().Use(ctx => CreateScheduler(ctx.GetInstance<IJobFactory>()));

            // MapCall IDateTimeProvider
            For<IDateTimeProvider>().Use<DateTimeProvider>();
            // log4net
            For<ILog>().Use(ctx =>
                ctx.ParentType == null
                    ? LogManager.GetLogger("MapCallScheduler.UnknownLogger")
                    : LogManager.GetLogger(ctx.ParentType));

            // NHibernate
            For<ISessionFactory>()
               .Singleton()
               .Use(ctx => CreateSessionFactory(ctx));

            void RegisterISessionForProfile(string profile, ILifecycle lifecycle)
            {
                Profile(profile,
                    p => p.For<ISession>()
                          .LifecycleIs(lifecycle)
                          .Use(ctx => ctx.GetInstance<ISessionFactory>()
                                         .OpenSession(ctx.GetInstance<ChangeTrackingInterceptor>())));
            }

            RegisterISessionForProfile(REGISTER_ISESSION, new HybridHttpQuartzLifecycle());
            RegisterISessionForProfile(UNIT_OF_WORK, new ContainerLifecycle());

            // MapCall ErrorEmailer
            For<IErrorMessageGenerator>().Use<ErrorMessageGenerator>();

            // MapCall Notifier
            For<ISmtpClientFactory>().Use<SmtpClientFactory>();
            For<ISmtpClient>().Use(ctx => ctx.GetInstance<ISmtpClientFactory>().Build());
            For<INotifier>().Use<RazorNotifier>();
            For<INotificationService>().Use<NotificationService>();

            // needs to be a singleton so that a single cache can be built
            For<ITemplateService>().Use(_ => new TemplateService()).Singleton();

            // everything from MapCallScheduler.Core (which meets the default
            // StructureMap conventions)
            Scan(s => {
                s.AssemblyContainingType<MapCallSchedulerConfiguration>();
                s.WithDefaultConventions();
            });

            Scan(s => {
                s.AssemblyContainingType<IRawDataRepository>();
                s.WithDefaultConventions();
            });

            Scan(s => {
                s.AssemblyContainingType<IExcelImportFactory>();
                s.WithDefaultConventions();
            });

            // for AssetUploadProcessor
            For<IAssetUploadFileService>().Use<AssetUploadFileService>();
            For<IObjectMapperFactory>().Use<ObjectMapperFactory>();
            For<IUnitOfWorkFactory>().Use<UnitOfWorkFactory>();
            For(typeof(MMSINC.Data.V2.IRepository<>)).Use(typeof(Repository<>));
            For<IAuthenticationService<User>>().Use<DangerousAuthenticationService>();

            // for whatever reason, scanning doesn't pick this one up
            For<IWrappedImapClient>().Use<WrappedImapClient>();

            MapCallDependencies.RegisterRepositories(this);

            For<MMSINC.Data.NHibernate.IRepository<WorkOrder>>().Use<MMSINC.Data.NHibernate.RepositoryBase<WorkOrder>>();

            // SAP Integration
            For<ISAPWorkOrderStatusUpdateRepository>().Use<SAPWorkOrderStatusUpdateRepository>();
            For<ISAPCreateUnscheduledWorkOrderRepository>().Use<SAPCreateUnscheduledWorkOrderRepository>();
            For<ISAPNewServiceInstallationRepository>().Use<SAPNewServiceInstallationRepository>();
            For<ISAPEquipmentRepository>().Use<SAPEquipmentRepository>();
            For<ISAPInspectionRepository>().Use<SAPInspectionRepository>();
            For<ISAPWorkOrderRepository>().Use<SAPWorkOrderRepository>();
            For<ISAPCreatePreventiveWorkOrderRepository>().Use<SAPCreatePreventiveWorkOrderRepository>();
            For<ISAPShortCycleWorkOrderRepository>().Use<SAPShortCycleWorkOrderRepository>();
            For<ISAPNotificationRepository>().Use<SAPNotificationRepository>();
            For<ISAPHttpClient>().Use(() => new SAPHttpClient {
                UserName = ConfigurationManager.AppSettings.EnsureValue("SAPWebServiceUserName"),
                Password = ConfigurationManager.AppSettings.EnsureValue("SAPWebServicePassword"),
                BaseAddress = new Uri(ConfigurationManager.AppSettings.EnsureValue("SAPWebServiceUrl"))
            });
            For<MMSINC.Data.IUnitOfWorkFactory>().Use<MMSINC.Data.NHibernate.UnitOfWorkFactory>();
            For<MMSINC.Data.NHibernate.IRepository<ProductionWorkOrder>>()
               .Use<MMSINC.Data.NHibernate.RepositoryBase<ProductionWorkOrder>>();
            For<MMSINC.Data.NHibernate.IRepository<AsBuiltImage>>().Use<MMSINC.Data.NHibernate.RepositoryBase<AsBuiltImage>>();
            For<MMSINC.Data.NHibernate.IRepository<GasMonitor>>().Use<MMSINC.Data.NHibernate.RepositoryBase<GasMonitor>>();
            For<MMSINC.Data.NHibernate.IRepository<Hydrant>>().Use<MMSINC.Data.NHibernate.RepositoryBase<Hydrant>>();
            For<MMSINC.Data.NHibernate.IRepository<HydrantInspection>>().Use<MMSINC.Data.NHibernate.RepositoryBase<HydrantInspection>>();
            For<MMSINC.Data.NHibernate.IRepository<Valve>>().Use<MMSINC.Data.NHibernate.RepositoryBase<Valve>>();
            For<MMSINC.Data.NHibernate.IRepository<ValveInspection>>().Use<MMSINC.Data.NHibernate.RepositoryBase<ValveInspection>>();
            For<MMSINC.Data.NHibernate.IRepository<BlowOffInspection>>().Use<MMSINC.Data.NHibernate.RepositoryBase<BlowOffInspection>>();
            For<MMSINC.Data.NHibernate.IRepository<SewerOpening>>().Use<MMSINC.Data.NHibernate.RepositoryBase<SewerOpening>>();
            For<MMSINC.Data.NHibernate.IRepository<SewerMainCleaning>>().Use<MMSINC.Data.NHibernate.RepositoryBase<SewerMainCleaning>>();
            For<MMSINC.Data.NHibernate.IRepository<NotificationPurpose>>().Use<MMSINC.Data.NHibernate.RepositoryBase<NotificationPurpose>>();
            For<IActiveMQConfiguration>().Use<WorkOrdersConfiguration>();
            For<IActiveMQServiceFactory>().Use<ApacheActiveMQServiceFactory>();

            //ShortCycle
            For<IActiveMQServiceFactory>().Use<ApacheActiveMQServiceFactory>();
            For<IActiveMQConfiguration>().Use<WorkOrdersConfiguration>();
            For<ISecureAuthClient>().Use<SecureAuthClient>();
            For<ISecureAuthClientFactory>().Use<SecureAuthClientFactory>();
            For<IHttpClientFactory>().Use<HttpClientFactory>();
            For<ISecureAuthHttpClientFactory>().Use<SecureAuthHttpClientFactory>();

            // Kafka
            For<IKafkaServiceFactory<IKafkaProducer>>().Use<KafkaServiceFactory<KafkaProducer>>();
            For<IKafkaServiceFactory<IKafkaConsumer>>().Use<KafkaServiceFactory<KafkaConsumer>>();
            For<IGISMessageBrokerConfiguration>().Use<GISMessageBrokerConfiguration>();

            // APIM Integration
            For<IAPIMClientFactory>().Use<APIMClientFactory>();

            // Individual API Integration - LIMS
            For<ILIMSClientConfiguration>().Use<LIMSClientConfiguration>();
            For<ILIMSApiClient>().Use<LIMSApiClient>().Singleton();

            // Needed because of the StructureMapInterceptor/ChangeTrackingInterceptor
            // calling container.BuildUp on entities.
            For<IImageToPdfConverter>().Use<ImageToPdfConverter>();

            // Documents
            For<IDocumentService>().Use<DocumentService>();
        }

        #endregion

        #region Private Methods

        protected virtual IScheduler CreateScheduler(IJobFactory jobFactory)
        {
            var properties = new NameValueCollection {
                {
                    "quartz.threadPool.threadCount",
                    ConfigurationManager.AppSettings.ContainsKey("QUARTZ_THREAD_COUNT")
                        ? ConfigurationManager.AppSettings["QUARTZ_THREAD_COUNT"]
                        : "20"
                }
            };
            var factory = new StdSchedulerFactory(properties);
            var scheduler = factory.GetScheduler();
            scheduler.Result.JobFactory = jobFactory;
            return scheduler.Result;
        }

        protected virtual ISessionFactory CreateSessionFactory(IContext context)
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
            // MC-5680: Don't assign an interceptor here. This will cause random places to
            // start looking for ISession in the default container rather than in the nested/profiled
            // containers.
            return config.BuildSessionFactory();
        }

        #endregion
    }
}
