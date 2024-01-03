using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteLeadCopperValidationMethod : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int VISUAL_CONFIRMATION = 1,
                             LEAD_SWAB_TEST = 2,
                             BUILD_CONSTRUCTION_DOCUMENT = 3,
                             PENDING = 4, 
                             CUSTOMER_SURVEY_RESULTS = 5,
                             HISTORIC_DOCUMENTATION = 6;
        }
    }
}
