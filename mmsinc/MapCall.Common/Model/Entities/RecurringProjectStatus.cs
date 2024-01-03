using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RecurringProjectStatus : EntityLookup
    {
        public struct Indices
        {
            public const int
                SUBMITTED = 1,
                AP_APPROVED = 2,
                COMPLETE = 3,
                CANCELED = 6,
                BPU_SUBSTITUTED_OUT = 7,
                REVIEWED = 8,
                MANAGER_ENDORSED = 11,
                AP_ENDORSED = 12,
                MUNICIPAL_RELOCATION_APPROVED = 13,
                PROPOSED = 14;
        }
    }

    [Serializable]
    public class RecurringProjectRegulatoryStatus : EntityLookup
    {
        public struct Indices
        {
            public const int BPU_APPROVED = 2;
        }
    }
}
