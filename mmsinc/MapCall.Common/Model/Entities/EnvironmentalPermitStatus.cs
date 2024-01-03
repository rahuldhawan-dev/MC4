using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalPermitStatus : EntityLookup
    {
        public struct Indices
        {
            public const int ACTIVE = 1, INACTIVE = 2, RENEWAL_PENDING = 3, CANCELLATION_PENDING = 4;
        }
    }
}
