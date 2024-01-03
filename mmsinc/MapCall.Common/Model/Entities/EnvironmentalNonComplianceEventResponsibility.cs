using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalNonComplianceEventResponsibility : EntityLookup
    {
        public struct Indices
        {
            public const int AMERICAN_WATER = 1, THIRD_PARTY = 2, NEW_ACQUISITION = 3, TBD = 4;
        }
    }
}
