using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class NearMissType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ENVIRONMENTAL = 1, SAFETY = 2;
        }
    }
}
