using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BlockCondition : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int
                IN = 1,
                OUT = 2,
                STUCK = 3,
                FLOATING = 4,
                MISSING = 5,
                IN_SEWER_MAIN = 6;
        }

        #endregion
    }
}
