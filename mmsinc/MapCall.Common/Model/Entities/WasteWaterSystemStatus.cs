using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WasteWaterSystemStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ACTIVE = 1, 
                             INACTIVE = 2, 
                             INACTIVE_SEE_NOTE = 3, 
                             PENDING = 4, 
                             PENDING_MERGER = 5;
        }

        public static int[] WaterlyLookupStatuses = {
            Indices.ACTIVE,
            Indices.PENDING,
            Indices.PENDING_MERGER
        };
    }
}