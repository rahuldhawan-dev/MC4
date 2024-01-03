using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class NoReadReason : EntityLookup
    {
        public struct Indices
        {
            public const int KIT_NOT_AVAILABLE = 1, NOT_DIRECTED_BY_MANAGER = 2, INSPECT_ONLY = 3;
        }
    }
}
