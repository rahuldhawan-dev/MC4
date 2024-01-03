using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MarkoutDamageUtilityDamageType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int COMMUNICATION = 1,
                             ELECTRIC = 2,
                             GAS = 3,
                             SEWER = 4,
                             WATER = 5;
        }
    }
}
