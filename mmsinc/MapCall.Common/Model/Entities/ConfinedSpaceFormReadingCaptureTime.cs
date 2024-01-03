using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ConfinedSpaceFormReadingCaptureTime : EntityLookup
    {
        public struct Indices
        {
            public const int PRE_ENTRY = 1, DURING_ENTRY = 2, POST_ENTRY = 3;
        }
    }
}
