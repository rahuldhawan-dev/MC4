using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PitcherFilterCustomerDeliveryMethod : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int OTHER = 3;
        }
    }
}
