using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RiskRegisterAssetGroup : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int SYSTEM = 1,
                             FACILITY = 2,
                             EQUIPMENT = 3,
                             TRANSMISSION_AND_DISTRIBUTION = 4,
                             METERING = 5,
                             CUSTOMER = 6;
        }
    }
}
