using System;
using System.Text;
using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.StringBuilderExtensions
    // ReSharper restore CheckNamespace
{
    public static class StringBuilderExtensions
    {
        #region Extensions Methods

        [Obsolete("Everything using this should use IHtmlString/HtmlString. MvcHtmlString is obsolete.")]
        public static MvcHtmlString ToMvcHtmlString(this StringBuilder sb)
        {
            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion
    }
}
