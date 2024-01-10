using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common;
using MapCall.Common.Metadata;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Configuration;
using MMSINC.ControllerFactories;
using MMSINC.ErrorHandlers;
using MMSINC.Filters;
using MMSINC.Metadata;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.ClassExtensions.RouteCollectionExtensions;
using MapCall.Common.Controllers;
using MapCallMVC.Configuration;
using StructureMap;
using DependencyRegistrar = MapCallMVC.Configuration.DependencyRegistrar;
using MapCall.Common.Configuration;

namespace MapCallMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : MapCallMvcApplication<User, User>
    {
        #region Fields

        private DependencyRegistrar _dependencyRegistrar;

        #endregion

        #region Properties

        public override DependencyRegistrar<User, User> DependencyRegistrar
        {
            get
            {
                return _dependencyRegistrar ??
                       (_dependencyRegistrar = new DependencyRegistrar());
            }
        }

        #endregion

        #region Private Methods

        protected override void HandleUnhandledError404s(RequestContext requestContext, IContainer container)
        {
            base.HandleUnhandledError404s(requestContext, container);

            IController c = container.GetInstance<ErrorController>();
            c.Execute(requestContext);
            ((ErrorController)c).Dispose();
        }

        #endregion

        #region Public Methods

        protected override void OnApplication_Start()
        {
            base.OnApplication_Start();
            log4net.Config.XmlConfigurator.Configure();
        }

        public override IControllerFactory CreateControllerFactory(IContainer container)
        {
            var factory = (CompositeControllerFactory)base.CreateControllerFactory(container);
            factory.Factories.Add(container.GetInstance<EntityLookupControllerFactory>());
            return factory;
        }

        public override void RegisterGlobalFilters(GlobalFilterCollection filters, IContainer container)
        {
            if (IsProduction || IsStaging)
            {
                // filters.Add(new MyRequireHttpsAttribute());
                filters.Add(container.GetInstance<ErrorEmailer>());
            }
            else
            {
                // This needs to be the first filter that runs or else it's useless.
                filters.Add(new TestRequestProcessingFilter());
            }

            var auth = container.GetInstance<MvcAuthorizationFilter>();
            auth.Authorizors.Add(container.GetInstance<RequiresUserAdminAuthorizer>());

            // This should place RoleAuthorizor after the normal anon/authenticated/admin checks
            var roleAuth = container.GetInstance<RoleAuthorizer>();
            roleAuth.ViewName = "~/Views/Shared/ForbiddenRoleAccess.cshtml";
            auth.Authorizors.Add(roleAuth);
            auth.Authorizors.Add(container.GetInstance<UserMustHaveProfileAuthorizer>());
            
            filters.Add(auth);
            var tokenFilter = container.GetInstance<SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>>();
            tokenFilter.ViewName = "~/Views/Error/InvalidSecureForm.cshtml";
            filters.Add(tokenFilter);
            filters.Add(container.GetInstance<UserStorageFilter>());
            filters.Add(container.GetInstance<AuditImageFilter>());
            filters.Add(container.GetInstance<AuditReportFilter>());
            filters.Add(container.GetInstance<AuditSelectFilter>());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NotFoundErrorHandlerAttribute {
                View = "~/Views/Error/NotFound.cshtml",
                DefaultErrorMessage = "This page could not be found."
            });
            filters.Add(new AjaxErrorHandlerAttribute {
                ShowExplicitErrorMessages = !IsProduction
            });
            filters.Add(new DeleteTemporaryFileFilter());
        }

        public override void RegisterRoutes(RouteCollection routes)
        {
            routes.AddResourcesRoute();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("proxies/{resource}.ashx/{*pathInfo}");
            // NOTE: The order of routes matters!
            // NOTE: The order of routes doesn't matter if you specifically generate a url and say which named route to use.

            var controllerNameSpaces = new[] { "MapCallMVC.Controllers", "MapCall.Common.Controllers" };

            routes.AddManyToManyRoutes("Employee", "ProductionSkillSets", controllerNameSpaces);
            routes.AddManyToManyRoutes("Equipment", "Sensor", controllerNameSpaces);
            routes.AddManyToManyRoutes("Equipment", "EquipmentMaintenancePlan", controllerNameSpaces);
            routes.AddManyToManyRoutes("Equipment", "Link", controllerNameSpaces);
            routes.AddManyToManyRoutes("Incident", "AuditLogEntries", controllerNameSpaces);
            routes.AddManyToManyRoutes("Incident", "IncidentEmployeeAvailability", controllerNameSpaces);
            routes.AddManyToManyRoutes("Incident", "IncidentInvestigation", controllerNameSpaces);
            routes.AddManyToManyRoutes("OperatingCenter", "AssetType", controllerNameSpaces);
            routes.AddManyToManyRoutes("OperatingCenter", "WaterSystem", controllerNameSpaces);
            routes.AddManyToManyRoutes("StandardOperatingProcedure", "StandardOperatingProcedureQuestion", controllerNameSpaces);
            routes.AddManyToManyRoutes("StandardOperatingProcedure", "StandardOperatingProcedurePositionGroupCommonNameRequirement", controllerNameSpaces);
            routes.AddManyToManyRoutes("StandardOperatingProcedure", "TrainingModule", controllerNameSpaces);
            routes.AddManyToManyRoutes("Town", "OperatingCenter", controllerNameSpaces);
            routes.AddManyToManyRoutes("Town", "PublicWaterSupply", controllerNameSpaces);
            routes.AddManyToManyRoutes("Town", "WasteWaterSystem", controllerNameSpaces);
            routes.AddManyToManyRoutes("Town", "Gradient", controllerNameSpaces);
            routes.AddManyToManyRoutes("TrainingModule", "TrainingRequirement", controllerNameSpaces);
            routes.AddManyToManyRoutes("TrainingModule", "AuditLogEntries", controllerNameSpaces);
            routes.AddManyToManyRoutes("TrainingRecord", "TrainingSession", controllerNameSpaces);
            routes.AddManyToManyRoutes("TrainingRecord", "AuditLogEntries", controllerNameSpaces);
            routes.AddManyToManyRoutes("TrainingRequirement", "AuditLogEntries", controllerNameSpaces);
            routes.AddManyToManyRoutes("Facility", "FacilityProcess", controllerNameSpaces, area: string.Empty);
            routes.AddManyToManyRoutes("Facility", "SystemDeliveryEntryType", controllerNameSpaces, area: string.Empty);
            routes.AddManyToManyRoutes("Facility", "FacilityFacilityArea", controllerNameSpaces, area: string.Empty);
            routes.AddManyToManyRoutes("OperatorLicense", "WasteWaterSystem", controllerNameSpaces, area: string.Empty);

            routes.MapRoute(
                name: "DefaultRouteForExtensions",
                url: "{controller}/{action}/{id}.{ext}",
                defaults: new { controller = "Home" }, // Can not have id param optional, conflicts with IndexRouteForExtensions
                namespaces: controllerNameSpaces
            );
            routes.MapRoute(
                name: "IndexRouteForExtensions",
                url: "{controller}/{action}.{ext}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: controllerNameSpaces
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: controllerNameSpaces
            );

            // This needs to come after the Default route or else anything using the default
            // route will have the wrong url generated.
            routes.MapRoute(
                name: "DefaultShowChild",
                url: "{controller}/Show/{id}/{action}",
                namespaces: controllerNameSpaces);
        }

        public override void RegisterValueProviderFactories(ValueProviderFactoryCollection factoryProviderCollection, IContainer container)
        {
            base.RegisterValueProviderFactories(factoryProviderCollection, container);
            factoryProviderCollection.Insert(0,
                container.GetInstance<SecureFormValueProviderFactory<SecureFormToken, SecureFormDynamicValue>>());
        }

        public override void RegisterBundles(System.Web.Optimization.BundleCollection bundles, MMSINC.Utilities.EmbeddedVirtualPathProvider embeddedPathProvider)
        {
            base.RegisterBundles(bundles, embeddedPathProvider);
            RegisterMapCallBundles(bundles, embeddedPathProvider);
        }

        #endregion
    }

    /// <summary>
    /// This filter exists to stop request processing between regression test scenarios in order to stop
    /// weird fluke failures having to do with singleton nhibernate session that exists during regression 
    /// testing. This must be an AuthorizationFilter as those are the filters that run first. This must
    /// also be the first filter that runs so that we aren't causing any potential db processing.
    /// </summary>
    public class TestRequestProcessingFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (MMSINC.MvcApplication.IsInTestMode && !MMSINC.MvcApplication.AllowTestRequests)
            {
                // Only return an EmptyResult or something that will otherwise not cause the database
                // to be touched. Don't put views here unless they're just plain text without any layout stuff.
                filterContext.Result = new EmptyResult();
            }
        }
    }
}