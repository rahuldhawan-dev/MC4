using MMSINC.Bundling;

namespace MapCall.Common.ClassExtensions
{
    public static class DotLessStyleBundleExtensions
    {
        #region Consts

        private const string COMMON_CSS_ROOT = "~/Embed/MapCall.Common.Content.CSS.";

        #endregion

        #region Private Methods

        private static void IncludeStyle(this DotLessStyleBundle bundle, params string[] files)
        {
            foreach (var file in files)
            {
                bundle.Include(COMMON_CSS_ROOT + file);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Includes the basic fine uploader stylesheet. 
        /// </summary>
        /// <param name="bundle"></param>
        public static void IncludeFileUploaderStyles(this DotLessStyleBundle bundle)
        {
            bundle.IncludeStyle("fineuploader-3.3.0.less");
        }

        public static void IncludeMultilistStyles(this DotLessStyleBundle bundle)
        {
            bundle.IncludeStyle("multilist.css");
        }

        #endregion
    }
}
