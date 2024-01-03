using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class NearMissCategory : ReadOnlyEntityLookup
    {
        public virtual NearMissType Type { get; set; }
        public struct Indices
        {
            public const int ERGONOMICS = 4;
            public const int OTHER = 9;
            public const int STORMWATER = 23;
        }
    }
}
