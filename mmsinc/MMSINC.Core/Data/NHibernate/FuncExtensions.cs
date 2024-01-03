using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace MMSINC.Data.NHibernate
{
    [ExcludeFromCodeCoverage]
    public static class FuncExtensions
    {
        #region Exposed Methods

        public static Func<TArgument, TReturn> Memoize<TArgument, TReturn>(Func<TArgument, TReturn> fn)
        {
            var cache = new ConcurrentDictionary<TArgument, TReturn>();

            return arg => cache.GetOrAdd(arg, fn);
        }

        public static Func<TArgument, TExtraArgument, TReturn> MemoizeExtra<TArgument, TExtraArgument, TReturn>(
            Func<TArgument, TExtraArgument, TReturn> fn)
        {
            var cache = new ConcurrentDictionary<TArgument, TReturn>();

            return (arg, extra) => cache.GetOrAdd(arg, a => fn(a, extra));
        }

        #endregion
    }
}
