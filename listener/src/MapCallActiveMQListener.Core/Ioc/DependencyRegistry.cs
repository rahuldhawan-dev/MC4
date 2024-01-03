using System;
using System.Configuration;
using FluentNHibernate.Cfg;
using log4net.Config;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model;
using MapCall.SAP.Model.Repositories;
using MapCallActiveMQListener.Library;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Common;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using MMSINC.Utilities;
using NHibernate;
using RazorEngine.Templating;
using StructureMap;
using StructureMap.Web;

namespace MapCallActiveMQListener.Ioc
{
    public class DependencyRegistry : DependencyRegistryBase
    {
        #region Constants

        public const string DEFAULT_CONNECTION_STRING = "Main";

        #endregion

        #region Properties

        public static DatabaseConfiguration DatabaseConfiguration => new MsSql2008Configuration(DEFAULT_CONNECTION_STRING);

        #endregion

        #region Constructors

        public DependencyRegistry()
        {
            XmlConfigurator.Configure();

            For<ISAPWorkOrderStatusUpdateRepository>().Use<SAPWorkOrderStatusUpdateRepository>();
            For<ISAPNewServiceInstallationRepository>().Use<SAPNewServiceInstallationRepository>();
            For<ISAPHttpClient>().Use(() => new SAPHttpClient {
                UserName = ConfigurationManager.AppSettings.EnsureValue("SAPWebServiceUserName"),
                Password = ConfigurationManager.AppSettings.EnsureValue("SAPWebServicePassword"),
                BaseAddress = new Uri(ConfigurationManager.AppSettings.EnsureValue("SAPWebServiceUrl"))
            });
            For<IDateTimeProvider>().Use<DateTimeProvider>();
            For<IUnitOfWorkFactory>().Use<UnitOfWorkFactory>();
            For<ISmtpClientFactory>().Use<SmtpClientFactory>();
            For<ISmtpClient>().Use(ctx => ctx.GetInstance<ISmtpClientFactory>().Build());
            For<INotificationService>().Use<NotificationService>();
            For<INotifier>().Use<RazorNotifier>();
            // needs to be a singleton so that a single cache can be built
            For<ITemplateService>().Use(_ => new TemplateService()).Singleton();
            // NHibernate
            For<ISessionFactory>()
                .Singleton()
                .Use(ctx => CreateSessionFactory(ctx));
            For<ISession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());
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
                         .Conventions.AddFromAssemblyOf<TownMap>();
                     conf.FluentMappings.AddDynamicMapping<NotificationConfiguration>();
                     conf.MergeMappings();
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
