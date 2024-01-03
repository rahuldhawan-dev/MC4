using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ValveNormalPosition : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int OPEN = 1, CLOSED = 2;
        }
    }
}
