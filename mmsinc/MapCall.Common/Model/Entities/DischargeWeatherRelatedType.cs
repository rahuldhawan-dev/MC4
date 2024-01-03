using MMSINC.Data;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DischargeWeatherRelatedType : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int
                DRY = 1,
                WET = 2;
        }

        #endregion
    }
}
