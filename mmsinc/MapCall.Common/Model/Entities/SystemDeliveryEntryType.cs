using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SystemDeliveryEntryType : EntityLookup
    {
        public struct Indices
        {
            public const int PURCHASED_WATER = 1,
                             DELIVERED_WATER = 2,
                             TRANSFERRED_TO = 3,
                             TRANSFERRED_FROM = 4,
                             WASTEWATER_COLLECTED = 5,
                             WASTEWATER_TREATED = 6,
                             UNTREATED_EFF_DISCHARGED = 7,
                             TREATED_EFF_DISCHARGED = 8,
                             TREATED_EFF_REUSED = 9,
                             BIOCHEMICAL_OXYGEN_DEMAND = 10;
        }

        public virtual SystemDeliveryType SystemDeliveryType { get; set; }
    }
}
