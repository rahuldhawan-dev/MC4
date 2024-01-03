using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterChangeOutStatus : EntityLookup
    {
        public struct Indices
        {
            public const int ALREADY_CHANGED = 1,
                             AW_TO_COMPLETE = 2,
                             CHANGED = 3,
                             SCHEDULED = 10;
        }
    }
}
