using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RecurringFrequencyUnit : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int DAY = 1, WEEK = 2, MONTH = 3, YEAR = 4;
        }

        public const string YEAR = "Year", MONTH = "Month", WEEK = "Week", DAY = "Day";
    }
}
