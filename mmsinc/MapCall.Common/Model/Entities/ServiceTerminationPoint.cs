using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceTerminationPoint : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int INSIDE_SHUTOFF = 1,
                             OTHER = 2;
        }
    }
}
