using System;
using System.Web;
using System.Web.WebPages;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.ClassExtensions
{
    public static class IHtmlStringExtensions
    {
        /// <summary>
        /// Wraps an IHtmlString inside a HelperResult func so it can be written out
        /// to things that use those things.
        /// </summary>
        public static Func<object, HelperResult> ToHelperResult(this IHtmlString htmlString)
        {
            // Throw for null here instead of in the helper result action cause otherwise
            // it'll be a lot harder to track down. 
            htmlString.ThrowIfNull("htmlString");
            return obj => new HelperResult(writer => writer.Write(htmlString.ToHtmlString()));
        }
    }
}
