using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerStoppageType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int MAIN = 1,
                             LATERAL = 2,
                             CSO_UNAPPROVED_BLOCKAGE = 3,
                             CSO_UNAPPROVED_MECHANICAL_AND_POWER_FAILURE = 4,
                             CSO_UNAPPROVED_WET_WEATHER_AND_II = 5,
                             CSO_UNAPPROVED_LINE_BREAK = 6,
                             CSO_UNAPPROVED_OTHER = 7,
                             CSO_APPROVED_LOCATION = 8,
                             PLANT_BYPASS_FLOW = 9;
        }
    }
}
