using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EPACode : EntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int LEAD = 1,
                             GALVANIZED_REQUIRING_REPLACEMENT = 2,
                             LEAD_STATUS_UNKNOWN = 3,
                             NOT_LEAD = 4;
        }

        #endregion
    }
}
