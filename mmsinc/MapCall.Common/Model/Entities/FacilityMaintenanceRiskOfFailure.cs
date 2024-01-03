using MMSINC.Data;
using MMSINC.Metadata;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityMaintenanceRiskOfFailure : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int LOW = 1,
                             LOW_MODERATE = 2,
                             MODERATE = 3,
                             MODERATE_HIGH = 4,
                             HIGH = 5,
                             CRITICAL = 6;
        }

        [View("Risk Score")]
        public virtual int RiskScore { get; set; }
    }
}
