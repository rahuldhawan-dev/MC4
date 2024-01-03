using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.ClassExtensions;
using MMSINC.Bundling;
using MapCall.Common.ClassExtensions.RouteCollectionExtensions;
using MapCall.Common.ClassExtensions.BetterScriptBundleExtensions;
using MapCall.Common.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility;
using MMSINC;
using MMSINC.Configuration;
using MMSINC.ErrorHandlers;
using MMSINC.Filters;
using MMSINC.Metadata;
using StructureMap;
using DependencyRegistrar = MapCallIntranet.Configuration.DependencyRegistrar;
using System.Net;
using System.Web.Optimization;
using MapCall.Common;
using MapCall.Common.Metadata;

namespace MapCallIntranet
{
    public class MvcApplication : MapCallMvcApplication<User, User>
    {
        #region Private Members

        #region Fields

        private DependencyRegistrar _dependencyRegistrar;

        #endregion

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

        protected override void OnApplication_Start()
        {
            base.OnApplication_Start();
            log4net.Config.XmlConfigurator.Configure();
            //Needed below line in .net 4.5.* for Okta to work. Other option is to upgrade .net to 4.6
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        // OWIN/NONCE/OKTA infinite redirect loop fix
        // https://www.tech4afric.com/how-to-fix-infinite-login-redirect-loop-in-okta-with-openconnectid-in-asp-net-4-x-when-an-app-is-deployed-to-a-server/#more-189
        void Session_Start(object sender, EventArgs e) { }

        public override void RegisterBundles(System.Web.Optimization.BundleCollection bundles, MMSINC.Utilities.EmbeddedVirtualPathProvider embeddedPathProvider)
        {
            base.RegisterBundles(bundles, embeddedPathProvider);
            //RegisterMapCallBundles(bundles, embeddedPathProvider);
            embeddedPathProvider.RegisterAssembly(typeof(MapCall.Common.Controllers.ResourcesController).Assembly);

            // NOTE NOTE NOTE NOTE NOTE
            // Style bundles MUST use the same directory name for the bundle path
            // as where the files are saved. Otherwise images load from the wrong spot.

            var baseCss = new DotLessStyleBundle("~/content/base-css");
            // These files need to come first as they represent base classes and junk
            // that other stylesheets rely on.
            baseCss.IncludeEmbedded(
                "theme-default.less",
                "theme.less",
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

            // Stylesheets that are used site-wide.
            baseCss.IncludeEmbedded("site.css",
                "nav.css",
                //"action-bar.css",
                "notifications.css",
                "adminPanel.css",
                "collapsePanel.css");

            baseCss.IncludeFileUploaderStyles();
            baseCss.IncludeEmbedded("fileuploader.css");

            baseCss.IncludeMultilistStyles();

            //// Page-specific stylesheets.
            baseCss.IncludeEmbedded("Map.Index.css");

            // calendar
            baseCss.IncludeEmbedded("fullcalendar.css");
            baseCss.IncludeEmbedded("fullcalendar.print.css");

            //Custom bundles from site's global.asax
            if (AdditionalBundles.Count > 0)
                baseCss.Include(AdditionalBundles.ToArray());

            bundles.Add(baseCss);

            var mapCss = new StyleBundle("~/content/map-css");
            mapCss.IncludeEmbedded("arcgis.css");
            mapCss.IncludeEmbedded("arcgis-tundra.css");
            mapCss.IncludeEmbedded("currentLocationStyle.css");
            bundles.Add(mapCss);

            var siteJs = new BetterScriptBundle("~/scripts/site-js");
            siteJs.IncludeEmbedded("simpleStorage.js");
            siteJs.IncludeEmbedded("user-storage.js");
            siteJs.IncludeJqueryCore();
            siteJs.IncludeEmbedded("form-state.js"); // This requires jQuery.
            siteJs.IncludeJqueryUnobtrusiveAjax();
            siteJs.IncludeJqueryValidation();

            siteJs.IncludeJqueryUICore();

            siteJs.IncludeCommonJqueryUIControls();
            siteJs.IncludeCollapsePanel();

            // jQuery UI controls
            siteJs.IncludeEmbedded("jquery.treeview.js");

            siteJs.IncludeCommonJQueryPlugins();

            // Misc jQuery plugins
            siteJs.IncludeEmbedded("jquery.scrollless.js",
                "ajaxtabs.js",
                "jqModal.js",
                "jquery.tablednd.js");

            siteJs.IncludeFileUploader();

            siteJs.IncludeMultilist();

            // Our site specific scripts
            siteJs.IncludeEmbedded("Application.js",
                "ajaxtable.js",
                "coordinatePicker.js",
                "nav.js",
                "adminPanel.js",
                //"action-bar.js",
                "ajaxify-table.js");

            //calendar
            siteJs.IncludeEmbedded("moment.min.js");
            siteJs.IncludeEmbedded("fullcalendar.js");
            siteJs.IncludeEmbedded("FunctionalLocationTable.js");

            bundles.Add(siteJs);

            var mapJs = new BetterScriptBundle("~/scripts/map-js");
            mapJs.IncludeJqueryCore();
            mapJs.IncludeJqueryUnobtrusiveAjax();
            mapJs.IncludeCommonJQueryPlugins();
            mapJs.IncludeEmbedded("arcgis.js",
                "jquery.esri.mappin.js", "jquery.esri.mappin.defaults.js", "Application.js", "map-legend.js");

            bundles.Add(mapJs);

            var chartJs = new BetterScriptBundle("~/scripts/chart-js");
            chartJs.IncludeAmCharts();
            bundles.Add(chartJs);

            var webComponentsBundle = new BetterScriptBundle("~/scripts/web-components");
            webComponentsBundle.IncludeEmbedded("WebComponents.dom-utils.js");
            webComponentsBundle.IncludeEmbedded("WebComponents.web-component-helper.js");
            webComponentsBundle.IncludeEmbedded("WebComponents.mc-checkboxlistitem.js");
            bundles.Add(webComponentsBundle);

            //Authorize.net
            var authCss = new DotLessStyleBundle("~/content/authorize-net-css");
            authCss.IncludeEmbedded("authorize-net.css");
            bundles.Add(authCss);
            var auth = new BetterScriptBundle("~/scripts/auth-js");
            auth.Include("~/Embed/MapCall.Common.Content.JS.AuthorizeNetPopup.js");
            bundles.Add(auth);
        }

        public override void RegisterGlobalFilters(GlobalFilterCollection filters, IContainer container)
        {
            if (IsProduction || IsStaging)
            {
                //filters.Add(new MyRequireHttpsAttribute());
                filters.Add(container.GetInstance<ErrorEmailer>());
            }
            filters.Add(new HandleErrorAttribute());
        }

        public override void RegisterRoutes(RouteCollection routes)
        {
            // this needs to come before the Default route, and make sure your passing in this namespace going forward.
            routes.AddResourcesRoute();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("proxies/{resource}.ashx/{*pathInfo}");
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "NearMiss", action = "New", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "MapCallIntranet.Controllers", "MapCall.Common.Controllers" }
            );
        }
    }
}
