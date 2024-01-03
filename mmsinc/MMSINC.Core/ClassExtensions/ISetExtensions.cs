using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace MMSINC.ClassExtensions.ISetExtensions
{
    public static class ISetExtensions
    {
        /// <summary>
        /// Adds an item to a set if it is not already in the set.
        /// </summary>
        public static TItem AddIfMissing<TItem>(this ISet<TItem> set, TItem item)
        {
            if (!set.Contains(item))
            {
                set.Add(item);
            }

            return item;
        }
    }
}
