using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StopWorkUsageType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int EMPLOYEE = 1, CONTRACTOR = 2, PUBLIC = 3;
        }
    }
}
