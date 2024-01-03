using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class IncidentClassification : EntityLookup
    {
        public struct Indices
        {
            public const int FATALITY = 1,
                             FIRST_AID = 2,
                             MEDICAL_TREATMENT = 6,
                             INFORMATION_ONLY = 11,
                             NOT_WORK_RELATED = 12,
                             LOST_TIME = 13,
                             RESTRICTED_DUTY = 14;
        }
    }
}
