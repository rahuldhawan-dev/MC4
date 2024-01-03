using System.Linq;
using System.Web.Optimization;

namespace MMSINC.Bundling
{
    /// <summary>
    /// Bundle class for minifying.
    /// </summary>
    public class BetterStyleBundle : ContentBundle
    {
        #region Fields

        private bool _enableMinifying;
        private CssMinify _cssMinify;

        #endregion

        #region Properties

        public bool EnableMinifying
        {
            get { return _enableMinifying; }
            set
            {
                _enableMinifying = value;
                if (_enableMinifying && !Transforms.Contains(_cssMinify))
                {
                    Transforms.Add(_cssMinify);
                }
                else if (!_enableMinifying && Transforms.Contains(_cssMinify))
                {
                    Transforms.Remove(_cssMinify);
                }
            }
        }

        #endregion

        #region Constructors

        public BetterStyleBundle(string virtualPath, string cdnPath = null)
            : base(virtualPath, cdnPath)
        {
            _cssMinify = new CssMinify();
            EnableMinifying = true;
        }

        #endregion

        #region Public Methods

        public override BundleResponse ApplyTransforms(BundleContext context, string bundleContent,
            System.Collections.Generic.IEnumerable<BundleFile> bundleFiles)
        {
            return base.ApplyTransforms(context, bundleContent, bundleFiles);
        }

        public override Bundle Include(string virtualPath, params IItemTransform[] transforms)
        {
            // StyleBundle doesn't fix image urls, so if you have images
            // in a subdirectory it chokes and you get lots of 404s. This
            // fixes that problem. 
            var tforms = transforms.ToList();
            tforms.Add(new CssUrlTransform());
            return base.Include(virtualPath, tforms.ToArray());
        }

        /// <summary>
        /// Includes a path but doesn't add the CssUrlTransform to it.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public Bundle IncludeWithoutTransform(string virtualPath)
        {
            return base.Include(virtualPath);
        }

        #endregion
    }
}
