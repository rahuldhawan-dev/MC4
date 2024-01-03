using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalNonComplianceEventEntityLevel : EntityLookup
    {
        public struct Indices
        {
            public const int EPA = 1, STATE = 2, COUNTY = 3, OSHA = 4, OTHER = 5;
        }
    }
}
