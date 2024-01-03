using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Utilities;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.NameValueCollectionExtensions
    // ReSharper restore CheckNamespace
{
    public static class NameValueCollectionExtensions
    {
        #region Extension Methods

        /// <summary>
        /// Attempts to return the value in the collection and converts it to the T type. Returns 
        /// the default value of T if the key doesn't exist in the collection.
        /// </summary>
        public static T? GetValueAs<T>(this NameValueCollection nvc, string key) where T : struct
        {
            var realValue = nvc.GetValueOrDefault(key);
            if (realValue == null)
            {
                return null;
            }

            var type = typeof(T);

            // If it's an enum the value is the name value(as opposed to the numerical value)
            // we wanna parse it this way.
            if (type.IsEnum)
            {
                return (T)Enum.Parse(type, realValue);
            }
            // If it's a DateTime we want to support the today/yesterday values.
            else if (type == typeof(DateTime))
            {
                return (T)(object)realValue.ToDateTime();
            }
            else
            {
                return (T)Convert.ChangeType(realValue, type);
            }
        }

        public static string GetValueOrDefault(this NameValueCollection nvc, string key)
        {
            var realValue = nvc[key];
            return (string.IsNullOrWhiteSpace(realValue) ? null : realValue);
        }

        public static IEnumerable<string> FindKeys(this NameValueCollection nvc, string keyPart)
        {
            return nvc.AllKeys.Where(s => s.Contains(keyPart));
        }

        public static bool ContainsKey(this NameValueCollection nvc, string key)
        {
            return nvc.AllKeys.Contains(key);
        }

        public static string EnsureValue(this NameValueCollection nvc, string key)
        {
            if (!nvc.ContainsKey(key))
            {
                throw new KeyNotFoundException(
                    String.Format("Key '{0}' was not found in the collection.",
                        key));
            }

            return nvc[key];
        }

        public static void Each(this NameValueCollection nvc, Action<string, string> fn)
        {
            foreach (var key in nvc.AllKeys)
            {
                if (key != null)
                    fn(key, nvc[key]);
            }
        }

        public static NameValueCollection Where(this NameValueCollection nvc, Func<string, string, bool> fn)
        {
            var ret = new NameValueCollection();

            foreach (var key in nvc.AllKeys)
            {
                if (fn(key, nvc[key]))
                {
                    ret[key] = nvc[key];
                }
            }

            return ret;
        }

        public static string ToQueryString(this NameValueCollection coll)
        {
            if (coll == null)
            {
                return String.Empty;
            }

            var ret = new StringBuilder("?");

            foreach (string key in coll)
            {
                var values = coll.GetValues(key);
                foreach (var value in values)
                {
                    ret.Append(String.Concat(key, "=",
                        HttpUtility.UrlEncode(value), "&"));
                }
            }

            return ret.Length == 1
                ? String.Empty
                : ret.ToString(0, ret.Length - 1);
        }

        #endregion
    }
}
