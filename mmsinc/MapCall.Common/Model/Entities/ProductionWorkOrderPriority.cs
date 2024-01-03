using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrderPriority : EntityLookup
    {
        public enum Indices
        {
            EMERGENCY = 1,
            HIGH = 2,
            ROUTINE = 3,
            MEDIUM = 4,
            LOW = 5,
            ROUTINE_OFF_SCHEDULED = 6
        }
    }
}