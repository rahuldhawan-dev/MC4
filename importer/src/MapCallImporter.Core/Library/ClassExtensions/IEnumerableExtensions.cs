using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace MapCallImporter.Library.ClassExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtensions
    {
        #region Exposed Methods

        public static IEnumerable<ItemWithIndex<T>> GetWithIndex<T>(this IEnumerable<T> that, int startIndex = 0)
        {
            return that.Select(item => new ItemWithIndex<T>(startIndex++, item));
        }

        #endregion
    }

    public class ItemWithIndex<TItem>
    {
        #region Properties

        public int Index { get; }
        public TItem Item { get; }

        #endregion

        #region Constructors

        public ItemWithIndex(int index, TItem item)
        {
            Index = index;
            Item = item;
        }

        #endregion

        #region Exposed Methods

        public void Deconstruct(out int index, out TItem item)
        {
            index = Index;
            item = Item;
        }

        #endregion
    }
}
