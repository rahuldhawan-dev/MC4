using System.Collections.Generic;

namespace MMSINC.Data
{
    public class SearchMappableArgs
    {
        #region Properties

        /// <summary>
        /// Gets the dictionary of properties and their values as they will be used
        /// in search criteria. Add/remove whatever you need to from this dictionary.
        /// </summary>
        public IDictionary<string, object> Properties { get; private set; }

        #endregion

        #region Constructor

        public SearchMappableArgs()
        {
            Properties = new Dictionary<string, object>();
        }

        #endregion
    }
}
