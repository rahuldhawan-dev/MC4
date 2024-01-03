using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerOpeningType : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int CATCH_BASIN = 1, CLEAN_OUT = 2, LAMPHOLE = 3, MANHOLE = 4, OUTFALL = 5, NPDES_REGULATOR = 6;
        }

        #endregion
    }
}
