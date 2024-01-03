using System;
using System.Configuration;
using AuthorizeNet;
using AuthorizeNet.Utility.NotProvided;
using MapCall.Common.Configuration;
using MapCall.Common.Helpers;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility;
using MapCall.Common.Utility.Notifications;
using MMSINC.Authentication;
using MMSINC.Common;
using MMSINC.Utilities.Documents;
using MMSINC.Configuration;
using MMSINC.Interface;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using NHibernate;
using NHibernate.Event;
using StructureMap;
using System.Web;
using System.Web.Mvc;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.AssetUploads;
using MapCall.SAP.Model;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Controllers;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ActiveMQ;
using MMSINC.Utilities.Permissions;
using MMSINC.Utilities.StructureMap;
using RazorEngine.Templating;
using SAP.DataTest.Model.Repositories;
using StructureMap.Web;
using Permits.Data.Client.Repositories;

namespace MapCallMVC.Configuration
{
    public class DependencyRegistrar : DependencyRegistrar<User, User>
    {
        #region Private Methods

        protected override void RegisterUtilities(ConfigurationExpression i)
        {
            i.For<IAssetUploadFileService>().Use<AssetUploadFileService>();
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
            i.For<IAuthenticationCookieFactory>().Use<MapCallAuthenticationCookieFactory<User>>();
            i.For<IRoleService>()
                .HybridHttpOrThreadLocalScoped()
                .Use<RoleService>();
            i.For<IBasicRoleService>()
                .Use(ctx => ctx.GetInstance<IRoleService>());
            i.For<IDocumentService>().Use<DocumentService>();
            i.For<ISmtpClientFactory>().Use<SmtpClientFactory>();
            i.For<IMailMessageFactory>().Use<MailMessageFactory>();
            i.For<INotificationService>().Use<NotificationService>();
            i.For<ITemplateService>().Use(_ => new TemplateService()).Singleton();
            i.For<INotifier>().Use<RazorNotifier>();
            i.For<IHtmlToPdfConverter>().Use<HtmlToPdfConverter>();
            i.For<IImageToPdfConverter>()
                .Use(() => new ImageToPdfConverter());
            i.For<IUrlHelper>().Use(() => new MyUrlHelper(HttpContext.Current.Request.RequestContext));
            i.For<IPermissionsObjectFactory>().Use<PermissionsObjectFactory>();
            i.For<IMembershipHelper>().Use<MembershipHelper>();

            i.For<HttpContextBase>().Use(ctx => new System.Web.HttpContextWrapper(HttpContext.Current));

            i.For<IViewModelFactory>().Use<ViewModelFactory>();
            i.For<IMapResultFactory>().Use<MapResultFactory>();

            i.For<ISecureFormTokenService>().Use<SecureFormTokenService<SecureFormToken, SecureFormDynamicValue>>();

            i.For<IExtendedCustomerGateway>()
                .Use(() => new CustomerGateway(
                    ConfigurationManager.AppSettings.EnsureValue(MissingMethods.AppSettingsKeys.AUTHORIZE_NET_LOGIN_ID),
                    ConfigurationManager.AppSettings.EnsureValue(MissingMethods.AppSettingsKeys.AUTHORIZE_NET_TX_KEY),
                    HttpApplicationBase.IsProduction ? ServiceMode.Live : ServiceMode.Test));

            i.For<IAssetCoordinateService>().Use<AssetCoordinateService>();
            i.For<ISAPCustomerOrderRepository>().Use<SAPCustomerOrderRepository>();
            i.For<ISAPDeviceRepository>().Use<SAPDeviceRepository>();
            i.For<ISAPEquipmentRepository>().Use<SAPEquipmentRepository>();
            i.For<ISAPFunctionalLocationRepository>().Use<SAPFunctionalLocationRepository>();
            i.For<IExtendedSAPHttpClientConfiguration>().Use<SAPHttpClientConfiguration>();
            i.For<ISAPHttpClient>().Use(ctx => ctx.GetInstance<ExtendedSAPHttpClientFactory>().Build());
            i.For<ISAPInspectionRepository>().Use<SAPInspectionRepository>();
            i.For<ISAPNewServiceInstallationRepository>().Use<SAPNewServiceInstallationRepository>();
            i.For<ISAPNotificationRepository>().Use<SAPNotificationRepository>();
            i.For<ISAPTechnicalMasterAccountRepository>().Use<SAPTechnicalMasterAccountRepository>();
            i.For<ISAPWBSElementRepository>().Use<SAPWBSElementRepository>();
            i.For<ISAPWorkOrderRepository>().Use<SAPWorkOrderRepository>();
            i.For<ISAPManufacturerRepository>().Use<SAPManufacturerRepository>();
            i.For<ISAPProgressUnscheduledWorkOrderRepository>().Use<SAPProgressUnscheduledWorkOrderRepository>();
            i.For<ISAPCreateUnscheduledWorkOrderRepository>().Use<SAPCreateUnscheduledWorkOrderRepository>();
            i.For<ISAPCompleteUnscheduledWorkOrderRepository>().Use<SAPCompleteUnscheduledWorkOrderRepository>();
            i.For<ISAPMaintenancePlanLookupRepository>().Use<SAPMaintenancePlanRepository>();
            i.For<ISAPCreatePreventiveWorkOrderRepository>().Use<SAPCreatePreventiveWorkOrderRepository>();
            i.For<ISAPWorkOrderStatusUpdateRepository>().Use<SAPWorkOrderStatusUpdateRepository>();
            i.For<IActiveMQServiceFactory>().Use<ApacheActiveMQServiceFactory>();
            i.For<MMSINC.Data.IUnitOfWorkFactory>().Use<MMSINC.Data.NHibernate.UnitOfWorkFactory>();
            i.For<ILog>()
                .AlwaysUnique()
                .Use(ctx => LogManager.GetLogger(ctx.ParentType == null ? ctx.RequestedName : ctx.ParentType.Name));
            i.For<IDisplayItemService>().Use<DisplayItemService>();
            i.For<IViewPageActivator>().Use<StructureMapViewPageActivator>();
        }

