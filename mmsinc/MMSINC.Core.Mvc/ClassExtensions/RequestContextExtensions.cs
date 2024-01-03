using System.Web.Routing;

namespace MMSINC.ClassExtensions
{
    public static class RequestContextExtensions
    {
        #region Extension Methods

        public static string GetUrl(this RequestContext context)
        {
            return context.HttpContext.Request.RawUrl;
        }

        #endregion
    }
}
