using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Sometimes a Sample Site status is set to inactive - this entity lookup will help illuminate why that may have happened.
    /// </summary>
    [Serializable]
    public class SampleSiteInactivationReason : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int CUSTOMER_DECLINED_PROGRAM = 1,
                             CUSTOMER_OPTED_OUT = 2,
                             CUSTOMER_SERVICE_LINE_REPLACED = 3,
                             COMPANY_SERVICE_LINE_REPLACED = 4,
                             INTERNAL_PLUMBING_REPLACED = 5,
                             BUILDING_DEMOLISHED = 6,
                             OTHER = 7,
                             NEW_SERVICE_DETAILS = 8;
        }
    }
}
