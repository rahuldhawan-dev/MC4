using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.TypeExtensions;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.DictionaryExtensions
    // ReSharper restore CheckNamespace
{
    public static class DictionaryExtensions
    {
        #region Extension Methods

        public static IEnumerable<KeyValuePair<Type, TThingy>> GetInOrderByType
            <TThingy>(this Dictionary<Type, TThingy> dict, Type type)
        {
            return dict
                  .Where(value => type.IsOrIsSubclassOf(value.Key))
                  .OrderBy(value => value.Key, new HieraricalTypeComparer());
        }

        public static ICollection<KeyValuePair<TKey, TValue>> MergeIn<TKey, TValue>(
            this ICollection<KeyValuePair<TKey, TValue>> mergeTo, IEnumerable<KeyValuePair<TKey, TValue>> mergeFrom)
        {
            if (mergeFrom == null)
            {
                return mergeTo;
            }

            foreach (var kv in mergeFrom)
            {
                mergeTo.Add(kv);
            }

            return mergeTo;
        }

        /// <summary>
        /// Merges one more dictionaries into one instance. Key/values are added in the order 
        /// they are added to the params array. Keys that already exist are overwritten if
        /// one of the merging dictionaries includes that key.
        /// </summary>
        public static IDictionary<TKey, TValue> MergeAndReplace<TKey, TValue>(this IDictionary<TKey, TValue> mergeTo,
            params IEnumerable<KeyValuePair<TKey, TValue>>[] mergeFrom)
        {
            foreach (var mergable in mergeFrom)
            {
                foreach (var kv in mergable)
                {
                    mergeTo[kv.Key] = kv.Value;
                }
            }

            return mergeTo;
        }

        public static string GetValue(this IDictionary<string, string> that, string key)
        {
            if (that.ContainsKey(key))
            {
                return that[key];
            }

            throw new KeyNotFoundException($"Key '{key}' was not found in the dictionary");
        }

        #endregion

        #region Nested Types

        public class HieraricalTypeComparer : IComparer<Type>
        {
            #region Exposed Methods

            public int Compare(Type x, Type y)
            {
                if (x.IsSubclassOf(y)) return 1;
                if (x == y) return 0;
                if (y.IsSubclassOf(x)) return -1;
                throw new ArgumentException(
                    String.Format(
                        "Type {0} cannot be compared to type {1} as they are not hierarically related.",
                        x, y));
            }

            #endregion
        }

        #endregion
    }
}
