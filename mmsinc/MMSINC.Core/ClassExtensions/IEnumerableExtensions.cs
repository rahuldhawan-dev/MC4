using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Utilities.Sorting;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.IEnumerableExtensions
    // ReSharper restore CheckNamespace
{
    // ReSharper disable InconsistentNaming
    public static class IEnumerableExtensions
        // ReSharper restore InconsistentNaming
    {
        #region Private Static Members

        private static Func<IEnumerable, ISorter> _sortingFactory;

        #endregion

        #region Constructors

        static IEnumerableExtensions()
        {
            ResetSortingFactory();
        }

        #endregion

        #region Extension Methods

        /// <summary>
        /// Makes Enumerable.Range make more sense for when you know the start and ending values.
        /// </summary>
        public static IEnumerable<int> Range(int start, int end)
        {
            return Enumerable.Range(start, ((end - start) + 1));
        }

        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }

            return collection;
        }

        public static ISorter Sorting(this IEnumerable set)
        {
            return _sortingFactory(set);
        }

        public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            return source.Any() ? source.Max() : default(TSource);
        }

        public static IEnumerable<TDestination> Map<TSource, TDestination>(this IEnumerable source,
            Func<TSource, TDestination> fn)
        {
            return from object obj in source select fn((TSource)obj);
        }

        public static IEnumerable<TDestination> MapAndFlatten<TSource, TDestination>(this IEnumerable source,
            Func<TSource, IEnumerable<TDestination>> fn)
        {
            foreach (var collection in source.Map(fn))
            {
                foreach (var item in collection)
                {
                    yield return item;
                }
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<TSource> Each<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            action.ThrowIfNull("action");
            foreach (var obj in source)
            {
                action(obj);
            }

            return source;
        }

        [DebuggerStepThrough]
        public static IEnumerable<TSource> EachWithIndex<TSource>(this IEnumerable<TSource> source,
            Action<TSource, int> action)
        {
            var i = 0;
            foreach (var obj in source)
            {
                action(obj, i++);
            }

            return source;
        }

        /// <summary>
        /// Not sure why this is needed when it exists in System.Linq, but it's definitely used in some tests.
        /// </summary>
        public static bool Any(this IEnumerable source)
        {
            foreach (var thing in source)
            {
                return true;
            }

            return false;
        }

        public static IEnumerable<TSource> Slice<TSource>(this IEnumerable<TSource> ary, int start, int? end = null)
        {
            var source = ary.ToArray();
            end = end ?? source.Count() - 1;
            var ret = new List<TSource>();

            for (var i = start; i <= end.Value; ++i)
            {
                ret.Add(source[i]);
            }

            return ret;
        }

        public static IEnumerable<T> MergeWith<T>(this IEnumerable<T> left, IEnumerable<T> right)
        {
            var result = new List<T>(left);

            foreach (var item in right)
            {
                result.Add(item);
            }

            return result;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        #endregion

        #region Exposed Static Methods

        public static void SetSortingFactory(Func<IEnumerable, ISorter> fn)
        {
            _sortingFactory = fn;
        }

        public static void ResetSortingFactory()
        {
            SetSortingFactory(set => new Sorter(set));
        }

        #endregion
    }
}
