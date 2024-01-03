using MMSINC.Data;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PublicWaterSupplyStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ACTIVE = 1, 
                             INACTIVE = 2, 
                             INACTIVE_SEE_NOTE = 3, 
                             PENDING = 4, 
                             PENDING_MERGER = 5;
        }
    }
}
