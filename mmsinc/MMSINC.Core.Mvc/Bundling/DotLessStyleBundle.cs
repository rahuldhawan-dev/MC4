using System.Linq;
using System.Web.Optimization;
using FluentNHibernate.Utils;

namespace MMSINC.Bundling
{
    /// <summary>
    /// Bundle class for minifying and parsing stylesheets with dotLess.
    /// </summary>
    public class DotLessStyleBundle : BetterStyleBundle
    {
        #region Constructors

        public DotLessStyleBundle(string virtualPath, string cdnPath = null) : base(virtualPath, cdnPath)
        {
            // We need to add the DotLessTransformer prior to the minification transformer
            // that's added by default in the StyleBundle constructor. Otherwise the minification
            // fails because it sees the .less syntax and goes all "WHAT DO I DO!?"

            Transforms.Insert(0, new DotLessTransform());
            //Transforms.Add(new DotLessTransform());
        }

        #endregion
    }
}
