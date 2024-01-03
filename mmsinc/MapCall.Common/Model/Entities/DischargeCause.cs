using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DischargeCause : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int
                RAIN = 1,
                RUNOFF_AFTER_RAIN = 2,
                SNOW = 3,
                RUNOFF_AFTER_SNOW = 4,
                LINE_BLOCKAGE = 5,
                EXCESSIVE_FLOW = 6,
                OTHER = 7;
        }

        #endregion
    }
}
