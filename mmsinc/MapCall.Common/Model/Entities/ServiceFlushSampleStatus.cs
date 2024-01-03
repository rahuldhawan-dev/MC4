using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceFlushSampleStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int TAKEN = 1,
                             ERROR_RESAMPLED = 2,
                             RESULTS_RECEIVED = 3;
        }
    }
}
