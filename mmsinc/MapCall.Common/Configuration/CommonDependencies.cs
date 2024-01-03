using System;
using FluentNHibernate.Cfg;
using MapCall.Common.Authentication;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility;
using MapCall.Common.Utility.Notifications;
using MMSINC.Authentication;
using MMSINC.Common;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using MMSINC.Utilities;
using MMSINC.Utilities.Documents;
using NHibernate;
using StructureMap;
using System.Security.Principal;
using System.Web;
using MapCall.Common.Model.Conventions;
using MMSINC.ClassExtensions;
using RazorEngine.Templating;
using StructureMap.Web;
using HttpContextWrapper = System.Web.HttpContextWrapper;
using MsSqlConfiguration = FluentNHibernate.Cfg.Db.MsSqlConfiguration;

namespace MapCall.Common.Configuration
{
    public class CommonDependencies
    {
        #region Private Static Methods

        private static ISessionFactory _nHibernateSessionFactory;

        #endregion

        #region Exposed Static Methods

        public static ISessionFactory NHibernateSessionFactory
        {
            get
            {
                return _nHibernateSessionFactory ??
                       (_nHibernateSessionFactory =
                           CreateNHibernateSessionFactory());
            }
        }

        public static IContainer Register(Action<ConfigurationExpression> fn = null)
        {
            return new Container(i => {
                RegisterModel(i);
                RegisterUtilities(i);
                if (fn != null)
                {
                    fn(i);
                }
            });
        }

        #endregion

        #region Private Static Methods

        protected static ISessionFactory CreateNHibernateSessionFactory()
        {
            try
            {
                return Fluently.Configure()
                               .Database(
                                    MsSqlConfiguration.MsSql2008.ConnectionString(
                                        c => c.FromConnectionStringWithKey("McProd")))
                               .Mappings(m => {
                                    m.HbmMappings.AddFromAssemblyOf<NotificationConfiguration>();
                                    m.FluentMappings.AddFromAssemblyOf<NotificationConfiguration>()
                                     .Conventions.AddFromAssemblyOf<MapCallConventions>();
                                    m.FluentMappings.AddDynamicMapping<NotificationConfiguration>();
                                })
                               .BuildSessionFactory();
            }
            catch (FluentConfigurationException e)
            {
                if (e.PotentialReasons.Count > 0)
                {
                    // if you're in 271, it's because you forgot the keyword virtual on a property
                    throw new Exception(String.Format("Potential Reasons:\n{0}",
                        String.Join("\n", e.PotentialReasons)));
                }

                // Just continue throwing the exception. The InnerException has extra info when unmapped entities get referenced.
                throw;
            }
        }

        private static void RegisterModel(ConfigurationExpression i)
        {
            i.For<IChangeTrackingInterceptor<User>>()
             .Use<ChangeTrackingInterceptor<User>>();
            i.For<ISessionFactory>()
             .Singleton()
             .Use(_ => CreateNHibernateSessionFactory());
            i.For<ISession>()
             .HybridHttpOrThreadLocalScoped()
             .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());
            i.For<ISession>()
             .HybridHttpOrThreadLocalScoped()
             .Use(ctx => new MMSINC.Data.NHibernate.SessionWrapper(
                  ctx.GetInstance<ISessionFactory>()
                     .OpenSession(ctx.GetInstance<ChangeTrackingInterceptor<User>>())));
            i.For<IModuleRepository>().Use<ModuleRepository>();
            i.For<IDocumentService>().Use<DocumentService>();
            i.For<IDocumentRepository>().Use<DocumentRepository>();
            i.For<IDocumentDataRepository>().Use<DocumentDataRepository>();
            i.For<IDocumentTypeRepository>().Use<DocumentTypeRepository>();
            i.For<IAuthenticationRepository<User>>().Use<AuthenticationRepository>();
            i.For<IIconSetRepository>().Use<IconSetRepository>();
            i.For<IRepository<MapIcon>>().Use<RepositoryBase<MapIcon>>();
            i.For<IRepository<NotificationPurpose>>().Use<RepositoryBase<NotificationPurpose>>();
            i.For<IAuthenticationLogRepository<AuthenticationLog, User>>().Use<MapCallAuthenticationLogRepository>();
            i.For<IAuditLogEntryRepository>().Use<AuditLogEntryRepository>();
            i.For<IActionItemTypeRepository>().Use<ActionItemTypeRepository>();
        }

        private static void RegisterUtilities(ConfigurationExpression i)
        {
            i.For<IPrincipal>()
             .HybridHttpOrThreadLocalScoped()
             .Use(() => HttpContext.Current.User);
            i.For<IAuthenticationService<User>>()
             .HybridHttpOrThreadLocalScoped()
             .Use<MapCallAuthenticationService>();
            i.For<IAuthenticationService>()
             .Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            i.For<IUrlHelper>()
             .Use(new MapCallUrlHelper());
            i.For<IAuthenticationCookieFactory>()
             .Use<MapCallAuthenticationCookieFactory<User>>();
            i.For<IBasicRoleService>()
             .HybridHttpOrThreadLocalScoped()
             .Use<BasicRoleService>();
            i.For<IDateTimeProvider>().Use<DateTimeProvider>();
            i.For<INotifier>().Use<RazorNotifier>();
            i.For<INotificationService>().Use<NotificationService>();
            // needs to be a singleton so that a single cache can be built
            i.For<ITemplateService>().Use(_ => new TemplateService()).Singleton();
            i.For<INotificationConfigurationRepository>()
             .Use<NotificationConfigurationRepository>();
            i.For<ISmtpClientFactory>().Use<SmtpClientFactory>();
            i.For<IMailMessageFactory>().Use<MailMessageFactory>();
            i.For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));
        }

        #endregion
    }
}
