using System.Linq;
using System.Web.Routing;

namespace MMSINC.ClassExtensions
{
    public static class RouteValueDictionaryExtensions
    {
        /// <summary>
        /// Merges two RouteValueDictionaries into a new instance.
        /// </summary>
        public static RouteValueDictionary Merge(this RouteValueDictionary left, RouteValueDictionary right)
        {
            var result = new RouteValueDictionary(left);

            foreach (var rv in right.Where(rv => !result.ContainsKey(rv.Key)))
            {
                result.Add(rv.Key, rv.Value);
            }

            return result;
        }

        /// <summary>
        /// Merges two RouteValueDictionaries into a new instance.
        /// </summary>
        public static RouteValueDictionary Merge(this RouteValueDictionary left, object right)
        {
            return left.Merge(new RouteValueDictionary(right));
        }
    }
}
