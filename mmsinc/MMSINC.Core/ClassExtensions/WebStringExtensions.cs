using System.Web;
using System.Web.Routing;

// ReSharper disable once CheckNamespace
namespace MMSINC.ClassExtensions.StringExtensions
{
    /// <summary>
    /// String extensions which are tied to System.Web and are thus incompatible with .Net Standard and
    /// beyond.
    /// </summary>
    public static class WebStringExtensions
    {
        public static RouteValueDictionary ToRouteValues(this string value)
        {
            var queryString = HttpUtility.ParseQueryString(value);
            if (queryString.HasKeys() == false) return new RouteValueDictionary();

            var routeValues = new RouteValueDictionary();
            foreach (string key in queryString.AllKeys)
                routeValues.Add(key, queryString[key]);

            return routeValues;
        }
    }
}
