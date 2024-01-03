using System;
using System.Web.Caching;

namespace MMSINC.Interface
{
    public interface ICache
    {
        #region Methods

        object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
            TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);

        object Get(string key);

        #endregion
    }
}
