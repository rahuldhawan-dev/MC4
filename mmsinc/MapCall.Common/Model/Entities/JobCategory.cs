using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class JobCategory : EntityLookup
    {
        public struct Indices
        {
            public const int
                CONSTRUCTION = 1,
                FSR = 2,
                LEAK_DETECTION = 3,
                MARKOUT = 4,
                METER_READING = 5,
                METER_TESTING = 6,
                PRODUCTION = 7,
                T_AND_D = 8,
                WATER_QUALITY = 9,
                CONTRACTOR = 10,
                OPERATIONS_TECH_GPS = 11;
        }
    }
}
