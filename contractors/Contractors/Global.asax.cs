using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.Bundling;
using MapCall.Common.ClassExtensions.RouteCollectionExtensions;
using MapCall.Common.ClassExtensions.BetterScriptBundleExtensions;
using MapCall.Common.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility;
using MMSINC;
using MMSINC.Configuration;
using MMSINC.ErrorHandlers;
using MMSINC.Filters;
using MMSINC.Metadata;
using StructureMap;
using DependencyRegistrar = Contractors.Configuration.DependencyRegistrar;
using MapCall.Common.Configuration;

namespace Contractors
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : MvcApplication<ContractorUser, ContractorUser>
    {
        #region Private Members

        private DependencyRegistrar<ContractorUser, ContractorUser> _dependencyRegistrar;

        #endregion

        #region Properties

        public override DependencyRegistrar<ContractorUser, ContractorUser> DependencyRegistrar
        {
            get
            {
                return _dependencyRegistrar ??
                    (_dependencyRegistrar = new DependencyRegistrar());
            }
        }

        #endregion

        #region Exposed Static Methods

        protected override void OnApplication_Start()
        {
            base.OnApplication_Start();
            EntityLookupLinkHelper.Enabled = false;
        }

        public override void RegisterGlobalFilters(GlobalFilterCollection filters, IContainer container)
        {
            if (IsProduction || IsStaging)
            {
                //filters.Add(new MyRequireHttpsAttribute());
                filters.Add(container.GetInstance<ErrorEmailer>());
            }
            filters.Add(container.GetInstance<MvcAuthorizationFilter>());
            filters.Add(container.GetInstance<AuditSelectFilter>());
            var tokenFilter = container
               .GetInstance<SecureTokenFilter<ContractorsSecureFormToken,
                    ContractorsSecureFormDynamicValue>>();
            tokenFilter.ViewName = "~/Views/Error/InvalidSecureForm.cshtml";
            filters.Add(tokenFilter);
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
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // this needs to come before the Default route.
            routes.AddResourcesRoute();

            var controllerNamespaces = new[] { "Contractors.Controllers", "Contractors.Controllers.WorkOrder", "MapCall.Common.Controllers" };
            routes.MapRoute(
               name: "DefaultRouteForExtensions",
               url: "{controller}/{action}/{id}.{ext}",
               defaults: new { controller = "Home" }, // Can not have id param optional, conflicts with IndexRouteForExtensions
               namespaces: controllerNamespaces
           );
            routes.MapRoute(
                name: "IndexRouteForExtensions",
                url: "{controller}/{action}.{ext}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: controllerNamespaces
            );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: controllerNamespaces
            );
        }

        public override void RegisterValueProviderFactories(ValueProviderFactoryCollection factoryProviderCollection, IContainer container)
        {
            base.RegisterValueProviderFactories(factoryProviderCollection, container);
            factoryProviderCollection.Insert(0,
                container
                   .GetInstance<SecureFormValueProviderFactory<
                        ContractorsSecureFormToken,
                        ContractorsSecureFormDynamicValue>>());
        }

        public override void RegisterBundles(System.Web.Optimization.BundleCollection bundles, MMSINC.Utilities.EmbeddedVirtualPathProvider embeddedPathProvider)
        {
            base.RegisterBundles(bundles, embeddedPathProvider);
            embeddedPathProvider.RegisterAssembly(
// ReSharper disable RedundantNameQualifier
                typeof(MapCall.Common.Controllers.ResourcesController).Assembly);
// ReSharper restore RedundantNameQualifier

            var baseCss = new DotLessStyleBundle("~/content/base-css");
            baseCss.IncludeEmbedded("theme-default.less");
            baseCss.Include("~/content/theme.less");
            baseCss.IncludeEmbedded(
                "reset.less",
                "jquery-ui-base.less",
                "jquery-ui-tabs.less",
                "jquery-ui-datepicker.less",
                "jquery-ui-autocomplete.less",
                "tables.less",
                "grid.less",
                "forms.less",
                "buttons.less", // Buttons must come after forms.css
                "validation.less");

            baseCss.Include("~/content/Site.css",
                            "~/content/Nav.css",
                            "~/content/jquery-ui-custom.css");
            baseCss.IncludeEmbedded("notifications.css", "collapsePanel.css");

            baseCss.IncludeFileUploaderStyles();
            baseCss.Include("~/content/fileuploader.css");

            // MVC adds multilist styles here. We don't use those in contractors afaik.
            // Then Map.index.css(MVC specific)
            // Then fullcalendar(not used in contractors?)

            bundles.Add(baseCss);

            var siteJs = new BetterScriptBundle("~/scripts/site-js");
            // mvc adds simpleStorage, not used here.
            // mvc adds userStorage, also not used here.
            siteJs.IncludeJqueryCore();
            // mvc adds form-state, not used here
            siteJs.IncludeJqueryValidation();

            siteJs.IncludeJqueryUICore();

            siteJs.IncludeCommonJqueryUIControls();
            siteJs.IncludeCollapsePanel();

            // jQuery UI controls
            siteJs.IncludeEmbedded("jquery.treeview.js");

            siteJs.IncludeCommonJQueryPlugins();

            // Misc jQuery plugins
            siteJs.IncludeEmbedded("jquery.scrollless.js", // I don't think we need this in contractors. This is for the MVC menu
                "ajaxtabs.js",
                "jqModal.js",
                "ajaxtable.js",
                "ajaxify-table.js",
                "Application.js");

            // UnobtrusiveAjax must come after ajaxtable.js, but only in contractors.
            // Why only in contractors? I couldn't figure that out.
            siteJs.IncludeJqueryUnobtrusiveAjax();

            siteJs.IncludeFileUploader();
            siteJs.IncludeMultilist();

            // Our site specific scripts
            siteJs.Include("~/Scripts/main.js");

            bundles.Add(siteJs);

            // Minifying fails on the arcgis css because it's already minified(I guess).
            var mapCss = new BetterStyleBundle("~/content/map-css");
            mapCss.EnableMinifying = false; // We don't do this in MVC?
            mapCss.IncludeWithoutTransform("~/Embed/MapCall.Common.Content.CSS.arcgis.css");
            mapCss.IncludeWithoutTransform("~/Embed/MapCall.Common.Content.CSS.arcgis-tundra.css");
            bundles.Add(mapCss);

            var mapJs = new BetterScriptBundle("~/scripts/map-js");
            mapJs.Include("~/Embed/MapCall.Common.Content.JS.arcgis.js");
            mapJs.Include("~/Embed/MapCall.Common.Content.JS.jquery.esri.mappin.js");
            bundles.Add(mapJs);
        }

        protected override void HandleUnhandledError404s(RequestContext requestContext, IContainer container)
        {
            base.HandleUnhandledError404s(requestContext, container);

            IController c = container.GetInstance<ErrorController>();
            c.Execute(requestContext);
        }

        #endregion
    }
}
