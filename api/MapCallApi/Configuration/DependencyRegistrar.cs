using System;
using System.Security.Principal;
using System.Web;
using log4net;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility;
//using MapCall.Common.Utility;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Configuration;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.ActiveMQ;
using MMSINC.Utilities.Pdf;
using MMSINC.Utilities.Permissions;
using NHibernate.Event;
using RazorEngine.Templating;
using StructureMap;
using StructureMap.Web;

namespace MapCallApi.Configuration
{
    public class DependencyRegistrar : DependencyRegistrar<User, User>
    {
        #region Private Methods

        protected override void RegisterModels(ConfigurationExpression i)
        {
            i.For<IAuthenticationRepository<User>>().Use<AuthenticationRepository>();
            i.For<IModuleRepository>().Use<ModuleRepository>();
            i.For<IAuthenticationLogRepository<AuthenticationLog, User>>().Use<MapCallAuthenticationLogRepository>();
            i.For<ISAPDeviceRepository>().Use<SAPDeviceRepository>();

            MapCallDependencies.RegisterRepositories(i);
        }

        protected override void RegisterUtilities(ConfigurationExpression i)
        {
            i.For<IAuthenticationService<User>>()
                .HybridHttpOrThreadLocalScoped()
                .Use<AuthenticationService>();
            i.For<IAuthenticationService<IAdministratedUser>>()
                .Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            i.For<IAuthenticationService<IUserWithProfile>>()
                .Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            i.For<IAuthenticationService>()
                .Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            i.For<IDateTimeProvider>().Use<DateTimeProvider>();
            i.For<IRoleService>()
                .HybridHttpOrThreadLocalScoped()
                .Use<RoleService>();
            i.For<IBasicRoleService>()
                .Use(ctx => ctx.GetInstance<IRoleService>());
            i.For<IAuthenticationCookieFactory>().Use<MapCallAuthenticationCookieFactory<User>>();
            i.For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));
            i.For<IMembershipHelper>().Use<MembershipHelper>();
            i.For<IActiveMQServiceFactory>().Use<ApacheActiveMQServiceFactory>();
            i.For<IAuditLogEntryRepository>().Use<AuditLogEntryRepository>();
            i.For<ILog>()
                .AlwaysUnique()
                .Use(ctx => LogManager.GetLogger(ctx.ParentType == null ? ctx.RequestedName : ctx.ParentType.Name));
            i.For<IDisplayItemService>().Use<DisplayItemService>();
            i.For<IExtendedSAPHttpClientConfiguration>().Use<SAPHttpClientConfiguration>();
            i.For<ISAPHttpClient>().Use(ctx => ctx.GetInstance<ExtendedSAPHttpClientFactory>().Build());
            i.For<ISAPWorkOrderStatusUpdateRepository>().Use<SAPWorkOrderStatusUpdateRepository>();
            i.For<ISAPNewServiceInstallationRepository>().Use<SAPNewServiceInstallationRepository>();
            i.For<ISAPShortCycleWorkOrderRepository>().Use<SAPShortCycleWorkOrderRepository>();
            i.For<INotificationService>().Use<NotificationService>();
            i.For<ITemplateService>().Use(_ => new TemplateService()).Singleton();
            i.For<INotifier>().Use<RazorNotifier>();
            i.For<IPermissionsObjectFactory>().Use<PermissionsObjectFactory>();
            i.For<IViewModelFactory>().Use<ViewModelFactory>();
            i.For<IUrlHelper>().Use<MapCallUrlHelper>();
            i.For<IImageToPdfConverter>().Use<ImageToPdfConverter>();
            i.For<IUnitOfWorkFactory>().Use<UnitOfWorkFactory>();
        }

        protected override Action<NHibernate.Cfg.Configuration> ConfigureSessionFactory(IContext container)
        {
            return cfg => {
                base.ConfigureSessionFactory(container)(cfg);
                cfg.EventListeners.FlushEventListeners = new IFlushEventListener[]
                    {new FixedDefaultFlushEventListener()};
                cfg.SetListener(ListenerType.PostInsert, container.GetInstance<AuditInsertListener>());
                cfg.SetListener(ListenerType.PostUpdate, container.GetInstance<AuditUpdateListener>());
                cfg.SetListener(ListenerType.PostDelete, container.GetInstance<AuditDeleteListener>());
            };
        }

        #endregion
    }

    public class AuthenticationService : AuthenticationServiceBase<User, AuthenticationLog>
    {
        #region Constructors

        public AuthenticationService(IContainer container, IPrincipal principal, IAuthenticationCookieFactory cookieFactory) : base(container, principal, cookieFactory) { }

        #endregion
    }
}
