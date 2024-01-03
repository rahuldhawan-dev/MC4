using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;

// ReSharper disable once CheckNamespace
namespace MMSINC.ClassExtensions.IListExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class IListExtensions
    {
        #region Exposed Methods

        /// <summary>
        /// Map a list of type `TIn' to a list of type `TOut', using the transform function `fn'.
        /// </summary>
        public static IList<TOut> Map<TIn, TOut>(this IList<TIn> list, Func<TIn, TOut> fn)
        {
            var ret = new List<TOut>();
            list.Each(item => ret.Add(fn(item)));
            return ret;
        }

        public static void RemoveSingle<TItem>(this IList<TItem> that, Func<TItem, bool> where)
        {
            that.Remove(that.Single(where));
        }

        /// <summary>
        /// Adds an item to a list if it is not already in the list.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public static TItem AddIfMissing<TItem>(this IList<TItem> list, TItem item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }

            return item;
        }

        /// <summary>
        /// Maps the given instance to a read only dictionary while safely ignoring duplicate keys.
        /// </summary>
        /// <typeparam name="TKey">The type of the key for the dictionary result.</typeparam>
        /// <typeparam name="TValue">The type of the value for the dictionary result.</typeparam>
        /// <param name="instance">The instance that this method is invoked upon.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="duplicateKeyAction">An action that is invoked when a duplicate key is found.</param>
        /// <returns>A read only dictionary of mapped items.</returns>
        public static IReadOnlyDictionary<TKey, TValue> MapToDictionary<TKey, TValue>(
            this IReadOnlyCollection<TValue> instance,
            Func<TValue, TKey> keySelector,
            Action<TKey> duplicateKeyAction)
        {
            if (instance == null)
            {
                return null;
            }

            var mappedDictionary = new Dictionary<TKey, TValue>(instance.Count);

            foreach (var item in instance)
            {
                var key = keySelector(item);

                if (mappedDictionary.ContainsKey(key))
                {
                    duplicateKeyAction(key);
                    continue;
                }

                mappedDictionary.Add(key, item);
            }

            return new ReadOnlyDictionary<TKey, TValue>(mappedDictionary);
        }

        #endregion
    }
}
