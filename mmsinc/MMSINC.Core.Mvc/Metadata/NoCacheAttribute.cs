using System.Web.Mvc;
using System.Web.UI;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Filter that disables client-side caching AND server-side caching for a given controller action.
    /// </summary>
    public class NoCacheAttribute : OutputCacheAttribute
    {
        #region Constructor

        public NoCacheAttribute()
        {
            Duration = 0;
            Location = OutputCacheLocation.None;
            NoStore = true;
            VaryByParam = "*"; // The * says it doesn't matter what parameters there are.
        }

        #endregion
    }

    /// <summary>
    /// Forces client-side caching, does not enable server-side caching.
    /// </summary>
    public class AlwaysCache : OutputCacheAttribute
    {
        #region Constructor

        public AlwaysCache()
        {
            // An hour should be fine.
            Duration = 3600;
            Location = OutputCacheLocation.Client;
            VaryByParam = "*"; // The * says it doesn't matter what parameters there are.
        }

        #endregion
    }
}
