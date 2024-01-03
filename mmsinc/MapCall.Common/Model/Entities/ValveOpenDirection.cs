using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ValveOpenDirection : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int RIGHT = 1, LEFT = 2;
        }
    }
}
