using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceUtilityType : EntityLookup
    {
        public struct Indices
        {
            public const int PUBLIC_FIRE_SERVICE = 2,
                             PRIVATE_FIRE_SERVICE = 3,
                             DOMESTIC_WATER = 5,
                             DOMESTIC_WASTEWATER = 6,
                             BULK_WATER = 22,
                             BULK_WATER_MASTER = 23,
                             NON_POTABLE = 32,
                             WASTE_WATER_WITH_DEDUCT_SERVICE = 36;
        }

        public virtual string Type { get; set; }
        public virtual string Division { get; set; }
    }
}
