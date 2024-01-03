using System.Collections.Generic;

namespace MMSINC.ClassExtensions
{
    public static class HashSetExtensions
    {
        #region Extension Methods

        public static void AddRange<TItem>(this HashSet<TItem> set, IEnumerable<TItem> range)
        {
            foreach (var item in range)
            {
                set.Add(item);
            }
        }

        #endregion
    }
}
