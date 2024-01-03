using log4net;
using MapCall.Common.Configuration;
using MapCall.Common.Helpers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility;
using MapCall.Common.Utility.Notifications;
using MMSINC.Authentication;
using MMSINC.Configuration;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.ActiveMQ;
using MMSINC.Utilities.Pdf;
using MMSINC.Utilities.Permissions;
using MMSINC.Utilities.StructureMap;
using RazorEngine.Templating;
using StructureMap;
using StructureMap.Web;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using MapCallIntranet.Services;
using MMSINC.Utilities.Documents;

namespace MapCallIntranet.Configuration
{
    public class DependencyRegistrar : DependencyRegistrar<User, User>
    {
        #region Private Methods

        protected override void RegisterModels(ConfigurationExpression i)
        {
            i.For<IAuthenticationRepository<User>>().Use<AuthenticationRepository>();
            i.For<IAssetCoordinateService>().Use<AssetCoordinateService>();
            i.For<IModuleRepository>().Use<ModuleRepository>();
            i.For<IAuthenticationLogRepository<AuthenticationLog, User>>().Use<MapCallAuthenticationLogRepository>();

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
            i.For<IMapResultFactory>().Use<MapResultFactory>();
            i.For<ITemplateService>().Use(_ => new TemplateService()).Singleton();
            i.For<INotifier>().Use<RazorNotifier>();
            i.For<IPermissionsObjectFactory>().Use<PermissionsObjectFactory>();
            i.For<IViewModelFactory>().Use<ViewModelFactory>();
            i.For<IImageToPdfConverter>().Use<ImageToPdfConverter>();
            i.For<IUnitOfWorkFactory>().Use<UnitOfWorkFactory>();

            i.For<IUrlHelper>().Use(() => new MyUrlHelper(HttpContext.Current.Request.RequestContext));
            i.For<IPermissionsObjectFactory>().Use<PermissionsObjectFactory>();

            i.For<HttpContextBase>().Use(ctx => new System.Web.HttpContextWrapper(HttpContext.Current));

            i.For<IViewModelFactory>().Use<ViewModelFactory>();
            i.For<IDisplayItemService>().Use<DisplayItemService>();
            i.For<IViewPageActivator>().Use<StructureMapViewPageActivator>();

            i.For<IDocumentService>().Use<DocumentService>();
            i.For<IDocumentDataRepository>().Use<DocumentDataRepository>();
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
