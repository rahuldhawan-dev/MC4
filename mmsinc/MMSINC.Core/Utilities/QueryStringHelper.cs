using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MMSINC.Utilities
{
    public static class QueryStringHelper
    {
        public static string JoinUrlWithQueryString(string baseUrl, string queryString)
        {
            if (String.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException("baseUrl");
            }

            return String.Format("{0}?{1}", baseUrl, queryString);
        }

        /// <summary>
        /// Takes a value of any type and prepares it for a querystring. This includes encoding it properly.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatQueryStringValue(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            // Probably gonna want to add encoding to this. 
            return HttpUtility.UrlEncode(value.ToString());
        }

        /// <summary>
        /// Takes a Dictionary and returns it formatted as a querystring. 
        /// </summary>
        /// <param name="keyVals"></param>
        /// <returns></returns>
        public static string BuildFromDictionary(IDictionary<string, object> keyVals)
        {
            if (keyVals == null)
            {
                throw new ArgumentNullException("keyVals");
            }

            var asIndividuals = (from kv in keyVals
                                 select BuildFromKeyValuePair(kv.Key, kv.Value)).ToArray();

            var allTogetherNow = String.Join("&", asIndividuals);

            // Don't append a ? to the beginning, it's less annoying to add it when needed
            // than it is to have to remove it if they don't want it.

            return allTogetherNow;
        }

        public static string BuildFromDictionary(string baseUrl, IDictionary<string, object> keyVals)
        {
            // Let the overload do the null check.
            var query = BuildFromDictionary(keyVals);

            return JoinUrlWithQueryString(baseUrl, query);
        }

        public static string BuildFromKeyValuePair(string key, object value)
        {
            // Keys can't be null as it'd never be considered a proper querystring value.
            // Values can be null. 
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException(
                    "A querystring key can not be null or empty");
            }

            return String.Format("{0}={1}", key, FormatQueryStringValue(value));
        }

        public static string BuildFromKeyValuePair(string url, string key, object value)
        {
            var query = BuildFromKeyValuePair(key, value);
            return JoinUrlWithQueryString(url, query);
        }

        public static string StripQueryString(this string url)
        {
            return url.Contains("?") ? new Regex(@"^([^?]+)\?.+$").Match(url).Groups[1].Value : url;
        }
    }
}
