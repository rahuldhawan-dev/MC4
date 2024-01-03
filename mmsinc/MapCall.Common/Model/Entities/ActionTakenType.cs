using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ActionTakenType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int TOOK_IMMEDIATE_ACTION_TO_MITIGATE_HAZARD = 1,
                             MADE_CORRECTIVE_ACTION_TO_ELIMINATE_HAZARD = 2,
                             INTERVENED_TO_CORRECT_UNSAFE_ACT = 3,
                             REPORTED_TO_MY_SUPERVISOR = 4,
                             SUBMITTED_A_WORK_ORDER = 5;
        }
    }
}
