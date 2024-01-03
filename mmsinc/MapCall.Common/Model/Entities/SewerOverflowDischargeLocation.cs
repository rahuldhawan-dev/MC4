using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerOverflowDischargeLocation : EntityLookup
    {
        public struct Indices
        {
            public const int RUNS_ON_GROUND = 1,
                             DITCH_OR_DETENTION_BASIN = 2,
                             STORM_SEWER = 3,
                             BODY_OF_WATER = 4,
                             OTHER = 5;
        }
    }
}

