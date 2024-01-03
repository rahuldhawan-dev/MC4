using System.Web.Optimization;
using MMSINC.Bundling;
using MMSINC.ClassExtensions.IEnumerableExtensions;

// ReSharper disable CheckNamespace
namespace MapCall.Common.ClassExtensions.BetterScriptBundleExtensions
    // ReSharper restore CheckNamespace
{
    public static class BetterScriptBundleExtensions
    {
        #region Consts

        public const string COMMON_JS_ROOT = "~/Embed/MapCall.Common.Content.JS.",
                            COMMON_CSS_ROOT = "~/Embed/MapCall.Common.Content.CSS.";

        #endregion

        #region Public Methods

        public static void IncludeEmbedded(this BetterScriptBundle that, params string[] include)
        {
            include.Each(i => that.Include(COMMON_JS_ROOT + i));
        }

        public static void IncludeEmbeddedWithoutTransform(this DotLessStyleBundle that, params string[] include)
        {
            include.Each(i => that.IncludeWithoutTransform(COMMON_CSS_ROOT + i));
        }

        public static void IncludeEmbedded(this DotLessStyleBundle that, params string[] include)
        {
            include.Each(i => that.Include(COMMON_CSS_ROOT + i));
        }

        public static void IncludeEmbedded(this StyleBundle that, params string[] include)
        {
            include.Each(i => that.Include(COMMON_CSS_ROOT + i));
        }

        /// <summary>
        /// Adds the jQuery and jQuery unobtrusive ajax scripts that are embedded in MapCall.Common.Mvc.
        /// </summary>
        public static void IncludeJqueryCore(this BetterScriptBundle bundle)
        {
            bundle.IncludeEmbedded("jquery-3.7.1.min.js");
        }

        /// <summary>
        /// Adds the jQuery unobtrusive ajax scripts that are embedded in MapCall.Common.Mvc.
        /// </summary>
        public static void IncludeJqueryUnobtrusiveAjax(this BetterScriptBundle bundle)
        {
            bundle.IncludeEmbedded("jquery.unobtrusive-ajax.js");
        }

        /// <summary>
        /// Adds the jQuery validation scripts that are embedded in MapCall.Common.Mvc.
        /// </summary>
        public static void IncludeJqueryValidation(this BetterScriptBundle bundle)
        {
            bundle.IncludeEmbedded("jquery.validate.js",
                "jquery.validate.unobtrusive.js",
                "jquery.validate.unobtrusive.fix.js",
                "required-when-validation.js",
                "validation.js");
        }

        /// <summary>
        /// Adds the core jQuery UI scripts that are required for jQuery UI controls 
        /// to work. Note that this does not add the controls, you need to add thos
        /// manually based on what the site uses(they're huge scripts).
        /// </summary>
        /// <param name="bundle"></param>
        public static void IncludeJqueryUICore(this BetterScriptBundle bundle)
        {
            bundle.IncludeEmbedded("jquery-ui.js");
        }

        /// <summary>
        /// Adds the common jQuery UI controls that we use on every single site.
        /// </summary>
        /// <param name="bundle"></param>
        public static void IncludeCommonJqueryUIControls(this BetterScriptBundle bundle)
        {
            // NOTE: This should include any scripts that are needed for any common
            // controls that are part of MMSINC.Core.Mvc/MapCall.Common.Mvc. 
            bundle.IncludeEmbedded("jquery.timepicker-1.4.3.js",
                "jquery.datepicker.unobtrusive.js",
                "jquery.rangepicker.unobtrusive.js",
                "jquery.autocomplete.unobtrusive.js",
                "jquery.detergent-1.0.js",
                "jquery.multiinput.js",
                "jquery.multistring.js");
        }

        public static void IncludeCommonJQueryPlugins(this BetterScriptBundle bundle)
        {
            bundle.IncludeEmbedded("jquery.extensions.js");
        }

        public static void IncludeCollapsePanel(this BetterScriptBundle bundle)
        {
            bundle.IncludeEmbedded("collapse-panel.js");
        }

        public static void IncludeMultilist(this BetterScriptBundle bundle)
        {
            bundle.IncludeEmbedded("jquery.tmpl.min.js");
            bundle.IncludeEmbedded("multilist-0.9.5.js");
        }

        public static void IncludeFileUploader(this BetterScriptBundle bundle)
        {
            bundle.IncludeEmbedded("fineuploader-jquery-3.3.0.js");
            bundle.IncludeEmbedded("fineuploader.unobtrusive.js");
        }

        public static void IncludeAmCharts(this BetterScriptBundle bundle)
        {
            bundle.IncludeEmbedded("amcharts.js",
                "amcharts.serial.js",
                "amcharts.themes.light.js",
                "amcharts.unobtrusive.js");
        }

        #endregion
    }
}
