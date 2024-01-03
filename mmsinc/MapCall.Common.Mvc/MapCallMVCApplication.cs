using System;
using System.Collections.Generic;
using System.Web.Optimization;
using MapCall.Common.ClassExtensions;
using MapCall.Common.ClassExtensions.BetterScriptBundleExtensions;
using MMSINC.Authentication;
using MMSINC.Bundling;
using MMSINC.Utilities;

namespace MapCall.Common
{
    public abstract class MapCallMvcApplication<TAssemblyOf, TUser>
        : MMSINC.MvcApplication<TAssemblyOf, TUser>
        where TUser : class, IAdministratedUser
    {
        // TODO: None of this needs to be static? -Ross 3/7/2014

        // This should be renamed, it's not adding bundles, it's adding stylesheets to a specific bundle.
        private static List<String> _additionalBundles;

        public static List<String> AdditionalBundles
        {
            get { return _additionalBundles ?? (_additionalBundles = new List<string>()); }
            set { _additionalBundles = value; }
        }

        // This should just be an override of RegisterBundles.
        protected static void RegisterMapCallBundles(BundleCollection bundles,
            EmbeddedVirtualPathProvider embeddedPathProvider)
        {
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
                "action-bar.css",
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
            siteJs.IncludeEmbedded("ajaxtable.js"); // This has to come before unobtrusive ajax.
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
                "coordinatePicker.js",
                "nav.js",
                "adminPanel.js",
                "action-bar.js",
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
           // mapJs.IncludeJqueryUICore();
           // mapJs.IncludeCommonJqueryUIControls();
            mapJs.IncludeEmbedded("arcgis.js",
                "jquery.esri.mappin.js",
                "jquery.esri.mappin.defaults.js",
                "Application.js",
                "map-legend.js");

            bundles.Add(mapJs);

            var chartJs = new BetterScriptBundle("~/scripts/chart-js");
            chartJs.IncludeAmCharts();
            bundles.Add(chartJs);

            var webComponentsBundle = new BetterScriptBundle("~/scripts/web-components");
            webComponentsBundle.IncludeEmbedded("WebComponents.dom-utils.js");
            webComponentsBundle.IncludeEmbedded("WebComponents.web-component-helper.js");
            webComponentsBundle.IncludeEmbedded("WebComponents.mc-datatable.js");
            webComponentsBundle.IncludeEmbedded("WebComponents.mc-checkboxlistitem.js");
            bundles.Add(webComponentsBundle);

            var litComponentBundle = new BetterScriptBundle("~/scripts/lit-components");
            litComponentBundle.IncludeEmbedded("LitComponents.lit-all.min.js");
            litComponentBundle.IncludeEmbedded("LitComponents.observable-array.js");
            bundles.Add(litComponentBundle);

            //Authorize.net
            var authCss = new DotLessStyleBundle("~/content/authorize-net-css");
            authCss.IncludeEmbedded("authorize-net.css");
            bundles.Add(authCss);
            var auth = new BetterScriptBundle("~/scripts/auth-js");
            auth.Include("~/Embed/MapCall.Common.Content.JS.AuthorizeNetPopup.js");
            bundles.Add(auth);
        }
    }
}