        protected override void RegisterModels(ConfigurationExpression i)
        {
            i.For<IAuthenticationRepository<User>>()
                .Use(ctx => ctx.GetInstance<AuthenticationRepository>());
            i.For<IForgotPasswordRepository<User>>()
                .Use(ctx => ctx.GetInstance<AuthenticationRepository>());
            i.For<IModuleRepository>().Use<ModuleRepository>();
            i.For<IAuthenticationLogRepository<AuthenticationLog, User>>().Use<MapCallAuthenticationLogRepository>();
            i.For<ITokenRepository<SecureFormToken, SecureFormDynamicValue>>().Use<SecureFormTokenRepository>();
            
            MapCallDependencies.RegisterRepositories(i);
            RegisterPermitsApiRepositories(i);
            i.Scan(s => {
                s.AssemblyContainingType<Historian.Data.Client.Entities.RawData>();
                s.WithDefaultConventions();
            });
        }

        private void RegisterPermitsApiRepositories(ConfigurationExpression i)
        {
            i.For<IPermitsRepositoryFactory>().Use<PermitsRepositoryFactory>();
            i.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.Bond>>()
             .Use<PermitsDataClientRepository<Permits.Data.Client.Entities.Bond>>();
            i.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.CompanyReport>>()
             .Use<PermitsDataClientRepository<Permits.Data.Client.Entities.CompanyReport>>();
            i.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.County>>()
             .Use<PermitsDataClientRepository<Permits.Data.Client.Entities.County>>();
            i.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.Drawing>>()
             .Use<PermitsDataClientRepository<Permits.Data.Client.Entities.Drawing>>();
            i.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.Municipality>>()
             .Use<PermitsDataClientRepository<Permits.Data.Client.Entities.Municipality>>();
            i.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.Payment>>()
             .Use<PermitsDataClientRepository<Permits.Data.Client.Entities.Payment>>();
            i.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.Permit>>()
             .Use<PermitsDataClientRepository<Permits.Data.Client.Entities.Permit>>();
            i.For<IPermitsDataClientRepository<Permits.Data.Client.Entities.State>>()
             .Use<PermitsDataClientRepository<Permits.Data.Client.Entities.State>>();
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
}
