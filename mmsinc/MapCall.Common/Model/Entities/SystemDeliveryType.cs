using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SystemDeliveryType : EntityLookup
    {
        public struct Indices
        {
            public const int WATER = 1, WASTE_WATER = 2;
        }
    }
}
