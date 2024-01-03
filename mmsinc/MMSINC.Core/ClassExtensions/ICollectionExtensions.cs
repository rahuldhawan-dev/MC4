using System;
using System.Collections;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.IOrderedDictionaryExtensions
    // ReSharper restore CheckNamespace
{
    public static class ICollectionExtensions
    {
        #region Extension Methods

        public static TDest[] ToArray<TDest>(this ICollection coll, Func<object, TDest> fn)
        {
            var ret = new TDest[coll.Count];
            var i = 0;

            foreach (var item in coll)
            {
                ret[i++] = fn(item);
            }

            return ret;
        }

        #endregion
    }
}
