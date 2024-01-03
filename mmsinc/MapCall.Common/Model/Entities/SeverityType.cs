using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SeverityType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int GREEN = 1, YELLOW = 2, RED = 3;
        }
    }
}
