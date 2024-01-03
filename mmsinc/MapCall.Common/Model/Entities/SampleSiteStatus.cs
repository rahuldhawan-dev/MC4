using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ACTIVE = 1, 
                             INACTIVE = 2, 
                             PENDING = 5, 
                             ARCHIVED_DUPLICATE_SITE = 6;
        }
    }
}
