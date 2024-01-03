using System;
using System.Web;
using System.Web.Caching;
using MMSINC.Common;
using MMSINC.Interface;

namespace MMSINC.DataPages
{
    // TODO: FilterCache should serialize the FilterBuilder instances and 
    //      return identical instances when they contain the same 
    //      property values. That way we don't clog up the cache with
    //      redundant FilterBuilders. 

    public interface IFilterCache
    {
        Guid AddFilterBuilderToCache(IFilterBuilder filterBuilder);
        IFilterBuilder GetFilterBuilder(Guid filterKey);
    }

    /// <summary>
    /// Class used to cache IFilterBuilder objects in the HttpContext Cache.
    /// </summary>
    public class FilterCache : IFilterCache
    {
        #region Fields

        private ICache _iCache;

        #endregion

        #region Properties

        internal virtual ICache ICache
        {
            get
            {
                if (_iCache == null)
                {
                    _iCache = new CacheWrapper(HttpContext.Current.Cache);
                }

                return _iCache;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a FilterBuilder instance to the cache and returns a key for retrieving the FilterBuilder later.
        /// </summary>
        /// <param name="filterBuilder"></param>
        /// <returns></returns>
        public Guid AddFilterBuilderToCache(IFilterBuilder filterBuilder)
        {
            // This caches the FilterBuilder for a minimum of 20 minutes. If it gets accessed before then, the cache is then
            // reset for another 20 minutes. This can probably be increased to something higher, talk to Alex about it. 

            var key = Guid.NewGuid();
            ICache.Add(key.ToString(),
                filterBuilder,
                null,
                Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(20),
                CacheItemPriority.NotRemovable, // Do we want this as NotRemovable?
                null);

            return key;
        }

        public IFilterBuilder GetFilterBuilder(Guid filterKey)
        {
            // Should throw an error if it doesn't exist?
            return (IFilterBuilder)ICache.Get(filterKey.ToString());
        }

        #endregion
    }
}
