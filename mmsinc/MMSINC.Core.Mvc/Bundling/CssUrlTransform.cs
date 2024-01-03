using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace MMSINC.Bundling
{
    /// <summary>
    /// Fixes the urls in stylesheets so that they continue to work inside a bundled file.
    /// This is the source code, slightly modified, for CssRewriteUrlTransform from System.Web.Optimization.
    /// The original class doesn't correctly transform urls when a site is running inside a 
    /// virtual directory.
    /// </summary>
    public class CssUrlTransform : IItemTransform
    {
        #region Properties

        /// <summary>
        /// Set this to false during testing as there's no way to get VirtualPathUtility
        /// to play nice in a unit test scenario.
        /// </summary>
        public static bool UseVirtualPathUtility { get; set; }

        #endregion

        #region Constructor

        static CssUrlTransform()
        {
            UseVirtualPathUtility = true;
        }

        #endregion

        // Methods
        internal static string ConvertUrlsToAbsolute(string baseUrl, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return content;
            }

            var regex = new Regex("url\\(['\"]?(?<url>[^)]+?)['\"]?\\)");
            return regex.Replace(content,
                match => "url(" + RebaseUrlToAbsolute(baseUrl, match.Groups["url"].Value) + ")");
        }

        public string Process(string includedVirtualPath, string input)
        {
            if (includedVirtualPath == null)
            {
                throw new ArgumentNullException("includedVirtualPath");
            }

            // Original class calls Substring(1) on the includedVirtualPath before passing it to
            // GetDirectory. That messes things up if the site's running in a virtual directory.
            return ConvertUrlsToAbsolute(VirtualPathUtility.GetDirectory(includedVirtualPath), input);
        }

        internal static string RebaseUrlToAbsolute(string baseUrl, string url)
        {
            if ((string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(baseUrl)) ||
                url.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                return url;
            }

            if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl = baseUrl + "/";
            }

            var almostAbsoluteUrl = baseUrl + url;
            if (UseVirtualPathUtility)
            {
                almostAbsoluteUrl = VirtualPathUtility.ToAbsolute(baseUrl + url);
            }

            // This is a gross hack to allow the use of embedded images in bundles. -Ross 1/23/2014
            almostAbsoluteUrl = almostAbsoluteUrl.Replace("/Embed/Resources/", "/Resources/");

            return almostAbsoluteUrl;
        }
    }
}
