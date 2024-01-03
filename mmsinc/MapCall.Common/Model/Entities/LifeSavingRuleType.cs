using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class LifeSavingRuleType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int PPE = 1, ALCOHOL = 2, WORK_ZONE_SAFETY = 3, CAVE_IN_PROTECTION = 4, APPROVED_TOOL_USAGE = 5, HAZARDOUS_ENERGY_CONTROL = 6, FALL_PROTECTION = 7, CONFINED_SPACE_SAFEGUARDS = 8, CONTACT_WITH_UTILITY_LINES = 9;
        }
    }
}
