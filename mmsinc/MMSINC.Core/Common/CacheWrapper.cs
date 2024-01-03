using System;
using System.Web.Caching;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class CacheWrapper : ICache
    {
        #region Fields

        private readonly Cache _innerCache;

        #endregion

        #region Constructors

        public CacheWrapper(Cache innerCache)
        {
            _innerCache = innerCache;
        }

        #endregion

        #region Methods

        public object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
            TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            return _innerCache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority,
                onRemoveCallback);
        }

        public object Get(string key)
        {
            return _innerCache.Get(key);
        }

        #endregion
    }
}
