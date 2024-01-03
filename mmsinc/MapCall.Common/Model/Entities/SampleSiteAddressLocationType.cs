using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteAddressLocationType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int FACILITY = 1, 
                             PREMISE = 2, 
                             CUSTOM = 3,
                             HYDRANT = 4,
                             SAMPLE_STATION = 5,
                             VALVE = 6, 
                             PENDING_ACQUISITION = 7;
        }
    }
}
