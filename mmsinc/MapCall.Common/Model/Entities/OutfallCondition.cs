using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OutfallCondition : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int
                GOOD = 1,
                FAIR = 2,
                POOR = 3,
                PLUGGED = 4;
        }

        #endregion
    }
}
