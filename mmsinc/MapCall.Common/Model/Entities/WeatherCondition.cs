using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WeatherCondition : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int
                DRY = 1,
                RAINING = 2,
                SNOWING = 3;
        }

        public static string DRY = "Dry",
                             RAINING = "Raining",
                             SNOWING = "Snowing";
        #endregion
    }
}
