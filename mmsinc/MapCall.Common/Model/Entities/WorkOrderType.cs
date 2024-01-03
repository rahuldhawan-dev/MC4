using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WorkOrderType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int TANDD_DEPARTMENT = 1, PRODUCTION = 2, SHORT_CYCLE = 3, UNKNOWN = 4;
        }
    }
}