using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PremiseStatusCode : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ACTIVE = 1,
                             INACTIVE = 2,
                             KILLED = 3,
                             NON_CONVERTED = 4;
        }
    }
}
