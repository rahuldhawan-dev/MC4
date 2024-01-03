using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CovidSubmissionStatus : EntityLookup
    {
        public struct Indices
        {
            public const int NEW = 1, IN_PROGRESS = 2, COMPLETE = 3;
        }
    }
}
